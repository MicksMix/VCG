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
	sealed class modCppCheck
	{
		
		// Specific checks for C++ code
		//=============================
		
		public static void CheckCPPCode(string CodeLine, string FileName)
		{
			// Carry out any specific checks for the language in question
			//
			// else-ifs have been avoided throughout due to C-programmers' tendency to efficiently
			// cram multiple functions into one line in one way or another.
			//====================================================================================
			
			TrackVarAssignments(CodeLine, FileName); // Check for matching new/delete, etc.
			TrackUserVarAssignments(CodeLine, FileName); // Track any variables which are passed in on the command line, from files, etc.
			CheckBuffer(CodeLine, FileName); // Track buffer sizes and check for overflows
			CheckDestructorThrow(CodeLine, FileName); // Identify entry to class destructor, report any exception throw within destructor
			CheckRace(CodeLine, FileName); // Check for race conditions and TOCTOU vulns
			CheckPrintF(CodeLine, FileName); // Check for printf format string vulnerabilities
			CheckUnsafeTempFiles(CodeLine, FileName); // Check for static/obvious filenames for temp files
			CheckReallocFailure(CodeLine, FileName); // Check for 'free' on failure
			CheckUnsafeSafe(CodeLine, FileName); // Check unsafe use of return values from 'safe' functions
			CheckCmdInjection(CodeLine, FileName); // Check for potential command injection
			
			//== Beta functionality ==
			if (modMain.asAppSettings.IncludeSigned)
			{
				CheckSigned(CodeLine, FileName); // Check for signed/unsigned integer comparisons
			}
			
		}
		
		private static void TrackVarAssignments(string CodeLine, string FileName)
		{
			// Keep record of malloc/new and track matching free and delete
			// Mismatches and potential errors will be added to the CodeTracker dictionary
			//============================================================================
			
			//== Track 'malloc', 'new', etc. ==
			if (CodeLine.Contains("malloc ") || CodeLine.Contains("malloc("))
			{
				modMain.ctCodeTracker.AddMalloc(CodeLine, FileName);
				
				//== Check for a 'fixed' malloc using numeric value instead of data type ==
				if (Regex.IsMatch(CodeLine, "\\b(malloc|xmalloc)\\b\\s*\\(\\s*\\d+\\s*\\)"))
				{
					frmMain.Default.ListCodeIssue("malloc( ) Using Fixed Value Instead of Variable Type Size", "The code uses a fixed value for malloc instead of the variable type size which is dependent on the platform (e.g. sizeof(int) instead of '4'). This can result in too much or too little memory being assigned with unpredicatble results such as performance impact, overflows or memory corruption.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
			if (CodeLine.Contains("new ") || CodeLine.Contains("new("))
			{
				modMain.ctCodeTracker.AddNew(CodeLine, FileName);
			}
			
			//== Check for matching 'free', 'delete', etc. ==
			if (CodeLine.Contains("free ") || CodeLine.Contains("free("))
			{
				modMain.ctCodeTracker.AddFree(CodeLine, FileName);
			}
			if (CodeLine.Contains("delete ") || CodeLine.Contains("delete("))
			{
				modMain.ctCodeTracker.AddDelete(CodeLine, FileName);
			}
			
		}
		
		private static void TrackUserVarAssignments(string CodeLine, string FileName)
		{
			// Keep record of user-controlled variables
			//=========================================
			string[] arrFragments = null;
			string strLeft = "";
			
			
			//== Track assignments from argv, system variables, ini files or registry ==
			if ((Regex.IsMatch(CodeLine, "\\w+\\s*\\=\\s*\\bargv\\b\\s*\\[")) || (Regex.IsMatch(CodeLine, "\\w+\\s*\\=\\s*\\b(getenv|GetPrivateProfileString|GetPrivateProfileInt)\\b\\s*\\(")) || (Regex.IsMatch(CodeLine, "\\w+\\s*\\=\\s*Registry\\:\\:\\w+\\-\\>OpenSubKey")))
			{
				// Extract the variable name
				arrFragments = CodeLine.Split("=".ToCharArray());
				strLeft = modMain.GetLastItem(arrFragments.First());
			}
			
			//== Store any discovered variables
			if (!string.IsNullOrEmpty(strLeft))
			{
				if (!modMain.ctCodeTracker.UserVariables.Contains(strLeft))
				{
					modMain.ctCodeTracker.UserVariables.Add(strLeft);
				}
			}
			
		}
		
		private static void CheckBuffer(string CodeLine, string FileName)
		{
			// Keep record of integer assignments and char arrays
			// Add to the CodeTracker dictionary for checking
			//===================================================
			string[] arrFragments = null;
			string strLeft = "";
			
			
			//== Keep track of int/short/long variables and constants to help with detection of buffer overflows, etc. ==
			//== Check for assignment and check it's not an array ==
			if (CodeLine.Contains("=") && !(CodeLine.Contains("==") || CodeLine.Contains("*") || CodeLine.Contains("[")) && (Regex.IsMatch(CodeLine, "\\b(short|int|long|uint16|uint32|size_t|UINT|INT|LONG)\\b")))
			{
				modMain.ctCodeTracker.AddInteger(CodeLine);
			}
			else if (Regex.IsMatch(CodeLine, "\\s*\\w+\\s*\\=") && !CodeLine.Contains("=="))
			{
				arrFragments = CodeLine.Split("=".ToCharArray());
				strLeft = modMain.GetLastItem(arrFragments.First());
				//For Each itmItem In ctCodeTracker.GetIntegers
				if (modMain.ctCodeTracker.GetIntegers().ContainsKey(strLeft))
				{
					modMain.ctCodeTracker.AddInteger(CodeLine);
					//Exit For
				}
				//Next
			}
			
			//== Track fixed buffers, char pointers, etc. to check for overflows (avoid recording any arrays of pointers) ==
			if (Regex.IsMatch(CodeLine, "\\b(char|TCHAR|BYTE)\\b") && CodeLine.Contains("[") && CodeLine.Contains("]"))
			{
				modMain.ctCodeTracker.AddBuffer(CodeLine);
			}
			if (Regex.IsMatch(CodeLine, "\\b(char|TCHAR|BYTE)\\b") && CodeLine.Contains("*"))
			{
				modMain.ctCodeTracker.AddCharStar(CodeLine);
			}
			
			//== Check strcpy for potential buffer overflows, using buffer list ==
			//If CodeLine.Contains("strcpy") Or CodeLine.Contains("strcat") Or CodeLine.Contains("strncpy") Or CodeLine.Contains("strncat") Or CodeLine.Contains("sprintf") Or CodeLine.Contains("memcpy") Or CodeLine.Contains("memmove") Then ctCodeTracker.CheckOverflow(CodeLine, FileName)
			if (Regex.IsMatch(CodeLine, "\\b(strcpy|strlcpy|strcat|strlcat|strncpy|strncat|sprintf|memcpy|memmove)\\b"))
			{
				modMain.ctCodeTracker.CheckOverflow(CodeLine, FileName);
			}
			
		}
		
		private static void CheckSigned(string CodeLine, string FileName)
		{
			// Keep record of unsigned int assignments and add to CodeTracker dictionary
			// Identify any signed/unsigned comparisons
			//==========================================================================
			
			//== Identify any unsigned integers ==
			if (Regex.IsMatch(CodeLine, "\\b(unsigned|UNSIGNED|size_t|uint16|uint32)\\b"))
			{
				modMain.ctCodeTracker.AddUnsigned(CodeLine);
			}
			
			//== Check for signed/unsigned integer comparisons ==
			if ((CodeLine.Contains("==") || CodeLine.Contains("!=") || CodeLine.Contains("<") || CodeLine.Contains(">")) && !(CodeLine.Contains("->") || CodeLine.Contains(">>") || CodeLine.Contains("<<") || CodeLine.Contains("<>")) && !Regex.IsMatch(CodeLine, "\\<\\s*\\w+\\s*\\>") && !Regex.IsMatch(CodeLine, "\\<\\s*\\w+\\s*\\w+\\s*\\>"))
			{
				if (System.Convert.ToBoolean(modMain.ctCodeTracker.CheckSignedComp(CodeLine)))
				{
					frmMain.Default.ListCodeIssue("Signed/Unsigned Comparison", "The code appears to compare a signed numeric value with an unsigned numeric value. This behaviour can return unexpected results as negative numbers will be forcibly cast to large positive numbers.", FileName, CodeIssue.HIGH, CodeLine);
				}
			}
			
		}
		
		private static void CheckUnsafeSafe(string CodeLine, string FileName)
		{
			// Check for use of dubious return values from'safe' functions
			//============================================================
			
			//== Identify any returned values being assigned to variables ==
			if (Regex.IsMatch(CodeLine, "w+\\s*\\=\\s*\\b(snprintf|strlcpy|strlcat|strlprintf|std_strlcpy|std_strlcat|std_strlprintf)\\b"))
			{
				frmMain.Default.ListCodeIssue("Potential Misuse of Safe Function", "The code appears to assign the return value of a 'safe' function to a variable. This value represents the amount of bytes that the function attempted to write, not the amount actually written. Any use of this value for pointer arithmetic similar operations may result in memory corruption", FileName, CodeIssue.HIGH, CodeLine);
			}
			
		}
		
		private static void CheckDestructorThrow(string CodeLine, string FileName)
		{
			// Identify entry and exit points of destructor in CodeTracker
			// Report any exception throw within destructor
			//============================================================
			bool blnHasCheckedBraces = false;
			
			//== Check for entry to/exit from destructor ==
			if (modMain.ctCodeTracker.IsDestructor == false && ((CodeLine.Contains("::~") || CodeLine.Contains(":: ~") || CodeLine.Contains(" ~")) && !CodeLine.Contains(";")))
			{
				modMain.ctCodeTracker.DestructorBraces = 0;
				if (CodeLine.Contains("{"))
				{
					modMain.ctCodeTracker.IsDestructor = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.DestructorBraces);
					blnHasCheckedBraces = true;
				}
				else
				{
					modMain.ctCodeTracker.IsDestructor = true;
				}
			}
			
			//== Check for any exceptions while in destructor ==
			if (modMain.ctCodeTracker.IsDestructor == true)
			{
				if (CodeLine.Contains("throw") && modMain.ctCodeTracker.DestructorBraces > 0)
				{
					frmMain.Default.ListCodeIssue("Exception Throw in Destructor", "Throwing an exception causes an exit from the function and should not be carried out in a class destructor as it prevents memory from being safely deallocated. If the destructor is being called due to an exception thrown elsewhere in the application this will result in unexpected termination of the application with possible loss or corruption of data.", FileName);
				}
				if (!blnHasCheckedBraces)
				{
					modMain.ctCodeTracker.IsDestructor = modMain.ctCodeTracker.TrackBraces(CodeLine, ref modMain.ctCodeTracker.DestructorBraces);
				}
			}
			
		}
		
		private static void CheckRace(string CodeLine, string FileName)
		{
			// Check for potential TOCTOU/race conditions
			//===========================================
			
			int intSeverity = 0; // For TOCTOU vulns, severity will be modified according to length of time between check and usage.
			
			
			//== Check for TOCTOU (Time Of Check, Time Of Use) vulnerabilities==
			if ((!modMain.ctCodeTracker.IsLstat) && (CodeLine.Contains(" lstat(") || CodeLine.Contains(" lstat ") || CodeLine.Contains(" stat(") || CodeLine.Contains(" stat ")) && ((!CodeLine.Contains("fopen")) && (!CodeLine.Contains("opendir"))))
			{
				// Check has taken place - begin monitoring for use of the file/dir
				modMain.ctCodeTracker.IsLstat = true;
			}
			else if (modMain.ctCodeTracker.IsLstat)
			{
				// Increase line count while monitoring
				modMain.ctCodeTracker.TocTouLineCount++;
				if (modMain.ctCodeTracker.TocTouLineCount < 2 && (CodeLine.Contains("fopen") || CodeLine.Contains("opendir")))
				{
					// Usage takes place almost immediately so no problem
					modMain.ctCodeTracker.IsLstat = false;
				}
				else if (modMain.ctCodeTracker.TocTouLineCount > 1 && (CodeLine.Contains("fopen") || CodeLine.Contains("opendir")))
				{
					// Usage takes place sometime later. Set severity accordingly and notify user
					modMain.ctCodeTracker.IsLstat = false;
					if (modMain.ctCodeTracker.TocTouLineCount > 5)
					{
						intSeverity = 2;
					}
					frmMain.Default.ListCodeIssue("Potential TOCTOU (Time Of Check, Time Of Use) Vulnerability", "The lstat() check occurs " + System.Convert.ToString(modMain.ctCodeTracker.TocTouLineCount) + " lines before fopen() is called. The longer the time between the check and the fopen(), the greater the likelihood that the check will no longer be valid.", FileName);
				}
			}
			
		}
		
		private static void CheckPrintF(string CodeLine, string FileName)
		{
			// Check for printf format string vulnerabilities
			//===============================================
			
			if (Regex.IsMatch(CodeLine, "\\bprintf\\b\\s*\\(\\s*\\w+\\s*\\)") && !CodeLine.Contains(",") && !CodeLine.Contains("\""))
			{
				frmMain.Default.ListCodeIssue("Possible printf( ) Format String Vulnerability", "The call to printf appears to be printing a variable directly to standard output. Check whether this variable can be controlled or altered by the user to determine whether a format string vulnerability exists.", FileName, CodeIssue.HIGH, CodeLine);
			}
			
		}
		
		private static void CheckUnsafeTempFiles(string CodeLine, string FileName)
		{
			// Identify any creation of temp files with static names
			//======================================================
			
			if (Regex.IsMatch(CodeLine, "\\=\\s*(_open|open|fopen|opendir)\\s*\\(\\s*\\\"*\\S*(temp|tmp)\\S*\\\"\\s*\\,\\s*\\S*\\s*\\)"))
			{
				frmMain.Default.ListCodeIssue("Unsafe Temporary File Allocation", "The application appears to create a temporary file with a static, hard-coded name. This causes security issues in the form of a classic race condition (an attacker creates a file with the same name between the application's creation and attempted usage) or a symbolic link attack where an attacker creates a symbolic link at the temporary file location.", FileName, CodeIssue.MEDIUM, CodeLine);
			}
			
		}
		
		private static void CheckReallocFailure(string CodeLine, string FileName)
		{
			// Identify any attempts to resize buffers that do not clear the buffer on failure
			//================================================================================
			string[] arrFragments = null;
			string strBuffer = "";
			string strDestination = "";
			
			
			//== Identify occurences of realloc ==
			if (Regex.IsMatch(CodeLine, "\\brealloc\\b\\s*\\("))
			{
				//== Extract variable names ==
				arrFragments = Regex.Split(CodeLine, "\\=\\s*\\brealloc\\b\\s*\\(");
				if (arrFragments.Count() < 2)
				{
					return;
				}
				
				//== Make sure we have the variable name and nothing else ==
				if (arrFragments.First().Contains("("))
				{
					strDestination = modMain.GetLastItem(arrFragments.First(), "(");
					strDestination = modMain.GetLastItem(strDestination);
				}
				else
				{
					strDestination = modMain.GetLastItem(arrFragments.First());
				}
				
				if (!string.IsNullOrEmpty(strDestination))
				{
					strBuffer = modMain.GetFirstItem(arrFragments[1], ",");
					if (strDestination == strBuffer)
					{
						frmMain.Default.ListCodeIssue("Dangerous Use of realloc( )", "The source and destination buffers are the same. A failure to resize the buffer will set the pointer to NULL, possibly causing unpredicatable behaviour.", FileName, CodeIssue.MEDIUM, CodeLine);
					}
					strDestination = System.Convert.ToString(strDestination.TrimStart("*".ToCharArray()).TrimStart());
					strBuffer = System.Convert.ToString(strBuffer.TrimStart("*".ToCharArray()).TrimStart());
					
					modMain.ctCodeTracker.DestinationBuffer = strDestination;
					modMain.ctCodeTracker.SourceBuffer = strBuffer;
				}
				
			}
			else if (!string.IsNullOrEmpty(modMain.ctCodeTracker.DestinationBuffer))
			{
				
				if (Regex.IsMatch(modMain.ctCodeTracker.DestinationBuffer, "(\\(|\\)|\\[|\\])") || Regex.IsMatch(modMain.ctCodeTracker.SourceBuffer, "(\\(|\\)|\\[|\\])"))
				{
					modMain.ctCodeTracker.DestinationBuffer = "";
					modMain.ctCodeTracker.SourceBuffer = "";
					return;
				}
				if (Regex.IsMatch(CodeLine, "\\bfree\\b\\s*\\(\\s*(" + modMain.ctCodeTracker.DestinationBuffer + "|" + modMain.ctCodeTracker.SourceBuffer + ")"))
				{
					modMain.ctCodeTracker.DestinationBuffer = "";
					modMain.ctCodeTracker.SourceBuffer = "";
				}
				else if (Regex.IsMatch(CodeLine, "(break|return|exit)"))
				{
					frmMain.Default.ListCodeIssue("Potential Memory Leak", "On failure, the realloc function returns a NULL pointer but leaves memory allocated. The code should be modified to free the memory if NULL is returned.", FileName, CodeIssue.MEDIUM, CodeLine);
					modMain.ctCodeTracker.DestinationBuffer = "";
					modMain.ctCodeTracker.SourceBuffer = "";
				}
				
			}
			
		}
		
		private static void CheckCmdInjection(string CodeLine, string FileName)
		{
			// Check for potential command injection
			//======================================
			bool blnIsFound = false;
			
			
			//== Are commands being passed to system? ==
			if (Regex.IsMatch(CodeLine, "\\b(system|popen|execlp)\\b\\s*\\("))
			{
				
				//== Is a user-controlled variable present? ==
				foreach (var strVar in modMain.ctCodeTracker.UserVariables)
				{
					if (CodeLine.Contains(System.Convert.ToString(strVar)))
					{
						frmMain.Default.ListCodeIssue("User Controlled Variable Used on System Command Line", "The application appears to allow the use of an unvalidated user-controlled variable [" + strVar + "] when executing a system command.", FileName, CodeIssue.HIGH, CodeLine);
						blnIsFound = true;
						break;
					}
				}
				if (blnIsFound == false && (Regex.IsMatch(CodeLine, "\\b(system|popen|execlp)\\b\\s*\\(\\s*\\bgetenv\\b")))
				{
					//== Is a system variable present? ==
					frmMain.Default.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated system variable when executing a system command.", FileName, CodeIssue.HIGH, CodeLine);
				}
				else if (blnIsFound == false && ((!CodeLine.Contains("\"")) || (CodeLine.Contains("\"") && CodeLine.Contains("+")) || (Regex.IsMatch(CodeLine, "\\b(system|popen|execlp)\\b\\s*\\(\\s*\\b(strcat|strncat)\\b"))))
				{
					//== Is an unidentified variable present? ==
					frmMain.Default.ListCodeIssue("Application Variable Used on System Command Line", "The application appears to allow the use of an unvalidated variable when executing a system command. Carry out a manual check to determine whether the variable is user-controlled.", FileName, CodeIssue.MEDIUM, CodeLine);
				}
			}
			
		}
		
	}
	
}
