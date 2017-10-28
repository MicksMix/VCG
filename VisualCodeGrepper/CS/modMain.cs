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

using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

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
	sealed class modMain
	{
		
		[DllImport("kernel32.dll", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
		public static extern bool AttachConsole(int dwProcessId);
		
		//== Array to be used when sorting on multiple coilumns ==
		public static Dictionary<string, int> dicColumns = new Dictionary<string, int>();
		
		//== Class instance to hold app settings ==
		public static AppSettings asAppSettings = new AppSettings();
		
		//== Class instances to track details of file/code scanning operations ==
		public static CodeTracker ctCodeTracker = new CodeTracker();
		public static ResultsTracker rtResultsTracker = new ResultsTracker();
		
		//== Used for sharing data between main chart and individual charts ==
		public static string strCurrentFileName = "";
		public static int intComments = 0;
		public static int intCodeIssues = 0;
		
		//== Placeholder to be used when modifying severity levels ==
		public static int intNewSeverity = -1;
		
		
		public static int ParseArgs()
		{
			// Read any command line args and start application as appropriate
			//================================================================
			int intIndex = 0;
			string[] arrArgs = Environment.GetCommandLineArgs();
			string strTarget = "";
			
			//== Deal with any command line options ==
			if (arrArgs.Count() != 1)
			{
				intIndex = 1;
				while (intIndex < arrArgs.Count())
				{
					
					switch (arrArgs[intIndex])
					{
						
					case "-t":
					case "--target":
						// Set target
						intIndex++;
						if (intIndex < arrArgs.Count())
						{
							strTarget = arrArgs[intIndex];
						}
						else
						{
							ShowError("No target specified!");
						}
						break;
						
					case "-l":
					case "--language":
						// Set language
						intIndex++;
						if (intIndex < arrArgs.Count())
						{
							if (!SetLanguage(arrArgs[intIndex]))
							{
								ShowError("Unrecognised language!");
							}
						}
						else
						{
							ShowError("No language specified!");
						}
						break;
						
					case "-e":
					case "--extensions":
						// Set file extensions
						intIndex++;
						if (intIndex < arrArgs.Count())
						{
							asAppSettings.FileSuffixes = (Array) (arrArgs[intIndex].Split("|".ToCharArray()));
						}
						else
						{
							ShowError("No extensions provided!");
						}
						break;
						
					case "-i":
					case "--import":
						// Import XML results
						intIndex++;
						if (intIndex < arrArgs.Count())
						{
							if (arrArgs[intIndex].ToLower().EndsWith(".xml"))
							{
								asAppSettings.IsXmlInputFile = true;
								asAppSettings.XmlInputFile = arrArgs[intIndex];
								frmMain.Default.ImportResultsXML(asAppSettings.XmlInputFile);
							}
							else if (arrArgs[intIndex].ToLower().EndsWith(".csv"))
							{
								asAppSettings.IsCsvInputFile = true;
								asAppSettings.CsvInputFile = arrArgs[intIndex];
								frmMain.Default.ImportResultsCSV(asAppSettings.CsvInputFile);
							}
							
							// If results are being imported for inspection then console-only mode must be off and we should not have a target!
							asAppSettings.IsConsole = false;
							strTarget = "";
							goto endOfWhileLoop;
						}
						else
						{
							ShowError("No input filename provided!");
						}
						break;
						
					case "-x":
					case "--export":
						// Automatically export XML results
						intIndex++;
						if (intIndex < arrArgs.Count())
						{
							asAppSettings.XmlOutputFile = arrArgs[intIndex];
							asAppSettings.IsXmlOutputFile = true;
						}
						else
						{
							ShowError("No XML results filename provided!");
						}
						break;
						
					case "-f":
					case "--csv-export":
						// Automatically export CSV results
						intIndex++;
						if (intIndex < arrArgs.Count())
						{
							asAppSettings.CsvOutputFile = arrArgs[intIndex];
							asAppSettings.IsCsvOutputFile = true;
						}
						else
						{
							ShowError("No CSV results filename provided!");
						}
						break;
						
					case "-r":
					case "--results":
						// Automatically export flat text results
						intIndex++;
						if (intIndex < arrArgs.Count())
						{
							asAppSettings.OutputFile = arrArgs[intIndex];
							asAppSettings.IsOutputFile = true;
						}
						else
						{
							ShowError("No results filename provided!");
						}
						break;
						
					case "-c":
					case "--console":
						// Run in console (hide GUI)
						AttachConsole(-1);
						asAppSettings.IsConsole = true;
						asAppSettings.DisplayBreakdownOption = false;
						asAppSettings.VisualBreakdownEnabled = false;
						break;
					case "-v":
					case "--verbose":
						// Verbose mode
						asAppSettings.IsVerbose = true;
						break;
						
					default:
						// Help
						if (intIndex > 2 && (!arrArgs[intIndex].StartsWith("-")))
						{
							ShowError("Error parsing commandline options!" + Constants.vbNewLine + "All options begin with '-' and take one parameter only.");
						}
						else
						{
							ShowHelp();
						}
						break;
						
				}
				
				intIndex++;
			}
endOfWhileLoop:
			1.GetHashCode() ; //VBConversions note: C# requires an executable line here, so a dummy line was added.
		}
		
		if (asAppSettings.IsConsole == true)
		{
			frmMain.Default.Hide();
		}
		
		// If target is provided on cmd line then load files
		if (!string.IsNullOrEmpty(strTarget))
		{
			frmMain.Default.LoadFiles(strTarget);
		}
		
		return intIndex;
		
	}
	
	public static void ShowHelp()
	{
		// Display help and show usage
		//============================
		
		string strHelp = "";
		
		strHelp = "Visual Code Grepper (VCG) 2.0 (C) Nick Dunn and John Murray, 2012-2014." + Constants.vbNewLine + "Usage:  VisualCodeGrepper [Options]" + Constants.vbNewLine + Constants.vbNewLine + "STARTUP:" + Constants.vbNewLine + "Set desired starting point for GUI. If using console mode these options will set target(s) to be scanned." + Constants.vbNewLine + " -t, --target <Filename|DirectoryName>:	Set target file or directory. Use this option either to load target immediately into GUI or to provide the target for console mode." + Constants.vbNewLine + " -l, --language <CPP|PLSQL|JAVA|CS|VB|PHP>:	Set target language (Default is C/C++)." + Constants.vbNewLine + " -e, --extensions <ext1|ext2|ext3>:	Set file extensions to be analysed (See ReadMe or Options screen for language-specific defaults)." + Constants.vbNewLine + " -i, --import <Filename>:	Import XML/CSV results to GUI." + Constants.vbNewLine + Constants.vbNewLine + "OUTPUT:" + Constants.vbNewLine + "Automagically export results to a file in the specified format. Use XML output if you wish to reload results into the GUI later on." + Constants.vbNewLine + " -x, --export <Filename>:	Automatically export results to XML file." + Constants.vbNewLine + " -f, --csv-export <Filename>:		Automatically export results to CSV file." + Constants.vbNewLine + " -r, --results <Filename>:	Automatically export results to flat text file." + Constants.vbNewLine + Constants.vbNewLine + "CONSOLE OPTIONS:" + Constants.vbNewLine + " -c, --console:		Run application in console only (hide GUI)." + Constants.vbNewLine + " -v, --verbose:		Set console output to verbose mode." + Constants.vbNewLine + " -h, --help:		Show help." + Constants.vbNewLine;
		
		Console.Write(strHelp);
		
	}
	
	private static bool SetLanguage(string NewLanguage)
	{
		// Get new langauge from command line
		//===================================
		bool blnRetVal = true;
		
		NewLanguage = NewLanguage.ToUpper();
		
		switch (NewLanguage)
		{
			case "C":
			case "C++":
			case "CPP":
				asAppSettings.TestType = AppSettings.C;
				break;
			case "JAVA":
				asAppSettings.TestType = AppSettings.JAVA;
				break;
			case "PL/SQL":
			case "PLSQL":
			case "SQL":
				asAppSettings.TestType = AppSettings.SQL;
				break;
			case "C#":
			case "C-SHARP":
			case "CS":
			case "CSHARP":
				asAppSettings.TestType = AppSettings.CSHARP;
				break;
			case "VB":
			case "VISUALBASIC":
			case "VISUAL-BASIC":
				asAppSettings.TestType = AppSettings.VB;
				break;
			case "PHP":
				asAppSettings.TestType = AppSettings.PHP;
				break;
			default:
				blnRetVal = false;
				break;
		}
		
		return blnRetVal;
		
	}
	
	public static void ShowError(string ErrorText)
	{
		// Show console error for incorrect command line options
		//======================================================
		
		Console.WriteLine("Error reading command line options!" + Constants.vbNewLine + ErrorText + Constants.vbNewLine);
		ShowHelp();
		
	}
	
	public static void LaunchNPP(string FileName, int LineNumber = 0)
	{
		// Launch NPP if available, launch Notepad if not
		//===============================================
		
		try
		{
			// If we're trying to open a file on a specific line in Notepad++ then the filename *must* be quoted to avoid erratic behaviour from Windows
			System.Diagnostics.Process.Start("notepad++.exe", "-n" + System.Convert.ToString(LineNumber) + " \"" + FileName + "\"");
		}
		catch (Exception)
		{
			System.Diagnostics.Process.Start("Notepad.exe", "\"" + FileName + "\"");
		}
		
	}
	
	public static void SelectLanguage(int Language)
	{
		// Set language and characteristics
		//=================================
		
		
		// Set language type
		asAppSettings.TestType = Language;
		
		//== Set the file types/suffixes for the chosen language ==
		SetSuffixes(Language);
		
		
		// This covers most languages - the different ones will be set individually, below
		asAppSettings.SingleLineComment = "//";
		asAppSettings.AltSingleLineComment = "";
		
		// Load list of unsafe functions
		switch (Language)
		{
			case AppSettings.C:
				asAppSettings.BadFuncFile = asAppSettings.CConfFile;
				LoadUnsafeFunctionList(AppSettings.C);
				break;
			case AppSettings.JAVA:
				asAppSettings.BadFuncFile = asAppSettings.JavaConfFile;
				LoadUnsafeFunctionList(AppSettings.JAVA);
				break;
			case AppSettings.SQL:
				asAppSettings.BadFuncFile = asAppSettings.PLSQLConfFile;
				LoadUnsafeFunctionList(AppSettings.SQL);
				asAppSettings.SingleLineComment = "--";
				break;
			case AppSettings.CSHARP:
				asAppSettings.BadFuncFile = asAppSettings.CSharpConfFile;
				LoadUnsafeFunctionList(AppSettings.CSHARP);
				break;
			case AppSettings.VB:
				asAppSettings.BadFuncFile = asAppSettings.VBConfFile;
				LoadUnsafeFunctionList(AppSettings.VB);
				asAppSettings.SingleLineComment = "'";
				asAppSettings.AltSingleLineComment = "REM";
				break;
			case AppSettings.PHP:
				asAppSettings.BadFuncFile = asAppSettings.PHPConfFile;
				LoadUnsafeFunctionList(AppSettings.PHP);
				asAppSettings.SingleLineComment = "//";
				asAppSettings.AltSingleLineComment = "\\#"; // This will be used in a regex so it must be escaped
				break;
			case AppSettings.COBOL:
				asAppSettings.BadFuncFile = asAppSettings.COBOLConfFile;
				LoadUnsafeFunctionList(AppSettings.COBOL);
				asAppSettings.SingleLineComment = "*";
				break;
		}
		
		
		// Set the GUI to display correct options for the language
		if (asAppSettings.IsConsole == true)
		{
			return;
		}
		
		dynamic with_1 = frmMain;
		switch (Language)
		{
			case AppSettings.C:
				with_1.JavaToolStripMenuItem.Checked = false;
				with_1.PLSQLToolStripMenuItem.Checked = false;
				with_1.CSToolStripMenuItem.Checked = false;
				with_1.VBToolStripMenuItem.Checked = false;
				with_1.PHPToolStripMenuItem.Checked = false;
				with_1.sslLabel.Text = "Language: C/C++   File Suffixes: " + asAppSettings.CSuffixes;
				break;
			case AppSettings.JAVA:
				with_1.CCToolStripMenuItem.Checked = false;
				with_1.PLSQLToolStripMenuItem.Checked = false;
				with_1.CSToolStripMenuItem.Checked = false;
				with_1.VBToolStripMenuItem.Checked = false;
				with_1.PHPToolStripMenuItem.Checked = false;
				with_1.sslLabel.Text = "Language: Java   File Suffixes: " + asAppSettings.JavaSuffixes;
				break;
			case AppSettings.SQL:
				with_1.CCToolStripMenuItem.Checked = false;
				with_1.JavaToolStripMenuItem.Checked = false;
				with_1.CSToolStripMenuItem.Checked = false;
				with_1.VBToolStripMenuItem.Checked = false;
				with_1.PHPToolStripMenuItem.Checked = false;
				asAppSettings.SingleLineComment = "--";
				with_1.sslLabel.Text = "Language: PL/SQL   File Suffixes: " + asAppSettings.PLSQLSuffixes;
				break;
			case AppSettings.CSHARP:
				with_1.CCToolStripMenuItem.Checked = false;
				with_1.JavaToolStripMenuItem.Checked = false;
				with_1.PLSQLToolStripMenuItem.Checked = false;
				with_1.VBToolStripMenuItem.Checked = false;
				with_1.PHPToolStripMenuItem.Checked = false;
				with_1.sslLabel.Text = "Language: C#   File Suffixes: " + asAppSettings.CSharpSuffixes;
				break;
			case AppSettings.VB:
				with_1.CCToolStripMenuItem.Checked = false;
				with_1.JavaToolStripMenuItem.Checked = false;
				with_1.PLSQLToolStripMenuItem.Checked = false;
				with_1.CSToolStripMenuItem.Checked = false;
				with_1.PHPToolStripMenuItem.Checked = false;
				with_1.sslLabel.Text = "Language: VB   File Suffixes: " + asAppSettings.VBSuffixes;
				break;
			case AppSettings.PHP:
				with_1.CCToolStripMenuItem.Checked = false;
				with_1.JavaToolStripMenuItem.Checked = false;
				with_1.PLSQLToolStripMenuItem.Checked = false;
				with_1.CSToolStripMenuItem.Checked = false;
				with_1.VBToolStripMenuItem.Checked = false;
				with_1.sslLabel.Text = "Language: PHP   File Suffixes: " + asAppSettings.PHPSuffixes;
				break;
			case AppSettings.COBOL:
				with_1.CCToolStripMenuItem.Checked = false;
				with_1.JavaToolStripMenuItem.Checked = false;
				with_1.PLSQLToolStripMenuItem.Checked = false;
				with_1.CSToolStripMenuItem.Checked = false;
				with_1.VBToolStripMenuItem.Checked = false;
				with_1.sslLabel.Text = "Language: COBOL   File Suffixes: " + asAppSettings.COBOLSuffixes;
				break;
		}
		
	}
	
	public static void SetSuffixes(int Language)
	{
		// Set the filetypes to scan
		//==========================
		
		asAppSettings.IsAllFileTypes = false;
		
		//== Check if wildcard/all files has been specified ==
		if (asAppSettings.CSuffixes.Contains(".*") || asAppSettings.CSuffixes.Trim() == "")
		{
			asAppSettings.IsAllFileTypes = true;
		}
		else
		{
			switch (Language)
			{
				case AppSettings.C:
					asAppSettings.FileSuffixes = asAppSettings.CSuffixes.Split("|".ToCharArray());
					break;
				case AppSettings.JAVA:
					asAppSettings.FileSuffixes = asAppSettings.JavaSuffixes.Split("|".ToCharArray());
					break;
				case AppSettings.SQL:
					asAppSettings.FileSuffixes = asAppSettings.PLSQLSuffixes.Split("|".ToCharArray());
					break;
				case AppSettings.CSHARP:
					asAppSettings.FileSuffixes = asAppSettings.CSharpSuffixes.Split("|".ToCharArray());
					break;
				case AppSettings.VB:
					asAppSettings.FileSuffixes = asAppSettings.VBSuffixes.Split("|".ToCharArray());
					break;
				case AppSettings.PHP:
					asAppSettings.FileSuffixes = asAppSettings.PHPSuffixes.Split("|".ToCharArray());
					break;
				case AppSettings.COBOL:
					asAppSettings.FileSuffixes = asAppSettings.COBOLSuffixes.Split("|".ToCharArray());
					break;
			}
			
			asAppSettings.NumSuffixes = asAppSettings.FileSuffixes.Length - 1;
		}
		
	}
	
	public static void LoadUnsafeFunctionList(int CurrentLanguage)
	{
		// Load appropriate list of bad functions from file (dependent on selected language)
		//==================================================================================
		
		string strDescription = "";
		string strConfFile = "";
		string[] arrFuncList = null;
		
		
		asAppSettings.BadFunctions.Clear();
		
		
		//ToDo: check these against their safe equivalents, make sure not flagging any false positives or false negatives, might be worthwhile to do a check (later) if it is flagged as _
		//eg. sprintf its not u_vsprintf, etc.
		
		// Check file exists
		if (!File.Exists(asAppSettings.BadFuncFile))
		{
			
			// Restore default file in case of bad registry entries, user placing non-existent file in Options dialog, etc.
			switch (CurrentLanguage)
			{
				case AppSettings.C:
					asAppSettings.BadFuncFile = Application.StartupPath + "\\cppfunctions.conf";
					break;
				case AppSettings.JAVA:
					asAppSettings.BadFuncFile = Application.StartupPath + "\\javafunctions.conf";
					break;
				case AppSettings.SQL:
					asAppSettings.BadFuncFile = Application.StartupPath + "\\plsqlfunctions.conf";
					break;
				case AppSettings.CSHARP:
					asAppSettings.BadFuncFile = Application.StartupPath + "\\csfunctions.conf";
					break;
				case AppSettings.VB:
					asAppSettings.BadFuncFile = Application.StartupPath + "\\vbfunctions.conf";
					break;
				case AppSettings.PHP:
					asAppSettings.BadFuncFile = Application.StartupPath + "\\phpfunctions.conf";
					break;
				case AppSettings.COBOL:
					asAppSettings.BadFuncFile = Application.StartupPath + "\\cobolfunctions.conf";
					break;
			}
			
			if (!File.Exists(asAppSettings.BadFuncFile))
			{
				Interaction.MsgBox("No config file found for bad functions.", MsgBoxStyle.Critical, "Error");
			}
			
		}
		else
		{
			try
			{
				foreach (var strLine in File.ReadLines(asAppSettings.BadFuncFile))
				{
					
					// Check for comments/whitespace
					if ((strLine.Trim() != null) && (!strLine.Trim().StartsWith("//")))
					{
						
						CodeIssue ciCodeIssue = new CodeIssue();
						
						// Build up array of bad functions and any associated descriptions
						if (strLine.Contains("=>"))
						{
							arrFuncList = Regex.Split(strLine, "=>");
							ciCodeIssue.FunctionName = arrFuncList.First();
							
							strDescription = System.Convert.ToString(arrFuncList.Last().Trim());
							
							// Extract severity level from description (if present)
							if (strDescription.StartsWith("[0]") || strDescription.StartsWith("[1]") || strDescription.StartsWith("[2]") || strDescription.StartsWith("[3]"))
							{
								ciCodeIssue.Severity = int.Parse(strDescription.Substring(1, 1));
								strDescription = strDescription.Substring(3).Trim();
							}
							
							ciCodeIssue.Description = strDescription;
						}
						else
						{
							ciCodeIssue.FunctionName = strLine;
							ciCodeIssue.Description = "";
						}
						
						if (!asAppSettings.BadFunctions.Contains(ciCodeIssue))
						{
							asAppSettings.BadFunctions.Add(ciCodeIssue);
						}
					}
				}
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		
		// Fix to stop temp content being wiped at start of scan
		if (!string.IsNullOrEmpty(asAppSettings.TempGrepText))
		{
			frmOptions.Default.LoadTempGrepContent(asAppSettings.TempGrepText);
		}
		
	}
	
	public static void LoadBadComments()
	{
		// Get list of bad comments from config file
		//==========================================
		
		try
		{
			foreach (var strLine in File.ReadLines(asAppSettings.BadCommentFile))
			{
				
				// Check for comments/whitespace
				if ((strLine.Trim() != null) && (!strLine.Trim().StartsWith("//")))
				{
					asAppSettings.BadComments.Add(strLine);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
		
	}
	
	public static void CheckCode(string CodeLine, string FileName)
	{
		// Scan line of code for anything requiring attention and return results
		//======================================================================
		
		int intIndex = 0;
		string strCleanName = "";
		string strTidyFuncName = "";
		
		//== Locate any unsafe functions for the language in question ==
		if (asAppSettings.BadFunctions.Count > 0)
		{
			for (intIndex = 0; intIndex <= asAppSettings.BadFunctions.Count - 1; intIndex++)
			{
				
				//== Sanitise the expression ready for insertion into regex ==
				strTidyFuncName = System.Convert.ToString(asAppSettings.BadFunctions[intIndex].FunctionName.Trim);
				
				//== Important - comparison MUST be case-sensitive for everything except PL/SQL, where it MUST be case-insenstive ==
				if (asAppSettings.TestType == AppSettings.SQL)
				{
					strTidyFuncName = strTidyFuncName.ToUpper();
				}
				
				//== Add word boundaries ONLY IF the expression does not contain whitespace or dots ==
				if ((!Regex.IsMatch(strTidyFuncName, "\\s+")) && (!strTidyFuncName.Contains(".")))
				{
					strCleanName = "\\b" + Regex.Escape(strTidyFuncName) + "\\b";
				}
				
				//== Important - comparison MUST be case-sensitive for everything except PL/SQL, where it MUST be case-insenstive ==
				if (asAppSettings.TestType == AppSettings.SQL)
				{
					if ((!string.IsNullOrEmpty(strCleanName)&& Regex.IsMatch(CodeLine.ToUpper(), strCleanName)) || (string.IsNullOrEmpty(strCleanName)&& CodeLine.ToUpper().Contains(System.Convert.ToString(asAppSettings.BadFunctions[intIndex].FunctionName.toupper))))
					{
						frmMain.Default.ListCodeIssue(System.Convert.ToString(asAppSettings.BadFunctions[intIndex].FunctionName), System.Convert.ToString(asAppSettings.BadFunctions[intIndex].Description), FileName, System.Convert.ToInt32(asAppSettings.BadFunctions[intIndex].Severity), CodeLine);
					}
				}
				else
				{
					//If CodeLine.Contains(asAppSettings.BadFunctions(intIndex).FunctionName) Then
					if ((!string.IsNullOrEmpty(strCleanName)&& Regex.IsMatch(CodeLine, strCleanName)) || (string.IsNullOrEmpty(strCleanName)&& CodeLine.Contains(System.Convert.ToString(asAppSettings.BadFunctions[intIndex].FunctionName))))
					{
						frmMain.Default.ListCodeIssue(strTidyFuncName, System.Convert.ToString(asAppSettings.BadFunctions[intIndex].Description), FileName, System.Convert.ToInt32(asAppSettings.BadFunctions[intIndex].Severity), CodeLine);
					}
				}
				strCleanName = "";
			}
		}
		
		//== Only carry out further code checks if required by user ==
		if (asAppSettings.IsConfigOnly == false)
		{
			
			//== Carry out any language-specific tests ==
			switch (asAppSettings.TestType)
			{
				case AppSettings.C:
					modCppCheck.CheckCPPCode(CodeLine, FileName);
					break;
				case AppSettings.JAVA:
					modJavaCheck.CheckJavaCode(CodeLine, FileName);
					break;
				case AppSettings.SQL:
					modPlSqlCheck.CheckPLSQLCode(CodeLine.ToUpper(), FileName);
					break;
				case AppSettings.CSHARP:
					modCSharpCheck.CheckCSharpCode(CodeLine, FileName);
					break;
				case AppSettings.VB:
					modVBCheck.CheckVBCode(CodeLine, FileName);
					break;
				case AppSettings.PHP:
					modPHPCheck.CheckPHPCode(CodeLine, FileName);
					break;
				case AppSettings.COBOL:
					modCobolCheck.CheckCobolCode(CodeLine, FileName);
					break;
			}
			
			//== Check for possible hard-coded passwords ==
			if (CodeLine.ToLower().Contains("password ") && ((CodeLine.ToLower()).IndexOf("password") + 1 < CodeLine.IndexOf("= \"") + 1 ) && !(CodeLine.Contains("''") || CodeLine.Contains("\"\"")))
			{
				frmMain.Default.ListCodeIssue("Code Appears to Contain Hard-Coded Password", "The code may contain a hard-coded password which an attacker could obtain from the source or by dis-assembling the executable. Please manually review the code:", FileName, CodeIssue.MEDIUM, CodeLine);
			}
		}
		
	}
	
	public static dynamic GetVarName(string CodeLine, bool SplitOnEquals = false)
	{
		// Extract the variable name from a line of code
		//==============================================
		string strVarName = "";
		string[] arrFragments = null;
		
		
		if (CodeLine.Contains("=") || SplitOnEquals)
		{
			arrFragments = CodeLine.Trim().Split("=".ToCharArray());
			strVarName = arrFragments.First();
		}
		else
		{
			arrFragments = CodeLine.Trim().Split(";".ToCharArray());
			strVarName = arrFragments.First();
		}
		
		strVarName = GetLastItem(strVarName);
		
		//== Be careful of anything which may break the regex ==
		strVarName = System.Convert.ToString(strVarName.TrimStart("(".ToCharArray()).Trim());
		strVarName = System.Convert.ToString(strVarName.TrimEnd(")".ToCharArray()).Trim());
		
		if (asAppSettings.TestType == AppSettings.PHP)
		{
			strVarName = "\\" + strVarName;
		}
		
		return strVarName;
		
	}
	
	public static string GetLastItem(string ListString, string Separator = "")
	{
		//Split string on specified character (default: space) and return last item
		//=========================================================================
		string strRetVal = "";
		string[] arrStrings = null;
		
		ListString = ListString.Trim();
		
		switch (Separator)
		{
			case " ":
				// This regex prevents a split on space from returning empty strings
				arrStrings = Regex.Split(ListString, "\\s+");
				break;
			default:
				arrStrings = ListString.Split(Separator.ToCharArray());
				break;
		}
		
		// Return final item
		strRetVal = System.Convert.ToString(arrStrings.Last().Trim());
		
		return strRetVal;
		
	}
	
	public static string GetFirstItem(string ListString, string Separator = "")
	{
		//Split string on specified character (default: space) and return first item
		//=========================================================================
		string strRetVal = "";
		string[] arrStrings = null;
		
		ListString = ListString.Trim();
		
		switch (Separator)
		{
			case " ":
				// This regex prevents a split on space from returning empty strings
				arrStrings = Regex.Split(ListString, "\\s+");
				break;
			default:
				arrStrings = ListString.Split(Separator.ToCharArray());
				break;
		}
		
		// Return first item
		strRetVal = System.Convert.ToString(arrStrings.First().Trim());
		
		return strRetVal;
		
	}
	
	public static void CheckFileLevelIssues(string FileName)
	{
		//List any file-level code issues (mis-matched deletes, mallocs, etc.)
		//====================================================================
		
		dynamic with_1 = frmMain;
		if (asAppSettings.TestType == AppSettings.C && ctCodeTracker.GetMemAssign().Count > 0)
		{
			with_1.ListMemoryIssue(ctCodeTracker.GetMemAssign());
		}
		else if (asAppSettings.TestType == AppSettings.JAVA)
		{
			if (ctCodeTracker.ImplementsClone == true)
			{
				with_1.ListCodeIssue("Class Implements Public 'clone' Method", "Cloning allows an attacker to instantiate a class without running any of the class constructors by deploying hostile code in the JVM.", FileName, CodeIssue.MEDIUM);
			}
			if (ctCodeTracker.IsSerialize == true)
			{
				with_1.ListCodeIssue("Class Implements Serialization", "Serialization can be used to save objects (and their state) when the JVM is switched off. The process flattens the object, saving it as a stream of bytes, allowing an attacker to view the inner state of an object and potentially view private attributes.", FileName, CodeIssue.MEDIUM);
			}
			if (ctCodeTracker.IsDeserialize == true)
			{
				with_1.ListCodeIssue("Class Implements Deserialization", "Deserialization allows the creation of an object from a stream of bytes, allowing the instantiation of a legitimate class without calling its constructor. This behaviour can be abused by an attacker to instantiate or replicate an object’s state.", FileName, CodeIssue.MEDIUM);
			}
			if (ctCodeTracker.HasXXEEnabled == true)
			{
				with_1.ListCodeIssue("XML Entity Expansion", "The class Uses JAXB and may allow XML entity expansion, which can render the application vulnerable to the use of XML bombs. Manually confirm that JAXB 2.0 or later is in use, which is not vulnerable, otherwise check the feasibility of disabling this feature and check for validation of incoming data.", FileName, CodeIssue.STANDARD);
			}
			if (ctCodeTracker.IsFileOpen == true)
			{
				with_1.ListCodeIssue("Failure To Release Resources In All Cases", "There appears to be no 'finally' block to release resources if an exception occurs, potentially resulting in DoS conditions from excessive resource consumption.", FileName, CodeIssue.MEDIUM, "", ctCodeTracker.FileOpenLine);
				if (ctCodeTracker.HasTry == false)
				{
					with_1.ListCodeIssue("FileStream Opened Without Exception Handling", "There appears to be no 'try' block to safely open the filestream, potentially resulting in server-side exceptions.", FileName, CodeIssue.MEDIUM, "", ctCodeTracker.FileOpenLine);
				}
			}
			if (ctCodeTracker.HasResourceRelease == false)
			{
				with_1.ListCodeIssue("Failure To Release Resources In All Cases", "There appears to be no release of resources in the 'finally' block, potentially resulting in DoS conditions from excessive resource consumption.", FileName, CodeIssue.MEDIUM, "", ctCodeTracker.FileOpenLine);
			}
		}
		
	}
	
}

}
