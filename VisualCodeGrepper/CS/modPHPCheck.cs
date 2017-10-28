// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using System.Text.RegularExpressions;

// VisualCodeGrepper - Code security scanner
// Copyright (C) 2012-2014 Nick Dunn and John Murray
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.



namespace VisualCodeGrepper
{
	sealed class modPHPCheck
	{
		
		// Specific checks for PHP code
		//=============================
		
		public static void CheckPHPCode(string CodeLine, string FileName)
		{
			// Carry out any specific checks for the language in question
			//===========================================================
			
			CheckSQLInjection(CodeLine, FileName); // Check for SQLi
			CheckXSS(CodeLine, FileName); // Check for XSS
			CheckLogDisplay(CodeLine, FileName); // Is data sanitised before being written to logs
			CheckRandomisation(CodeLine, FileName); // Locate any use of randomisation functions that are not cryptographically secure
			CheckFileValidation(CodeLine, FileName); // Find any unsafe file validation (checks against data from the HTTP request *instead of* the actual file
			CheckFileInclusion(CodeLine, FileName); // Locate any include files with unsafe extensions
			CheckExecutable(CodeLine, FileName); // Check for unvalidated variables being executed via cmd line/system calls
			CheckBackTick(CodeLine, FileName); // Check for user-supplied variables being executed on the cmdline due to backtick usage
			CheckRegisterGlobals(CodeLine, FileName); // Check for usage or simulation of register_globals
			CheckParseStr(CodeLine, FileName); // Check for any unsafe usage of parse_str
			
			//== Check for passwords being handled in a case-insensitive manner ==
			if (Regex.IsMatch(CodeLine, "(strtolower|strtoupper)\\s*\\(\\s*\\S*(Password|password|pwd|PWD|Pwd|Passwd|passwd)"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Password Management", "The application appears to handle passwords in a case-insensitive manner. This can greatly increase the likelihood of successful brute-force and/or dictionary attacks.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		public static void CheckSQLInjection(string CodeLine, string FileName)
		{
			// Check for any SQL injection problems
			//=====================================
			string strVarName = ""; // Holds the variable name for the dynamic SQL statement
			
			
			//== Only check unvalidated code ==
			if (modMain.ctCodeTracker.HasValidator == true)
			{
				return;
			}
			
			
			//== Is unsanitised dynamic SQL statement prepared beforehand? ==
			if (CodeLine.Contains("=") && (CodeLine.ToLower().Contains("sql") || CodeLine.ToLower().Contains("query") || CodeLine.ToLower().Contains("stmt") || CodeLine.ToLower().Contains("query")) && (CodeLine.Contains("\"") && (CodeLine.Contains("$") || CodeLine.Contains("+"))))
			{
				//== Extract variable name from assignment statement ==
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
				modMain.ctCodeTracker.HasVulnSQLString = true;
				if (Regex.IsMatch(strVarName, "^\\$[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.SQLStatements.Contains(strVarName))
				{
					modMain.ctCodeTracker.SQLStatements.Add(strVarName);
				}
			}
			
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
			{
				//== Remove any variables which have been sanitised from the list of vulnerable variables ==
				modCSharpCheck.RemoveSanitisedVars(CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "(mysql_query|mssql_query|pg_query)\\s*\\(") && !Regex.IsMatch(CodeLine, "mysql_real_escape_string"))
			{
				
				if (modMain.ctCodeTracker.HasVulnSQLString == true)
				{
					//== Check for use of pre-prepared statements ==
					foreach (var strVar in modMain.ctCodeTracker.SQLStatements)
					{
						if (Regex.IsMatch(CodeLine, System.Convert.ToString(strVar)))
						{
							frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via a pre-prepared dynamic SQL statement.", FileName, CodeIssue.CRITICAL, CodeLine);
							break;
						}
					}
				}
				else if (CodeLine.Contains("$"))
				{
					//== Dynamic SQL built into connection/update ==
					frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via dynamic SQL statements.", FileName, CodeIssue.CRITICAL, CodeLine);
					
				}
			}
			
		}
		
		public static void CheckXSS(string CodeLine, string FileName)
		{
			// Check for any XSS problems
			//===========================
			string strVarName = "";
			bool blnIsFound = false;
			//== Only check unvalidated code ==
			if (modMain.ctCodeTracker.HasValidator == true)
			{
				return;
			}
			
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
			{
				//== Remove any variables which have been sanitised from the list of vulnerable variables ==
				modCSharpCheck.RemoveSanitisedVars(CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "\\$\\w+\\s*\\=\\s*\\$_(GET|POST|COOKIE|REQUEST|SERVER)"))
			{
				//== Extract variable name from assignment statement ==
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
				if (Regex.IsMatch(strVarName, "^\\\\\\$[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.InputVars.Contains(strVarName))
				{
					modMain.ctCodeTracker.InputVars.Add(strVarName);
				}
			}
			else if (Regex.IsMatch(CodeLine, "\\b(print|echo|print_r)\\b") && CodeLine.Contains("$") && !Regex.IsMatch(CodeLine, "strip_tags"))
			{
				modCSharpCheck.CheckUserVarXSS(CodeLine, FileName);
			}
			else if (Regex.IsMatch(CodeLine, "\\b(print|echo|print_r)\\b\\s*\\$_(GET|POST|COOKIE|REQUEST|SERVER)") && !Regex.IsMatch(CodeLine, "strip_tags"))
			{
				frmMain.Default.ListCodeIssue("Potential XSS", "The application appears to reflect a user-supplied variable to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine);
			}
			
			//== Check for DOM-based XSS in .php pages ==
			if (FileName.ToLower().EndsWith(".php") || FileName.ToLower().EndsWith(".html") && !Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise|strip_tags"))
			{
				if (Regex.IsMatch(CodeLine, "\\s+var\\s+\\w+\\s*=\\s*\"\\s*\\<\\?\\s*\\=\\s*\\w+\\s*\\?\\>\"\\;"))
				{
					//== Extract variable name from assignment statement ==
					strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
					if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.SQLStatements.Contains(strVarName))
					{
						modMain.ctCodeTracker.InputVars.Add(strVarName);
					}
				}
				else if ((CodeLine.Contains("document.write(") && CodeLine.Contains("+") && CodeLine.Contains("\"")) || Regex.IsMatch(CodeLine, ".innerHTML\\s*\\=\\s*\\w+;"))
				{
					foreach (var strVar in modMain.ctCodeTracker.InputVars)
					{
						if (Regex.IsMatch(CodeLine, System.Convert.ToString(strVar)))
						{
							frmMain.Default.ListCodeIssue("Potential DOM-Based XSS", "The application appears to allow XSS via an unencoded/unsanitised input variable.", FileName, CodeIssue.HIGH, CodeLine);
							break;
						}
					}
				}
				else if (Regex.IsMatch(CodeLine, "\\)\\s*\\.innerHTML\\s*=\\s*(\\'|\\\")\\s*\\<\\s*\\?\\s*echo\\s*\\$_(GET|POST|COOKIE|SERVER|REQUEST)\\s*\\["))
				{
					frmMain.Default.ListCodeIssue("Potential DOM-Based XSS", "The application appears to allow XSS via an unencoded/unsanitised input variable.", FileName, CodeIssue.HIGH, CodeLine);
				}
			}
			
		}
		
		public static void CheckLogDisplay(string CodeLine, string FileName)
		{
			// Check output written to logs is sanitised first
			//================================================
			
			//== Only check unvalidated code ==
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") && !CodeLine.ToLower().Contains("password"))
			{
				modCSharpCheck.RemoveSanitisedVars(CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "AddLog|error_log") && CodeLine.ToLower().Contains("password"))
			{
				if (CodeLine.ToLower().IndexOf("log") + 1 < CodeLine.ToLower().IndexOf("password") + 1)
				{
					frmMain.Default.ListCodeIssue("Application Appears to Log User Passwords", "The application appears to write user passwords to logfiles or the screen, creating a risk of credential theft.", FileName, CodeIssue.HIGH, CodeLine);
				}
			}
			else if (Regex.IsMatch(CodeLine, "AddLog|error_log") && !CodeLine.ToLower().Contains("strip_tags"))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("Unsanitized Data Written to Logs", "The application appears to write unsanitized data to its logfiles. If logs are viewed by a browser-based application this exposes risk of XSS attacks.", FileName, CodeIssue.MEDIUM, CodeLine);
						break;
					}
				}
			}
			
		}
		
		private static void CheckRandomisation(string CodeLine, string FileName)
		{
			// Check for any random functions that are not cryptographically secure
			//=====================================================================
			
			//== Check for time or non-time-based seed ==
			if (Regex.IsMatch(CodeLine, "\\$\\w+\\s*\\=\\s*\\bopenssl_random_pseudo_bytes\\b\\s*\\(\\s*\\S+\\s*\\,\\s*(0|false|False|FALSE)"))
			{
				frmMain.Default.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the function with the 'secure' value deliberately set to 'false'. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "\\$\\w+\\s*\\=\\s*\\b(mt_rand|smt_rand)\\b\\s*\\(\\s*\\)") || Regex.IsMatch(CodeLine, "\\b(mt_rand|smt_rand)\\b\\s*\\(\\w*(T|t)ime\\w*\\)"))
			{
				frmMain.Default.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the mt_rand and/or smt_rand functions without a seed to generate pseudo-random values. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "\\b(mt_rand|smt_rand)\\b\\s*\\(\\s*\\S+\\s*\\)"))
			{
				frmMain.Default.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the mt_rand function. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker, although this is partly mitigated by a seed that does not appear to be time-based.", FileName, CodeIssue.STANDARD, CodeLine);
			}
			
		}
		
		private static void CheckFileValidation(string CodeLine, string FileName)
		{
			// Check for any decisions based on characteristics of the $_FILES array
			//======================================================================
			
			//== Identify relevant 'if' statements ==
			if (Regex.IsMatch(CodeLine, "\\bif\\b\\s*\\(\\s*\\$_FILES\\s*\\[\\s*\\$\\w+\\s*\\]\\s*\\[\\s*\\'") || Regex.IsMatch(CodeLine, "\\bif\\b\\s*\\(\\s*\\!?\\s*isset\\s*\\(?\\s*\\$_FILES\\s*\\[\\s*\\$\\w+\\s*\\]\\s*\\[\\s*\\'"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Processing of $_FILES Array", "The code appears to use data within the $_FILES array in order to make to decisions. this is obtained direct from the HTTP request and may be modified by the client to cause unexpected behaviour.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		private static void CheckFileInclusion(string CodeLine, string FileName)
		{
			// Check for any user-defined variables being used to name include files
			//======================================================================
			bool blnIsFound = false;
			
			//== Identify relevant 'include' statements ==
			if (Regex.IsMatch(CodeLine, "\\b(file_include|include|require|include_once|require_once)\\b\\s*\\(\\s*\\$"))
			{
				//== Check for use of user-defined variables ==
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, "\\b(file_include|include|require|include_once|require_once)\\b\\s*\\(\\s*" + System.Convert.ToString(strVar)) || Regex.IsMatch(CodeLine, "\\b(file_include|include|require|include_once|require_once)\\b\\s*\\(\\s*\\w+\\s*\\.\\s*" + System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("File Inclusion Vulnerability", "The code appears to use a user-controlled variable as a parameter for an include statement which could lead to a file include vulnerability.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false)
				{
					frmMain.Default.ListCodeIssue("Variable Used as FileName", "The application appears to use a variable name in order to define a filename used by the application. It is unclear whether this variable can be controlled by the user - carry out a manual inspection to confirm.", FileName, CodeIssue.LOW, CodeLine);
				}
			}
			else if (Regex.IsMatch(CodeLine, "\\b(file_include|include|require|include_once|require_once)\\b\\s*\\(\\s*(\\'|\\\")\\w+\\.(inc|txt|dat)"))
			{
				//== Check for use of unsafe extensions ==
				frmMain.Default.ListCodeIssue("File Inclusion Vulnerability", "The code appears to use an unsafe file extension for an include statement which could allow an attacker to download it directly and read the uncompiled code.", FileName, CodeIssue.HIGH, CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "\\b(fwrite|file_get_contents|fopen|glob|popen|file|readfile)\\b\\s*\\(\\s*\\$"))
			{
				//== Check for use of user-defined variables ==
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, "\\b(fwrite|file_get_contents|fopen|glob|popen|file|readfile)\\b\\s*\\(\\s*" + System.Convert.ToString(strVar)) || Regex.IsMatch(CodeLine, "\\b(fwrite|file_get_contents|fopen|glob|popen|file|readfile)\\b\\s*\\(\\s*\\w+\\s*\\.\\s*" + System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("File Access Vulnerability", "The code appears to user-controlled variable as a parameter when accessing the filesystem. This could lead to a system compromise.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false)
				{
					frmMain.Default.ListCodeIssue("Variable Used as FileName", "The application appears to use a variable name in order to define a filename used by the application. It is unclear whether this variable can be controlled by the user - carry out a manual inspection to confirm.", FileName, CodeIssue.LOW, CodeLine);
				}
			}
			
		}
		
		public static void CheckExecutable(string CodeLine, string FileName)
		{
			// Check for unvalidated variables being executed via cmd line/system calls
			//=========================================================================
			bool blnIsFound = false;
			
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
			{
				return;
			}
			
			if (Regex.IsMatch(CodeLine, "\\b(exec|shell_exec|proc_open|eval|system|popen|passthru|pcntl_exec|assert)\\b") && !Regex.IsMatch(CodeLine, "escapeshellcmd"))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of an unvalidated user-controlled variable when executing a command.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false && CodeLine.Contains("$"))
				{
					frmMain.Default.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated variable when executing a command. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
		}
		
		private static void CheckBackTick(string CodeLine, string FileName)
		{
			// Check for user-supplied variables being executed on the cmdline due to backtick usage
			//======================================================================================
			bool blnIsFound = false;
			
			
			if (Regex.IsMatch(CodeLine, "`\\s*\\S*\\s*\\$_(GET|POST|COOKIE|REQUEST|SERVER)"))
			{
				frmMain.Default.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of a HTTP request variable within backticks, allowing commandline execution.", FileName, CodeIssue.HIGH, CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "`\\s*\\S*\\s*\\$\\w+"))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of a user-controlled variable within backticks, allowing commandline execution.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false)
				{
					frmMain.Default.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of a variable within backticks, allowing commandline execution. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
		}
		
		private static void CheckPHPEvaluation(string CodeLine, string FileName)
		{
			// Check for unvalidated variables being executed via cmd line/system calls
			//=========================================================================
			bool blnIsFound = false;
			
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
			{
				return;
			}
			
			if (Regex.IsMatch(CodeLine, "\\b(preg_replace|create_function)\\b") && !Regex.IsMatch(CodeLine, "strip_tags"))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("Function May Evaluate PHP Code Contained in User Controlled Variable", "The application appears to allow the use of an unvalidated user-controlled variable in conjunction with a function that will evaluate PHP code.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false && CodeLine.Contains("$"))
				{
					frmMain.Default.ListCodeIssue("Function May Evaluate PHP Code", "The application appears to allow the use of an unvalidated variable in conjunction with a function that will evaluate PHP code. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
		}
		
		private static void CheckRegisterGlobals(string CodeLine, string FileName)
		{
			// Check for any unsafe use of Global Variables
			//=============================================
			string[] arrFragments = null;
			
			if (modMain.ctCodeTracker.IsRegisterGlobals == true)
			{
				return;
			}
			
			if (modMain.ctCodeTracker.IsArrayMerge == false)
			{
				
				if (Regex.IsMatch(CodeLine, "\\bini_set\\b\\s*\\(\\s*(\\'|\\\")register_globals(\\'|\\\")\\s*\\,\\s*(1|true|TRUE|True|\\$\\w+)"))
				{
					// Is it being re-enabled
					frmMain.Default.ListCodeIssue("Use of 'register_globals'", "The application appears to re-activate the use of the dangerous 'register_globals' facility. Anything passed via GET or POST or COOKIE is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine);
					
				}
				else if (Regex.IsMatch(CodeLine, "\\$\\w+\\s*\\=\\s*\\barray_merge\\b\\s*\\(\\s*\\$_(GET|POST|COOKIE|REQUEST|SERVER)\\s*\\,\\s*\\$_(GET|POST|COOKIE|REQUEST|SERVER)"))
				{
					// Is it being simulated
					modMain.ctCodeTracker.IsArrayMerge = true;
					// Get name of the array of input parameters
					arrFragments = Regex.Split(CodeLine, "\\=\\s*\\barray_merge\\b\\s*\\(\\s*\\$_(GET|POST|COOKIE|REQUEST|SERVER)\\s*\\,");
					modMain.ctCodeTracker.GlobalArrayName = modMain.GetLastItem(arrFragments.First());
					frmMain.Default.ListCodeIssue("Indiscriminate Merging of Input Variables", "The application appears to incorporate all incoming GET and POST data into a single array. This can facilitate GET to POST conversion and may result in unexpected behaviour or unintentionally change variables.", FileName, CodeIssue.HIGH, CodeLine);
				}
				
			}
			else if (modMain.ctCodeTracker.IsArrayMerge == true)
			{
				if (Regex.IsMatch(CodeLine, "\\bglobal\\b") && Regex.IsMatch(CodeLine, modMain.ctCodeTracker.GlobalArrayName))
				{
					modMain.ctCodeTracker.IsRegisterGlobals = true;
					frmMain.Default.ListCodeIssue("Use of 'register_globals'", "The application appears to attempt to simulate the use of the dangerous 'register_globals' facility. Anything passed via GET or POST or COOKIE is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine);
				}
			}
			
		}
		
		private static void CheckParseStr(string CodeLine, string FileName)
		{
			// Check for any unsafe use of parse_str which offers similar dangers to Global Variables
			//=======================================================================================
			var blnIsFound = false;
			
			
			//== Identify unssafe usage of parse_str, with an input var, but no destination array ==
			if (Regex.IsMatch(CodeLine, "\\bparse_str\\b\\s*\\(\\s*\\$\\w+\\s*\\)"))
			{
				
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, "\\bparse_str\\b\\s*\\(\\s*" + System.Convert.ToString(strVar) + "\\s*\\)"))
					{
						frmMain.Default.ListCodeIssue("Use of 'parse_str' with User Controlled Variable", "The application appears to use parse_str in an unsafe manner in combination with a user-controlled variable. Anything passed as part of the input string is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false)
				{
					frmMain.Default.ListCodeIssue("Use of 'parse_str'", "The application appears to use parse_str in an unsafe manner. Anything passed as part of the input string is automatically assigned as a global variable in the code, with potentially serious consequences. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
		}
		
		public static void CheckPhpIni(string CodeLine, string FileName)
		{
			// Check config file for unsafe settings
			//======================================
			
			// Ignore any comments
			if (CodeLine.Trim().StartsWith(";"))
			{
				modMain.rtResultsTracker.OverallCommentCount++;
				modMain.rtResultsTracker.CommentCount++;
			}
			else if (CodeLine.Trim() == "")
			{
				modMain.rtResultsTracker.OverallWhitespaceCount++;
				modMain.rtResultsTracker.WhitespaceCount++;
			}
			else
			{
				
				if (Regex.IsMatch(CodeLine, "\\bregister_globals\\b\\s*=\\s*\\b(on|ON|On)\\b"))
				{
					// register_globals
					frmMain.Default.ListCodeIssue("Use of 'register_globals'", "The application appears to activate the use of the dangerous 'register_globals' facility. Anything passed via GET or POST or COOKIE is automatically assigned as a global variable in the code, with potentially serious consequences.", FileName, CodeIssue.CRITICAL, CodeLine);
				}
				else if (Regex.IsMatch(CodeLine, "\\bsafe_mode\\b\\s*=\\s*\\b(off|OFF|Off)\\b"))
				{
					// safe_mode
					frmMain.Default.ListCodeIssue("De-Activation of 'safe_mode'", "The application appears to de-activate the use of 'safe_mode', which can increase risks for any CGI-based applications.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
				else if (Regex.IsMatch(CodeLine, "\\b(magic_quotes_gpc|magic_quotes_runtime|magic_quotes_sybase)\\b\\s*=\\s*\\b(off|OFF|Off)\\b"))
				{
					// magic_quotes
					frmMain.Default.ListCodeIssue("De-Activation of 'magic_quotes'", "The application appears to de-activate the use of 'magic_quotes', greatly increasing the risk of SQL injection.", FileName, CodeIssue.HIGH, CodeLine);
				}
				else if (Regex.IsMatch(CodeLine, "\\bdisable_functions\\b\\s*=\\s*\\w+"))
				{
					// Is disable_functions being used
					modMain.ctCodeTracker.HasDisableFunctions = true;
				}
				else if (Regex.IsMatch(CodeLine, "\\bmysql.default_user\\b\\s*=\\s*\\broot\\b"))
				{
					// Log in to MySQL as 'root'
					frmMain.Default.ListCodeIssue("Log in to MySQL as 'root'", "The application appears to log in to MySQL as 'root', greatly increasing the consequences of a successful SQL injection attack.", FileName, CodeIssue.HIGH, CodeLine);
				}
				
				modMain.rtResultsTracker.OverallCodeCount++;
				modMain.rtResultsTracker.CodeCount++;
				
			}
			
			modMain.rtResultsTracker.OverallLineCount++;
			modMain.rtResultsTracker.LineCount++;
			
		}
		
	}
	
}
