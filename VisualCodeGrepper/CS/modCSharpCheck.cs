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
	sealed class modCSharpCheck
	{
		
		// Specific checks for C# code
		//============================
		
		public static void CheckCSharpCode(string CodeLine, string FileName)
		{
			// Carry out any specific checks for the language in question
			//===========================================================
			
			IdentifyLabels(CodeLine, FileName);
			CheckInputValidation(CodeLine, FileName); // Has .NET default validation been turned off
			CheckSQLInjection(CodeLine, FileName); // Check for SQLi
			CheckXSS(CodeLine, FileName); // Check for XSS
			CheckSecureStorage(CodeLine, FileName); // Are sensitive variables stored without using SecureString
			CheckIntOverflow(CodeLine, FileName); // Are int overflows being trapped
			CheckLogDisplay(CodeLine, FileName); // Is data sanitised before being written to logs
			CheckFileRace(CodeLine, FileName); // Check for race conditions and TOCTOU vulns
			CheckSerialization(CodeLine, FileName); // Identify serializable objects and check their security permissions
			CheckHTTPRedirect(CodeLine, FileName); // Check for safe redirects and safe use of URLs
			CheckRandomisation(CodeLine, FileName); // Locate any use of randomisation functions that are not cryptographically secure
			CheckSAML2Validation(CodeLine, FileName); // Check for correct implementation of inherited SAML2 functions
			CheckUnsafeTempFiles(CodeLine, FileName); // Check for static/obvious filenames for temp files
			CheckUnsafeCode(CodeLine, FileName); // Check for use and abuse of the "unsafe" directive
			CheckThreadIssues(CodeLine, FileName); // Check for good/bad thread management
			CheckExecutable(CodeLine, FileName); // Check for unvalidated variables being executed via cmd line/system calls
			CheckWebConfig(CodeLine, FileName); // Check config file to determine whether .NET debugging and default errors are enabled
			
			if (Regex.IsMatch(CodeLine, "\\S*(Password|password|pwd|passwd)\\S*(\\.|\\-\\>)(ToLower|ToUpper)\\s*\\("))
			{
				frmMain.Default.ListCodeIssue("Unsafe Password Management", "The application appears to handle passwords in a case-insensitive manner. This can greatly increase the likelihood of successful brute-force and/or dictionary attacks.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		public static void IdentifyLabels(string CodeLine, string FileName)
		{
			// Locate and record any labels in asp pages. These will be checked for XSS later.
			//================================================================================
			string[] arrFragments = null;
			string strLabel = "";
			
			//== Detect default .net input validation
			if (modMain.ctCodeTracker.HasValidator == false && (FileName.ToLower().EndsWith(".asp") || FileName.ToLower().EndsWith(".aspx")) && CodeLine.Contains("<asp:Label ID=\""))
			{
				arrFragments = Regex.Split(CodeLine, "\\<asp\\:Label\\s+ID=\"");
				strLabel = modMain.GetFirstItem(arrFragments.Last(), "\"");
				if (!string.IsNullOrEmpty(strLabel)&& !modMain.ctCodeTracker.AspLabels.Contains(strLabel))
				{
					modMain.ctCodeTracker.AspLabels.Add(strLabel);
				}
			}
			
		}
		
		public static void CheckInputValidation(string CodeLine, string FileName)
		{
			// Check any input validation of user-controlled variables (or lack of)
			//=====================================================================
			
			//== Detect default .net input validation
			if (modMain.ctCodeTracker.HasValidator == false && FileName.ToLower().EndsWith(".config") && CodeLine.ToLower().Contains("<pages validateRequest=\"true\""))
			{
				modMain.ctCodeTracker.HasValidator = true;
			}
			else if (modMain.ctCodeTracker.HasValidator == false && FileName.ToLower().EndsWith(".xml") && CodeLine.ToLower().Contains("<pages> element with validateRequest=\"true\""))
			{
				modMain.ctCodeTracker.HasValidator = true;
			}
			else if (FileName.ToLower().EndsWith(".config") && CodeLine.ToLower().Contains("<pages validateRequest=\"false\""))
			{
				//== .NET validation turned off deliberately ==
				modMain.ctCodeTracker.HasValidator = false;
				frmMain.Default.ListCodeIssue("Potential Input Validation Issues", "The application appears to deliberately de-activate the default .NET input validation functionality.", FileName, CodeIssue.HIGH, CodeLine);
			}
			else if (FileName.ToLower().EndsWith(".xml") && CodeLine.ToLower().Contains("<pages> element with validateRequest=\"false\""))
			{
				//== .NET validation turned off deliberately ==
				modMain.ctCodeTracker.HasValidator = false;
				frmMain.Default.ListCodeIssue("Potential Input Validation Issues", "The application appears to deliberately de-activate the default .NET input validation functionality.", FileName, CodeIssue.HIGH, CodeLine);
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
			if (CodeLine.Contains("=") && (CodeLine.ToLower().Contains("sql") || CodeLine.ToLower().Contains("query")) && (CodeLine.Contains("\"") && (CodeLine.Contains("&") || CodeLine.Contains("+"))))
			{
				//== Extract variable name from assignment statement ==
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
				modMain.ctCodeTracker.HasVulnSQLString = true;
				if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.SQLStatements.Contains(strVarName))
				{
					modMain.ctCodeTracker.SQLStatements.Add(strVarName);
				}
			}
			
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
			{
				//== Remove any variables which have been sanitised from the list of vulnerable variables ==
				RemoveSanitisedVars(CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "ExecuteQuery|ExecuteSQL|ExecuteStatement|SqlCommand\\("))
			{
				
				//== Check usage of java.sql.Statement.executeQuery, etc. ==
				if (CodeLine.Contains("\"") && CodeLine.Contains("&"))
				{
					//== Dynamic SQL built into connection/update ==
					frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via dynamic SQL statements.", FileName, CodeIssue.CRITICAL, CodeLine);
				}
				else if (modMain.ctCodeTracker.HasVulnSQLString == true)
				{
					//== Otherwise check for use of pre-prepared statements ==
					foreach (var strVar in modMain.ctCodeTracker.SQLStatements)
					{
						if (CodeLine.Contains(System.Convert.ToString(strVar)))
						{
							frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection via a pre-prepared dynamic SQL statement.", FileName, CodeIssue.CRITICAL, CodeLine);
							break;
						}
					}
				}
			}
			
		}
		
		public static void CheckXSS(string CodeLine, string FileName)
		{
			// Check for any XSS problems
			//===========================
			string strVarName = "";
			string[] arrFragments = null;
			bool blnIsFound = false;
			//== Only check unvalidated code ==
			if (modMain.ctCodeTracker.HasValidator == true)
			{
				return;
			}
			
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
			{
				//== Remove any variables which have been sanitised from the list of vulnerable variables ==
				RemoveSanitisedVars(CodeLine);
				return;
			}
			else if (Regex.IsMatch(CodeLine, "\\bHttpCookie\\b\\s+\\S+\\s+=\\s+\\S+\\.Cookies\\.Get\\("))
			{
				//== Extract variable name from assignment statement ==
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
				if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.InputVars.Contains(strVarName))
				{
					modMain.ctCodeTracker.InputVars.Add(strVarName);
				}
			}
			else if (Regex.IsMatch(CodeLine, "\\bRequest\\b\\.Form\\(\""))
			{
				//== Extract variable name from assignment statement ==
				arrFragments = Regex.Split(CodeLine, "\\bRequest\\b\\.Form\\(\"");
				strVarName = modMain.GetFirstItem(arrFragments.First(), "\"");
				if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.InputVars.Contains(strVarName))
				{
					modMain.ctCodeTracker.InputVars.Add(strVarName);
				}
			}
			else if (CodeLine.Contains("=") && (CodeLine.Contains(".Value")) || Regex.IsMatch(CodeLine, "=\\s*Request\\.QueryString\\["))
			{
				//== Extract variable name from assignment statement ==
				strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
				if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.InputVars.Contains(strVarName))
				{
					modMain.ctCodeTracker.InputVars.Add(strVarName);
				}
			}
			
			if (CodeLine.Contains("Response.Write(") && CodeLine.Contains("Request.Form("))
			{
				//== Classic ASP XSS==
				frmMain.Default.ListCodeIssue("Potential XSS", "The application appears to reflect user input to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine);
			}
			else if (CodeLine.Contains("Response.Write(") && CodeLine.Contains("\"") && CodeLine.Contains("+"))
			{
				CheckUserVarXSS(CodeLine, FileName);
			}
			else if (CodeLine.Contains("Response.Write(") && !CodeLine.Contains("\""))
			{
				CheckUserVarXSS(CodeLine, FileName);
			}
			else if (CodeLine.Contains(".Text ="))
			{
				foreach (var strLabel in modMain.ctCodeTracker.AspLabels)
				{
					if (CodeLine.Contains(System.Convert.ToString(strLabel)))
					{
						if (CodeLine.Contains("Request.QueryString[") || CodeLine.Contains(".Cookies.Get("))
						{
							frmMain.Default.ListCodeIssue("Potential XSS", "The application appears to reflect a user-supplied variable to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine);
						}
						else
						{
							CheckUserVarXSS(CodeLine, FileName);
						}
					}
				}
			}
			
			
			//== Check for use of raw strings in HTML output ==
			if (Regex.IsMatch(CodeLine, "\\bHtml\\b\\.Raw\\("))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (CodeLine.Contains(System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("Potential XSS", "The application uses the potentially dangerous Html.Raw construct in conjunction with a user-supplied variable.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				
				if (!blnIsFound)
				{
					frmMain.Default.ListCodeIssue("Potential XSS", "The application uses the potentially dangerous Html.Raw construct.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
			
			//== Check for DOM-based XSS in .asp pages ==
			if (FileName.ToLower().EndsWith(".asp") || FileName.ToLower().EndsWith(".aspx"))
			{
				if (Regex.IsMatch(CodeLine, "\\s+var\\s+\\w+\\s*=\\s*\"\\s*\\<\\%\\s*\\=\\s*\\w+\\%\\>\"\\;") && !Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise"))
				{
					//== Extract variable name from assignment statement ==
					strVarName = System.Convert.ToString(modMain.GetVarName(CodeLine));
					if (Regex.IsMatch(strVarName, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.SQLStatements.Contains(strVarName))
					{
						modMain.ctCodeTracker.InputVars.Add(strVarName);
					}
				}
				else if (((CodeLine.Contains("document.write(") && CodeLine.Contains("+") && CodeLine.Contains("\"")) || Regex.IsMatch(CodeLine, ".innerHTML\\s*\\=\\s*\\w+;")) && !Regex.IsMatch(CodeLine, "\\s*\\S*\\s*validate|encode|sanitize|sanitise\\s*\\S*\\s*"))
				{
					foreach (var strVar in modMain.ctCodeTracker.InputVars)
					{
						if (CodeLine.Contains(System.Convert.ToString(strVar)))
						{
							frmMain.Default.ListCodeIssue("Potential DOM-Based XSS", "The application appears to allow XSS via an unencoded/unsanitised input variable.", FileName, CodeIssue.HIGH, CodeLine);
							break;
						}
					}
				}
			}
			
		}
		
		public static void CheckUserVarXSS(string CodeLine, string FileName)
		{
			// Check for presence of user controlled variables in a line which writes data the screen
			//=======================================================================================
			bool blnIsFound = false;
			
			foreach (var strVar in modMain.ctCodeTracker.InputVars)
			{
				if (CodeLine.Contains(System.Convert.ToString(strVar)))
				{
					frmMain.Default.ListCodeIssue("Potential XSS", "The application appears to reflect a user-supplied variable to the screen with no apparent validation or sanitisation.", FileName, CodeIssue.HIGH, CodeLine);
					blnIsFound = true;
					break;
				}
			}
			
			if (!blnIsFound)
			{
				frmMain.Default.ListCodeIssue("Potential XSS", "The application appears to reflect data to the screen with no apparent validation or sanitisation. It was not clear if this variable is controlled by the user.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		public static void CheckSecureStorage(string CodeLine, string FileName)
		{
			// Check if passwords are stored with char[] or String instead of SecureString
			//============================================================================
			
			if (Regex.IsMatch(CodeLine, "\\s+(String|char\\[\\])\\s+\\S*(Password|password|key)\\S*"))
			{
				frmMain.Default.ListCodeIssue("Insecure Storage of Sensitive Information", "The code uses standard strings and byte arrays to store sensitive transient data such as passwords and cryptographic private keys instead of the more secure SecureString class.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		public static void CheckIntOverflow(string CodeLine, string FileName)
		{
			// Check whether precautions are in place to deal with integer overflows
			//======================================================================
			
			if (Regex.IsMatch(CodeLine, "\\bint\\b\\s*\\w+\\s*\\=\\s*\\bchecked\\b\\s+\\("))
			{
				// A check is in place, exit function
				return ;
			}
			else if ((Regex.IsMatch(CodeLine, "\\bint\\b\\s*\\w+\\s*\\=\\s*\\bunchecked\\b\\s+\\(")) && (CodeLine.Contains("+") || CodeLine.Contains("*")))
			{
				// Checks have been switched off
				frmMain.Default.ListCodeIssue("Integer Operation With Overflow Check Deliberately Disabled", "The code carries out integer operations with a deliberate disabling of overflow defences. Manually review the code to ensure that it is safe.", FileName, CodeIssue.STANDARD, CodeLine);
			}
			else if ((Regex.IsMatch(CodeLine, "\\bint\\b\\s*\\w+\\s*\\=")) && (CodeLine.Contains("+") || CodeLine.Contains("*")))
			{
				// Unchecked operation
				frmMain.Default.ListCodeIssue("Integer Operation Without Overflow Check", "The code carries out integer operations without enabling overflow defences. Manually review the code to ensure that it is safe", FileName, CodeIssue.STANDARD, CodeLine);
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
			
			if (CodeLine.ToLower().Contains(".ProcessStartInfo("))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (CodeLine.Contains(System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of an unvalidated user-controlled variable when executing a command.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false && ((!CodeLine.Contains("\"")) || (CodeLine.Contains("\"") && CodeLine.Contains("+"))))
				{
					frmMain.Default.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated variable when executing a command. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
		}
		
		public static void CheckLogDisplay(string CodeLine, string FileName)
		{
			// Check output written to logs is sanitised first
			//================================================
			
			
			//== Only check unvalidated code ==
			if (modMain.ctCodeTracker.HasValidator == true && !CodeLine.ToLower().Contains("password"))
			{
				return;
			}
			
			if (Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") && !CodeLine.ToLower().Contains("password"))
			{
				RemoveSanitisedVars(CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "LogError|Logger|logger|Logging|logging|System\\.Diagnostics\\.Debug|System\\.Diagnostics\\.Trace") && CodeLine.ToLower().Contains("password"))
			{
				if (CodeLine.ToLower().IndexOf("log") + 1 < CodeLine.ToLower().IndexOf("password") + 1)
				{
					frmMain.Default.ListCodeIssue("Application Appears to Log User Passwords", "The application appears to write user passwords to logfiles creating a risk of credential theft.", FileName, CodeIssue.HIGH, CodeLine);
				}
			}
			else if (Regex.IsMatch(CodeLine, "LogError|Logger|logger|Logging|logging|System\\.Diagnostics\\.Debug|System\\.Diagnostics\\.Trace"))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (CodeLine.Contains(System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("Unsanitized Data Written to Logs", "The application appears to write unsanitized data to its logfiles. If logs are viewed by a browser-based application this exposes risk of XSS attacks.", FileName, CodeIssue.MEDIUM, CodeLine);
						break;
					}
				}
			}
			
		}
		
		public static void CheckSerialization(string CodeLine, string FileName)
		{
			// Check for insecure object serialization and deserialization
			//============================================================
			string strClassName = "";
			string[] arrFragments = null;
			
			
			if (modMain.ctCodeTracker.IsSerializable == true)
			{
				//== File content is deserialized into onjects - flag this up for further investigation ==
				if (Regex.IsMatch(CodeLine, "\\.(Deserialize|ReadObject)\\s*\\("))
				{
					frmMain.Default.ListCodeIssue("Unsafe Object Deserialization", "The code allows objects to be deserialized. This can allow potentially hostile objects to be instantiated directly from data held in the filesystem.", FileName, CodeIssue.STANDARD, CodeLine);
				}
			}
			
			if (modMain.ctCodeTracker.IsSerializable == false && CodeLine.Contains("using System.Runtime.Serialization"))
			{
				//== Serialization is implemented in the code module ==
				modMain.ctCodeTracker.IsSerializable = true;
			}
			else if (modMain.ctCodeTracker.IsSerializable == true && modMain.ctCodeTracker.IsSerializableClass == false && CodeLine.Contains("[Serializable"))
			{
				//== Serialization is implemented for the class ==
				modMain.ctCodeTracker.IsSerializableClass = true;
			}
			else if (modMain.ctCodeTracker.IsSerializable == true && modMain.ctCodeTracker.IsSerializableClass == false && (CodeLine.Contains("[assembly: SecurityPermission(") || CodeLine.Contains("[SecurityPermissionAttribute(")))
			{
				//== Serialization is safely implemented so discontinue the checks ==
				modMain.ctCodeTracker.IsSerializable = false;
				modMain.ctCodeTracker.IsSerializableClass = false;
			}
			else if (modMain.ctCodeTracker.IsSerializableClass == true && CodeLine.Contains("public class "))
			{
				//== Extract the vulnerable class name and write out results ==
				
				// Now we have the class name this must be reset to false
				modMain.ctCodeTracker.IsSerializableClass = false;
				
				// Trim away any redundant text following the classname
				arrFragments = CodeLine.Split("{".ToCharArray());
				arrFragments = arrFragments.First().Split(':');
				
				strClassName = modMain.GetLastItem(arrFragments.First());
				if (Regex.IsMatch(strClassName, "^[a-zA-Z0-9_]*$"))
				{
					frmMain.Default.ListCodeIssue("Unsafe Object Serialization", "The code allows the object " + strClassName + " to be serialized. This can allow potentially sensitive data to be saved to the filesystem.", FileName, CodeIssue.STANDARD, CodeLine);
				}
			}
			
		}
		
		public static void CheckHTTPRedirect(string CodeLine, string FileName)
		{
			// Check for safe use HTTP redirects
			//==================================
			bool blnIsFound = false;
			
			
			//== Check for secure HTTP usage ==
			if (CodeLine.Contains("Response.Redirect(") && CodeLine.Contains("HTTP:"))
			{
				frmMain.Default.ListCodeIssue("URL request sent over HTTP:", "The URL used in the HTTP request appears to be unencrypted. Check the code manually to ensure that sensitive data is not being submitted.", FileName, CodeIssue.STANDARD, CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "Response\\.Redirect\\(") && !Regex.IsMatch(CodeLine, "Response\\.Redirect\\(\\s*\\\"\\S+\\\"\\s*\\)"))
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, "Response\\.Redirect\\(\\s*" + System.Convert.ToString(strVar)) || Regex.IsMatch(CodeLine, "Response\\.Redirect\\(\\s*(\\\"\\S+\\\"|S+)\\s*(\\+|\\&)\\s*" + System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("URL Request Gets Path from Unvalidated Variable", "The URL used in the HTTP request is loaded from an unsanitised variable. This can allow an attacker to redirect the user to a site under the control of a third party.", FileName, CodeIssue.MEDIUM, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false)
				{
					frmMain.Default.ListCodeIssue("URL Request Gets Path from Variable", "The URL used in the HTTP request appears to be loaded from a variable. Check the code manually to ensure that malicious URLs cannot be submitted by an attacker.", FileName, CodeIssue.STANDARD, CodeLine);
				}
			}
			
		}
		
		private static void CheckRandomisation(string CodeLine, string FileName)
		{
			// Check for any random functions that are not cryptographically secure
			//=====================================================================
			
			//== Check for non-time-based seed ==
			if (Regex.IsMatch(CodeLine, "\\bRandomize\\b\\(\\)") || Regex.IsMatch(CodeLine, "\\bRandomize\\b\\(\\w*(T|t)ime\\w*\\)"))
			{
				modMain.ctCodeTracker.HasSeed = false;
			}
			else if (Regex.IsMatch(CodeLine, "\\bRandomize\\b\\(\\S+\\)"))
			{
				modMain.ctCodeTracker.HasSeed = true;
			}
			
			//== Check for unsafe functions Next() or NextBytes() ==
			if (Regex.IsMatch(CodeLine, "\\bRandom\\b\\.Next(Bytes\\(|\\()"))
			{
				if (modMain.ctCodeTracker.HasSeed)
				{
					frmMain.Default.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the Next() and/or NextBytes() functions. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker, although this is partly mitigated by a seed that does not appear to be time-based.", FileName, CodeIssue.STANDARD, CodeLine);
				}
				else
				{
					frmMain.Default.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the Next() and/or NextBytes() functions without a seed to generate pseudo-random values. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
		}
		
		private static void CheckSAML2Validation(string CodeLine, string FileName)
		{
			// Check for validation of SAML2 conditions
			//=========================================
			
			//== Locate entry into overridden SAML2 function ==
			if (modMain.ctCodeTracker.IsSamlFunction == false && Regex.IsMatch(CodeLine, "\\boverride\\b\\s+\\bvoid\\b\\s+\\bValidateConditions\\b\\(\\bSaml2Conditions\\b"))
			{
				if (CodeLine.Contains("{"))
				{
					modMain.ctCodeTracker.IsSamlFunction = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.ClassBraces);
				}
				else
				{
					modMain.ctCodeTracker.IsSamlFunction = true;
				}
			}
			else if (modMain.ctCodeTracker.IsSamlFunction == true)
			{
				//== Report issue if function is empty ==
				if (CodeLine.Trim() != "" && CodeLine.Trim() != "{" && CodeLine.Trim() != "}")
				{
					if (Regex.IsMatch(CodeLine, "\\s*\\S*\\s*validate|encode|sanitize|sanitise\\S*\\(\\S*\\s*conditions"))
					{
						modMain.ctCodeTracker.IsSamlFunction = false;
					}
				}
				else
				{
					modMain.ctCodeTracker.IsSamlFunction = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.ClassBraces);
					if (modMain.ctCodeTracker.IsSamlFunction == false)
					{
						frmMain.Default.ListCodeIssue("Insufficient SAML2 Condition Validation", "The code includes a token handling class that inherits from Saml2SecurityTokenHandler. It appears not to perform any validation on the Saml2Conditions object passed, violating its contract with the superclass and undermining authentication/authorisation conditions.", FileName, CodeIssue.MEDIUM);
					}
				}
				
			}
			
		}
		
		private static void CheckUnsafeTempFiles(string CodeLine, string FileName)
		{
			// Identify any creation of temp files with static names
			//======================================================
			
			if (Regex.IsMatch(CodeLine, "\\=\\s*File\\.Open\\(\\\"\\S*(temp|tmp)\\S*\\\"\\,"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker creates a file with the same name between the application's creation and attempted usage) or a symbolic link attack where an attacker creates a symbolic link at the temporary file location.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		public static void CheckFileRace(string CodeLine, string FileName)
		{
			// Check for potential TOCTOU/race conditions
			//===========================================
			
			int intSeverity = 0; // For TOCTOU vulns, severity will be modified according to length of time between check and usage.
			
			
			//== Check for TOCTOU (Time Of Check, Time Of Use) vulnerabilities==
			if ((!modMain.ctCodeTracker.IsLstat) && (Regex.IsMatch(CodeLine, "(File|Directory)\\.Exists\\(") && !Regex.IsMatch(CodeLine, "Process\\.Start\\(|new\\s+FileInfo\\(|Directory\\.GetFiles\\(|\\.FileName\\;")))
			{
				// Check has taken place - begin monitoring for use of the file/dir
				modMain.ctCodeTracker.IsLstat = true;
			}
			else if (modMain.ctCodeTracker.IsLstat)
			{
				// Increase line count while monitoring
				if (CodeLine.Trim() != "" && CodeLine.Trim() != "{" && CodeLine.Trim() != "}")
				{
					modMain.ctCodeTracker.TocTouLineCount++;
				}
				
				if (modMain.ctCodeTracker.TocTouLineCount < 2 && Regex.IsMatch(CodeLine, "Process\\.Start\\(|new\\s+FileInfo\\(|Directory\\.GetFiles\\(|\\.FileName\\;"))
				{
					// Usage takes place almost immediately so no problem
					modMain.ctCodeTracker.IsLstat = false;
				}
				else if (modMain.ctCodeTracker.TocTouLineCount > 1 && Regex.IsMatch(CodeLine, "Process\\.Start\\(|new\\s+FileInfo\\(|Directory\\.GetFiles\\(|\\.FileName\\;"))
				{
					// Usage takes place sometime later. Set severity accordingly and notify user
					modMain.ctCodeTracker.IsLstat = false;
					if (modMain.ctCodeTracker.TocTouLineCount > 5)
					{
						intSeverity = 2;
					}
					frmMain.Default.ListCodeIssue("Potential TOCTOU (Time Of Check, Time Of Use) Vulnerability", "The .Exists() check occurs " + System.Convert.ToString(modMain.ctCodeTracker.TocTouLineCount) + " lines before the file/directory is accessed. The longer the time between the check and the fopen(), the greater the likelihood that the check will no longer be valid.", FileName);
				}
			}
			
		}
		
		private static void CheckUnsafeCode(string CodeLine, string FileName)
		{
			// Identify any unsafe code directives
			//====================================
			
			if (modMain.ctCodeTracker.IsUnsafe == false && Regex.IsMatch(CodeLine, "\\bunsafe\\b"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Code Directive", "The uses the 'unsafe' directive which allows the use of C-style pointers in the code. This code has an increased risk of unexpected behaviour, including buffer overflows, memory leaks and crashes.", FileName, CodeIssue.MEDIUM, CodeLine);
				if (CodeLine.Contains("{"))
				{
					modMain.ctCodeTracker.IsUnsafe = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.UnsafeBraces);
				}
				else
				{
					modMain.ctCodeTracker.IsUnsafe = true;
				}
			}
			if (modMain.ctCodeTracker.IsUnsafe == true)
			{
				//== Locate any fixed size buffers ==
				if (Regex.IsMatch(CodeLine, "\\bfixed\\b\\s+char\\s+\\w+\\s*\\["))
				{
					modMain.ctCodeTracker.AddBuffer(CodeLine);
				}
				else if (Regex.IsMatch(CodeLine, "\\bfixed\\b\\s+byte\\s+\\w+\\s*\\["))
				{
					modMain.ctCodeTracker.AddBuffer(CodeLine, "byte");
				}
				modMain.ctCodeTracker.IsUnsafe = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.UnsafeBraces);
			}
			
		}
		
		private static void CheckThreadIssues(string CodeLine, string FileName)
		{
			// Identify potential for race conditions and deadlocking
			//=======================================================
			bool blnIsRace = false;
			string strSyncObject = "";
			
			
			
			//== Identify object locked for use in synchronized block ==
			if (modMain.ctCodeTracker.IsSynchronized == false && Regex.IsMatch(CodeLine, "\\block\\b\\s*\\(\\s*\\w+\\s*\\)"))
			{
				strSyncObject = GetSyncObject(CodeLine);
				modMain.ctCodeTracker.LockedObject = strSyncObject;
				modMain.ctCodeTracker.SyncIndex++;
			}
			
			
			
			//== Identify entry into a synchronized block ==
			//== The synchronized may be followed by method type and name for a synchronized method, or by braces for a synchronized block ==
			if (modMain.ctCodeTracker.IsSynchronized == false && Regex.IsMatch(CodeLine, "\\block\\b\\s*\\S*\\s*\\S*\\s*\\("))
			{
				if (CodeLine.Contains("{"))
				{
					modMain.ctCodeTracker.IsSynchronized = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.SyncBraces);
				}
				else
				{
					modMain.ctCodeTracker.IsSynchronized = true;
				}
				
			}
			else if (modMain.ctCodeTracker.IsSynchronized == false)
			{
				
				//== Check for any unsafe modifications to instance variables ==
				if (modMain.ctCodeTracker.GlobalVars.Count > 0)
				{
					foreach (var itmItem in modMain.ctCodeTracker.GlobalVars)
					{
						blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem);
						if (blnIsRace)
						{
							break;
						}
					}
				}
				
				if (blnIsRace == false && modMain.ctCodeTracker.GetSetMethods.Count > 0)
				{
					foreach (var itmItem in modMain.ctCodeTracker.GetSetMethods)
					{
						blnIsRace = CheckRaceCond(CodeLine, FileName, itmItem);
						if (blnIsRace)
						{
							break;
						}
					}
				}
				
			}
			else if (modMain.ctCodeTracker.IsSynchronized)
			{
				//== Track the amount of code that is inside the lock - resources may be locked unnecessarily ==
				if (CodeLine.Trim() != "{" && CodeLine.Trim() != "}")
				{
					modMain.ctCodeTracker.SyncLineCount++;
				}
				
				//== Check whether still inside synchronized code ==
				modMain.ctCodeTracker.IsSynchronized = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.SyncBraces);
				
				//== Check for large areas of locked code and potential deadlock ==
				CheckSyncIssues(CodeLine, FileName);
			}
			
		}
		
		private static void CheckSyncIssues(string CodeLine, string FileName)
		{
			// Check for, and report on, any issues located inside the synchronized block or when leaving the block
			//=====================================================================================================
			int intSeverity = 0;
			int intIndex = 0;
			string strSyncObject = "";
			string strOuterSyncObject = "";
			
			
			//== Report potentially excessive locking when leaving the code block ==
			if (modMain.ctCodeTracker.IsSynchronized == false)
			{
				
				if (modMain.ctCodeTracker.SyncLineCount > 14)
				{
					intSeverity = CodeIssue.MEDIUM;
				}
				else if (modMain.ctCodeTracker.SyncLineCount > 10)
				{
					intSeverity = CodeIssue.STANDARD;
				}
				else if (modMain.ctCodeTracker.SyncLineCount > 6)
				{
					intSeverity = CodeIssue.LOW;
				}
				
				if (modMain.ctCodeTracker.SyncLineCount > 6)
				{
					frmMain.Default.ListCodeIssue("Thread Locks - Possible Performance Impact", "There are " + System.Convert.ToString(modMain.ctCodeTracker.SyncLineCount) + " lines of code in the locked code block. Manually check the code to ensure any shared resources are not being locked unnecessarily.", FileName, intSeverity);
				}
				
				modMain.ctCodeTracker.SyncLineCount = 0;
				
			}
			else if (!string.IsNullOrEmpty(modMain.ctCodeTracker.LockedObject)&& Regex.IsMatch(CodeLine, "\\block\\b\\s*\\(\\s*\\w+\\s*\\)"))
			{
				//== Build dictionary for potential deadlocks by tracking synchronized blocks inside synchronized blocks ==
				strOuterSyncObject = modMain.ctCodeTracker.LockedObject;
				strSyncObject = GetSyncObject(CodeLine);
				
				if (!string.IsNullOrEmpty(strSyncObject))
				{
					//== Check if this sync block already exists ==
					foreach (var itmItem in modMain.ctCodeTracker.SyncBlockObjects)
					{
						if (itmItem.BlockIndex == modMain.ctCodeTracker.SyncIndex)
						{
							intIndex = System.Convert.ToInt32(itmItem.BlockIndex);
							//== Add to existing block ==
							if (!itmItem.InnerObjects.Contains(strSyncObject))
							{
								itmItem.InnerObjects.Add(strSyncObject);
							}
							break;
						}
					}
					
					//== Create new sync block an add inner object name ==
					if (intIndex == 0)
					{
						modJavaCheck.AddNewSyncBlock(strOuterSyncObject, strSyncObject);
					}
					
					modJavaCheck.CheckDeadlock(strOuterSyncObject, strSyncObject, FileName);
					
				}
			}
			
		}
		
		private static string GetSyncObject(string CodeLine)
		{
			// Extract the name of a synchronized object from a line of code
			//==============================================================
			string strSyncObject = "";
			string[] strFragments = null;
			
			
			strFragments = Regex.Split(CodeLine, "\\block\\b\\s*\\(");
			strSyncObject = modMain.GetFirstItem(strFragments.Last(), ")");
			if (!string.IsNullOrEmpty(strSyncObject))
			{
				modMain.ctCodeTracker.LockedObject = strSyncObject;
			}
			
			return strSyncObject;
			
		}
		
		private static bool CheckRaceCond(string CodeLine, string FileName, KeyValuePair<string, string> DictionaryItem)
		{
			// Check if line contains any references to public variables of servlets or to getter/setter methods of servlets
			//==============================================================================================================
			string strServletName = "";
			string[] arrFragments = null;
			bool blnRetVal = false;
			
			
			if (CodeLine.Contains("." + System.Convert.ToString(DictionaryItem.Key)))
			{
				arrFragments = Regex.Split(CodeLine, "." + System.Convert.ToString(DictionaryItem.Key));
				strServletName = modMain.GetLastItem(arrFragments.First());
				if (modMain.ctCodeTracker.ServletInstances.Count > 0 && modMain.ctCodeTracker.ServletInstances.ContainsKey(strServletName))
				{
					if (DictionaryItem.Value == modMain.ctCodeTracker.ServletInstances[strServletName])
					{
						frmMain.Default.ListCodeIssue("Possible Race Condition", "A global variable is being used/modified without a 'lock' block.", FileName, CodeIssue.HIGH);
						blnRetVal = true;
					}
				}
			}
			
			return blnRetVal;
			
		}
		
		public static void RemoveSanitisedVars(string CodeLine)
		{
			// Remove any variables which have been sanitised from the list of vulnerable variables
			//=====================================================================================
			
			if (modMain.ctCodeTracker.InputVars.Count > 0)
			{
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (!(strVar.contains("(") || strVar.contains(")") || strVar.contains("[") || strVar.contains("]") || strVar.contains(" ") || strVar.contains("+") || strVar.contains("*")))
					{
						if (Regex.IsMatch(CodeLine, strVar + "\\s*\\=\\s*\\S*(validate|encode|sanitize|sanitise)\\S*\\(" + System.Convert.ToString(strVar)))
						{
							modMain.ctCodeTracker.InputVars.Remove(strVar);
							break;
						}
					}
				}
			}
			
		}
		
		public static void CheckWebConfig(string CodeLine, string FileName)
		{
			// Report any security issues in config file such as debugging or .net default errors
			//===================================================================================
			
			if (!FileName.ToLower().EndsWith("web.config"))
			{
				return;
			}
			
			if (Regex.IsMatch(CodeLine, "\\<\\s*customErrors\\s+mode\\s*\\=\\s*\\\"Off\\\"\\s*\\/\\>"))
			{
				frmMain.Default.ListCodeIssue(".NET Default Errors Enabled", "The application is configured to display .NET default errors. This can provide an attacker with useful information and should not be used in a live application.", FileName, CodeIssue.MEDIUM);
			}
			else if (Regex.IsMatch(CodeLine, "\\bdebug\\b\\s*\\=\\s*\\\"\\s*true\\s*\\\""))
			{
				frmMain.Default.ListCodeIssue(".NET Debugging Enabled", "The application is configured to return .NET debug information. This can provide an attacker with useful information and should not be used in a live application.", FileName, CodeIssue.MEDIUM);
			}
			
		}
		
	}
	
}
