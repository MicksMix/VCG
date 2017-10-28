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
	sealed class modCobolCheck
	{
		
		// Specific checks for COBOL code
		//===============================
		
		public static void CheckCobolCode(string CodeLine, string FileName)
		{
			// Carry out any specific checks for the language in question
			//===========================================================
			
			TrackVarAssignments(CodeLine, FileName); // Check for matching new/delete, etc.
			CheckBuffer(CodeLine, FileName); // Track buffer sizes and check for overflows
			CheckSigned(CodeLine, FileName); // Check for signed/unsigned integer comparisons
			CheckInputValidation(CodeLine, FileName); // How is input being handled before display/storage
			CheckFileAccess(CodeLine, FileName); // Are sensitive variables stored insecurely
			CheckLogDisplay(CodeLine, FileName); // Is data sanitised before being written to logs
			CheckFileRace(CodeLine, FileName); // Check for race conditions and TOCTOU vulns
			CheckRandomisation(CodeLine, FileName); // Locate any use of randomisation functions that are not cryptographically secure
			CheckUnsafeTempFiles(CodeLine, FileName); // Check for static/obvious filenames for temp files
			CheckDynamicCall(CodeLine, FileName); // Identify any user controlled variables used for dynamic function calls
			
			if (Regex.IsMatch(CodeLine, "(LOWER|UPPER)\\-CASE\\s*\\(\\S*(Password|password|PASSWORD|pwd|PWD|passwd|PASSWD)"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Password Management", "The application appears to handle passwords in a case-insensitive manner. This can greatly increase the likelihood of successful brute-force and/or dictionary attacks.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		private static void CheckIdentificationDivision(string CodeLine, string FileName)
		{
			// The Identification Division of a COBOL program should ideally contain only one PROGRAM-ID for maintainability
			//==============================================================================================================
			string[] arrFragments = null;
			
			// Do we have a PROGRAM-ID
			if (string.IsNullOrEmpty(modMain.ctCodeTracker.ProgramId)&& Regex.IsMatch(CodeLine, "\\bPROGRAM-ID\\b\\s*\\w+"))
			{
				
				// Record the ID
				arrFragments = Regex.Split(CodeLine, "\\bPROGRAM-ID\\b\\s*");
				modMain.ctCodeTracker.ProgramId = System.Convert.ToString(arrFragments.Last().Trim());
				
				// Does the ID match the filename
				if (!Regex.IsMatch(FileName.ToLower(), "(\\\\|\\/)" + modMain.ctCodeTracker.ProgramId + "\\.{w}3"))
				{
					frmMain.Default.ListCodeIssue("Filename Does Not Match PROGRAM-ID", "The filename does not Match PROGRAM-ID which can make code more difficult to read and maintain.", FileName, CodeIssue.LOW, CodeLine);
				}
			}
			else if (!string.IsNullOrEmpty(modMain.ctCodeTracker.ProgramId)&& Regex.IsMatch(CodeLine, "\\bPROGRAM-ID\\b"))
			{
				// Report any instance of multiple IDs
				frmMain.Default.ListCodeIssue("Multiple Use of PROGRAM-ID", "The code has more than one PROGRAM-ID which can make code more difficult to read and maintain (Original ID:" + modMain.ctCodeTracker.ProgramId + ").", FileName, CodeIssue.LOW, CodeLine);
			}
			
		}
		
		private static void TrackVarAssignments(string CodeLine, string FileName)
		{
			// Track the input and allocation of user-supplied variables
			//==========================================================
			string strVar = "";
			string strCalc = "";
			string strVarCollections = "";
			
			string[] strAssignments = null;
			string[] strFragments = null;
			
			
			//== Keep track of int/short/long variables and constants to help with detection of buffer overflows, etc. ==
			if (Regex.IsMatch(CodeLine, "\\w+\\s+\\bPIC\\b"))
			{
				
				//== Check for numeric variable ==
				
				//== Check for fixed length buffers ==
				if (Regex.IsMatch(CodeLine, "\\w+\\s+\\bPIC\\b\\s*\\([A,9]*\\)"))
				{
					
				}
				
			}
			else if (Regex.IsMatch(CodeLine, "\\bACCEPT\\b\\s+\\w+"))
			{
				
				//== Track user-controlled variables ==
				strVar = System.Convert.ToString(modMain.GetLastItem(CodeLine).TrimEnd(".".ToCharArray()));
				if (Regex.IsMatch(strVar, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.InputVars.Contains(strVar))
				{
					modMain.ctCodeTracker.InputVars.Add(strVar);
				}
				
			}
			else if (Regex.IsMatch(CodeLine, "\\bCOMPUTE\\b\\s+\\w+\\s*=\\s*\\w+"))
			{
				
				// If a new variable is allocated then check if it is derived from any user-supplied variables
				strAssignments = Regex.Split(CodeLine, "\\bCOMPUTE\\b\\s+\\w+\\s*=");
				strVarCollections = System.Convert.ToString(strAssignments.Last().Trim());
				
				if (string.IsNullOrEmpty(strVarCollections))
				{
					return;
				}
				
				// Cycle though input vars to see if they're on the right side of the = sign
				foreach (var strItem in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(strVarCollections, "\\b" + System.Convert.ToString(strItem) + "\\b"))
					{
						// If we have an input var then extract the left of the = sign
						strFragments = Regex.Split(CodeLine, "=");
						strVar = modMain.GetLastItem(strFragments.First());
						strCalc = System.Convert.ToString(strFragments.Last().Trim());
						if (strCalc.Contains(System.Convert.ToString(strItem)))
						{
							if (Regex.IsMatch(strVar, "^[a-zA-Z0-9_]*$") && !modMain.ctCodeTracker.InputVars.Contains(strVar))
							{
								modMain.ctCodeTracker.InputVars.Add(strVar);
							}
						}
						break;
					}
				}
				
			}
			
		}
		
		private static void CheckBuffer(string CodeLine, string FileName)
		{
			// Keep record of pointer/buffer assignments and add to the CodeTracker dictionary for checking
			// Check any pointer and buffer manipulation fo overflows, out-of-bounds reads, etc.
			//=============================================================================================
			
			if (Regex.IsMatch(CodeLine, "\\bPOINTER\\b"))
			{
				
			}
			else if (Regex.IsMatch(CodeLine, "\\bSET\\b"))
			{
				
			}
			else if (Regex.IsMatch(CodeLine, "\\bMOVE\\b\\s+\\w+\\s+\\bTO\\b\\s+\\w+"))
			{
				
			}
			else if (Regex.IsMatch(CodeLine, "\\bMOVE\\b\\s+\\w+\\s*\\(\\s*\\w+\\s*\\:\\s*\\w+\\s*\\)\\s*\\bTO\\b\\s+\\w+"))
			{
				
			}
			else if (Regex.IsMatch(CodeLine, "\\bMOVE\\b\\s+\\w+\\s*\\bTO\\b\\s+\\w+\\s*\\(\\s*\\w+\\s*\\:\\s*\\w+\\s*\\)"))
			{
				
			}
			else if (Regex.IsMatch(CodeLine, "\\bMOVE\\b\\s+('|\")\\w+('|\")\\s*\\bTO\\b\\s+\\w+"))
			{
				
			}
			
		}
		
		private static void CheckSigned(string CodeLine, string FileName)
		{
			// Keep record of unsigned int assignments and add to CodeTracker dictionary
			// Identify any signed/unsigned comparisons
			//==========================================================================
			
			//== Identify any unsigned integers ==
			if (Regex.IsMatch(CodeLine, "\\bUNSIGNED\\b"))
			{
				modMain.ctCodeTracker.AddUnsigned(CodeLine);
			}
			
			//== Check for signed/unsigned integer comparisons ==
			if (CodeLine.Contains("=") || CodeLine.Contains("<") || CodeLine.Contains(">"))
			{
				if (System.Convert.ToBoolean(modMain.ctCodeTracker.CheckSignedComp(CodeLine)))
				{
					frmMain.Default.ListCodeIssue("Signed/Unsigned Comparison", "The code appears to compare a signed numeric value with an unsigned numeric value. This behaviour can return unexpected results as negative numbers will be forcibly cast to large positive numbers.", FileName, CodeIssue.HIGH, CodeLine);
				}
			}
			
		}
		
		public static void AddUnsigned(string CodeLine)
		{
			// Take the variable name and its details and add them to the list
			//================================================================
			string strDescription = "";
			string strVarType = "unsigned";
			
		}
		
		private static void CheckInputValidation(string CodeLine, string FileName)
		{
			
		}
		
		private static void CheckFileAccess(string CodeLine, string FileName)
		{
			// Check whether user input is being used to open files
			//=====================================================
			string strVar = "";
			
			if (Regex.IsMatch(CodeLine, "\\bOPEN\\b\\s+\\w+"))
			{
				// Cycle though input vars to see if they're on the right side of the = sign
				foreach (var strItem in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, "\\bOPEN\\b\\s+\\w*" + System.Convert.ToString(strItem)))
					{
						frmMain.Default.ListCodeIssue("User Controlled File/Directory Name", "The code uses a user-controlled value when opening a file/directory. Manually inspect the code to ensure safe usage.", FileName, CodeIssue.LOW, CodeLine);
						break;
					}
				}
			}
			
		}
		
		private static void CheckLogDisplay(string CodeLine, string FileName)
		{
			// Check output written to logs is sanitised first and check for logged passwords
			//===============================================================================
			string strLogCodeLine = CodeLine;
			
			strLogCodeLine = strLogCodeLine.ToLower();
			
			if (Regex.IsMatch(strLogCodeLine, "validate|encode|sanitize|sanitise") && !strLogCodeLine.Contains("password"))
			{
				return;
			}
			
			if (Regex.IsMatch(CodeLine, "logerror|logger|logging|\\blog\\b") && CodeLine.Contains("password"))
			{
				if (strLogCodeLine.IndexOf("log") + 1 < strLogCodeLine.IndexOf("password") + 1)
				{
					frmMain.Default.ListCodeIssue("Application Appears to Log User Passwords", "The application appears to write user passwords to logfiles creating a risk of credential theft.", FileName, CodeIssue.HIGH, CodeLine);
				}
			}
			else if (Regex.IsMatch(strLogCodeLine, "logerror|logger|logging|\\blog\\b"))
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
		
		private static void CheckFileRace(string CodeLine, string FileName)
		{
			// Check for potential TOCTOU/race conditions
			//===========================================
			
			int intSeverity = 0; // For TOCTOU vulns, severity will be modified according to length of time between check and usage.
			
			
			//== Check for TOCTOU (Time Of Check, Time Of Use) vulnerabilities==
			if ((!modMain.ctCodeTracker.IsLstat) && (Regex.IsMatch(CodeLine, "\\bCALL\\b\\s+'CBL_CHECK_FILE_EXIST'")))
			{
				// Check has taken place - begin monitoring for use of the file/dir
				modMain.ctCodeTracker.IsLstat = true;
			}
			else if (modMain.ctCodeTracker.IsLstat)
			{
				// Increase line count while monitoring
				if (CodeLine.Trim() != "")
				{
					modMain.ctCodeTracker.TocTouLineCount++;
				}
				
				if (modMain.ctCodeTracker.TocTouLineCount < 2 && Regex.IsMatch(CodeLine, "\\bOPEN\\b"))
				{
					// Usage takes place almost immediately so no problem
					modMain.ctCodeTracker.IsLstat = false;
				}
				else if (modMain.ctCodeTracker.TocTouLineCount > 1 && Regex.IsMatch(CodeLine, "\\bOPEN\\b"))
				{
					// Usage takes place sometime later. Set severity accordingly and notify user
					modMain.ctCodeTracker.IsLstat = false;
					if (modMain.ctCodeTracker.TocTouLineCount > 5)
					{
						intSeverity = 2;
					}
					frmMain.Default.ListCodeIssue("Potential TOCTOU (Time Of Check, Time Of Use) Vulnerability", "The check for the file's existence occurs " + System.Convert.ToString(modMain.ctCodeTracker.TocTouLineCount) + " lines before the file/directory is accessed. The longer the time between the check and the OPEN call, the greater the likelihood that the check will no longer be valid.", FileName);
				}
			}
			
		}
		
		private static void CheckRandomisation(string CodeLine, string FileName)
		{
			// Check for any random functions that are not cryptographically secure
			//=====================================================================
			
			//== Check for unsafe functions ==
			if (Regex.IsMatch(CodeLine, "\\bRANDOM\\b"))
			{
				frmMain.Default.ListCodeIssue("Use of Deterministic Pseudo-Random Values", "The code appears to use the RANDOM function. The resulting values, while appearing random to a casual observer, are predictable and may be enumerated by a skilled and determined attacker.", FileName, CodeIssue.STANDARD, CodeLine);
			}
			
		}
		
		private static void CheckUnsafeTempFiles(string CodeLine, string FileName)
		{
			// Check for attempts to open temp files with obvious names
			//=========================================================
			
			//== Check for unsafe functions ==
			if (Regex.IsMatch(CodeLine, "\\bOPEN\\b\\s+\\w+\\s+\\S*temp"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker or other application creates a file with the same name between the application's creation and attempted usage).", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		private static void CheckDynamicCall(string CodeLine, string FileName)
		{
			// Check for dynamic function calls
			//=================================
			bool blnIsFound = false;
			
			
			//== Check for sanitisation of variables ==
			//If Regex.IsMatch(CodeLine, "validate|encode|sanitize|sanitise") Then Exit Sub
			
			//== Check for function calls ==
			if (Regex.IsMatch(CodeLine, ".\\bCALL\\b\\s+('|\")\\w+('|\")\\s+\\bUSING\\b"))
			{
				
				// If it's a static call check for user-supplied arguments
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (CodeLine.Contains(System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("User Controlled Variable Used as Parameter for Application Call", "The code appears to allow the use of an unvalidated user-controlled variable when executing an application call: " + System.Convert.ToString(strVar) +". Manually check to ensure the parameter is used safely.", FileName, CodeIssue.LOW, CodeLine);
					}
				}
				
			}
			else if (Regex.IsMatch(CodeLine, ".\\bCALL\\b\\s+\\w+\\s+\\bUSING\\b"))
			{
				
				// If it's a dynamic function call check for user controlled function name
				foreach (var strVar in modMain.ctCodeTracker.InputVars)
				{
					if (Regex.IsMatch(CodeLine, ".\\bCALL\\b\\s+" + System.Convert.ToString(strVar) + "\\s+\\bUSING\\b"))
					{
						frmMain.Default.ListCodeIssue("User Controlled Variable Used for Dynamic Function Call", "The code appears to allow the use of an unvalidated user-controlled variable when executing a dynamic application call.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false)
				{
					frmMain.Default.ListCodeIssue("Dynamic Function Call", "The code appears to allow the use of an unvalidated variable when executing a dynamic application call. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
		}
		
	}
	
}
