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
	sealed class modPlSqlCheck
	{
		
		// Specific checks for PL/SQL code
		//================================
		
		public static void CheckPLSQLCode(string CodeLine, string FileName)
		{
			// Carry out any specific checks for the language in question
			//===========================================================
			
			
			CheckCrypto(CodeLine, FileName); // Check for use Oracle encryption packages for sensitive data
			CheckSqlInjection(CodeLine, FileName); // Check usage of EXECUTE IMMEDIATE and OPEN FOR
			CheckPrivs(CodeLine, FileName); // Check privilege assignments for packages
			CheckTransControl(CodeLine, FileName); // Check potential for data corruption for inappropriate use of COMMIT/ROLLBACK
			CheckErrorHandling(CodeLine, FileName); // Identify error handling via return values instead of raising an exception
			CheckViewFormat(CodeLine, FileName); // Identify any data formatting within views
			
		}
		
		private static void CheckCrypto(string CodeLine, string FileName)
		{
			// Check for use Oracle encryption packages for sensitive data such as passwords
			//==============================================================================
			
			//== Do not perform this check for SQL*Plus files ==
			if (FileName.EndsWith(".sql"))
			{
				return;
			}
			
			//== Check for use of DBMS_CRYPTO package (reversible encryption) or DBMS_OBFUSCATION_TOOLKIT (hashes) for anything that appears to deal with passwords ==
			if (modMain.ctCodeTracker.IsOracleEncrypt == false && (CodeLine.Contains("DBMS_CRYPTO") || CodeLine.Contains("DBMS_OBFUSCATION_TOOLKIT")))
			{
				modMain.ctCodeTracker.IsOracleEncrypt = true;
			}
			if (modMain.ctCodeTracker.IsOracleEncrypt == false && CodeLine.Contains("PASSWORD") && !CodeLine.Contains("ACCEPT"))
			{
				frmMain.Default.ListCodeIssue("Code Appears to Process Passwords Without the Use of a Standard Oracle Encryption Module", "The code contains references to 'password'. The absence of any hashing or decryption functions indicates that the password may be stored as plaintext.", FileName, CodeIssue.HIGH);
			}
			
		}
		
		private static void CheckSqlInjection(string CodeLine, string FileName)
		{
			// Check for use of EXECUTE IMMEDIATE or OPEN FOR in combination with user-supplied data
			//======================================================================================
			string strVarName = ""; // Holds the variable name for the dynamic SQL statement
			string[] arrFragments = null;
			
			
			//== Is unsanitised dynamic SQL statement prepared beforehand? ==
			if (modMain.ctCodeTracker.IsInsideSQLVarDec == true)
			{
				if (Regex.IsMatch(CodeLine, "(\\'|\\\")\\s*(SELECT|UPDATE|DELETE|INSERT|MERGE|CREATE|SAVEPOINT|ROLLBACK|DROP)"))
				{
					if (!modMain.ctCodeTracker.SQLStatements.Contains(modMain.ctCodeTracker.CurrentVar))
					{
						modMain.ctCodeTracker.SQLStatements.Add(modMain.ctCodeTracker.CurrentVar);
					}
					modMain.ctCodeTracker.IsInsideSQLVarDec = false;
				}
				else if (CodeLine.Contains(";"))
				{
					modMain.ctCodeTracker.IsInsideSQLVarDec = false;
				}
			}
			else
			{
				if (Regex.IsMatch(CodeLine, "\\bPROCEDURE\\b\\s+\\w+"))
				{
					//== Check if we are inside a procedure so we can extract any input variables ==
					modMain.ctCodeTracker.IsInsideProcDec = true;
				}
				else if (modMain.ctCodeTracker.IsInsideProcDec == true)
				{
					//== Get any varnames that are passed in to a procedure ==
					if (Regex.IsMatch(CodeLine, "\\w+\\s+\\bIN\\b"))
					{
						arrFragments = Regex.Split(CodeLine, "\\bIN\\b");
						strVarName = modMain.GetLastItem(arrFragments.First());
						modMain.ctCodeTracker.InputVars.Add(strVarName);
					}
					if (CodeLine.Contains(")"))
					{
						modMain.ctCodeTracker.IsInsideProcDec = false;
					}
				}
				else if ((CodeLine.Contains(":=") && Regex.IsMatch(CodeLine, "(\\'|\\\")\\s*(SELECT|UPDATE|DELETE|INSERT|MERGE|CREATE|SAVEPOINT|ROLLBACK|DROP)")) || (Regex.IsMatch(CodeLine, "(SQL|QRY|QUERY)\\w*\\s*\\:\\=")))
				{
					//== Extract variable name from assignment statement ==
					arrFragments = CodeLine.Split(":".ToCharArray());
					strVarName = System.Convert.ToString(arrFragments.First().Trim());
					if (!modMain.ctCodeTracker.SQLStatements.Contains(strVarName))
					{
						modMain.ctCodeTracker.SQLStatements.Add(strVarName);
					}
				}
				else if (Regex.IsMatch(CodeLine, "\\:\\=\\s*$"))
				{
					//== Declaration starts on next line ==
					modMain.ctCodeTracker.IsInsideSQLVarDec = true;
					//== Extract variable name from assignment statement ==
					arrFragments = CodeLine.Split(":".ToCharArray());
					modMain.ctCodeTracker.CurrentVar = System.Convert.ToString(arrFragments.First().Trim());
				}
			}
			
			//== Check for misuse of sql statements ==
			if (modMain.ctCodeTracker.IsInsidePlSqlExecuteStmt == false)
			{
				if ((CodeLine.Contains("EXECUTE IMMEDIATE") || CodeLine.Contains("OPEN FOR")) && (Regex.IsMatch(CodeLine, "(\\'\\\")\\s*\\|\\|\\s*\\w+") || Regex.IsMatch(CodeLine, "\\w+\\s*\\|\\|\\s*(\\'\\\")\\s*\\|\\|")))
				{
					frmMain.Default.ListCodeIssue("Variable concatenated with dynamic SQL statement.", "Statement is potentially vulnerable to SQL injection, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine);
				}
				else if ((CodeLine.Contains("EXECUTE IMMEDIATE") || CodeLine.Contains("OPEN FOR")) && !(CodeLine.Contains("'") || CodeLine.Contains("\"")))
				{
					foreach (var strVar in modMain.ctCodeTracker.SQLStatements)
					{
						if (CodeLine.Contains(System.Convert.ToString(strVar)))
						{
							frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection through use of an input variable within a query, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine);
							break;
						}
					}
				}
				else if ((CodeLine.Contains("EXECUTE IMMEDIATE") || CodeLine.Contains("OPEN FOR")) && !CodeLine.Contains(";"))
				{
					modMain.ctCodeTracker.IsInsidePlSqlExecuteStmt = true;
				}
				
			}
			else
			{
				if (Regex.IsMatch(CodeLine, "(\\'\\\")\\s*\\|\\|\\s*\\w+") || Regex.IsMatch(CodeLine, "\\w+\\s*\\|\\|\\s*(\\'\\\")"))
				{
					frmMain.Default.ListCodeIssue("Variable concatenated with dynamic SQL statement.", "Statement is potentially vulnerable to SQL injection, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine);
				}
				else if (!(CodeLine.Contains("'") || CodeLine.Contains("\"")))
				{
					foreach (var strVar in modMain.ctCodeTracker.SQLStatements)
					{
						if (CodeLine.Contains(System.Convert.ToString(strVar)))
						{
							frmMain.Default.ListCodeIssue("Potential SQL Injection", "The application appears to allow SQL injection through use of an input variable within a query, depending on the origin of input variables and opportunities for an attacker to modify them before they reach the procedure.", FileName, CodeIssue.CRITICAL, CodeLine);
							break;
						}
					}
				}
				if (CodeLine.Contains(";"))
				{
					modMain.ctCodeTracker.IsInsidePlSqlExecuteStmt = false;
				}
			}
			
		}
		
		private static void CheckPrivs(string CodeLine, string FileName)
		{
			// Check privilege assignments for packages and highlight anything too liberal
			//============================================================================
			
			
			//== Check for 'CREATE OR REPLACE PACKAGE BODY' without 'AUTHID CURRENT_USER' ==
			if (modMain.ctCodeTracker.IsNewPackage == false && (CodeLine.Contains("CREATE OR REPLACE PACKAGE BODY") || CodeLine.Contains("CREATE PACKAGE BODY")))
			{
				modMain.ctCodeTracker.IsNewPackage = true;
			}
			
			//== Check the privs for any new package ==
			if (modMain.ctCodeTracker.IsNewPackage == true)
			{
				if (Regex.IsMatch(CodeLine, "\\bAUTHID\\b\\s+\\bCURRENT_USER\\b"))
				{
					// If package is running as current user there's no problem - set to false and carry on
					modMain.ctCodeTracker.IsNewPackage = false;
				}
				else if (Regex.IsMatch(CodeLine, "\\bAUTHID\\b\\s+\\bDEFINER\\b"))
				{
					// If package is running as definer then give a warning
					frmMain.Default.ListCodeIssue("Package Running Under Potentially Excessive Permissions", "The use of AUTHID DEFINER allows a user to run functions from this package in the role of the definer (usually a developer or administrator).", FileName);
					modMain.ctCodeTracker.IsNewPackage = false;
				}
				if (modMain.ctCodeTracker.IsNewPackage == true && Regex.IsMatch(CodeLine, "\\b(AS|IS)\\b"))
				{
					// If we've reached this point then the package has been defined with no specified privileges and so is running as definer
					frmMain.Default.ListCodeIssue("Package Running Under Potentially Excessive Permissions", "The failure to use AUTHID CURRENT_USER allows a user to run functions from this package in the role of the definer (usually a developer or administrator).", FileName, CodeIssue.STANDARD, "1");
					modMain.ctCodeTracker.IsNewPackage = false;
				}
			}
			
		}
		
		private static void CheckTransControl(string CodeLine, string FileName)
		{
			// Check potential for data corruption for inappropriate use of COMMIT/ROLLBACK
			//=============================================================================
			
			//== Do not perform this check for SQL*Plus files ==
			if (FileName.EndsWith(".sql"))
			{
				return;
			}
			
			//== Check for transactional control in non-autonomous procedures ==
			if (CodeLine.Contains("PRAGMA AUTONOMOUS_TRANSACTION"))
			{
				modMain.ctCodeTracker.IsAutonomousProcedure = true;
			}
			
			//== If the procedure is not autonomous identify any transactional controls ==
			if (modMain.ctCodeTracker.IsAutonomousProcedure == false && (CodeLine.Contains("COMMIT") || CodeLine.Contains("ROLLBACK")))
			{
				frmMain.Default.ListCodeIssue("Stored Procedure Contains COMMIT and/or ROLLBACK Statements in Procedures/Functions, Without the Use of PRAGMA AUTONOMOUS_TRANSACTION.", "This can result in data corruption, since rolling back or committing will split a wider logical transaction into two possibly conflicting sub-transactions. Exceptions to this include auditing procedures and long-running worker procedures.", FileName, CodeIssue.LOW);
			}
			
		}
		
		private static void CheckErrorHandling(string CodeLine, string FileName)
		{
			// Identify error handling via return values instead of raising an exception due to
			// risk of data corruption and implications for maintenance and bugs
			//=================================================================================
			
			//== Check for error handling with output parameters and magic numbers ==
			if (CodeLine.Contains("ERROR") && CodeLine.Contains("OUT") && CodeLine.Contains("NUMBER"))
			{
				frmMain.Default.ListCodeIssue("Error Handling With Output Parameters.", "The code appears to use output parameter(s) which implicitly signal an error by returning a special value, rather than raising an exception. This can make code harder to maintain and more error prone and can result in unexpected behaviour and data corruption.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		private static void CheckViewFormat(string CodeLine, string FileName)
		{
			// Identify any data formatting within views due to risk of DoS and data corruption
			//=================================================================================
			
			
			//== Check for data formatting within views ==
			if (modMain.ctCodeTracker.IsView == false && CodeLine.Contains("CREATE OR REPLACE VIEW"))
			{
				modMain.ctCodeTracker.IsView = true;
			}
			
			if (modMain.ctCodeTracker.IsView == true)
			{
				if (CodeLine.Contains("\\") && ((CodeLine.IndexOf("\\*") + 1 != CodeLine.IndexOf("\\") + 1 ) && (CodeLine.IndexOf("\\\\") + 1 != CodeLine.IndexOf("\\") + 1)))
				{
					modMain.ctCodeTracker.IsView = false;
				}
				else if (CodeLine.Contains("TO_CHAR") || CodeLine.Contains("TRIM(") || CodeLine.Contains("TO_NUMBER") || CodeLine.Contains("UPPER(") || CodeLine.Contains("LOWER("))
				{
					frmMain.Default.ListCodeIssue("Data Formatting Within VIEW.", "This can can result in performance issues and can facilitate DoS attacks in any situation where any attacker manages to cause repeated queries against the view. There is also a possibility of data corruption due to mismatch between views and underlying tables.", FileName, CodeIssue.STANDARD, CodeLine);
				}
			}
			
		}
		
	}
	
}
