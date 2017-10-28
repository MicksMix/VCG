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
	
	public class CodeTracker
	{
		// Stores details around the current code block in order to
		// facilitate the location of mismatched 'malloc'-'dealloc' and
		// other issues which require checks from multiple lines
		// throughout the file.
		//===============================================================
		
		
		//==================================================
		// Constants to identify usages of sizeof in strncpy
		//--------------------------------------------------
		private const int MISCALC_SIZE_OF = -1;
		private const int OFF_BY_ONE_SIZE_OF = -2;
		private const int SOURCE_SIZE_OF = -3;
		private const int DEST_SIZE_OF = -4;
		//==================================================
		
		
		//========================================================================
		// Constants to identify types of buffer comparison for overflow detection
		//------------------------------------------------------------------------
		private const int NO_DANGER = 0;
		private const int POINTER_INTO_ARRAY = 1;
		private const int CMDLINE_INTO_ARRAY = 2;
		private const int SOURCE_LARGER_THAN_DEST = 3;
		private const int WRONG_LIMIT_NO_DANGER = 4;
		private const int OFF_BY_ONE = 5;
		private const int SOURCE_LIMIT = 6;
		private const int STRNCAT_MISUSE = 7;
		//========================================================================
		
		
		//========================================
		// C++ Details
		// Used for tracking details of the C code
		//----------------------------------------
		public bool IsDestructor = false;
		public bool IsLstat = false;
		
		public int DestructorBraces = 0;
		public int TocTouLineCount = 0;
		
		public string DestinationBuffer = ""; // Keep track of this to determnine correct error checking for realloc
		public string SourceBuffer = ""; // Keep track of this to determnine correct error checking for realloc
		
		public ArrayList UserVariables = new ArrayList(); // List of user-controlled varaibles from commandline
		
		private Dictionary<string, string> dicMemAssign = new Dictionary<string, string>(); // Dictionary of instances of new/malloc
		private Dictionary<string, string> dicBuffer = new Dictionary<string, string>(); // Dictionary of fixed buffers
		private Dictionary<string, string> dicInteger = new Dictionary<string, string>(); // Dictionary of integer assignments to help identify any buffer sizes
		private Dictionary<string, string> dicUnsigned = new Dictionary<string, string>(); // Dictionary of any unsigned integers to help identify signed/unsigned comparisons
		//=======================================
		
		
		//===========================================
		// Java details
		// Used for tracking details of the Java code
		//-------------------------------------------
		public bool IsServlet = false;
		public bool IsRuntime = false;
		public bool ImplementsClone = false;
		public bool IsSerialize = false;
		public bool IsDeserialize = false;
		public bool HasValidator = false;
		public bool HasVulnSQLString = false;
		public bool HasHttpRequestData = false;
		public bool IsInsideClass = false;
		public bool HasGetVariables = false;
		public bool IsSynchronized = false;
		public bool IsInsideMethod = false;
		public bool IsPrivileged = false;
		public bool HasXXEEnabled = false;
		public bool HasPrimitives = false;
		public bool IsFileOpen = false;
		public bool HasTry = false;
		public bool HasResourceRelease = true;
		public bool HasIntent = false;
		
		public int SerializeBraces = 0;
		public int DeserializeBraces = 0;
		public int ClassBraces = 0;
		public int SyncBraces = 0;
		public int MethodBraces = 0;
		public int PrivBraces = 0;
		public int SyncLineCount = 0;
		public int SyncIndex = 0;
		public int PrivLineCount = 0;
		public int FileOpenLine = 0;
		
		public string HttpRequestVar = "";
		public string ServletName = "";
		
		public ArrayList GetVariables = new ArrayList();
		public ArrayList SQLStatements = new ArrayList();
		public ArrayList PrivateInstanceVars = new ArrayList();
		public ArrayList ServletNames = new ArrayList(); // The list of servlet class names
		public ArrayList SyncBlockObjects = new ArrayList(); // List of locked object names and any inner locked objects in nested synchronized blocks
		public ArrayList AndroidIntents = new ArrayList(); // Android intents, stored to determine whether used explicitly or implicitly
		
		public Dictionary<string, string> ServletInstances = new Dictionary<string, string>(); // Maps each Servlet object onto its class name
		private Dictionary<string, string> dicStatic = new Dictionary<string, string>(); // Instances of static variables to look for non-threadsafe operations
		//===========================================
		
		
		//==============================================
		// PL/SQL details
		// Used for tracking details of the PL/SQL code
		//----------------------------------------------
		public bool IsOracleEncrypt = false;
		public bool IsAutonomousProcedure = false;
		public bool IsView = false;
		public bool IsNewPackage = false;
		public bool IsAuth = false;
		public bool IsInsideSQLVarDec = false;
		public bool IsInsidePlSqlExecuteStmt = false;
		public bool IsInsideProcDec = false;
		
		public string CurrentVar = "";
		//==============================================
		
		
		//=========================================
		// C# details
		// Used for tracking details of the C# code
		//-----------------------------------------
		public bool HasSeed = false;
		public bool IsSamlFunction = false;
		public bool IsSerializable = false;
		public bool IsSerializableClass = false;
		public bool IsUnsafe = false;
		
		public int UnsafeBraces = 0;
		
		public ArrayList InputVars = new ArrayList();
		public ArrayList CookieVals = new ArrayList();
		public ArrayList AspLabels = new ArrayList();
		//=========================================
		
		
		//=========================================
		// PHP details
		// Used for tracking details of the PHP code
		//-----------------------------------------
		public bool IsRegisterGlobals = false;
		public bool IsArrayMerge = false;
		public string GlobalArrayName = "";
		public bool HasDisableFunctions = false;
		//=========================================
		
		//============================================
		// COBOL details
		// Used for tracking details of the COBOL code
		//--------------------------------------------
		public string ProgramId = "";
		//============================================
		
		//===================================================
		// Used in C++, C# and Java scans to handle thread issues
		//---------------------------------------------------
		public string LockedObject = "";
		public Dictionary<string, string> GlobalVars = new Dictionary<string, string>();
		public Dictionary<string, string> InstanceVars = new Dictionary<string, string>();
		public Dictionary<string, string> GetSetMethods = new Dictionary<string, string>();
		//===================================================
		
		
		//RegEx object for parsing the more complex C expressions, etc.
		//=============================================================
		Regex reRegex;
		
		
		public CodeTracker()
		{
			//Initialise variables
			//====================
			
			Reset();
			ResetCDictionaries();
			
		}
		
		public void Reset()
		{
			// Reset/empty all variables in preparation for scanning a code module
			//====================================================================
			
			// Java details
			IsServlet = false;
			ImplementsClone = false;
			IsRuntime = false;
			IsSerialize = false;
			IsDeserialize = false;
			HasValidator = false;
			HasVulnSQLString = false;
			HasHttpRequestData = false;
			IsInsideClass = false;
			HasGetVariables = false;
			IsSynchronized = false;
			IsInsideMethod = false;
			IsPrivileged = false;
			HasXXEEnabled = false;
			HasPrimitives = false;
			IsFileOpen = false;
			HasTry = false;
			HasResourceRelease = true;
			HasIntent = false;
			
			HttpRequestVar = "";
			ServletName = "";
			LockedObject = "";
			
			SerializeBraces = 0;
			DeserializeBraces = 0;
			ClassBraces = 0;
			SyncBraces = 0;
			MethodBraces = 0;
			PrivBraces = 0;
			SyncLineCount = 0;
			SyncIndex = 0;
			PrivLineCount = 0;
			FileOpenLine = 0;
			
			GetVariables.Clear();
			SQLStatements.Clear();
			dicStatic.Clear();
			PrivateInstanceVars.Clear();
			SyncBlockObjects.Clear();
			AndroidIntents.Clear();
			
			// PL/SQL details
			IsOracleEncrypt = false;
			IsAutonomousProcedure = false;
			IsView = false;
			IsNewPackage = false;
			IsAuth = false;
			IsInsideSQLVarDec = false;
			IsInsidePlSqlExecuteStmt = false;
			IsInsideProcDec = false;
			
			CurrentVar = "";
			
			
			// C++ Details
			IsDestructor = false;
			IsLstat = false;
			
			DestructorBraces = 0;
			TocTouLineCount = 0;
			
			DestinationBuffer = "";
			SourceBuffer = "";
			
			dicMemAssign.Clear();
			dicUnsigned.Clear();
			
			
			// C# Details
			HasSeed = false;
			IsSamlFunction = false;
			IsSerializable = false;
			IsSerializableClass = false;
			IsUnsafe = false;
			
			InputVars.Clear();
			CookieVals.Clear();
			AspLabels.Clear();
			
			
			// PHP Details
			IsRegisterGlobals = false;
			IsArrayMerge = false;
			
			GlobalArrayName = "";
			
			
			// COBOL details
			ProgramId = "";
			
		}
		
		public void ResetCDictionaries()
		{
			// Reset the C++ Dictionaries which have a project-wide scope, not file-wide
			//==========================================================================
			
			dicBuffer.Clear();
			dicInteger.Clear();
			UserVariables.Clear();
			
			// Used for tracking thread issues in C++ and Java
			ServletNames.Clear();
			GlobalVars.Clear();
			GetSetMethods.Clear();
			
		}
		
		public void AddMalloc(string CodeLine, string FileName)
		{
			// Take the variable name and its details and add them to the list
			//================================================================
			string strVarName = "";
			string strDescription = "";
			string strTemp = "";
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			string[] arrVarNames = null;
			
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			foreach (var strStatement in arrStatements)
			{
				
				// The split means that we will get a final empty statement
				if (strStatement == "")
				{
					break;
				}
				
				//== Identify occurences of malloc ==
				if (strStatement.Contains("=") && strStatement.Contains("malloc"))
				{
					arrFragments = strStatement.Trim().Split("=".ToCharArray());
					
					//== Use of malloc in C++ is discouraged in favour of delete ==
					if (arrFragments[1].Contains("malloc") && FileName.EndsWith(".cpp"))
					{
						strDescription = "malloc without free. The use of malloc() and free() functions in C++ code is not recommended and can result in errors that would otherwise have been avoided with new and delete.|Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " FileName: " + FileName;
					}
					else if (arrFragments[1].Contains("malloc"))
					{
						strDescription = "malloc without free.|Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " FileName: " + FileName;
					}
					
					if (!string.IsNullOrEmpty(strDescription))
					{
						
						//== If we have a result add it to the dictionary ==
						arrVarNames = arrFragments.First().Trim().Split();
						strVarName = System.Convert.ToString(arrVarNames.Last().Trim());
						strVarName = System.Convert.ToString(strVarName.Replace("*", "").Trim());
						
						// This varname may be in the dictionary already (e.g. different functions using same local variable names)
						if (!string.IsNullOrEmpty(strVarName))
						{
							if (dicMemAssign.ContainsKey(strVarName))
							{
								dicMemAssign[strVarName] = strDescription;
							}
							else
							{
								dicMemAssign.Add(strVarName, strDescription);
							}
						}
						
					}
					
				}
			}
			
		}
		
		public void AddFree(string CodeLine, string FileName)
		{
			// Take the variable name and its details and delete them from the list
			//=====================================================================
			string strVarName = "";
			string strDescription = "";
			string strNewDescription = "";
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			string[] arrReportFragments = null;
			
			int intNumFrees = 0;
			
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			foreach (var strStatement in arrStatements)
			{
				
				// The split means that we will get a final empty statement
				if (strStatement == "")
				{
					break;
				}
				
				//== Check statement contains a 'free' and extract varname from brackets ==
				if (System.Convert.ToInt32(strStatement.IndexOf("free") + 1 < strStatement.IndexOf("(") + 1) < strStatement.IndexOf(")") + 1 )
				{
					arrFragments = strStatement.Split("(".ToCharArray());
					strVarName = System.Convert.ToString(arrFragments.Last().Trim());
					arrFragments = strVarName.Split(")".ToCharArray());
					strVarName = arrFragments.First();
					strVarName = System.Convert.ToString(strVarName.Replace("*", "").Trim());
				}
				
				
				//== If we've managed to extract a variable name then assign a description ==
				if (!string.IsNullOrEmpty(strVarName))
				{
					
					if (dicMemAssign.ContainsKey(strVarName))
					{
						strDescription = dicMemAssign[strVarName];
					}
					
					if (strDescription.Trim() == "")
					{
						// Variable not in dictionary - list the error
						strNewDescription = "1 free |Potential memory leak/system crash - free without malloc.";
					}
					else if (strDescription.Contains("new ") && strStatement.Contains(" without delete"))
					{
						// Mismatched new and free
						strNewDescription = "1 free |Potential memory leak or heap corruption - inappropriate use of new and free. Mixing of new and free operators can result in a failure to de-allocate memory as this behavior is compiler dependent and technically undefined.";
					}
					else if (strDescription.Contains(" free ") && !strDescription.Contains("without free "))
					{
						// More than one free for this variable (it has already been processed and had its description changed)
						arrReportFragments = strDescription.Split("|".ToCharArray()); // Break the description up - it has the format: "n delete| [description |] Line: n Filename: filename"
						arrFragments = arrReportFragments[0].Split();
						intNumFrees = System.Convert.ToInt32(arrFragments[0].Trim());
						intNumFrees++;
						strNewDescription = (intNumFrees).ToString() + " free |Multiple frees detected. Check code paths manually to ensure that variables cannot be freed more than once.";
						
						// Add any extra description for previously identified error
						if (arrReportFragments.Length > 1)
						{
							strDescription = strDescription + "|" + arrReportFragments[1];
						}
					}
					else
					{
						strNewDescription = "1 free ";
					}
				}
				
				//== Locate variable in dictionary and modify its description ==
				if (!string.IsNullOrEmpty(strNewDescription)&& !string.IsNullOrEmpty(strVarName)&& FileName.EndsWith(".cpp"))
				{
					dicMemAssign[strVarName] = strNewDescription + "|The use of malloc() and free() functions in C++ code is not recommended and can result in errors that would otherwise have been avoided with new and delete.|Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " FileName: " + FileName;
				}
				else if (!string.IsNullOrEmpty(strNewDescription)&& !string.IsNullOrEmpty(strVarName))
				{
					dicMemAssign[strVarName] = strNewDescription + "|Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " FileName: " + FileName;
				}
				
			}
			
		}
		
		public void AddNew(string CodeLine, string FileName)
		{
			// Take the variable name and its details and add them to the list
			//================================================================
			string strVarName = "";
			string strDescription = "";
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			string[] arrVarNames = null;
			
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			foreach (var strStatement in arrStatements)
			{
				
				// The split means that we will get a final empty statement
				if (strStatement == "")
				{
					break;
				}
				
				if (strStatement.Contains("=") && strStatement.Contains("new"))
				{
					arrFragments = strStatement.Trim().Split("=".ToCharArray());
					if (arrFragments[1].Contains("new") && arrFragments[1].Contains("["))
					{
						strDescription = "new [] without delete.|Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " FileName: " + FileName;
					}
					else if (arrFragments[1].Contains("new"))
					{
						strDescription = "new without delete.|Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " FileName: " + FileName;
					}
					
					if (!string.IsNullOrEmpty(strDescription))
					{
						//== If we have a result add it to the dictionary ==
						arrVarNames = arrFragments.First().Trim().Split();
						strVarName = System.Convert.ToString(arrVarNames.Last().Trim());
						strVarName = System.Convert.ToString(strVarName.Replace("*", "").Trim());
						
						// This varname may be in the dictionary already (e.g. different functions using same local variable names)
						if (!string.IsNullOrEmpty(strVarName))
						{
							if (dicMemAssign.ContainsKey(strVarName))
							{
								dicMemAssign[strVarName] = strDescription;
							}
							else
							{
								dicMemAssign.Add(strVarName, strDescription);
							}
						}
						
					}
				}
			}
			
		}
		
		public void AddDelete(string CodeLine, string FileName)
		{
			// Take the variable name and its details and modify the dictionary accordingly
			//=============================================================================
			string strVarName = "";
			string strDescription = "";
			string strNewDescription = "";
			
			int intNumDeletes = 0;
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			string[] arrReportFragments = null;
			
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			foreach (var strStatement in arrStatements)
			{
				
				// The split means that we will get a final empty statement
				if (strStatement == "")
				{
					break;
				}
				
				//== Get type of delete (vector or normal) and then extract variable name ==
				if (strStatement.Contains("delete") && strStatement.Contains("["))
				{
					arrFragments = strStatement.Split("[".ToCharArray());
					strVarName = System.Convert.ToString(arrFragments.Last().Trim());
					strVarName = System.Convert.ToString(strVarName.Replace("*", "").Trim());
				}
				else if (strStatement.Contains("delete "))
				{
					arrFragments = strStatement.Split(" ".ToCharArray());
					strVarName = System.Convert.ToString(arrFragments.Last().Trim());
					strVarName = System.Convert.ToString(strVarName.Replace("*", "").Trim());
				}
				
				
				//== If we've managed to extract a variable name then assign a description ==
				if (!string.IsNullOrEmpty(strVarName))
				{
					
					//== Check if the item is in the dictionary ==
					if (dicMemAssign.ContainsKey(strVarName))
					{
						strDescription = dicMemAssign[strVarName];
					}
					
					if (strDescription.Trim() == "")
					{
						// Variable not in dictionary - list the error
						strNewDescription = "1 delete |Potential memory leak/system crash - delete without new.";
					}
					else if ((strDescription.Contains("new [") && !strStatement.Contains("[")) || (!strDescription.Contains("new [") && strStatement.Contains("[")))
					{
						// Mismatched new and delete, one vector, the other scalar
						strNewDescription = "1 delete |Potential memory leak or heap corruption - mismatched new and delete. Mixing of scalar and vector operators can result in too much or too little memory being deallocated.";
					}
					else if (strDescription.Contains("malloc") && !strDescription.StartsWith("1 delete "))
					{
						// Mismatched new and delete, one vector, the other scalar
						strNewDescription = "1 delete |Potential memory leak or heap corruption - malloc should not be used in conjunction with delete. This behavior is compiler dependent and technically undefined, with no guarantee that delete will internally use free.";
					}
					else if (strDescription.Contains(" delete ") && !strDescription.Contains("without delete "))
					{
						// More than one delete for this variable (it has already been processed and had its description changed
						arrReportFragments = strDescription.Split("|".ToCharArray()); // Break the description up - it has the format: "n delete| [description |] Line: n Filename: filename"
						arrFragments = arrReportFragments[0].Split();
						intNumDeletes = System.Convert.ToInt32(arrFragments[0].Trim());
						intNumDeletes++;
						strNewDescription = (intNumDeletes).ToString() + " delete |Multiple deletes detected. Check code paths manually to ensure that variables cannot be deleted more than once.";
						
						// Add any extra description for previously identified error
						if (arrReportFragments.Length > 1)
						{
							strDescription = strDescription + "|" + arrReportFragments[1];
						}
					}
					else
					{
						strNewDescription = "1 delete ";
					}
				}
				
				//== Locate variable in dictionary and modify its description ==
				if (!string.IsNullOrEmpty(strNewDescription)&& !string.IsNullOrEmpty(strVarName))
				{
					dicMemAssign[strVarName] = strNewDescription + "|Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " FileName: " + FileName;
				}
				
			}
			
		}
		
		public void AddBuffer(string CodeLine, string BuffType = "")
		{
			// Take the variable name and its details and add them to the list
			//================================================================
			string strVarName = "";
			string strDescription = "";
			
			var blnIsMultDecs = false;
			
			string[] arrStatements = null;
			string[] arrParams = null;
			string[] arrFragments = null;
			string[] arrVarNames = null;
			string[] arrAllocations = null;
			
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			foreach (var strStatement in arrStatements)
			{
				
				// The split means that we will get a final empty statement
				if (strStatement.Trim() == "")
				{
					break;
				}
				
				// Do we have multiple char decalarations on the same line
				if (Regex.IsMatch(strStatement, "\\b" + BuffType + "\\b\\s+\\w+\\s*\\[\\s*[a-z,A-Z,0-9,_]\\s*\\]\\s*\\,\\s*(\\w+\\s*\\[|\\*\\s*\\w+)") || Regex.IsMatch(strStatement, "\\b" + BuffType + "\\b\\s*\\*\\s*\\w+\\s*\\,\\s*(\\w+\\s*\\[|\\*\\s*\\w+)"))
				{
					blnIsMultDecs = true;
				}
				else
				{
					blnIsMultDecs = false;
				}
				
				// Split on commas to take account of paramaters within variable declarations
				// or multiple declarations on same line
				arrParams = strStatement.Trim().Split(",".ToCharArray());
				foreach (var strParameter in arrParams)
				{
					
					strVarName = "";
					strDescription = "*";
					
					if (strParameter.Trim() != "")
					{
						
						//== Check statement contains a char array and extract length from brackets ==
						if (!strParameter.Contains("*") && (strParameter.IndexOf("[") + 1 < strParameter.IndexOf("]") + 1))
						{
							arrFragments = strParameter.Split("[".ToCharArray());
							
							strDescription = System.Convert.ToString(arrFragments.Last().Trim());
							arrAllocations = strDescription.Split("]".ToCharArray());
							strDescription = System.Convert.ToString(arrAllocations.First().Trim());
							
							//== Split 'char' from varname if required ==
							strVarName = System.Convert.ToString(arrFragments.First().Trim());
							arrVarNames = strVarName.Split();
							if (Regex.IsMatch(strVarName, "\\b" + BuffType + "\\b"))
							{
								strVarName = System.Convert.ToString(arrVarNames.Last().Trim());
							}
							else
							{
								strVarName = System.Convert.ToString(arrVarNames.First().Trim());
							}
							
						}
						
						//== This varname may be in the dictionary already (e.g. different functions using same local variable names) ==
						if (!string.IsNullOrEmpty(strVarName)&& !string.IsNullOrEmpty(strDescription))
						{
							if (dicBuffer.ContainsKey(strVarName))
							{
								dicBuffer[strVarName] = strDescription;
							}
							else
							{
								dicBuffer.Add(strVarName, strDescription);
							}
						}
					}
				}
			}
			
		}
		
		public void AddCharStar(string CodeLine, string BuffType = "")
		{
			// Take the variable name and its details and add them to the list
			//================================================================
			string strVarName = "";
			string strDescription = "*";
			string strTemp = "";
			
			string[] arrStatements = null;
			string[] arrParams = null;
			string[] arrFragments = null;
			
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			foreach (var strStatement in arrStatements)
			{
				
				// The split means that we will get a final empty statement
				if (strStatement.Trim() == "")
				{
					break;
				}
				
				if (Regex.IsMatch(strStatement, "\\b" + BuffType + "\\b\\s+\\w+\\s*\\[\\s*[a-z,A-Z,0-9,_]\\s*\\]\\s*\\,\\s*(\\w+\\s*\\[|\\*\\s*\\w+)") || Regex.IsMatch(strStatement, "\\b" + BuffType + "\\b\\s*\\*\\s*\\w+\\s*\\,\\s*(\\w+\\s*\\[|\\*\\s*\\w+)"))
				{
					
					//== Remove 'char' from start ==
					arrFragments = Regex.Split(strStatement, "\\b" + BuffType + "\\b\\s*");
					strVarName = System.Convert.ToString(arrFragments.Last().Trim());
					//== Extract varnames from any potential assignment statement ==
					arrFragments = strVarName.Split("=".ToCharArray());
					strVarName = System.Convert.ToString(arrFragments.First().Trim());
					// If this is a list of parameters in function declaration we need to split on commas
					arrParams = strStatement.Trim().Split(",".ToCharArray());
					foreach (var strParameter in arrParams)
					{
						
						if (Regex.IsMatch(strParameter, "\\*\\s*\\w+") && !strParameter.Contains("["))
						{
							arrFragments = strParameter.Split("*".ToCharArray());
							strVarName = System.Convert.ToString(arrFragments.Last().Trim());
							
							//== Trim any extraneous braces ==
							if (Regex.IsMatch(strVarName, "\\w+\\s*\\)"))
							{
								arrFragments = strVarName.Split(")".ToCharArray());
								strVarName = System.Convert.ToString(arrFragments.First().Trim());
							}
							
							if (dicBuffer.ContainsKey(strVarName))
							{
								dicBuffer[strVarName] = strDescription;
							}
							else
							{
								dicBuffer.Add(strVarName, strDescription);
							}
						}
						
						strVarName = "";
					}
				}
				else if (Regex.IsMatch(strStatement, "\\b" + BuffType + "\\b\\s*\\*\\s*\\w+"))
				{
					arrParams = strStatement.Trim().Split(",".ToCharArray());
					foreach (var strParameter in arrParams)
					{
						
						if (!strParameter.Contains("["))
						{
							
							//== Extract varnames from any potential assignment statement ==
							arrFragments = strVarName.Split("=".ToCharArray());
							strVarName = System.Convert.ToString(arrFragments.First().Trim());
							//== Remove 'char *' from start ==
							arrFragments = strParameter.Split("*".ToCharArray());
							strVarName = System.Convert.ToString(arrFragments.Last().Trim());
							
							//== Trim any extraneous braces ==
							if (Regex.IsMatch(strVarName, "\\w+\\s*\\)"))
							{
								arrFragments = strVarName.Split(")".ToCharArray());
								strVarName = System.Convert.ToString(arrFragments.First().Trim());
							}
							
							if (strParameter.Trim() != "")
							{
								if (dicBuffer.ContainsKey(strVarName))
								{
									dicBuffer[strVarName] = strDescription;
								}
								else
								{
									dicBuffer.Add(strVarName, strDescription);
								}
							}
							
							strVarName = "";
						}
					}
				}
			}
			
		}
		
		public void AddPointer(string CodeLine, string BuffType = "")
		{
			// Take the variable name and its details and add them to the list
			// N.B. - This function deals with COBOL pointers
			//================================================================
			
		}
		
		public void AddInteger(string CodeLine)
		{
			// Take the variable name and its details and add them to the list
			//================================================================
			string strDescription = "";
			string strTemp = ""; // used as placeholder
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			string[] arrPlaceHolders = null;
			ArrayList arlVarNames = new ArrayList();
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			
			foreach (var strStatement in arrStatements)
			{
				//== If it's a pointer or an array then don't bother ==
				if (strStatement.Contains("=") && (!strStatement.Contains("[")) && (!strStatement.Contains("*")))
				{
					
					arrFragments = strStatement.Trim().Split("=".ToCharArray());
					strDescription = System.Convert.ToString(arrFragments.Last().Trim());
					
					//== Split the statement on comma if multiple variables are defined ==
					if (arrFragments.First().Contains(","))
					{
						arrPlaceHolders = arrFragments.First().Split(',');
						foreach (var strVarName in arrPlaceHolders)
						{
							if (strVarName.Contains(" "))
							{
								// strVarName = modMain.GetLastItem(strVarName); // This will deal with the first item which will be "int varname" VBConversions Warning: A foreach variable can't be assigned to in C#.
							}
							
							//== Be careful of anything which may break the regex ==
							// strVarName = System.Convert.ToString(strVarName.TrimStart("(".ToCharArray()).Trim()); VBConversions Warning: A foreach variable can't be assigned to in C#.
							// strVarName = System.Convert.ToString(strVarName.TrimEnd(")".ToCharArray()).Trim()); VBConversions Warning: A foreach variable can't be assigned to in C#.
							
							arlVarNames.Add(strVarName);
						}
					}
					else
					{
						// Otherwise split on spaces and take last item as varname
						strTemp = modMain.GetLastItem(arrFragments.First());
						
						//== Be careful of anything which may break the regex ==
						strTemp = System.Convert.ToString(strTemp.TrimStart("(".ToCharArray()).Trim());
						strTemp = System.Convert.ToString(strTemp.TrimEnd(")".ToCharArray()).Trim());
						
						arlVarNames.Add(strTemp);
					}
					
					//== The varnames may be in the dictionary already (e.g. different functions using same local variable names) ==
					foreach (var strVarName in arlVarNames)
					{
						if ((string) strVarName != "")
						{
							
							if (dicInteger.ContainsKey(strVarName.ToString()))
							{
								dicInteger[strVarName.ToString()] = strDescription;
							}
							else
							{
								dicInteger.Add(strVarName.ToString(), strDescription);
							}
							//== If this is a new signed integer remove any with a duplicate name from the unsigned dictionary ==
							if ((!strStatement.Contains("unsigned ")) && (dicUnsigned.ContainsKey(strVarName.ToString())))
							{
								dicUnsigned.Remove(strVarName.ToString());
							}
						}
					}
					
				}
			}
			
		}
		
		public void AddUnsigned(string CodeLine)
		{
			// Take the variable name and its details and add them to the list
			//================================================================
			string strDescription = "";
			string strVarType = "unsigned";
			string strTemp = ""; // used as placeholder
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			string[] arrPlaceHolders = null;
			ArrayList arlVarNames = new ArrayList();
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			
			
			
			//== Extract variable names ==
			foreach (var strStatement in arrStatements)
			{
				
				//== RegEx should trap function input parameters, declarations of multiple unsigned vars on same line, etc. ==
				if (Regex.IsMatch(strStatement, "\\b(unsigned|UNSIGNED|size_t|uint16|uint32|UINT)\\b\\s+\\w+\\s*(\\,|$|=|\\))") || Regex.IsMatch(strStatement, "\\b(unsigned|UNSIGNED)\\b\\s+\\b(short|int|long|INT|LONG)\\b\\s+\\w+\\s*(\\,|$|=|\\))") || Regex.IsMatch(strStatement, "\\b(short|long|LONG|unsigned|UNSIGNED)\\b\\s+\\b(short|long|LONG|unsigned|UNSIGNED)\\b\\s+\\b(int|INT)\\b\\s+\\w+\\s*(\\,|$|=|\\))") || Regex.IsMatch(strStatement, "\\b(unsigned|UNSIGNED)\\b\\s+\\b(short|int|long|INT|LONG)\\b\\s+\\w+(\\,|$|=|\\))") || Regex.IsMatch(strStatement, "\\b(short|int|long|INT|LONG)\\b\\s+\\b(unsigned|UNSIGNED)\\b\\s+\\w+(\\,|$|=|\\))"))
				{
					
					//== Strip off anything which follows the equals sign (if present) as we won't need it ==
					arrFragments = strStatement.Trim().Split("=".ToCharArray());
					strDescription = System.Convert.ToString(arrFragments.First().Trim());
					
					//== Multiple declarations, comma separated ==
					
					
					//== Obtain each comma-separated statement ==
					arrPlaceHolders = strDescription.Split(",".ToCharArray());
					foreach (var strVarName in arrPlaceHolders)
					{
						if (Regex.IsMatch(strVarName, "\\b(unsigned|UNSIGNED|size_t|uint16|uint32|UINT)\\b"))
						{
							// strVarName = modMain.GetLastItem(strVarName); VBConversions Warning: A foreach variable can't be assigned to in C#.
						}
						
						//== Be careful of anything which may break the regex ==
						// strVarName = System.Convert.ToString(strVarName.TrimStart("(".ToCharArray()).Trim()); VBConversions Warning: A foreach variable can't be assigned to in C#.
						// strVarName = System.Convert.ToString(strVarName.TrimEnd(")".ToCharArray()).Trim()); VBConversions Warning: A foreach variable can't be assigned to in C#.
						arlVarNames.Add(strVarName);
					}
					
				}
				
				//== The varnames may be in the dictionary already (e.g. different functions using same local variable names) ==
				foreach (var strVarName in arlVarNames)
				{
					if ((string) strVarName != "")
					{
						if (dicUnsigned.ContainsKey(strVarName.ToString()))
						{
							dicUnsigned[strVarName.ToString()] = strVarType;
						}
						else
						{
							dicUnsigned.Add(strVarName.ToString(), strVarType);
						}
					}
				}
				
			}
			
		}
		
		public void CheckOverflow(string CodeLine, string FileName)
		{
			// Check to see if the strcpy is copying within limits of buffer sizes
			//====================================================================
			string strSource = "";
			string strDestination = "";
			string strExpression = "";
			string strParameter = "";
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			
			int intLimit = 0;
			int intOverflowType = NO_DANGER;
			
			bool blnIsStrN = false;
			bool blnIsCat = false;
			
			
			//== Split line into statements to account for any instances of multiple statements on same line ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			foreach (var strStatement in arrStatements)
			{
				
				try
				{
					blnIsCat = false;
					blnIsStrN = false;
					intOverflowType = NO_DANGER;
					
					// Remove all spaces to simplify parsing
					//strStatement = strStatement.Replace(" ", "")
					
					if (strStatement == "")
					{
						goto NextLoop;
					}
					
					// Determine type of expression
					if (strStatement.Contains("strcpy"))
					{
						strExpression = "strcpy";
					}
					else if (strStatement.Contains("strlcpy"))
					{
						strExpression = "strlcpy";
					}
					else if (strStatement.Contains("strcat"))
					{
						blnIsCat = true; // Set this boolean var to avoid repeated calls to .contains()
						strExpression = "strcat";
					}
					else if (strStatement.Contains("strlcat"))
					{
						blnIsCat = true; // Set this boolean var to avoid repeated calls to .contains()
						strExpression = "strlcat";
					}
					else if (strStatement.Contains("strncpy"))
					{
						blnIsStrN = true; // Set this boolean var to avoid repeated calls to .contains()
						strExpression = "strncpy";
					}
					else if (strStatement.Contains("strncat"))
					{
						blnIsCat = true; // Set this boolean var to avoid repeated calls to .contains()
						blnIsStrN = true; // Set this boolean var to avoid repeated calls to .contains()
						strExpression = "strncat";
					}
					else if (strStatement.Contains("sprintf"))
					{
						strExpression = "sprintf";
					}
					else if (strStatement.Contains("memcpy"))
					{
						blnIsStrN = true; // Set this boolean var as memcpy has char limit
						strExpression = "memcpy";
					}
					else if (strStatement.Contains("memmove"))
					{
						blnIsStrN = true; // Set this boolean var as memcpy has char limit
						strExpression = "memmove";
					}
					else
					{
						goto NextLoop;
					}
					
					arrFragments = Regex.Split(strStatement, "\\b" + strExpression + "\\b");
					
					
					//== Check statement contains a workable expression and extract varnames from brackets ==
					if (System.Convert.ToInt32(strStatement.IndexOf(strExpression) + 1 < strStatement.IndexOf("(") + 1) < strStatement.IndexOf(")") + 1 )
					{
						
						strParameter = arrFragments.Last();
						
						// Locate size/type of source and destination buffers
						arrFragments = strParameter.Split(",".ToCharArray());
						// Remove spaces from fragments and remove any leading/trailing braces
						strDestination = arrFragments[0].Trim().TrimStart("(".ToCharArray()).Trim();
						
						// Annoyingly sprintf has parameters in a different order from the others...
						if (strExpression == "sprintf")
						{
							strSource = System.Convert.ToString(arrFragments.Last().Trim.TrimEnd(")").Trim);
						}
						else
						{
							strSource = arrFragments[1].Trim().TrimEnd(")".ToCharArray()).Trim();
							if (blnIsStrN)
							{
								intLimit = GetStrncpyLimit(System.Convert.ToString(arrFragments[2].Trim().TrimEnd(")".ToCharArray())), strSource, strDestination, blnIsCat && blnIsStrN);
							}
						}
						
						
						//== Compare buffer sizes and types to determine if there is any possibility of overflow ==
						//== In the case of strncpy check for number or variable as length argument and check for any sizeof() buffoonery ==
						intOverflowType = CompareBufferLengths(strSource, strDestination, blnIsStrN, intLimit, blnIsCat);
						
					}
					
					
					if (blnIsStrN == false && intOverflowType != NO_DANGER)
					{
						
						switch (intOverflowType)
						{
							case CMDLINE_INTO_ARRAY:
								// Is strcpy copying a commandline argument to char[]
								frmMain.Default.ListCodeIssue("Unsafe Use of " + strExpression + " Allows Buffer Overflow", "A user-supplied string from the commandline is being copied to a fixed length destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
							case POINTER_INTO_ARRAY:
								// Is strcpy copying a char* to char[]
								frmMain.Default.ListCodeIssue("Unsafe Use of " + strExpression + " Allows Buffer Overflow", "A char* is being copied to a fixed length destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
							case SOURCE_LARGER_THAN_DEST:
								// Mismatched array sizes
								frmMain.Default.ListCodeIssue("Unsafe Use of " + strExpression + " Allows Buffer Overflow", "The source buffer is larger than the destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
						}
						
					}
					else if (blnIsStrN == true && intOverflowType != NO_DANGER)
					{
						
						switch (intOverflowType)
						{
							case CMDLINE_INTO_ARRAY:
								// Is strcpy copying a commandline argument to char[]
								frmMain.Default.ListCodeIssue("Unsafe Use of " + strExpression + " Allows Buffer Overflow", "The size limit is larger than the destination buffer, while the source is a user-supplied string from the commandline, and so could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
							case POINTER_INTO_ARRAY:
								// Is strcpy copying a char* to char[]
								frmMain.Default.ListCodeIssue("Unsafe Use of " + strExpression + " Allows Buffer Overflow", "The size limit is larger than the destination buffer, while the source is a char* and so, could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
							case SOURCE_LARGER_THAN_DEST:
								// Mismatched array sizes
								frmMain.Default.ListCodeIssue("Unsafe Use of " + strExpression + " Allows Buffer Overflow", "The source buffer and size limit are BOTH larger than the destination buffer and could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
							case WRONG_LIMIT_NO_DANGER:
								// No immediate danger but wrong size limit in place
								frmMain.Default.ListCodeIssue("Unsafe Use of " + strExpression +".", "Although the source buffer is not large enough to deliver a buffer overflow to the destination buffer, the size limit used by strncpy is larger then the destination and would theoretically such an attack if the source buffer were modified in the future.", FileName, CodeIssue.STANDARD, CodeLine);
								break;
							case OFF_BY_ONE:
								// Incorrect use of sizeof()
								frmMain.Default.ListCodeIssue("Off-by-One Error - Unsafe Use of " + strExpression + " Allows Buffer Overflow", "The size limit is one byte larger than the destination buffer due to incorrect use of sizeof( ) and so could allow a buffer overflow to take place.", FileName, CodeIssue.HIGH, CodeLine);
								break;
							case SOURCE_LIMIT:
								// Incorrect use of sizeof()
								frmMain.Default.ListCodeIssue("Programmer Error - Unsafe Use of " + strExpression + " Allows Buffer Overflow", "The size limit is set to the size of the source buffer, rather than the destination buffer due to mistaken use of sizeof( ) and so could allow a buffer overflow to take place.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
							case STRNCAT_MISUSE:
								// Incorrect use of strncat (uses size of source or destination as limit)
								frmMain.Default.ListCodeIssue("Programmer Error - Unsafe Use of " + strExpression + " Allows Buffer Overflow", "The size limit is set to the size of the source or destination buffer. For safe usage the limit should be set to the allowed size of the destination buffer minus the current size of the destination buffer.", FileName, CodeIssue.CRITICAL, CodeLine);
								break;
						}
						
					}
				}
				catch
				{
					goto NextLoop;
				}
NextLoop:
				strSource = "";
				strDestination = "";
				strExpression = "";
				intLimit = 0;
				blnIsStrN = false;
			}
			
		}
		
		public bool TrackBraces(string CodeLine, ref int BraceCount)
		{
			// Track matching open/close braces to determine whether still inside function, loop, etc.
			//========================================================================================
			bool blnRetVal = true;
			
			// We need this to cover the special case of blank lines before the braces/routine
			// causing the function to return 'false' before any of the routine has been scanned
			if (CodeLine.Contains("{") || CodeLine.Contains("}"))
			{
				
				// It's necessary to count every single brace in a line
				foreach (char chrChar in CodeLine)
				{
					if (chrChar == '{')
					{
						BraceCount++;
					}
				}
				foreach (char chrChar in CodeLine)
				{
					if (chrChar == '}')
					{
						BraceCount--;
					}
				}
				
				if (BraceCount < 1)
				{
					blnRetVal = false;
				}
				
			}
			
			return blnRetVal;
			
		}
		
		public Dictionary<string, string> GetMemAssign()
		{
			//Return dictionary of any potentially problematic memory assignments
			//===================================================================
			Dictionary<string, string> dicReturnDic = new Dictionary<string, string>();
			
			// Build new dictionary, ignoring all instances of variables that are correctly allocated and deleted
			foreach (var kyKey in dicMemAssign.Keys)
			{
				if (!(dicMemAssign[kyKey].StartsWith("1 delete ") || dicMemAssign[kyKey].StartsWith("1 free ")))
				{
					dicReturnDic.Add(kyKey, dicMemAssign[kyKey]);
				}
			}
			
			// Return the remaining list of faulty allocations
			return dicReturnDic;
			
		}
		
		private int GetStrncpyLimit(string BufferSize, string SourceName, string DestName, bool IsStrncat)
		{
			// Get content of square braces and check if numeric
			// If non-numeric check for sizeof() issues
			// and try and get value for variable used for buffer size
			//========================================================
			int intRetVal = 0;
			string strTemp = "";
			
			
			if (BufferSize == "")
			{
				// This shouldn't happen, but you never know...
				intRetVal = 0;
			}
			else if (Information.IsNumeric(BufferSize))
			{
				// Simplest case - hardcoded numeric buffer
				intRetVal = int.Parse(BufferSize);
			}
			else if (BufferSize.Contains("sizeof"))
			{
				// Check that sizeof has been correctly used
				if (SourceName != "" && BufferSize.Contains(SourceName))
				{
					intRetVal = SOURCE_SIZE_OF;
				}
				else if (DestName != "" && BufferSize.Contains(DestName) && IsStrncat)
				{
					intRetVal = DEST_SIZE_OF;
				}
				else if (DestName != "" && Regex.IsMatch(BufferSize, "\\bsizeof\\b\\s*\\(\\s*\\b" + DestName + "\\b\\s*\\-"))
				{
					intRetVal = MISCALC_SIZE_OF;
				}
				else if (DestName != "" && Regex.IsMatch(BufferSize, "\\bsizeof\\b\\s*\\(\\s*\\b" + DestName + "\\b"))
				{
					intRetVal = OFF_BY_ONE_SIZE_OF;
				}
			}
			else
			{
				// Check against dictionary of variable names
				strTemp = dicInteger[BufferSize];
				if (Information.IsNumeric(strTemp))
				{
					intRetVal = int.Parse(strTemp);
				}
			}
			
			return intRetVal;
			
		}
		
		private int CompareBufferLengths(string SourceBuffer, string DestinationBuffer, bool IsStrN = false, int SizeLimit = 0, bool IsCat = false)
		{
			// Take two buffer names and return true if source is larger than destination
			//===========================================================================
			int intRetVal = NO_DANGER;
			
			
			if (SourceBuffer == "" || DestinationBuffer == "")
			{
				// Parsing has failed to identify buffers
				intRetVal = NO_DANGER;
			}
			else if (dicBuffer[DestinationBuffer] == "*")
			{
				// Copying to a destination with no fixed limit
				intRetVal = NO_DANGER;
			}
			else if (Regex.IsMatch(SourceBuffer, "\\bargv\\b"))
			{
				// Copying an unlimited length string from cmdline into fixed length buffer
				intRetVal = CMDLINE_INTO_ARRAY;
			}
			else if (dicBuffer[SourceBuffer] == "*")
			{
				// Copying an unlimited length string into fixed length buffer
				intRetVal = POINTER_INTO_ARRAY;
			}
			else if (IsStrN == false && (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)))
			{
				// Classic overflow - source buffer larger than destination buffer
				intRetVal = SOURCE_LARGER_THAN_DEST;
			}
			else if (IsStrN == false && IsCat == true && ((GetBufferLength(SourceBuffer) + GetBufferLength(DestinationBuffer)) > GetBufferLength(DestinationBuffer)))
			{
				// Overflow from unsafe use of strcat
				intRetVal = SOURCE_LARGER_THAN_DEST;
			}
			else if (IsStrN == true && (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)) && (SizeLimit >= double.Parse(DestinationBuffer)))
			{
				// Overflow from unsafe use of strncpy/strncat
				intRetVal = SOURCE_LARGER_THAN_DEST;
			}
			else if (IsStrN == true && (GetBufferLength(SourceBuffer) <= GetBufferLength(DestinationBuffer)) && (SizeLimit > double.Parse(DestinationBuffer)))
			{
				// Overflow from unsafe use of strncpy/strncat
				intRetVal = WRONG_LIMIT_NO_DANGER;
			}
			else if (IsStrN == true && (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)) && SizeLimit == OFF_BY_ONE_SIZE_OF)
			{
				// Overflow from unsafe use of strncpy/strncat
				intRetVal = OFF_BY_ONE;
			}
			else if (IsStrN == true && (GetBufferLength(SourceBuffer) > GetBufferLength(DestinationBuffer)) && SizeLimit == SOURCE_SIZE_OF)
			{
				// Overflow from unsafe use of strncpy
				intRetVal = SOURCE_LIMIT;
			}
			else if (IsStrN == true && IsCat == true && ((GetBufferLength(SourceBuffer) + GetBufferLength(DestinationBuffer)) >= SizeLimit))
			{
				// Overflow from unsafe use of strncat
				intRetVal = SOURCE_LARGER_THAN_DEST;
			}
			else if (IsStrN == true && IsCat == true && (SizeLimit == DEST_SIZE_OF | SizeLimit == SOURCE_SIZE_OF | SizeLimit == MISCALC_SIZE_OF | SizeLimit == OFF_BY_ONE_SIZE_OF))
			{
				// Use of strncat without safe limit
				intRetVal = STRNCAT_MISUSE;
			}
			
			return intRetVal;
			
		}
		
		private int GetBufferLength(string BufferName)
		{
			// Take name of buffer and attempt to find its length
			//===================================================
			int intRetVal = 0;
			string strLength = "";
			string strVarVal = "";
			
			
			strLength = dicBuffer[BufferName];
			
			if (string.IsNullOrEmpty(strLength))
			{
				intRetVal = 0;
			}
			else if (Information.IsNumeric(strLength))
			{
				intRetVal = int.Parse(strLength);
			}
			else
			{
				// If we have a non-numeric string we need to see if we have an
				// identifiable variable with a value
				intRetVal = GetInteger(strLength);
			}
			
			return intRetVal;
			
		}
		
		private int GetInteger(string VarName)
		{
			//Get value of numeric variable - check recursively for variables that
			// are assigned the value of another variable
			//====================================================================
			int intRetVal = 0;
			string strResult = "";
			
			if (!dicInteger.ContainsKey(VarName))
			{
				return 0;
			}
			
			strResult = dicInteger[VarName];
			
			if (!string.IsNullOrEmpty(strResult))
			{
				if (Information.IsNumeric(strResult))
				{
					intRetVal = int.Parse(strResult);
				}
				else
				{
					intRetVal = GetInteger(strResult);
				}
			}
			
			return intRetVal;
			
		}
		
		public Dictionary<string, string> GetIntegers()
		{
			//Get dictionary of numeric variables (used primarily to identify buffer sizes)
			//=============================================================================
			
			return dicInteger;
			
		}
		
		public int CheckSignedComp(string CodeLine)
		{
			// Get status of numeric variables - signed or unsigned
			// Return true if signed is compared with unsigned, otherwise return false
			//========================================================================
			
			string strLeftSide = "";
			string strRightSide = "";
			string strOperator = "";
			
			bool blnIsSizeOfR = false;
			bool blnIsSizeOfL = false;
			
			string[] arrStatements = null;
			string[] arrFragments = null;
			
			
			//== Split line into statements to account for any instances of multiple statements ==
			//== on same line or comparisons inside for loops ==
			arrStatements = CodeLine.Trim().Split(";".ToCharArray());
			
			foreach (var strStatement in arrStatements)
			{
				
				//== If it's an empty string then don't bother ==
				if (strStatement.Trim() != "")
				{
					
					//== Get the comparison operator ==
					if (strStatement.Contains("=="))
					{
						strOperator = "==";
					}
					else if (strStatement.Contains("!="))
					{
						strOperator = "!=";
					}
					else if (strStatement.Contains("<="))
					{
						strOperator = "<=";
					}
					else if (strStatement.Contains(">="))
					{
						strOperator = ">=";
					}
					else if (strStatement.Contains("<") && !CodeLine.Contains("<<"))
					{
						strOperator = "<";
					}
					else if (strStatement.Contains(">") && !(CodeLine.Contains(">>") || CodeLine.Contains("->")))
					{
						strOperator = ">";
					}
					else
					{
						strOperator = "";
					}
					
					
					//== If comparison is taking place continue with check ==
					if (!string.IsNullOrEmpty(strOperator))
					{
						
						//== Break down code to get the operand either side of the comparison ==
						arrFragments = Regex.Split(strStatement, strOperator);
						if (arrFragments.Count() < 2)
						{
							return System.Convert.ToInt32(false);
						}
						
						strLeftSide = System.Convert.ToString(arrFragments.First().Trim());
						strRightSide = System.Convert.ToString(arrFragments.ElementAt(1).Trim());
						
						//== The sizeof operator returns a signed integer ==
						if (Regex.IsMatch(strRightSide, "\\bsizeof\\b\\s*\\("))
						{
							blnIsSizeOfR = true;
						}
						if (Regex.IsMatch(strLeftSide, "\\bsizeof\\b\\s*\\("))
						{
							blnIsSizeOfL = true;
						}
						
						//== Get the items immediately adjacent to the comparison operator and trim any spaces/braces from the edges ==
						arrFragments = Regex.Split(strLeftSide, "(\\(|\\s+)");
						strLeftSide = modMain.GetLastItem(arrFragments.Last());
						strLeftSide = strLeftSide.Trim("(".ToCharArray());
						strLeftSide = strLeftSide.Trim(")".ToCharArray());
						
						arrFragments = Regex.Split(strRightSide, "(\\)|\\s+)");
						strRightSide = modMain.GetFirstItem(arrFragments.First());
						strRightSide = strRightSide.Trim("(".ToCharArray());
						strRightSide = strRightSide.Trim(")".ToCharArray());
						
						//== Remove any increment/decrement operators (++/--) ==
						strRightSide = TrimOperators(strRightSide);
						strLeftSide = TrimOperators(strLeftSide);
						
						//== Exit if we have no expression, string expression, etc. ==
						if (string.IsNullOrEmpty(strLeftSide)|| string.IsNullOrEmpty(strRightSide))
						{
							return System.Convert.ToInt32(false);
						}
						if (strLeftSide.Contains("\"") || strRightSide.Contains("\""))
						{
							return System.Convert.ToInt32(false);
						}
						if (strLeftSide.Contains("'") || strRightSide.Contains("'"))
						{
							return System.Convert.ToInt32(false);
						}
						
						//== If either side of the comparison is NULL then we don't need to proceed any further ==
						if (strLeftSide == "NULL" || strRightSide == "NULL")
						{
							return System.Convert.ToInt32(false);
						}
						
						//== If we find an unsigned comparison anywhere then just exit function and return 'true' ==
						if (Information.IsNumeric(strLeftSide) && Information.IsNumeric(strRightSide))
						{
							return System.Convert.ToInt32(false);
						}
						else if (strLeftSide.StartsWith("0x") || strRightSide.StartsWith("0x"))
						{
							return System.Convert.ToInt32(false);
						}
						else if (Information.IsNumeric(strLeftSide) && !Information.IsNumeric(strRightSide))
						{
							if (Regex.IsMatch(strLeftSide, "\\-\\d+") && dicUnsigned.ContainsKey(strRightSide))
							{
								return System.Convert.ToInt32(true);
							}
							else
							{
								return System.Convert.ToInt32(false);
							}
							
						}
						else if (Information.IsNumeric(strRightSide) && !Information.IsNumeric(strLeftSide))
						{
							if (Regex.IsMatch(strRightSide, "\\-\\d+") && dicUnsigned.ContainsKey(strLeftSide))
							{
								return System.Convert.ToInt32(true);
							}
							else
							{
								return System.Convert.ToInt32(false);
							}
						}
						else
						{
							//== Both sides are variable names so check in dictionary ==
							if ((dicUnsigned.ContainsKey(strLeftSide)) && (((!dicUnsigned.ContainsKey(strRightSide)) && dicInteger.ContainsKey(strRightSide)) && (!blnIsSizeOfR)) || (((!dicUnsigned.ContainsKey(strLeftSide)) && dicInteger.ContainsKey(strLeftSide)) && (!blnIsSizeOfL)) && (dicUnsigned.ContainsKey(strRightSide)))
							{
								return System.Convert.ToInt32(true);
							}
						}
						
					}
				}
			}
			
			//== If we have reached this point then no signed/unsigned comparison has been encountered ==
			return System.Convert.ToInt32(false);
			
		}
		
		private string TrimOperators(string VarName)
		{
			// Trim any increment and decrement operators from variable names
			//===============================================================
			string strRetVal = VarName;
			
			if (VarName.StartsWith("++") || VarName.EndsWith("++"))
			{
				strRetVal = VarName.Trim("+".ToCharArray());
			}
			if (VarName.StartsWith("--") || VarName.EndsWith("--"))
			{
				strRetVal = VarName.Trim("-".ToCharArray());
			}
			
			return strRetVal.Trim();
			
		}
		
	}
	
}
