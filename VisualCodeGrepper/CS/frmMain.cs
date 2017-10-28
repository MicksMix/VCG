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
//using System.String;
using System.Threading;
using System.Web;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Xml;
using System.Threading.Tasks;
//using System.Threading.Tasks.Task;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Runtime.InteropServices;

// VisualCodeGrepper - Code security scanner
// Copyright (C) 2012-2013 Nick Dunn and John Murray
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
	
	public partial class frmMain
	{
		
#region Default Instance
		
		private static frmMain defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmMain Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmMain();
					defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
				}
				
				return defaultInstance;
			}
			set
			{
				defaultInstance = value;
			}
		}
		
		static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
		{
			defaultInstance = null;
		}
		
#endregion
		
		//===============================
		//== Columns for summary table ==
		//-------------------------------
		const int PRIORITY_COL = 0;
		const int SEVERITY_COL = 1;
		const int TITLE_COL = 2;
		const int DESC_COL = 3;
		const int FILE_COL = 4;
		const int LINE_COL = 5;
		//===============================
		
		[DllImport("kernel32.dll", ExactSpelling=true, CharSet=CharSet.Ansi, SetLastError=true)]
		public static extern bool AttachConsole(int dwProcessId);
		
		//=========================================
		//== Configurable options for XML export ==
		//-----------------------------------------
		public bool SaveCheckState = true;
		public bool SaveFiltered = false;
		//=========================================
		
		
		//========================================================
		//== Application details stored from previous execution ==
		//--------------------------------------------------------
		string[] arrPreviousDirs;
		//========================================================
		
		
		//== Current position in Rich Text Box (for assingning fonts) ==
		long lngPosition = 0;
		
		//== Search text ==
		string strPrevSearch = "";
		
		//== Output files ==
		StreamWriter swOutputFile;
		StreamWriter swCsvOutputFile;
		XmlWriterSettings xwsXMLOutputSettings = new XmlWriterSettings();
		XmlWriter xwOutputWriter;
		
		//== Sort order for listview (ascending or descending) ==
		bool blnIsAscendingSeverity = false;
		bool blnIsAscendingTitle = false;
		bool blnIsAscendingDescription = false;
		bool blnIsAscendingFile = false;
		bool blnIsFiltered = false; // Will be set to true if we're not showing all results
		public int intFilterMin = CodeIssue.POSSIBLY_SAFE;
		public int intFilterMax = CodeIssue.CRITICAL;
		
		
		public void NewTargetToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			ShowSelectDirectoryDialog();
		}
		
		private void ShowSelectDirectoryDialog()
		{
			//Load files to be scanned
			//========================
			string strTargetFolder = "";
			
			
			var dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = true;
			CommonFileDialogResult result = dialog.ShowDialog();
			
			// Get target directory
			//fbFolderBrowser.ShowDialog()
			
			if (result == CommonFileDialogResult.Ok)
			{
				strTargetFolder = dialog.FileName.ToString();
				LoadFiles(strTargetFolder);
			}
		}
		
		public void NewTargetFileToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			//Load single file to be scanned
			//==============================
			string strTargetFile = "";
			
			
			// Get target file
			
			if (ofdOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				strTargetFile = ofdOpenFileDialog.FileName;
				LoadFiles(strTargetFile);
			}
			
		}
		
		public void CCToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set code type to be tested and uncheck other menu items
			//========================================================
			
			if (CCToolStripMenuItem.Checked == true)
			{
				modMain.SelectLanguage(AppSettings.C);
			}
			
		}
		
		public void JavaToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set code type to be tested and uncheck other menu items
			//========================================================
			
			if (JavaToolStripMenuItem.Checked == true)
			{
				modMain.SelectLanguage(AppSettings.JAVA);
			}
			
		}
		
		public void PLSQLToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set code type to be tested and uncheck other menu items
			//========================================================
			
			if (PLSQLToolStripMenuItem.Checked == true)
			{
				modMain.SelectLanguage(AppSettings.SQL);
			}
			
		}
		
		public void CSToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set code type to be tested and uncheck other menu items
			//========================================================
			
			if (CSToolStripMenuItem.Checked == true)
			{
				modMain.SelectLanguage(AppSettings.CSHARP);
			}
			
		}
		
		public void VBToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set code type to be tested and uncheck other menu items
			//========================================================
			
			if (VBToolStripMenuItem.Checked == true)
			{
				modMain.SelectLanguage(AppSettings.VB);
			}
			
		}
		
		public void PHPToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set code type to be tested and uncheck other menu items
			//========================================================
			
			if (PHPToolStripMenuItem.Checked == true)
			{
				modMain.SelectLanguage(AppSettings.PHP);
			}
			
		}
		
		public void COBOLToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set code type to be tested and uncheck other menu items
			//========================================================
			
			if (COBOLToolStripMenuItem.Checked == true)
			{
				modMain.SelectLanguage(AppSettings.COBOL);
			}
			
		}
		
		public void DisplayError(string ErrorMessage, string Caption = "", MsgBoxStyle MsgBoxStyle = default(MsgBoxStyle))
		{
			// Display error message to user, depending on GUI/Console mode
			//=============================================================
			
			if (modMain.asAppSettings.IsConsole)
			{
				Console.WriteLine(ErrorMessage);
			}
			else
			{
				Interaction.MsgBox(ErrorMessage, MsgBoxStyle, Caption);
			}
			
		}
		
		public void ScanFiles(bool CommentScan, bool CodeScan)
		{
			// Iterate through the files in the directory and compile the results
			//===================================================================
			
			string[] arrShortName = null;
			string strLine = "";
			string strScanResult = "";
			var strTrimmedComment = "";
			
			bool blnIsCommented = false;
			bool blnIsColoured = false;
			
			
			if (CommentScan == false && CodeScan == false)
			{
				return;
			}
			
			// Initialise variables before proceeding
			modMain.rtResultsTracker.Reset();
			if (!modMain.asAppSettings.IsConsole)
			{
				frmBreakdown.Default.Close();
				ShowLoading("Collecting Info...", modMain.rtResultsTracker.FileList.Count);
			}
			else
			{
				Console.WriteLine("Collecting Info...");
			}
			
			
			//== Open output files as required ==
			if (modMain.asAppSettings.IsOutputFile)
			{
				swOutputFile = new StreamWriter(modMain.asAppSettings.OutputFile);
			}
			
			
			//== Iterate through files in selected directory and perform selected scans ==
			if (modMain.rtResultsTracker.FileList.Count != null)
			{
				
				if (modMain.asAppSettings.AltSingleLineComment.StartsWith("\\"))
				{
					strTrimmedComment = modMain.asAppSettings.AltSingleLineComment.TrimStart(new char[] {'\\'});
				}
				
				modMain.ctCodeTracker.ResetCDictionaries();
				
				foreach (string strItem in modMain.rtResultsTracker.FileList)
				{
					//
					//todo: add threading here
					//
					
					if (!File.Exists(strItem))
					{
						continue;
					}
					
					IncrementLoadingBar(strItem);
					
					arrShortName = strItem.Split(new char[] {'\\'});
					modMain.ctCodeTracker.Reset();
					
					// Check for php.ini file and handle this separately
					if ((modMain.asAppSettings.TestType == AppSettings.PHP && arrShortName.Last().ToLower() == "php.ini") && modMain.asAppSettings.IsConfigOnly == false)
					{
						
						modMain.ctCodeTracker.HasDisableFunctions = false;
						
						foreach (string tempLoopVar_strLine in File.ReadAllLines(strItem))
						{
							strLine = tempLoopVar_strLine;
							modPHPCheck.CheckPhpIni(strLine, strItem);
						}
						
						if (modMain.ctCodeTracker.HasDisableFunctions == true)
						{
							ListCodeIssue("Failure to use 'disable_functions'", "The ini file fails to use the 'disable_functions' feature, greatly increasing the segverity of a successful compromise.", strItem);
						}
						
						
					}
					else
					{
						
						// Otherwise split the line into code and comments and scan each part
						foreach (string tempLoopVar_strLine in File.ReadAllLines(strItem))
						{
							strLine = tempLoopVar_strLine;
							
							modMain.rtResultsTracker.OverallLineCount++;
							modMain.rtResultsTracker.LineCount++;
							
							if (!string.IsNullOrWhiteSpace(strLine)) // Check that it's not a blank line
							{
								if (blnIsCommented == false) // Check whether we're already inside a comment block
								{
									
									// Multiple conditions for single line comments.
									// A lot of the difficulties caused by VB and PHP which have two types of single line comment
									// and VB/COBOL which have no multiple line comment.
									
									if (modMain.asAppSettings.TestType == AppSettings.COBOL && (strLine.TrimStart().Substring(0, 1) == modMain.asAppSettings.SingleLineComment || Regex.IsMatch(strLine, "^(\\s*\\d{6}\\s*)\\*")))
									{
										// COBOL's system for whole-line comments is simpler than other languages
										strScanResult = System.Convert.ToString(ScanLine(CommentScan, CodeScan, strLine, strItem, modMain.asAppSettings.SingleLineComment, ref blnIsColoured).ToString());
									}
									else if ((strLine.Contains(modMain.asAppSettings.SingleLineComment) && modMain.asAppSettings.SingleLineComment == "//" && !strLine.ToLower().Contains("http:" + modMain.asAppSettings.SingleLineComment)) || (modMain.asAppSettings.TestType == AppSettings.SQL && strLine.Contains(modMain.asAppSettings.SingleLineComment)) || (modMain.asAppSettings.TestType == AppSettings.VB && strLine.Contains(modMain.asAppSettings.SingleLineComment) && !(strLine.Contains("\"") && (strLine.IndexOf("\"") + 1 < strLine.IndexOf("'") + 1))))
									{
										strScanResult = System.Convert.ToString(ScanLine(CommentScan, CodeScan, strLine, strItem, modMain.asAppSettings.SingleLineComment, ref blnIsColoured).ToString());
									}
									else if (modMain.asAppSettings.AltSingleLineComment.Trim() != "" && Regex.IsMatch(strLine, "\\b" + modMain.asAppSettings.AltSingleLineComment + "\\b"))
									{
										strScanResult = System.Convert.ToString(ScanLine(CommentScan, CodeScan, strLine, strItem, strTrimmedComment, ref blnIsColoured).ToString());
									}
									else if ((!(modMain.asAppSettings.TestType == AppSettings.VB)) && (strLine.Contains(modMain.asAppSettings.BlockStartComment) && strLine.Contains(modMain.asAppSettings.BlockEndComment)) && (strLine.IndexOf(modMain.asAppSettings.BlockStartComment) + 1 < strLine.IndexOf(modMain.asAppSettings.BlockEndComment) + 1 ) && !(strLine.Contains("/*/")))
									{
										strScanResult = System.Convert.ToString(ScanLine(CommentScan, CodeScan, strLine, strItem, "both", ref blnIsColoured).ToString());
									}
									else if ((!(modMain.asAppSettings.TestType == AppSettings.VB)) && (strLine.Contains(modMain.asAppSettings.BlockStartComment) && !(strLine.Contains("/*/")) && !(strLine.Contains("print") && (strLine.IndexOf(modMain.asAppSettings.BlockStartComment) + 1 > strLine.IndexOf("print") + 1))))
									{
										blnIsCommented = true;
										strScanResult = System.Convert.ToString(ScanLine(CommentScan, CodeScan, strLine, strItem, modMain.asAppSettings.BlockStartComment, ref blnIsColoured).ToString());
									}
									else
									{
										modMain.rtResultsTracker.OverallCodeCount++;
										modMain.rtResultsTracker.CodeCount++;
										
										// Scan code for dangerous functions
										modMain.CheckCode(strLine, strItem);
									}
									
								}
								else if ((!(modMain.asAppSettings.TestType == AppSettings.VB)) && strLine.Contains(modMain.asAppSettings.BlockEndComment)) // End of a comment block
								{
									blnIsCommented = false;
									strScanResult = System.Convert.ToString(ScanLine(CommentScan, CodeScan, strLine, strItem, modMain.asAppSettings.BlockEndComment, ref blnIsColoured).ToString());
								}
								else
								{
									modMain.rtResultsTracker.CommentCount++;
									modMain.rtResultsTracker.OverallCommentCount++;
								}
								
							}
							else
							{
								//== If we have whitespace then add it to the whitespace count ==
								modMain.rtResultsTracker.OverallWhitespaceCount++;
								modMain.rtResultsTracker.WhitespaceCount++;
							}
						}
						
						//== List any file-level code issues (mis-matched deletes, mallocs, etc.) ==
						if (modMain.asAppSettings.TestType == AppSettings.C | modMain.asAppSettings.TestType == AppSettings.JAVA)
						{
							modMain.CheckFileLevelIssues(strItem);
						}
					}
					
					//== Update graphs and tables ==
					if (modMain.asAppSettings.IsConsole == false)
					{
						UpdateGraphs(strItem, arrShortName, blnIsColoured);
						blnIsColoured = false;
					}
					else
					{
						if (modMain.asAppSettings.IsVerbose == true)
						{
							Console.WriteLine("Scanning file: " + strItem);
						}
					}
					modMain.rtResultsTracker.FileCount++;
					modMain.rtResultsTracker.ResetFileCountVars();
					
					//== Avoid the GUI locking or hanging during processing ==
					Application.DoEvents();
				}
			}
			
			//== Close output files if required ==
			if (modMain.asAppSettings.IsOutputFile)
			{
				swOutputFile.Close();
			}
			
			//== Export CSV results if required ==
			if (modMain.asAppSettings.IsCsvOutputFile)
			{
				ExportResultsCSV(intFilterMin, intFilterMax, modMain.asAppSettings.CsvOutputFile);
			}
			
			//== Export XML results if required ==
			if (modMain.asAppSettings.IsXmlOutputFile)
			{
				ExportResultsXML(intFilterMin, intFilterMax, modMain.asAppSettings.XmlOutputFile);
			}
			
			if (modMain.asAppSettings.IsConsole == true)
			{
				Console.WriteLine("Finished scanning...");
				Console.WriteLine("Closing VCG.");
				this.Dispose();
			}
			
		}
		
		public bool ScanLine(bool CommentScan, bool CodeScan, string CodeLine, string FileName, string Comment, ref bool IsColoured)
		{
			// Split the line into comments and code before carrying out the relevant checks
			// N.B. - InStr has been used as Split requires a single character
			//==============================================================================
			
			string strCode = "";
			string strComment = "";
			
			bool blnRetVal = false;
			
			string[] arrSubStrings = null;
			string[] arrTemp = null;
			
			int intPos = 0;
			
			//== Split line into comments and code ==
			if (Comment == "both")
			{
				intPos = CodeLine.IndexOf(modMain.asAppSettings.BlockStartComment) + 1;
				arrSubStrings = new[] {CodeLine.Substring(0, intPos - 1), CodeLine.Substring(intPos + 1)};
				intPos = (arrSubStrings[1]).IndexOf(modMain.asAppSettings.BlockEndComment) + 1;
				arrTemp = new[] {arrSubStrings[1].Substring(0, intPos - 1), arrSubStrings[1].Substring(intPos + 1)};
			}
			else
			{
				intPos = CodeLine.IndexOf(Comment) + 1;
				if (CodeLine.Length > intPos)
				{
					arrSubStrings = new[] {CodeLine.Substring(0, intPos - 1), CodeLine.Substring(intPos + Comment.Length - 1)};
				}
				else
				{
					arrSubStrings = new[] {CodeLine.Substring(0, intPos - 1), ""};
				}
			}
			
			//== The position of code and comments in the array depends on type of comment ==
			if (Comment == modMain.asAppSettings.SingleLineComment || Comment == modMain.asAppSettings.BlockStartComment)
			{
				strCode = System.Convert.ToString(arrSubStrings[0].Trim());
				strComment = System.Convert.ToString(arrSubStrings[1].Trim());
			}
			else if (Comment == modMain.asAppSettings.BlockEndComment)
			{
				strCode = System.Convert.ToString(arrSubStrings[1].Trim());
				strComment = System.Convert.ToString(arrSubStrings[0].Trim());
			}
			else if (Comment == "both")
			{
				strCode = System.Convert.ToString(arrSubStrings[0].Trim() + arrTemp[1].Trim());
				strComment = System.Convert.ToString(arrSubStrings[1].Trim());
			}
			
			//== Check comment content ==
			if (CommentScan && strComment.Length > 0)
			{
				blnRetVal = CheckComment(strComment, FileName, ref IsColoured);
			}
			
			//== Scan code for dangerous functions ==
			if (CodeScan && strCode.Length > 0)
			{
				modMain.CheckCode(strCode, FileName);
			}
			
			return blnRetVal;
			
		}
		
		public bool CheckComment(string CodeLine, string FileName, ref bool IsColoured)
		{
			// Scan comment content for anything requiring attention and return results
			//=========================================================================
			// First two params used for the scan, and to create return val
			// Final param passed by reference, solely to be altered for the purposes of calling func
			//-------------------------------------------------------------------------
			
			bool blnRetVal = false;
			
			Font fntTitleFont = new Font("Century Gothic", 9, FontStyle.Bold, GraphicsUnit.Point);
			Font fntTextFont = new Font("Century Gothic", 9, FontStyle.Regular, GraphicsUnit.Point);
			
			string strTitle = "";
			string strDescription = "";
			
			
			modMain.rtResultsTracker.OverallCommentCount++;
			modMain.rtResultsTracker.CommentCount++;
			
			//== Look through comments for anything indicating unfixed or shoddy code ==
			foreach (string strComment in modMain.asAppSettings.BadComments)
			{
				if (CodeLine.ToLower().Contains(strComment))
				{
					blnRetVal = true;
					
					strTitle = "Comment Indicates Potentially Unfinished Code - " + Constants.vbNewLine;
					strDescription = " Line: " + System.Convert.ToString(modMain.rtResultsTracker.LineCount) + " - " + FileName + Constants.vbNewLine;
					WriteResult(strTitle, strDescription, CodeLine, CodeIssue.INFO);
					
					modMain.rtResultsTracker.FixMeCount++;
					modMain.rtResultsTracker.OverallFixMeCount++;
					IsColoured = true;
					
					// Update collection and listbox
					AddToResultCollection("Comment Indicates Potentially Unfinished Code", "The comment includes some wording which indicates that the developer regards it as unfinished or does not trust it to work correctly.", FileName, CodeIssue.INFO, Convert.ToInt32(modMain.rtResultsTracker.LineCount), CodeLine);
					
					break;
				}
			}
			
			//== Check for any passwords included in comments ==
			if (Regex.IsMatch(CodeLine, "password\\s*="))
			{
				// Update collection and listbox
				AddToResultCollection("Comment Appears to Contain Password", "The comment appears to include a password. If the password is valid then it could allow access to unauthorised users.", FileName, CodeIssue.HIGH, Convert.ToInt32(modMain.rtResultsTracker.LineCount), CodeLine);
				blnRetVal = true;
			}
			
			return blnRetVal;
			
		}
		
		public void ListCodeIssue(string FunctionName, string Description, string FileName, int Severity = 0, string CodeLine = "", int LineNumber = 0)
		{
			// Notify user of any code issues found for the bad function list from files or the language-specific tests
			//=========================================================================================================
			string strTitle = "";
			string strDescription = "";
			
			
			if (modMain.asAppSettings.OutputLevel < Severity)
			{
				return;
			}
			
			if (LineNumber == 0)
			{
				LineNumber = Convert.ToInt32(modMain.rtResultsTracker.LineCount);
			}
			
			// Set issue title and description
			strTitle = "Potentially Unsafe Code - " + FunctionName + Constants.vbNewLine;
			strDescription = "Line: " + System.Convert.ToString(LineNumber) + " - " + FileName + Constants.vbNewLine + Description + Constants.vbNewLine;
			
			WriteResult(strTitle, strDescription, CodeLine, Severity);
			
			// Update data/count
			modMain.rtResultsTracker.BadFuncCount++;
			modMain.rtResultsTracker.OverallBadFuncCount++;
			
			// Update collection and listbox
			AddToResultCollection(FunctionName, Description, FileName, Severity, LineNumber, CodeLine);
			
		}
		
		public void ListMemoryIssue(Dictionary<string, string> IssueDictionary)
		{
			// Notify user of any issues found in the dictionary of variable names and associated memory mis-management
			//=========================================================================================================
			string strTitle = "";
			string strDescription = "";
			
			string[] arrDescriptions = null;
			string[] arrFragments = null;
			
			foreach (var kyKey in IssueDictionary.Keys)
			{
				strTitle = "Potential Memory Mis-management. Variable Name: " + System.Convert.ToString(kyKey) + Constants.vbNewLine;
				arrDescriptions = IssueDictionary[kyKey].Split(new char[] {'|'});
				
				foreach (var strItem in arrDescriptions)
				{
					strDescription += strItem + Constants.vbNewLine;
				}
				strDescription += Constants.vbNewLine;
				
				WriteResult(strTitle, strDescription, "");
				
				// Update data/count
				modMain.rtResultsTracker.BadFuncCount++;
				modMain.rtResultsTracker.OverallBadFuncCount++;
				
				arrFragments = Regex.Split(arrDescriptions.Last(), "FileName: ");
				
				// Update collection and listbox
				AddToResultCollection("Potential Memory Mis-management. Variable Name: " + System.Convert.ToString(kyKey), arrDescriptions.First(), System.Convert.ToString(arrFragments.Last().Trim()));
				
			}
			
		}
		
		public void WriteResult(string Title, string Description, string CodeLine, int Severity = 0)
		{
			// Write results out to main screen in appropriate format
			//=======================================================
			
			Font fntTitleFont = new Font("Century Gothic", 9, FontStyle.Bold, GraphicsUnit.Point);
			Font fntTextFont = new Font("Century Gothic", 9, FontStyle.Regular, GraphicsUnit.Point);
			Font fntCodeFont = new Font("Consolas", 9, FontStyle.Regular, GraphicsUnit.Point);
			
			if (modMain.asAppSettings.OutputLevel < Severity)
			{
				return;
			}
			
			
			if (modMain.asAppSettings.IsConsole == false)
			{
				
				
				// Set font style and colour for title
				rtbResults.Select(Convert.ToInt32(lngPosition), Title.Length);
				rtbResults.SelectionFont = fntTitleFont;
				
				if (Title.Trim() != "")
				{
					switch (Severity)
					{
						case CodeIssue.CRITICAL:
							rtbResults.SelectionColor = Color.Purple;
							Title = "CRITICAL: " + Title;
							break;
						case CodeIssue.HIGH:
							rtbResults.SelectionColor = Color.Red;
							Title = "HIGH: " + Title;
							break;
						case CodeIssue.MEDIUM:
							rtbResults.SelectionColor = Color.Orange;
							Title = "MEDIUM: " + Title;
							break;
						case CodeIssue.LOW:
							rtbResults.SelectionColor = Color.CornflowerBlue;
							Title = "LOW: " + Title;
							break;
						case CodeIssue.INFO:
							rtbResults.SelectionColor = Color.Blue;
							Title = "SUSPICIOUS COMMENT: " + Title;
							break;
						case CodeIssue.POSSIBLY_SAFE:
							rtbResults.SelectionColor = Color.Green;
							Title = "POTENTIAL ISSUE: " + Title;
							break;
						default:
							rtbResults.SelectionColor = Color.Goldenrod;
							Title = "STANDARD: " + Title;
							break;
					}
					
					rtbResults.AppendText(Title);
					lngPosition += Title.Length;
				}
				
				// Set font style and colour for description
				rtbResults.Select(Convert.ToInt32(lngPosition), Description.Length);
				rtbResults.SelectionFont = fntTextFont;
				rtbResults.SelectionColor = Color.Black;
				
				rtbResults.AppendText(Description);
				lngPosition += Description.Length;
				
				// Set font style and colour for code
				if (CodeLine.Trim() != "")
				{
					CodeLine += Constants.vbNewLine + Constants.vbNewLine;
					
					rtbResults.Select(Convert.ToInt32(lngPosition), CodeLine.Length);
					rtbResults.SelectionFont = fntCodeFont;
					rtbResults.SelectionColor = Color.Black;
					
					rtbResults.AppendText(CodeLine);
					lngPosition += CodeLine.Length;
				}
				else
				{
					rtbResults.AppendText(Constants.vbNewLine);
					lngPosition += Constants.vbNewLine.Length;
				}
			}
			
			//== Write details to output files if required ==
			if (modMain.asAppSettings.IsOutputFile)
			{
				swOutputFile.Write(Title);
				swOutputFile.Write(Description);
			}
			
		}
		
		private void AddToResultCollection(string Title, string Description, string FileName, int Severity = 0, int LineNumber = 0, string CodeLine = "", bool IsChecked = false, string CheckColour = "")
		{
			// Build results collection and add into results listbox
			//======================================================
			
			ScanResult srScanResult = new ScanResult();
			ListViewItem lviItem = new ListViewItem();
			Color colOriginalColour = modMain.asAppSettings.ListItemColour;
			FileGroup fgFileGroup = new FileGroup();
			IssueGroup igIssueGroup = new IssueGroup();
			ColorConverter ccConverter = new ColorConverter();
			
			string[] arrInts = null;
			int intR = 0; // For colour represented as RGB components
			int intG = 0;
			int intB = 0;
			
			if (modMain.asAppSettings.OutputLevel < Severity)
			{
				return;
			}
			
			
			//== Add to results collection ==
			srScanResult.ItemKey = modMain.rtResultsTracker.CurrentIndex;
			srScanResult.Title = Title;
			srScanResult.Description = Description;
			srScanResult.FileName = FileName;
			srScanResult.SetSeverity(Severity);
			srScanResult.LineNumber = LineNumber;
			srScanResult.CodeLine = CodeLine;
			srScanResult.IsChecked = IsChecked;
			
			if (CheckColour.Contains(","))
			{
				arrInts = CheckColour.Split(new char[] {','});
				intR = int.Parse(arrInts[0]);
				intG = int.Parse(arrInts[1]);
				intB = int.Parse(arrInts[2]);
				srScanResult.CheckColour = Color.FromArgb(intR, intG, intB);
			}
			else
			{
				srScanResult.CheckColour = (Color) (ccConverter.ConvertFromString(CheckColour));
			}
			
			
			modMain.rtResultsTracker.ScanResults.Add(srScanResult);
			
			//== If this is a 'fix me' comment then add it to the comments collection ==
			if (Severity == CodeIssue.INFO)
			{
				modMain.rtResultsTracker.FixMeList.Add(srScanResult);
			}
			
			
			if (!modMain.asAppSettings.IsConsole)
			{
				//== Add to listview ==
				lviItem.Name = Convert.ToString(modMain.rtResultsTracker.CurrentIndex);
				lviItem.Text = Convert.ToString(srScanResult.Severity());
				lviItem.SubItems.Add(srScanResult.SeverityDesc());
				lviItem.SubItems.Add(Title);
				lviItem.SubItems.Add(Description);
				lviItem.SubItems.Add(FileName);
				lviItem.SubItems.Add(Convert.ToString(LineNumber));
				
				lvResults.Items.Add(lviItem);
				if (srScanResult.IsChecked == true)
				{
					SetCheckState(Convert.ToInt32(lviItem.Name), true, srScanResult.CheckColour);
				}
			}
			
			//== Add to results groups ==
			if (modMain.rtResultsTracker.FileGroups.ContainsKey(FileName))
			{
				// Add the issue to the array in dictionary entry for this file
				modMain.rtResultsTracker.FileGroups[FileName].AddDetail(modMain.rtResultsTracker.CurrentIndex, Severity, Title, Description, LineNumber, CodeLine);
			}
			else
			{
				// Create a new file group and add the first issue
				fgFileGroup.FileName = FileName;
				fgFileGroup.AddDetail(modMain.rtResultsTracker.CurrentIndex, Severity, Title, Description, LineNumber, CodeLine);
				modMain.rtResultsTracker.FileGroups.Add(FileName, fgFileGroup);
			}
			
			if (modMain.rtResultsTracker.IssueGroups.ContainsKey(Title))
			{
				// Add the issue to the array in dictionary entry for this general issue title
				modMain.rtResultsTracker.IssueGroups[Title].AddDetail(modMain.rtResultsTracker.CurrentIndex, FileName, LineNumber, CodeLine);
			}
			else
			{
				// Create a new issue title group and add the first issue
				igIssueGroup.Title = Title;
				igIssueGroup.SetSeverity(Severity);
				igIssueGroup.AddDetail(modMain.rtResultsTracker.CurrentIndex, FileName, LineNumber, CodeLine);
				modMain.rtResultsTracker.IssueGroups.Add(Title, igIssueGroup);
			}
			
			modMain.rtResultsTracker.CurrentIndex++;
			
			modMain.asAppSettings.ListItemColour = colOriginalColour;
			
		}
		
		public void SortRTBResults(int SortCriteria = 0)
		{
			// Sort results and display in rich text box
			//==========================================
			SeverityComparer scSevComp = new SeverityComparer();
			FileComparer fcFileComp = new FileComparer();
			string strDescription = "";
			bool blnCurrentOrder = false;
			
			
			rtbResults.Text = "";
			
			switch (SortCriteria)
			{
				case PRIORITY_COL:
					blnCurrentOrder = blnIsAscendingSeverity;
					blnIsAscendingSeverity = true;
					modMain.rtResultsTracker.ScanResults.Sort(scSevComp);
					blnIsAscendingSeverity = blnCurrentOrder;
					break;
				case FILE_COL:
					blnCurrentOrder = blnIsAscendingFile;
					blnIsAscendingFile = true;
					modMain.rtResultsTracker.ScanResults.Sort(fcFileComp);
					blnIsAscendingFile = blnCurrentOrder;
					break;
				default:
					break;
					// Do Nothing
			}
			
			foreach (var srResultItem in modMain.rtResultsTracker.ScanResults)
			{
				if ((Convert.ToInt32(srResultItem.Severity) <= intFilterMin) && (Convert.ToInt32(srResultItem.Severity) >= intFilterMax))
				{
					strDescription = System.Convert.ToString(srResultItem.Description.ToString());
					WriteResult(srResultItem.Title.ToString() + Constants.vbNewLine, strDescription.ToString() + Constants.vbNewLine + "Line: " + srResultItem.LineNumber.ToString() + " - Filename: " + srResultItem.FileName.ToString() + Constants.vbNewLine, System.Convert.ToString(srResultItem.CodeLine.Trim.ToString()), Convert.ToInt32(srResultItem.Severity));
				}
				
				//== Avoid the GUI locking or hanging during processing ==
				//Application.DoEvents()
			}
			
		}
		
		private void GroupRTBByIssue()
		{
			// Re-write RTB results, grouped by issue
			//=======================================
			string strDescription = "";
			string strTitle = "";
			bool blnIsFirst = true;
			
			
			rtbResults.Text = "";
			
			// Loop through the issues
			foreach (KeyValuePair<string, IssueGroup> idIssueDic in modMain.rtResultsTracker.IssueGroups)
			{
				
				strTitle = System.Convert.ToString(idIssueDic.Key);
				strDescription = "";
				
				// Check it is within range of severity filter
				if (idIssueDic.Value.Severity()  <= intFilterMin && idIssueDic.Value.Severity()  >= intFilterMax)
				{
					foreach (KeyValuePair<int, string[]> arrDetails in idIssueDic.Value.GetDetails())
					{
						strDescription += Constants.vbNewLine + "Line: " + arrDetails.Value[IssueGroup.LINE] + " - Filename: " + arrDetails.Value[IssueGroup.FILE];
						if (arrDetails.Value[IssueGroup.CODE].Trim()  != "")
						{
							strDescription += Constants.vbNewLine + arrDetails.Value[IssueGroup.CODE].Trim();
						}
					}
				}
				
				WriteResult(strTitle, strDescription + Constants.vbNewLine, "", System.Convert.ToInt32(idIssueDic.Value.Severity()));
				
				//== Avoid the GUI locking or hanging during processing ==
				//Application.DoEvents()
			}
			
		}
		
		private void GroupRTBByFile()
		{
			// Re-write RTB results, grouped by file
			//======================================
			string strDescription = "";
			int intSeverity = CodeIssue.POSSIBLY_SAFE;
			string strTitle = "";
			bool blnIsFirst = true;
			
			
			rtbResults.Text = "";
			
			// Loop through the issues
			foreach (KeyValuePair<string, FileGroup> fdFileDic in modMain.rtResultsTracker.FileGroups)
			{
				
				strTitle = System.Convert.ToString(fdFileDic.Key);
				strDescription = "";
				intSeverity = CodeIssue.POSSIBLY_SAFE;
				
				foreach (KeyValuePair<int, string[]> arrDetails in fdFileDic.Value.GetDetails())
				{
					
					if (intSeverity > fdFileDic.Value.GetSeverity(arrDetails.Key))
					{
						intSeverity = System.Convert.ToInt32(fdFileDic.Value.GetSeverity(arrDetails.Key));
					}
					
					if (intSeverity <= intFilterMin & intSeverity >= intFilterMax)
					{
						strDescription += Constants.vbNewLine + "Line: " + arrDetails.Value[FileGroup.LINE] + " - " + arrDetails.Value[FileGroup.SEVERITY] + ": " + arrDetails.Value[FileGroup.ISSUE] + Constants.vbNewLine + arrDetails.Value[FileGroup.DESC];
						if (arrDetails.Value[IssueGroup.CODE].Trim()  != "")
						{
							strDescription += System.Convert.ToString(arrDetails.Value[IssueGroup.CODE].Trim());
						}
					}
				}
				
				// only write to screen if the issue is within filter bounds
				if (intSeverity <= intFilterMin & intSeverity >= intFilterMax)
				{
					WriteResult(strTitle, strDescription + Constants.vbNewLine, "", intSeverity);
				}
			}
			
			//== Avoid the GUI locking or hanging during processing ==
			//Application.DoEvents()
			
		}
		
		public void UpdateGraphs(string FileName, string[] ShortName, bool IsColoured)
		{
			// Populate the display of file data and store for later re-use
			//=============================================================
			FileData fdFileData = new FileData();
			
			
			//== Store for later use ==
			fdFileData.ShortName = ShortName[ShortName.Count() - 1];
			fdFileData.FileName = FileName;
			fdFileData.LineCount = modMain.rtResultsTracker.LineCount;
			fdFileData.CodeCount = modMain.rtResultsTracker.CodeCount;
			fdFileData.CommentCount = modMain.rtResultsTracker.CommentCount;
			fdFileData.WhitespaceCount = modMain.rtResultsTracker.WhitespaceCount;
			fdFileData.FixMeCount = modMain.rtResultsTracker.FixMeCount;
			fdFileData.BadFuncCount = modMain.rtResultsTracker.BadFuncCount;
			modMain.rtResultsTracker.FileDataList.Add(fdFileData);
			
			//== Add to chart ==
			if (!modMain.asAppSettings.IsConsole)
			{
				UpdateFileView(fdFileData, modMain.rtResultsTracker.FileCount, IsColoured);
			}
			
			//== Clear variables ==
			modMain.rtResultsTracker.ResetFileCountVars();
			
		}
		
		public void UpdateFileView(FileData FileDetails, int Index, bool IsColoured)
		{
			
			//== Add results into graphs and tables ==
			frmBreakdown.Default.dgvResults.Rows.Add(1);
			if (IsColoured == true)
			{
				frmBreakdown.Default.dgvResults.DefaultCellStyle.BackColor = Color.Red;
			}
			else
			{
				frmBreakdown.Default.dgvResults.DefaultCellStyle.BackColor = Color.White;
			}
			frmBreakdown.Default.dgvResults[0, Index].Value = FileDetails.ShortName;
			frmBreakdown.Default.dgvResults[1, Index].Value = FileDetails.LineCount;
			frmBreakdown.Default.dgvResults[3, Index].Value = FileDetails.CodeCount;
			frmBreakdown.Default.dgvResults[4, Index].Value = FileDetails.CommentCount;
			frmBreakdown.Default.dgvResults[5, Index].Value = FileDetails.WhitespaceCount;
			frmBreakdown.Default.dgvResults[6, Index].Value = FileDetails.FixMeCount;
			frmBreakdown.Default.dgvResults[7, Index].Value = FileDetails.BadFuncCount;
			frmBreakdown.Default.dgvResults[8, Index].Value = FileDetails.FileName;
			
		}
		
		public static void CountFixMeComments(string FileName)
		{
			// If fixme array is not empty, and filename for item is same as in codebreakdown results will populate panel with relevant info
			//==============================================================================================================================
			
			if (!(modMain.rtResultsTracker.FixMeList.Count == null))
			{
				
				foreach (ScanResult srItem in modMain.rtResultsTracker.FixMeList)
				{
					if (srItem.FileName == FileName)
					{
						frmIndividualBreakdown.Default.pnlResults.Visible = true;
						frmIndividualBreakdown.Default.lblBreakdown.Text += "Line: " + System.Convert.ToString(srItem.LineNumber) + Constants.vbNewLine + "Contains: '" + srItem.CodeLine + "'" + Constants.vbNewLine + Constants.vbNewLine;
					}
				}
			}
			
		}
		
		public static void ExportComments()
		{
			// Display all comments which indicate poor/unfinished code as Rich Text
			//======================================================================
			
			string strTitle = "";
			string strDetails = "";
			
			
			if (modMain.rtResultsTracker.OverallLineCount == 0)
			{
				return;
			}
			
			frmCodeDetails.Default.Close();
			frmCodeDetails.Default.rtbCodeDetails.Clear();
			
			dynamic with_1 = frmMain;
			with_1.ShowLoading("Formatting...", modMain.rtResultsTracker.FixMeList.Count);
			
			foreach (ScanResult srItem in modMain.rtResultsTracker.FixMeList)
			{
				strTitle = Constants.vbNewLine + "File: " + srItem.FileName + Constants.vbNewLine;
				strDetails = "Line: " + System.Convert.ToString(srItem.LineNumber) + Constants.vbNewLine + "Contains: '" + srItem.CodeLine + "'" + Constants.vbNewLine;
				with_1.SetRtbText(strTitle);
				with_1.SetRtbCode(strDetails);
				with_1.IncrementLoadingBar(strTitle);
			}
			
			
			frmCodeDetails.Default.rtbCodeDetails.Select(0, 0);
			frmLoading.Default.Close();
			frmCodeDetails.Default.Show();
			
		}
		
		public void SetRtbCode(string CodeString)
		{
			//Set format for the details screen and and display the offending code
			//====================================================================
			
			Font fntFont = new Font("Courier New", 9, FontStyle.Regular, GraphicsUnit.Point);
			
			
			// == NB. it is 71 chars that fill up a typical (vertical horizontal) whitepsace on a word doc ==
			dynamic with_1 = frmCodeDetails;
			with_1.rtbCodeDetails.Select(frmCodeDetails.Default.rtbCodeDetails.Text.Length, 1);
			with_1.rtbCodeDetails.SelectionFont = fntFont;
			with_1.rtbCodeDetails.SelectionColor = Color.Black;
			with_1.rtbCodeDetails.SelectionBackColor = Color.LightGray;
			with_1.rtbCodeDetails.AppendText(CodeString);
			
		}
		
		public void SetRtbText(string TitleString)
		{
			//Set format for the details screen and give details of location
			//==============================================================
			
			Font fntTextFont = new Font("Century Gothic", 10, FontStyle.Regular, GraphicsUnit.Point);
			
			dynamic with_1 = frmCodeDetails;
			with_1.rtbCodeDetails.Select(frmCodeDetails.Default.rtbCodeDetails.Text.Length, 1);
			with_1.rtbCodeDetails.SelectionFont = fntTextFont;
			with_1.rtbCodeDetails.SelectionColor = Color.Black;
			with_1.rtbCodeDetails.AppendText(TitleString);
			
		}
		
		public void ShowLoading(string Title, int NumberOfItems)
		{
			
			if (modMain.asAppSettings.IsConsole == true)
			{
				return;
			}
			
			dynamic with_1 = frmLoading;
			with_1.Show(this);
			with_1.Text = Title;
			with_1.pbProgressBar.Maximum = NumberOfItems;
			
		}
		
		public void IncrementLoadingBar(string LabelText)
		{
			
			if (modMain.asAppSettings.IsConsole == true)
			{
				return;
			}
			
			dynamic with_1 = frmLoading;
			with_1.pbProgressBar.Increment(1);
			with_1.lblProgress.Text = LabelText;
			with_1.Refresh(); //TODO: make the loading bar represent what is being checked for, ie. the title would be "unsafe methods", "Pointer problems", etc. and go to 100 then re-loop
			
			this.Refresh();
			
		}
		
		public void ExitToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		public void BannedFunctionsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show relevant bad functions file for language
			//==============================================
			
			switch (modMain.asAppSettings.TestType)
			{
				case AppSettings.C:
					modMain.LaunchNPP(modMain.asAppSettings.CConfFile);
					break;
				case AppSettings.JAVA:
					modMain.LaunchNPP(modMain.asAppSettings.JavaConfFile);
					break;
				case AppSettings.SQL:
					modMain.LaunchNPP(modMain.asAppSettings.PLSQLConfFile);
					break;
				case AppSettings.CSHARP:
					modMain.LaunchNPP(modMain.asAppSettings.CSharpConfFile);
					break;
				case AppSettings.PHP:
					modMain.LaunchNPP(modMain.asAppSettings.PHPConfFile);
					break;
			}
			
		}
		
		public void StartScanningToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Ensure that config-only scanning is switched off before proceeding with full scan
			//==================================================================================
			
			modMain.asAppSettings.IsConfigOnly = false;
			
			FullScan();
			
		}
		
		private void FullScan()
		{
			// Run the code breakdown from the main form
			//==========================================
			int intIndex = 0;
			
			
			//== GUI or console? ==
			if (modMain.asAppSettings.IsConsole)
			{
				ScanFiles(true, true);
				this.Hide();
			}
			else
			{
				
				//== If no data available then scan files mode ==
				if (modMain.rtResultsTracker.OverallLineCount == 0)
				{
					frmBreakdown.Default.dgvResults.SelectAll();
					frmBreakdown.Default.dgvResults.Rows.Clear();
					ScanFiles(true, true);
				}
				else
				{
					for (intIndex = 0; intIndex <= modMain.rtResultsTracker.FileDataList.Count - 1; intIndex++)
					{
						UpdateFileView(modMain.rtResultsTracker.FileDataList[intIndex] as FileData, intIndex, false);
					}
				}
				
				ShowResults();
			}
			
		}
		
		private void ShowPercentages()
		{
			// Fill in percentages on the datagram thing
			//==========================================
			//On Error Resume Next
			
			int intIndex = 0;
			
			if (frmBreakdown.Default.dgvResults.Rows.Count < 2)
			{
				return;
			}
			
			// ToDo: This throws an exception during integer conversion. If possible find a fix and remove the "On Error Resume Next"
			for (intIndex = 0; intIndex <= frmBreakdown.Default.dgvResults.Rows.Count - 2; intIndex++)
			{
				frmBreakdown.Default.dgvResults[2, intIndex].Value = Math.Round((double) Math.Abs(System.Convert.ToInt32(System.Convert.ToInt32(Conversion.Int(frmBreakdown.Default.dgvResults[1, intIndex].Value)) / modMain.rtResultsTracker.OverallLineCount) * 100), 3);
			}
			
		}
		
		private void ShowPieChart()
		{
			// Display pie chart with code breakdown
			//======================================
			
			//== Create chart ==
			frmBreakdown.Default.chtResults.Series[0].Points.AddY(modMain.rtResultsTracker.OverallCodeCount);
			frmBreakdown.Default.chtResults.Series[0].Points.AddY(modMain.rtResultsTracker.OverallWhitespaceCount);
			frmBreakdown.Default.chtResults.Series[0].Points.AddY(modMain.rtResultsTracker.OverallCommentCount);
			frmBreakdown.Default.chtResults.Series[0].Points.AddY(modMain.rtResultsTracker.OverallFixMeCount);
			frmBreakdown.Default.chtResults.Series[0].Points.AddY(modMain.rtResultsTracker.OverallBadFuncCount);
			
			frmBreakdown.Default.chtResults.Series[0].Points[0].LegendText = "Overall code (" + System.Convert.ToString(modMain.rtResultsTracker.OverallCodeCount) + " lines)";
			frmBreakdown.Default.chtResults.Series[0].Points[1].LegendText = "Overall whitespace (" + System.Convert.ToString(modMain.rtResultsTracker.OverallWhitespaceCount) + " lines)";
			frmBreakdown.Default.chtResults.Series[0].Points[2].LegendText = "Overall comments (" + System.Convert.ToString(modMain.rtResultsTracker.OverallCommentCount) + " comments)";
			frmBreakdown.Default.chtResults.Series[0].Points[3].LegendText = "Potentially broken/unfinished flags (" + System.Convert.ToString(modMain.rtResultsTracker.OverallFixMeCount) + " Counts)";
			frmBreakdown.Default.chtResults.Series[0].Points[4].LegendText = "Potentially dangerous code (" + System.Convert.ToString(modMain.rtResultsTracker.OverallBadFuncCount) + " Counts)";
			
			frmBreakdown.Default.chtResults.Series["Series1"]["BarLabelStyle"] = "Right";
			frmBreakdown.Default.chtResults.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
			frmBreakdown.Default.chtResults.Series["Series1"]["DrawingStyle"] = "Cylinder";
			
			//== Show percentage breakdowns ==
			frmBreakdown.Default.lblResults.Text = modMain.rtResultsTracker.OverallLineCount + " Lines: " + Constants.vbNewLine + System.Convert.ToString(modMain.rtResultsTracker.OverallCommentCount) + " Comments (~" + System.Convert.ToString(Math.Round((double) Math.Abs(System.Convert.ToInt32((double) modMain.rtResultsTracker.OverallCommentCount / modMain.rtResultsTracker.OverallLineCount) * 100), 1)) + "%)" + Constants.vbNewLine + System.Convert.ToString(modMain.rtResultsTracker.OverallWhitespaceCount) + " Lines of Whitespace (~" + System.Convert.ToString(Math.Round((double) Math.Abs(System.Convert.ToInt32((double) modMain.rtResultsTracker.OverallWhitespaceCount / modMain.rtResultsTracker.OverallLineCount) * 100), 1)) + "%)" + Constants.vbNewLine + System.Convert.ToString(modMain.rtResultsTracker.OverallLineCount - System.Convert.ToInt32(modMain.rtResultsTracker.OverallCommentCount + modMain.rtResultsTracker.OverallWhitespaceCount)) + " Lines of Code (including comment-appended code) (~" + System.Convert.ToString(100 - (Math.Round((double) Math.Abs(System.Convert.ToInt32((double) modMain.rtResultsTracker.OverallCommentCount / modMain.rtResultsTracker.OverallLineCount) * 100), 1) + Math.Round((double) Math.Abs(System.Convert.ToInt32((double) modMain.rtResultsTracker.OverallWhitespaceCount / modMain.rtResultsTracker.OverallLineCount) * 100), 1))) + "%)";
			
		}
		
		public frmMain()
		{
			
			// This call is required by the designer.
			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
			// Add any initialization after the InitializeComponent() call.
			
		}
		
		public void VisualSecurityBreakdownToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show breakdown of good code/bad code
			//=====================================
			int intIndex = 0;
			
			modMain.asAppSettings.IsConfigOnly = false;
			
			//== If no data available then scan files in 'code only' mode ==
			if (modMain.rtResultsTracker.OverallLineCount == 0)
			{
				ScanFiles(false, true);
			}
			else
			{
				for (intIndex = 0; intIndex <= modMain.rtResultsTracker.FileDataList.Count - 1; intIndex++)
				{
					UpdateFileView(modMain.rtResultsTracker.FileDataList[intIndex] as FileData, intIndex, false);
				}
			}
			
			ShowResults();
			
		}
		
		private void ShowResults(bool ShowReminder = false)
		{
			// Display the results in the GUI according to user preferences
			//=============================================================
			
			
			//== Exit if in cmd line only mode ==
			if (modMain.asAppSettings.IsConsole == true)
			{
				return;
			}
			
			//== Show results ==
			tcMain.SelectTab(1);
			tabResults.Focus();
			tabResults.Show();
			frmLoading.Default.Close();
			
			//== Show options for visual breakdown ==
			if (ShowReminder && modMain.asAppSettings.DisplayBreakdownOption == true)
			{
				frmBreakdownReminder.Default.ShowDialog();
			}
			
			//== Show Visual Breakdown if required ==
			if ((!ShowReminder) || (modMain.asAppSettings.VisualBreakdownEnabled))
			{
				ShowPercentages();
				ShowPieChart();
				frmBreakdown.Default.Show();
			}
			
		}
		
		public void VisualCommentBreakdownToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show breakdown of code/comments and ToDo/FixMe comments
			//========================================================
			int intIndex = 0;
			
			modMain.asAppSettings.IsConfigOnly = false;
			
			//== If no data available then scan files in 'code only' mode ==
			if (modMain.rtResultsTracker.OverallLineCount == 0)
			{
				ScanFiles(true, false);
			}
			else
			{
				for (intIndex = 0; intIndex <= modMain.rtResultsTracker.FileDataList.Count - 1; intIndex++)
				{
					UpdateFileView(modMain.rtResultsTracker.FileDataList[intIndex] as FileData, intIndex, false);
				}
			}
			
			ShowResults();
			
		}
		
		public void AboutVCGToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			AboutBox frmAbout = new AboutBox();
			frmAbout.ShowDialog(this);
		}
		
		public void VisualCodeBreakdownToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show breakdown of code/comments/whitespace
			//===========================================
			int intIndex = 0;
			
			//== If no data available then scan files in 'comment only' mode ==
			if (modMain.rtResultsTracker.OverallLineCount == 0)
			{
				modMain.asAppSettings.IsConfigOnly = false;
				ScanFiles(true, false);
			}
			else
			{
				for (intIndex = 0; intIndex <= modMain.rtResultsTracker.FileDataList.Count - 1; intIndex++)
				{
					UpdateFileView(modMain.rtResultsTracker.FileDataList[intIndex] as FileData, intIndex, false);
				}
			}
			
			ShowResults(false);
			
		}
		
		public void ExportFixMeCommentsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			ExportComments();
		}
		
		public void OptionsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show the options dialog
			//========================
			frmOptions frmOpt = new frmOptions();
			
			frmOpt.ShowDialog(this);
			
		}
		
		public void FilterResultsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Filter results according to user requirements
			//==============================================
			
			frmFilter frmResultFilter = new frmFilter();
			frmResultFilter.ShowDialog(this);
			
		}
		
		public void DeleteItemToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			DeleteScanResult(Type.Missing, (EventArgs) Type.Missing);
		}
		
		public void frmMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			// Save settings to registry
			//==========================
			int intIndex = 0;
			
			
			// Is the main Window visible or not
			if (modMain.asAppSettings.IsConsole == false)
			{
				// Save window size and location to Registry
				Interaction.SaveSetting("VisualCodeGrepper", "FormLocation", "Top", Convert.ToString(this.Top));
				Interaction.SaveSetting("VisualCodeGrepper", "FormLocation", "Left", Convert.ToString(this.Left));
				Interaction.SaveSetting("VisualCodeGrepper", "FormSize", "Height", Convert.ToString(this.Height));
				Interaction.SaveSetting("VisualCodeGrepper", "FormSize", "Width", Convert.ToString(this.Width));
				
				// Save previous filenames to Registry
				for (intIndex = 0; intIndex <= 4; intIndex++)
				{
					Interaction.SaveSetting("VisualCodeGrepper", "Startup", "Target" + System.Convert.ToString(intIndex), System.Convert.ToString(cboTargetDir.Items[intIndex].ToString()));
				}
			}
			
			// Save Language and test settings to registry
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "Language", Convert.ToString(modMain.asAppSettings.StartType));
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "CConfFile", modMain.asAppSettings.CConfFile);
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "JavaConfFile", modMain.asAppSettings.JavaConfFile);
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "PLSQLConfFile", modMain.asAppSettings.PLSQLConfFile);
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "CSConfFile", modMain.asAppSettings.CSharpConfFile);
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "VBConfFile", modMain.asAppSettings.VBConfFile);
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "PHPConfFile", modMain.asAppSettings.PHPConfFile);
			Interaction.SaveSetting("VisualCodeGrepper", "Startup", "COBOLConfFile", modMain.asAppSettings.COBOLConfFile);
			
		}
		
		public void frmMain_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// Implement keyboard shortcuts
			//=============================
			
			if (e.KeyCode == Keys.F & e.Modifiers == Keys.Control) // Find
			{
				tcMain.SelectedTab = tabResults;
				tabResults.Focus();
				FindText();
			}
			else if (e.KeyCode == Keys.F & e.Modifiers == Keys.Control & e.Modifiers == Keys.Shift) // Find next
			{
				tcMain.SelectedTab = tabResults;
				tabResults.Focus();
				if (!string.IsNullOrEmpty(strPrevSearch))
				{
					rtbResults.Find(strPrevSearch);
				}
			}
			else if ((e.KeyCode == Keys.F5 | (e.KeyCode == Keys.R & e.Modifiers == Keys.Control)) && !string.IsNullOrEmpty(modMain.rtResultsTracker.TargetDirectory)) // Scan
			{
				FullScan();
			}
			
		}
		
		public void frmMain_Load(object sender, System.EventArgs e)
		{
			// Get settings from registry and apply to app
			//============================================
			int intIndex = 0;
			
			
			//== Get bad comments from config file ==
			modMain.LoadBadComments();
			
			// Get Language and test settings from registry
			modMain.asAppSettings.TestType = Convert.ToInt32(Interaction.GetSetting("VisualCodeGrepper", "Startup", "Language", "0"));
			modMain.asAppSettings.CConfFile = Interaction.GetSetting("VisualCodeGrepper", "Startup", "CConfFile", Application.StartupPath + "\\cppfunctions.conf");
			modMain.asAppSettings.JavaConfFile = Interaction.GetSetting("VisualCodeGrepper", "Startup", "JavaConfFile", Application.StartupPath + "\\javafunctions.conf");
			modMain.asAppSettings.PLSQLConfFile = Interaction.GetSetting("VisualCodeGrepper", "Startup", "PLSQLConfFile", Application.StartupPath + "\\plsqlfunctions.conf");
			modMain.asAppSettings.CSharpConfFile = Interaction.GetSetting("VisualCodeGrepper", "Startup", "CSConfFile", Application.StartupPath + "\\csfunctions.conf");
			modMain.asAppSettings.VBConfFile = Interaction.GetSetting("VisualCodeGrepper", "Startup", "VBConfFile", Application.StartupPath + "\\vbfunctions.conf");
			modMain.asAppSettings.PHPConfFile = Interaction.GetSetting("VisualCodeGrepper", "Startup", "PHPConfFile", Application.StartupPath + "\\phpfunctions.conf");
			modMain.asAppSettings.COBOLConfFile = Interaction.GetSetting("VisualCodeGrepper", "Startup", "COBOLConfFile", Application.StartupPath + "\\cobolfunctions.conf");
			
			
			// Parse and process any command line args
			intIndex = modMain.ParseArgs();
			
			if (modMain.asAppSettings.IsConsole)
			{
				AttachConsole(-1);
			}
			
			// Implement context menu for text boxes, etc.
			AddContextMenu();
			
			// Get previous window size and location from Registry
			this.Top = Convert.ToInt32(Interaction.GetSetting("VisualCodeGrepper", "FormLocation", "Top", "50"));
			this.Left = Convert.ToInt32(Interaction.GetSetting("VisualCodeGrepper", "FormLocation", "Left", "100"));
			this.Height = Convert.ToInt32(Interaction.GetSetting("VisualCodeGrepper", "FormSize", "Height", "515"));
			this.Width = Convert.ToInt32(Interaction.GetSetting("VisualCodeGrepper", "FormSize", "Width", "835"));
			
			// Reset any bad/corrupted registry entries
			if ((this.Top < 0) || (this.Top > Screen.PrimaryScreen.Bounds.Height - 50))
			{
				this.Top = 50;
			}
			if ((this.Left < 0) || (this.Left > Screen.PrimaryScreen.Bounds.Width - 50))
			{
				this.Left = 100;
			}
			
			// Get previous filenames from registry and load into combo box
			for (intIndex = 0; intIndex <= 4; intIndex++)
			{
				cboTargetDir.Items.Insert(intIndex, Interaction.GetSetting("VisualCodeGrepper", "Startup", "Target" + System.Convert.ToString(intIndex), ""));
			}
			
			// Set current language
			if (modMain.asAppSettings.TestType == AppSettings.COBOL)
			{
				modMain.asAppSettings.IncludeCobol = true;
			}
			modMain.SelectLanguage(modMain.asAppSettings.TestType);
			
			// Import results or carry out scan
			if (!modMain.asAppSettings.IsConsole)
			{
				if (modMain.asAppSettings.IsXmlInputFile)
				{
					ImportResultsXML(modMain.asAppSettings.XmlInputFile);
				}
			}
			
			// Remind user about language selection if required
			if (!modMain.asAppSettings.IsConsole && Interaction.GetSetting("VisualCodeGrepper", "Startup", "LangReminder", "1") != "0")
			{
				frmReminder.Default.ShowDialog(this); // default to true
			}
			
			// Visual Breakdown display options
			if (!modMain.asAppSettings.IsConsole)
			{
				modMain.asAppSettings.DisplayBreakdownOption = Interaction.GetSetting("VisualCodeGrepper", "DisplayOptions", "BreakdownReminder", "1") == "1"; // default to true
				modMain.asAppSettings.VisualBreakdownEnabled = Interaction.GetSetting("VisualCodeGrepper", "DisplayOptions", "ShowBreakdown", "0") != "0"; // default to false
			}
			
		}
		
		public void frmMain_Shown(object sender, System.EventArgs e)
		{
			//If we are in console mode then begin the scan, otherwise show the form
			//======================================================================
			
			if (modMain.asAppSettings.IsConsole == true)
			{
				this.Visible = false;
				FullScan();
			}
			
		}
		
		public void cboTargetDir_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Allow user to drag file/directory into combobox
			//================================================
			string[] arrFiles = null;
			
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				
				// Assign dragged files to array of filenames
				arrFiles = (string[]) (e.Data.GetData(DataFormats.FileDrop, false));
				
				// Assign first name in list to combo box
				cboTargetDir.Text = arrFiles[0];
			}
			
		}
		
		public void cboTargetDir_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Enable dragging of item(s) into combobox
			//=========================================
			
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			
		}
		
		public void cboTargetDir_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// If user presses the 'Enter' key then attemp to scan the directory they've specified
			//====================================================================================
			
			if (e.KeyCode == Keys.Enter)
			{
				if (Directory.Exists(cboTargetDir.Text))
				{
					LoadFiles(cboTargetDir.Text);
				}
				else
				{
					MessageBox.Show("<{0}> does not exist.", cboTargetDir.Text);
				}
			}
		}
		
		public void lbTargets_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			SetDeleteMenu();
		}
		
		public void tcMain_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			SetDeleteMenu();
		}
		
		public void LoadFiles(string TargetFolder, bool ClearPrevious = false)
		{
			//Load files to be scanned
			//========================
			dynamic objResults = default(dynamic);
			int intIndex = 0;
			int intFileCount = 0;
			
			//If TargetFolder.Count = 0 Then Exit Sub
			
			TargetFolder = TargetFolder.Trim();
			if (TargetFolder == "" || TargetFolder == modMain.rtResultsTracker.TargetDirectory)
			{
				return;
			}
			
			if (ClearPrevious)
			{
				ClearResults();
			}
			cboTargetDir.Text = TargetFolder;
			
			
			try
			{
				// Check whether we have a file or directory
				if (Directory.Exists(TargetFolder))
				{
					// Iterate through files from target directory and obtain all relevant filenames
					ThreadedFileCollector thrdFileCollector = new ThreadedFileCollector();
					DirectoryInfo di = new DirectoryInfo(TargetFolder);
					objResults = thrdFileCollector.CollectFiles(di, "*.*");
					
					if (modMain.asAppSettings.IsConsole == false)
					{
						ShowLoading("Loading files from target directory...", Convert.ToInt32(objResults.Count));
					}
					else
					{
						Console.WriteLine("Loading files from target directory...");
						Console.WriteLine();
					}
				}
				else if (File.Exists(TargetFolder))
				{
					objResults = new Collection();
					objResults.Add(TargetFolder);
				}
				else
				{
					//this is not an existing file or folder
					return ;
				}
				
				foreach (var objTargetFile in objResults as IEnumerable)
				{
					modMain.rtResultsTracker.FileList.Add(objTargetFile);
				}
				
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				else
				{
					Console.WriteLine();
					Console.WriteLine("Loaded " + System.Convert.ToString(modMain.rtResultsTracker.FileList.Count) + " files from target directory.");
					Console.WriteLine();
				}
				
				
				if (modMain.rtResultsTracker.FileList.Count == 0)
				{
					DisplayError("No target files for the selected language could be found in this location", "Error", MsgBoxStyle.Exclamation);
				}
				else
				{
					
					//MsgBox(rtResultsTracker.FileList.Count & " Files loaded", MsgBoxStyle.Information, "Success")
					if (modMain.asAppSettings.IsConsole == false)
					{
						sslLabel.Text += "   [" + System.Convert.ToString(modMain.rtResultsTracker.FileList.Count) + " Files]";
					}
					
					if (modMain.asAppSettings.IsConsole == false)
					{
						
						// Enable scan menus
						SetScanMenus(true);
						
						foreach (string item in modMain.rtResultsTracker.FileList)
						{
							lbTargets.Items.Add(item);
						}
						
						modMain.rtResultsTracker.TargetDirectory = TargetFolder;
						cboTargetDir.Text = TargetFolder;
						
						//== Add to list of previous targets ==
						if (!cboTargetDir.Items.Contains(cboTargetDir.Text))
						{
							// Move first 4 items along to next space in the list
							for (intIndex = 3; intIndex >= 0; intIndex--)
							{
								cboTargetDir.Items.Item[intIndex + 1] = cboTargetDir.Items.Item[intIndex];
							}
							cboTargetDir.Items.Item[0] = cboTargetDir.Text;
						}
					}
				}
				
			}
			catch (Exception exError)
			{
				DisplayError(exError.Message, "Error", MsgBoxStyle.Critical);
			}
			
		}
		
		public void SaveResultsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for output file and save results from RichTextBox
			//==================================================================
			string strResultsFile = "";
			string strResults = "";
			
			int intIndex = 0;
			
			StreamWriter swResultFile = default(StreamWriter);
			
			
			//== Get filename from user ==
			sfdSaveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
			
			
			if (sfdSaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				strResultsFile = sfdSaveFileDialog.FileName;
			}
			if (strResultsFile.Trim() == "")
			{
				return;
			}
			
			
			try
			{
				//== Open file ==
				swResultFile = new StreamWriter(strResultsFile);
				
				ShowLoading("Exporting results to file...", rtbResults.Lines.Count());
				
				//== Write results ==
				foreach (var strLine in rtbResults.Lines)
				{
					swResultFile.WriteLine(strLine);
					intIndex++;
					IncrementLoadingBar("Line: " + System.Convert.ToString(intIndex));
				}
				
				frmLoading.Default.Close();
				
				//== Close file ==
				swResultFile.Close();
				
			}
			catch (Exception exError)
			{
				DisplayError(exError.Message, "Error", MsgBoxStyle.Critical);
			}
			
		}
		
		private void AddContextMenu()
		{
			// Provide pop-up-menu for the relevant controls
			//==============================================
			
			if (modMain.asAppSettings.IsConsole == true)
			{
				return;
			}
			
			ContextMenu cmFullContextMenu = new ContextMenu(); // The filenames combobox allows cut/copy/paste
			ContextMenu cmResultsContextMenu = new ContextMenu(); // The results are just for copying, not modification
			ContextMenu cmResultsListContextMenu = new ContextMenu(); // The results table allows a file to be opened in its associated app or Notepad++
			
			// ComboBox
			MenuItem menuItem1Cut = new MenuItem("Cut");
			MenuItem menuItem2Copy = new MenuItem("Copy");
			MenuItem menuItem3Paste = new MenuItem("Paste");
			MenuItem menuItem4Divider = new MenuItem("-");
			MenuItem menuItem5SelectAll = new MenuItem("Select All");
			
			// RichTextBox
			MenuItem menuItem6Copy = new MenuItem("Copy");
			MenuItem menuItem9Divider = new MenuItem("-");
			MenuItem menuItem10SelectAll = new MenuItem("Select All");
			MenuItem menuItem7Divider = new MenuItem("-");
			MenuItem menuItem8Find = new MenuItem("Find");
			MenuItem menuItem13Divider = new MenuItem("-");
			MenuItem menuItem11Sort = new MenuItem("Sort on Severity");
			MenuItem menuItem12Sort = new MenuItem("Sort on FileName");
			MenuItem menuItem18Divider = new MenuItem("-");
			MenuItem menuItem19FilterResults = new MenuItem("Filter Results...");
			MenuItem menuItem20ExportFiltered = new MenuItem("Export Filtered XML Results...");
			
			// ListBox
			MenuItem menuItem14OpenFile = new MenuItem("Open Code in Associated Editor");
			MenuItem menuItem15OpenAtLine = new MenuItem("Open Code at This Line in Notepad++");
			MenuItem menuItem16Divider = new MenuItem("-");
			MenuItem menuItem17Order = new MenuItem("Order on Multiple Columns...");
			MenuItem menuItem21Divider = new MenuItem("-");
			MenuItem menuItem22FilterResults = new MenuItem("Filter Results...");
			MenuItem menuItem23ExportFiltered = new MenuItem("Export Filtered XML Results...");
			MenuItem menuItem24Divider = new MenuItem("-");
			MenuItem menuItem25SelectColour = new MenuItem("Select Colour When Checked...");
			MenuItem menuItem28ChangeSeverity = new MenuItem("Change Severity...");
			MenuItem menuItem26Divider = new MenuItem("-");
			MenuItem menuItem27DeleteItem = new MenuItem("Delete Selected Item(s)");
			
			//== Full context menu for combo box ==
			menuItem1Cut.Click += CutToolStripMenuItem_Click;
			menuItem2Copy.Click += CopyToolStripMenuItem_Click;
			menuItem3Paste.Click += PasteToolStripMenuItem_Click;
			menuItem5SelectAll.Click += SelectAllToolStripMenuItem_Click;
			
			cmFullContextMenu.MenuItems.Add(menuItem1Cut);
			cmFullContextMenu.MenuItems.Add(menuItem2Copy);
			cmFullContextMenu.MenuItems.Add(menuItem3Paste);
			cmFullContextMenu.MenuItems.Add(menuItem4Divider);
			cmFullContextMenu.MenuItems.Add(menuItem5SelectAll);
			
			
			//== Specialised menu for results ==
			menuItem6Copy.Click += CopyToolStripMenuItem_Click;
			menuItem10SelectAll.Click += SelectAllToolStripMenuItem_Click;
			menuItem8Find.Click += FindToolStripMenuItem_Click;
			menuItem11Sort.Click += SortRichTextResultsOnSeverityToolStripMenuItem_Click;
			menuItem12Sort.Click += SortRichTextResultsOnFileNameToolStripMenuItem_Click;
			menuItem19FilterResults.Click += FilterResultsToolStripMenuItem_Click;
			menuItem20ExportFiltered.Click += ExportFilteredResultsXML;
			
			cmResultsContextMenu.MenuItems.Add(menuItem6Copy);
			cmResultsContextMenu.MenuItems.Add(menuItem9Divider);
			cmResultsContextMenu.MenuItems.Add(menuItem10SelectAll);
			cmResultsContextMenu.MenuItems.Add(menuItem7Divider);
			cmResultsContextMenu.MenuItems.Add(menuItem8Find);
			cmResultsContextMenu.MenuItems.Add(menuItem13Divider);
			cmResultsContextMenu.MenuItems.Add(menuItem11Sort);
			cmResultsContextMenu.MenuItems.Add(menuItem12Sort);
			cmResultsContextMenu.MenuItems.Add(menuItem18Divider);
			cmResultsContextMenu.MenuItems.Add(menuItem19FilterResults);
			cmResultsContextMenu.MenuItems.Add(menuItem20ExportFiltered);
			
			
			//== File menu for results table ==
			menuItem14OpenFile.Click += OpenFileInEditor;
			menuItem15OpenAtLine.Click += OpenAtCodeBlock;
			menuItem17Order.Click += OrderOnMultColumns;
			menuItem22FilterResults.Click += FilterResultsToolStripMenuItem_Click;
			menuItem23ExportFiltered.Click += ExportFilteredResultsXML;
			menuItem25SelectColour.Click += SelectCheckColour;
			menuItem28ChangeSeverity.Click += SetSeverity;
			menuItem27DeleteItem.Click += DeleteScanResult;
			
			cmResultsListContextMenu.MenuItems.Add(menuItem14OpenFile);
			cmResultsListContextMenu.MenuItems.Add(menuItem15OpenAtLine);
			cmResultsListContextMenu.MenuItems.Add(menuItem16Divider);
			cmResultsListContextMenu.MenuItems.Add(menuItem17Order);
			cmResultsListContextMenu.MenuItems.Add(menuItem21Divider);
			cmResultsListContextMenu.MenuItems.Add(menuItem22FilterResults);
			cmResultsListContextMenu.MenuItems.Add(menuItem23ExportFiltered);
			cmResultsListContextMenu.MenuItems.Add(menuItem24Divider);
			cmResultsListContextMenu.MenuItems.Add(menuItem25SelectColour);
			cmResultsListContextMenu.MenuItems.Add(menuItem28ChangeSeverity);
			cmResultsListContextMenu.MenuItems.Add(menuItem26Divider);
			cmResultsListContextMenu.MenuItems.Add(menuItem27DeleteItem);
			
			//== Assign menus to controls ==
			cboTargetDir.ContextMenu = cmFullContextMenu;
			rtbResults.ContextMenu = cmResultsContextMenu;
			lvResults.ContextMenu = cmResultsListContextMenu;
			
		}
		
		private void SelectCheckColour(System.Object sender, System.EventArgs e)
		{
			// Allow user to modify the colour for checked listbox items
			//==========================================================
			
			if (cdColorDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
			{
				modMain.asAppSettings.ListItemColour = cdColorDialog.Color;
			}
			
		}
		
		private void DeleteScanResult(System.Object sender, System.EventArgs e)
		{
			// Delete a ScanResult from the results collection
			//================================================
			int intIndex = 0;
			int intCount = 0;
			
			string strKey = "";
			string strTitle = "";
			string strFileName = "";
			
			ArrayList arrDeleteItems = new ArrayList();
			
			
			// Get number of items to be deleted, exit if nothing to delete
			if (lvResults.SelectedItems.Count < 1)
			{
				return;
			}
			
			intCount = lvResults.SelectedItems.Count;
			
			// Iterate through results and find selected items
			foreach (var objResult in modMain.rtResultsTracker.ScanResults)
			{
				
				strKey = System.Convert.ToString(objResult.ItemKey.ToString());
				strTitle = Convert.ToString(objResult.Title);
				strFileName = Convert.ToString(objResult.FileName);
				
				if (lvResults.Items.ContainsKey(strKey))
				{
					if (lvResults.Items[strKey].Selected == true)
					{
						// Get index of selected object for deletion
						// Result objects must be deleted afterwards as
						// we would get an error if we deleted them during the iteration
						arrDeleteItems.Add(modMain.rtResultsTracker.ScanResults.IndexOf(objResult));
						
						// Remove from groups
						// If groups are empty they should be deleted
						if (modMain.rtResultsTracker.FileGroups.ContainsKey(strFileName))
						{
							modMain.rtResultsTracker.FileGroups[strFileName].DeleteDetail(Convert.ToInt32(strKey));
							if (modMain.rtResultsTracker.FileGroups[strFileName].GetItemCount()  == 0)
							{
								modMain.rtResultsTracker.FileGroups.Remove(strFileName);
							}
						}
						
						if (modMain.rtResultsTracker.IssueGroups.ContainsKey(strTitle))
						{
							modMain.rtResultsTracker.IssueGroups[strTitle].DeleteDetail(Convert.ToInt32(strKey));
							if (modMain.rtResultsTracker.IssueGroups[strTitle].GetItemCount()  == 0)
							{
								modMain.rtResultsTracker.IssueGroups.Remove(strTitle);
							}
						}
						
						
						// Remove from listview
						lvResults.Items[strKey].Remove();
						
						// Exit sub if all objects are deleted
						intIndex++;
						if (intIndex >= intCount)
						{
							break;
						}
					}
				}
			}
			
			
			// Delete selected objects
			// We must do this backwards (highest index first) as the
			// indexes will change for all items later in the list
			// following the deletion
			if (arrDeleteItems.Count < 1)
			{
				return;
			}
			
			arrDeleteItems.Sort();
			for (intIndex = arrDeleteItems.Count - 1; intIndex >= 0; intIndex--)
			{
				modMain.rtResultsTracker.ScanResults.RemoveAt(Convert.ToInt32(arrDeleteItems[intIndex]));
			}
			
			
			// Update Rich Text results
			SetRTBView(modMain.asAppSettings.RTBGrouping);
			
		}
		
		public void CutToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Handle each control as appropriate
			// Has to be done in a slightly awkward way as ActiveControl returns the container but we
			// need the control which has focus (and we don't always want same action for controls)
			//=======================================================================================
			
			if (cboTargetDir.Focused)
			{
				if (cboTargetDir.SelectedText != "")
				{
					Clipboard.SetText(cboTargetDir.SelectedText);
					cboTargetDir.SelectedText = "";
				}
			}
			
		}
		
		public void CopyToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Handle each control as appropriate
			// Has to be done in a slightly awkward way as ActiveControl returns the container but we
			// need the control which has focus (and we don't always want same action for controls)
			//=======================================================================================
			
			if (rtbResults.Focused)
			{
				if (rtbResults.SelectedText != "")
				{
					Clipboard.SetText(rtbResults.SelectedText);
				}
			}
			else if (cboTargetDir.Focused)
			{
				if (cboTargetDir.SelectedText != "")
				{
					Clipboard.SetText(cboTargetDir.SelectedText);
				}
			}
			
		}
		
		public void PasteToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Handle each control as appropriate
			// Has to be done in a slightly awkward way as ActiveControl returns the container but we
			// need the control which has focus (and we don't always want same action for controls)
			//=======================================================================================
			
			if (cboTargetDir.Focused)
			{
				if (cboTargetDir.SelectedText != "")
				{
					Clipboard.SetText(cboTargetDir.SelectedText);
				}
			}
			
		}
		
		public void FindToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Handle each control as appropriate
			// Has to be done in a slightly awkward way as ActiveControl returns the container but we
			// need the control which has focus (and we don't always want same action for controls)
			//=======================================================================================
			
			if (rtbResults.Focused)
			{
				FindText();
			}
			
		}
		
		public void SelectAllToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Handle each control as appropriate
			// Has to be done in a slightly awkward way as ActiveControl returns the container but we
			// need the control which has focus (and we don't always want same action for controls)
			//=======================================================================================
			
			if (rtbResults.Focused)
			{
				rtbResults.SelectAll();
			}
			else if (cboTargetDir.Focused)
			{
				cboTargetDir.SelectAll();
			}
			
		}
		
		public void FindText()
		{
			// Locate the selected text in the Results box
			//============================================
			string strSearch = "";
			
			strSearch = Interaction.InputBox("Enter Text to Search For:", "Find", strPrevSearch);
			rtbResults.Find(strSearch);
			strPrevSearch = strSearch;
			
		}
		
		public void ToolStripMenuItem9_Click(System.Object sender, System.EventArgs e)
		{
			// Clear all results and target directory
			//=======================================
			ClearResults();
		}
		
		public void ClearResults()
		{
			// Clear all results and target directory
			//=======================================
			
			// Clear variables
			modMain.rtResultsTracker.TargetDirectory = "";
			modMain.rtResultsTracker.Reset();
			modMain.rtResultsTracker.ResetFileListVars();
			
			//Clear UI
			lbTargets.Items.Clear();
			lvResults.Items.Clear();
			rtbResults.Clear();
			cboTargetDir.Text = "";
			
			// Disable menus
			SetScanMenus(false);
			
			// Reset text in status bar
			modMain.SelectLanguage(modMain.asAppSettings.TestType);
			
		}
		
		public void SetScanMenus(bool NewSetting)
		{
			// Enable or disable scanning menus when target is selected/deselected
			//====================================================================
			
			if (modMain.asAppSettings.IsConsole == true)
			{
				return;
			}
			
			StartScanningToolStripMenuItem.Enabled = NewSetting;
			VisualCodeBreakdownToolStripMenuItem.Enabled = NewSetting;
			VisualSecurityBreakdownToolStripMenuItem.Enabled = NewSetting;
			VisualBadFuncBreakdownToolStripMenuItem.Enabled = NewSetting;
			VisualCommentBreakdownToolStripMenuItem.Enabled = NewSetting;
			ExportFixMeCommentsToolStripMenuItem.Enabled = NewSetting;
			
			if (!ReferenceEquals(tcMain.SelectedTab, tabResultsTable))
			{
				DeleteItemToolStripMenuItem.Enabled = false;
			}
			else
			{
				DeleteItemToolStripMenuItem.Enabled = true;
			}
			
		}
		
		public void SetDeleteMenu()
		{
			// Enable delete menu if a scan result is selected in the table
			//=============================================================
			
			if ((ReferenceEquals(tcMain.SelectedTab, tabResultsTable) || tabResultsTable.Focused == true) && lvResults.SelectedItems.Count > 0)
			{
				DeleteItemToolStripMenuItem.Enabled = true;
			}
			else
			{
				DeleteItemToolStripMenuItem.Enabled = false;
			}
			
		}
		
		private void OpenAtCodeBlock(System.Object sender, System.EventArgs e)
		{
			// Open the file listed in the summary table at the given line number
			//===================================================================
			string strFileName = "";
			int intLineNum = 0;
			int intResult = (int) Constants.vbOK;
			
			if (lvResults.SelectedItems.Count == 0)
			{
				return;
			}
			
			// Get the filename and line number from the table
			strFileName = lvResults.SelectedItems[0].SubItems[FILE_COL].Text;
			intLineNum = Convert.ToInt32(lvResults.SelectedItems[0].SubItems[LINE_COL].Text);
			
			// If we have a file, try to open it in its associated application
			if (!string.IsNullOrEmpty(strFileName))
			{
				if (intLineNum > 0)
				{
					modMain.LaunchNPP(strFileName, intLineNum);
				}
				else
				{
					intResult = (int) (Interaction.MsgBox("This type of issue does not have an associated code block and line number. Open file at line 1?", Constants.vbOKCancel, "No Line Number Available"));
					if (intResult == (int) Constants.vbOK)
					{
						modMain.LaunchNPP(strFileName);
					}
				}
			}
			
		}
		
		public void lvResults_Click(object sender, System.EventArgs e)
		{
			// If an item is selected then enable the 'delete' menu item
			//==========================================================
			
			if (lvResults.SelectedItems.Count > 0)
			{
				DeleteItemToolStripMenuItem.Enabled = true;
			}
			else
			{
				DeleteItemToolStripMenuItem.Enabled = false;
			}
			
		}
		
		public void lvResults_DoubleClick(object sender, System.EventArgs e)
		{
			// If an item is selected then enable the 'delete' menu item
			// Open any file that has been double-clicked in the appropriate editor
			//=====================================================================
			
			if (lvResults.SelectedItems.Count > 0)
			{
				DeleteItemToolStripMenuItem.Enabled = true;
			}
			else
			{
				DeleteItemToolStripMenuItem.Enabled = false;
			}
			
			OpenFileInEditor(Type.Missing, (EventArgs) Type.Missing);
			
		}
		
		private void OpenFileInEditor(System.Object sender, System.EventArgs e)
		{
			// Open the associated file listed in the summary table
			//=====================================================
			string strFileName = "";
			ProcessStartInfo psiStart = new ProcessStartInfo();
			
			
			if (lvResults.SelectedItems.Count == 0)
			{
				return;
			}
			
			// Get the filename from the table
			strFileName = lvResults.SelectedItems[0].SubItems[FILE_COL].Text;
			
			// If we have a file, try to open it in its associated application
			if (!string.IsNullOrEmpty(strFileName))
			{
				try
				{
					psiStart.FileName = strFileName;
					psiStart.UseShellExecute = true;
					
					Process.Start(psiStart);
				}
				catch
				{
					DisplayError("Error reading file: " + strFileName, "File Error", MsgBoxStyle.Exclamation);
				}
			}
			
		}
		
		public void lvResults_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			// Sort the items according to which column was clicked
			//=====================================================
			ListViewItem lviItem = default(ListViewItem);
			Color colOriginalColour;
			
			lvResults.Items.Clear();
			SortResults(e.Column);
			
			foreach (var itmItem in modMain.rtResultsTracker.ScanResults)
			{
				if (!blnIsFiltered || (blnIsFiltered && Convert.ToInt32(itmItem.Severity) >= intFilterMax && Convert.ToInt32(itmItem.Severity) <= intFilterMin))
				{
					lviItem = new ListViewItem();
					
					colOriginalColour = modMain.asAppSettings.ListItemColour;
					
					//== Add to listview ==
					lviItem.Name = Convert.ToString(itmItem.ItemKey);
					lviItem.Text = Convert.ToString(itmItem.Severity);
					lviItem.SubItems.Add(itmItem.SeverityDesc.ToString());
					lviItem.SubItems.Add(itmItem.Title.ToString());
					lviItem.SubItems.Add(itmItem.Description.ToString());
					lviItem.SubItems.Add(itmItem.FileName.ToString());
					lviItem.SubItems.Add(itmItem.LineNumber.ToString());
					
					if (Convert.ToBoolean(itmItem.IsChecked))
					{
						modMain.asAppSettings.ListItemColour = (Color) itmItem.CheckColour;
						lviItem.Checked = Convert.ToBoolean(itmItem.IsChecked);
					}
					
					lvResults.Items.Add(lviItem);
					
				}
			}
			
		}
		
		public void ToolStripMenuItem10_Click(System.Object sender, System.EventArgs e)
		{
			ExportResultsXML();
		}
		
		public void ToolStripMenuItem11_Click(System.Object sender, System.EventArgs e)
		{
			ImportResultsXML();
		}
		
		public void ToolStripMenuItem14_Click(System.Object sender, System.EventArgs e)
		{
			ExportResultsCSV();
		}
		
		public void ToolStripMenuItem15_Click(System.Object sender, System.EventArgs e)
		{
			ImportResultsCSV();
		}
		
		public void SortRichTextResultsOnSeverityToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			SortRTBResults();
		}
		
		public void SortRichTextResultsOnFileNameToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			SortRTBResults(FILE_COL);
		}
		
		public void StatusBarToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show/hide status bar
			//=====================
			
			ssStatusStrip.Visible = StatusBarToolStripMenuItem.Checked;
			
		}
		
		public void GroupRichTextResultsByIssueToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set grouping style for RTB results and uncheck other menu items
			//================================================================
			
			if (GroupRichTextResultsByIssueToolStripMenuItem.Checked)
			{
				SetRTBView(AppSettings.ISSUEGROUP);
			}
			
		}
		
		public void GroupRichTextResultsByFileToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Set grouping style for RTB results and uncheck other menu items
			//================================================================
			
			if (GroupRichTextResultsByFileToolStripMenuItem.Checked)
			{
				SetRTBView(AppSettings.FILEGROUP);
			}
			
		}
		
		public void ShowIndividualRichTextResultsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Show individual results
			//========================
			
			if (ShowIndividualRichTextResultsToolStripMenuItem.Checked)
			{
				SetRTBView(AppSettings.INDIVIDUAL);
			}
			
		}
		
		private void SetRTBView(int ViewOption)
		{
			// Set the RTB view option for grouping results
			//=============================================
			
			modMain.asAppSettings.RTBGrouping = ViewOption;
			
			switch (ViewOption)
			{
				case AppSettings.ISSUEGROUP:
					// Show all occurences for each particular issue
					GroupRichTextResultsByFileToolStripMenuItem.Checked = false;
					ShowIndividualRichTextResultsToolStripMenuItem.Checked = false;
					GroupRTBByIssue();
					break;
				case AppSettings.FILEGROUP:
					// Show the different issues for each file
					GroupRichTextResultsByIssueToolStripMenuItem.Checked = false;
					ShowIndividualRichTextResultsToolStripMenuItem.Checked = false;
					GroupRTBByFile();
					break;
				case AppSettings.INDIVIDUAL:
					// Default view - each issue shown separately
					// We are using an arbitrary value as PRIORITY_COL or FILE_COL will result in sorting taking place
					GroupRichTextResultsByFileToolStripMenuItem.Checked = false;
					GroupRichTextResultsByIssueToolStripMenuItem.Checked = false;
					SortRTBResults(-1);
					break;
			}
			
		}
		
		private void ExportFilteredResultsXML(System.Object sender, System.EventArgs e)
		{
			ExportResultsXML(intFilterMin, intFilterMax);
		}
		
		public void ExportResultsXML(int FilterMinimum = 0, int FilterMaximum = 0, string ExportFile = "")
		{
			// Export all errors to XML file
			//==============================
			XmlWriterSettings xwsSettings = new XmlWriterSettings();
			XmlWriter xwWriter = default(XmlWriter);
			ColorConverter ccConverter = new ColorConverter();
			
			string strResultsFile = "";
			string strLanguage = "";
			
			
			//== Exit if no results ==
			if (modMain.rtResultsTracker.ScanResults.Count == 0)
			{
				if (modMain.asAppSettings.IsConsole)
				{
					Console.WriteLine("No results to write. Exiting application.");
				}
				return;
			}
			
			
			if (SaveFiltered == true)
			{
				FilterMinimum = intFilterMin;
				FilterMaximum = intFilterMax;
			}
			
			//== Get language for results in text form in order to put meaningful comment at start of file ==
			switch (modMain.asAppSettings.TestType)
			{
				case AppSettings.C:
					strLanguage = "C";
					break;
				case AppSettings.JAVA:
					strLanguage = "Java";
					break;
				case AppSettings.SQL:
					strLanguage = "PL/SQL";
					break;
				case AppSettings.CSHARP:
					strLanguage = "C#";
					break;
				case AppSettings.PHP:
					strLanguage = "PHP";
					break;
			}
			
			if (ExportFile == "")
			{
				//== Get filename from user ==
				sfdSaveFileDialog.Filter = "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*";
				
				if (sfdSaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					strResultsFile = sfdSaveFileDialog.FileName;
				}
				
				if (strResultsFile.Trim() == "")
				{
					return;
				}
				else
				{
					ExportFile = strResultsFile;
				}
				
			}
			
			
			//== Create file and write output ==
			try
			{
				xwWriter = XmlWriter.Create(ExportFile, xwsSettings);
				
				
				// Use indention for the xml output
				xwsSettings.Indent = true;
				
				
				// Begin document with Xml declaration
				xwWriter.WriteStartDocument();
				
				// Write a comment.
				xwWriter.WriteComment("XML Export of VCG Results for directory: " + modMain.rtResultsTracker.TargetDirectory +". Scanned for " + strLanguage + " security issues.");
				
				// Write the root element.
				xwWriter.WriteStartElement("CodeIssueCollection");
				
				//== Display progress to screen as appropriate ==
				if (modMain.asAppSettings.IsConsole == false)
				{
					ShowLoading("Exporting results to XML...", modMain.rtResultsTracker.ScanResults.Count);
				}
				else if (modMain.asAppSettings.IsConsole == true && modMain.asAppSettings.IsVerbose)
				{
					Console.WriteLine("Exporting results to XML. Number of records: " + (modMain.rtResultsTracker.ScanResults.Count).ToString());
				}
				
				
				// Loop through issues and write data for each one
				foreach (var itmItem in modMain.rtResultsTracker.ScanResults)
				{
					if (Convert.ToInt32(itmItem.Severity) <= FilterMinimum && Convert.ToInt32(itmItem.Severity) >= FilterMaximum)
					{
						
						
						xwWriter.WriteStartElement("CodeIssue");
						
						xwWriter.WriteStartElement("Priority");
						xwWriter.WriteString(Convert.ToString(itmItem.Severity));
						xwWriter.WriteEndElement();
						
						xwWriter.WriteStartElement("Severity");
						xwWriter.WriteString(Convert.ToString(itmItem.SeverityDesc));
						xwWriter.WriteEndElement();
						
						xwWriter.WriteStartElement("Title");
						xwWriter.WriteString(Convert.ToString(itmItem.Title));
						xwWriter.WriteEndElement();
						
						xwWriter.WriteStartElement("Description");
						xwWriter.WriteString(Convert.ToString(itmItem.Description));
						xwWriter.WriteEndElement();
						
						xwWriter.WriteStartElement("FileName");
						xwWriter.WriteString(Convert.ToString(itmItem.FileName));
						xwWriter.WriteEndElement();
						
						xwWriter.WriteStartElement("Line");
						xwWriter.WriteString(Convert.ToString(itmItem.LineNumber));
						xwWriter.WriteEndElement();
						
						xwWriter.WriteStartElement("CodeLine");
						xwWriter.WriteString(Convert.ToString(itmItem.CodeLine));
						xwWriter.WriteEndElement();
						
						if (SaveCheckState)
						{
							xwWriter.WriteStartElement("Checked");
							xwWriter.WriteString(Convert.ToString(itmItem.IsChecked));
							xwWriter.WriteEndElement();
							xwWriter.WriteStartElement("CheckColour");
							xwWriter.WriteString(ccConverter.ConvertToString(itmItem.CheckColour));
							xwWriter.WriteEndElement();
						}
						
						// End of this issue
						xwWriter.WriteEndElement();
						
					}
					
					if (modMain.asAppSettings.IsConsole == false)
					{
						IncrementLoadingBar("Formatting XML...");
					}
					
				}
				
				frmLoading.Default.Close();
				
				// Close XmlTextWriter
				xwWriter.WriteEndElement();
				xwWriter.WriteEndDocument();
				xwWriter.Close();
				
				
				xwWriter.Close();
				
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				DisplayError("XML output successfully exported.");
				
			}
			catch (Exception exError)
			{
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				DisplayError(exError.Message);
			}
			
		}
		
		public void ImportResultsXML(string FileName = "")
		{
			// Import previous scan results from XML file
			//===========================================
			string strResultsFile = "";
			string strDescription = "";
			
			XmlTextReader xrReader = default(XmlTextReader);
			XmlNodeType xntNodeType;
			ScanResult srResultItem = new ScanResult();
			ColorConverter ccConverter = new ColorConverter();
			
			string[] arrInts = null;
			string strColour = "";
			int intR = 0; // For colour represented as RGB components
			int intG = 0;
			int intB = 0;
			
			
			//== Get user permission if app has a set of current results in place ==
			if (modMain.rtResultsTracker.ScanResults.Count > 0 && modMain.asAppSettings.IsConsole == false)
			{
				if (Interaction.MsgBox("Overwrite current results with file contents?", MsgBoxStyle.YesNo, "Overwrite Results?") == MsgBoxResult.No)
				{
					return;
				}
			}
			
			if (FileName == "")
			{
				//== Get filename from user ==
				ofdOpenFileDialog.Filter = "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*";
				
				if (ofdOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					strResultsFile = ofdOpenFileDialog.FileName;
					if (!(System.IO.File.Exists(strResultsFile)))
					{
						return;
					}
				}
				
				if (strResultsFile.Trim() == "")
				{
					return;
				}
			}
			else
			{
				strResultsFile = FileName;
			}
			
			//== Remove any previous results ==
			ClearResults();
			
			//== Load results from file ==
			try
			{
				
				xrReader = new XmlTextReader(strResultsFile);
				
				FileInfo fiFileInfo = new FileInfo(strResultsFile);
				
				//== Display progress to screen as appropriate ==
				if (modMain.asAppSettings.IsConsole == false)
				{
					ShowLoading("Importing results from XML file...", Convert.ToInt32(fiFileInfo.Length));
				}
				else if (modMain.asAppSettings.IsConsole == true && modMain.asAppSettings.IsVerbose)
				{
					Console.WriteLine("Importing results from XML file. File size: " + System.Convert.ToString(fiFileInfo.Length));
				}
				
				
				//== Loop through each XML element
				while (xrReader.Read())
				{
					
					xntNodeType = xrReader.NodeType;
					
					//if node type was element
					if (xntNodeType == XmlNodeType.Element)
					{
						
						//== Populate data from each tag ==
						if (xrReader.Name == "Priority")
						{
							srResultItem.SetSeverity(int.Parse(xrReader.ReadInnerXml().ToString()));
						}
						if (xrReader.Name == "Title")
						{
							srResultItem.Title = xrReader.ReadInnerXml().ToString();
						}
						if (xrReader.Name == "Description")
						{
							srResultItem.Description = xrReader.ReadInnerXml().ToString();
						}
						if (xrReader.Name == "FileName")
						{
							srResultItem.FileName = xrReader.ReadInnerXml().ToString();
						}
						if (xrReader.Name == "Line")
						{
							srResultItem.LineNumber = Convert.ToInt32(xrReader.ReadInnerXml().ToString());
						}
						if (xrReader.Name == "CodeLine")
						{
							srResultItem.CodeLine = xrReader.ReadInnerXml().ToString();
						}
						if (xrReader.Name == "Checked")
						{
							srResultItem.IsChecked = Convert.ToBoolean(xrReader.ReadInnerXml().ToString());
						}
						
						if (xrReader.Name == "CheckColour")
						{
							strColour = xrReader.ReadInnerXml().ToString();
							if (strColour.Contains(","))
							{
								arrInts = strColour.Split(new char[] {','});
								intR = int.Parse(arrInts[0]);
								intG = int.Parse(arrInts[1]);
								intB = int.Parse(arrInts[2]);
								srResultItem.CheckColour = Color.FromArgb(intR, intG, intB);
							}
							else
							{
								srResultItem.CheckColour = Color.FromName(strColour);
							}
						}
						
						if (xrReader.NodeType == XmlNodeType.EndElement & xrReader.Name == "CodeIssue")
						{
							//== Place result in collection and write output to screen ==
							AddToResultCollection(srResultItem.Title, srResultItem.Description, srResultItem.FileName, srResultItem.Severity(), srResultItem.LineNumber, srResultItem.CodeLine, srResultItem.IsChecked, ccConverter.ConvertToString(srResultItem.CheckColour));
							strDescription = srResultItem.Description;
							WriteResult(srResultItem.Title + Constants.vbNewLine, strDescription + Constants.vbNewLine + "Line: " + System.Convert.ToString(srResultItem.LineNumber) + " - Filename: " + srResultItem.FileName + Constants.vbNewLine, srResultItem.CodeLine, srResultItem.Severity());
						}
						
					}
					
					if (modMain.asAppSettings.IsConsole == false)
					{
						IncrementLoadingBar("Reading XML element: " + srResultItem.Description);
					}
					
				}
				
				xrReader.Close();
				frmLoading.Default.Close();
				
			}
			catch (Exception exError)
			{
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				DisplayError(exError.Message);
			}
			
		}
		
		public void ExportResultsCSV(int FilterMinimum = 0, int FilterMaximum = 0, string ExportFile = "")
		{
			// Export all errors to XML file
			//==============================
			StreamWriter swResultFile = default(StreamWriter);
			ColorConverter ccConverter = new ColorConverter();
			
			string strResultsFile = "";
			string strLanguage = "";
			string strDescription = "";
			string strCodeLine = "";
			
			
			
			//== Exit if no results ==
			if (modMain.rtResultsTracker.ScanResults.Count == 0)
			{
				if (modMain.asAppSettings.IsConsole)
				{
					Console.WriteLine("No results to write. Exiting application.");
				}
				return;
			}
			
			
			if (SaveFiltered == true)
			{
				FilterMinimum = intFilterMin;
				FilterMaximum = intFilterMax;
			}
			
			
			if (ExportFile == "")
			{
				//== Get filename from user ==
				sfdSaveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
				
				if (sfdSaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					strResultsFile = sfdSaveFileDialog.FileName;
				}
				
				if (strResultsFile.Trim() == "")
				{
					return;
				}
			}
			else
			{
				strResultsFile = ExportFile;
			}
			
			
			try
			{
				//== Open file ==
				swResultFile = new StreamWriter(strResultsFile);
				
				
				
				//== Display progress to screen as appropriate ==
				if (modMain.asAppSettings.IsConsole == false)
				{
					ShowLoading("Exporting results to CSV file...", modMain.rtResultsTracker.ScanResults.Count);
				}
				else if (modMain.asAppSettings.IsConsole == true && modMain.asAppSettings.IsVerbose)
				{
					Console.WriteLine("Exporting results to CSV. Number of records: " + (modMain.rtResultsTracker.ScanResults.Count).ToString());
				}
				
				
				// Loop through issues and write data for each one
				foreach (var itmItem in modMain.rtResultsTracker.ScanResults)
				{
					if (Convert.ToInt32(itmItem.Severity) <= FilterMinimum && Convert.ToInt32(itmItem.Severity) >= FilterMaximum)
					{
						
						// Sanitise free-form text to prevent quotes from breaking things
						strDescription = System.Convert.ToString(itmItem.Description.ToString().Replace("\"", "\"\""));
						strCodeLine = System.Convert.ToString(itmItem.CodeLine.ToString().Replace("\"", "\"\""));
						strDescription = System.Convert.ToString(itmItem.Description.ToString().Replace(Constants.vbNewLine, ""));
						
						
						if (SaveCheckState)
						{
							swResultFile.WriteLine(itmItem.Severity + "," + itmItem.SeverityDesc + ",\"" + itmItem.Title + "\",\"" + strDescription + "\"," + itmItem.FileName + "," + itmItem.LineNumber + ",\"" + strCodeLine + "\"," + itmItem.IsChecked + ",\"" + ccConverter.ConvertToString(itmItem.CheckColour) + "\"");
						}
						else
						{
							swResultFile.WriteLine(itmItem.Severity + "," + itmItem.SeverityDesc + ",\"" + itmItem.Title + "\",\"" + strDescription + "\"," + itmItem.FileName + "," + itmItem.LineNumber + ",\"" + strCodeLine + "\"");
						}
						
					}
					
					if (modMain.asAppSettings.IsConsole == false)
					{
						IncrementLoadingBar("Writing CSV File...");
					}
					
				}
				
				swResultFile.Close();
				
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				DisplayError("CSV output successfully exported.", "Success", Constants.vbOKOnly);
				
			}
			catch (Exception exError)
			{
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				DisplayError(exError.Message);
			}
			
		}
		
		public void ImportResultsCSV(string FileName = "")
		{
			// Import previous scan results from XML file
			//===========================================
			string strResultsFile = "";
			string strDescription = "";
			
			string[] arrItems = null;
			string[] arrInts = null;
			
			ScanResult srResultItem = new ScanResult();
			ColorConverter ccConverter = new ColorConverter();
			
			int intR = 0; // For colour represented as RGB components
			int intG = 0;
			int intB = 0;
			
			
			//== Get user permission if app has a set of current results in place ==
			if (modMain.rtResultsTracker.ScanResults.Count > 0 && modMain.asAppSettings.IsConsole == false)
			{
				if (Interaction.MsgBox("Overwrite current results with file contents?", MsgBoxStyle.YesNo, "Overwrite Results?") == MsgBoxResult.No)
				{
					return;
				}
			}
			
			if (FileName == "")
			{
				//== Get filename from user ==
				ofdOpenFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
				
				if (ofdOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					strResultsFile = ofdOpenFileDialog.FileName;
					if (!(System.IO.File.Exists(strResultsFile)))
					{
						return;
					}
				}
				
				if (strResultsFile.Trim() == "")
				{
					return;
				}
			}
			else
			{
				strResultsFile = FileName;
			}
			
			//== Remove any previous results ==
			ClearResults();
			
			//== Load results from file ==
			try
			{
				
				FileInfo fiFileInfo = new FileInfo(strResultsFile);
				using (Microsoft.VisualBasic.FileIO.TextFieldParser tfpParser = new Microsoft.VisualBasic.FileIO.TextFieldParser(strResultsFile))
				{
					
					//== Display progress to screen as appropriate ==
					if (modMain.asAppSettings.IsConsole == false)
					{
						ShowLoading("Importing results from CSV file...", Convert.ToInt32(fiFileInfo.Length));
					}
					else if (modMain.asAppSettings.IsConsole == true && modMain.asAppSettings.IsVerbose)
					{
						Console.WriteLine("Importing results from CSV file. File size: " + System.Convert.ToString(fiFileInfo.Length));
					}
					
					tfpParser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
					tfpParser.SetDelimiters(",");
					
					
					while (!tfpParser.EndOfData)
					{
						try
						{
							arrItems = tfpParser.ReadFields();
							
							if (arrItems.Length == 9 | arrItems.Length == 7)
							{
								
								//== Populate data from each tag ==
								srResultItem.SetSeverity(int.Parse(arrItems[0]));
								srResultItem.Title = arrItems[2];
								srResultItem.Description = arrItems[3];
								srResultItem.FileName = arrItems[4];
								srResultItem.LineNumber = Convert.ToInt32(arrItems[5]);
								srResultItem.CodeLine = arrItems[6];
								
								if (arrItems.Length == 9)
								{
									srResultItem.IsChecked = Convert.ToBoolean(arrItems[7]);
									if (arrItems[8].Contains(","))
									{
										arrInts = arrItems[8].Split(new char[] {','});
										intR = int.Parse(arrInts[0]);
										intG = int.Parse(arrInts[1]);
										intB = int.Parse(arrInts[2]);
										srResultItem.CheckColour = Color.FromArgb(intR, intG, intB);
									}
									else
									{
										srResultItem.CheckColour = Color.FromName(arrItems[8]);
									}
								}
								
								//== Place result in collection and write output to screen ==
								AddToResultCollection(srResultItem.Title, srResultItem.Description, srResultItem.FileName, srResultItem.Severity(), srResultItem.LineNumber, srResultItem.CodeLine, srResultItem.IsChecked, ccConverter.ConvertToString(srResultItem.CheckColour));
								strDescription = srResultItem.Description;
								WriteResult(srResultItem.Title + Constants.vbNewLine, strDescription + Constants.vbNewLine + "Line: " + System.Convert.ToString(srResultItem.LineNumber) + " - Filename: " + srResultItem.FileName + Constants.vbNewLine, srResultItem.CodeLine, srResultItem.Severity());
							}
							
							if (modMain.asAppSettings.IsConsole == false)
							{
								IncrementLoadingBar("Reading CSV element: " + srResultItem.Description);
							}
							
						}
						catch (Microsoft.VisualBasic.FileIO.MalformedLineException exError)
						{
							modMain.ShowError("Line " + exError.Message + "is not valid and will be skipped.");
						}
					}
				}
				
				
				
				//== Notify user of success ==
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				//DisplayError("CSV output successfully imported.")
				
			}
			catch (Exception exError)
			{
				if (modMain.asAppSettings.IsConsole == false)
				{
					frmLoading.Default.Close();
				}
				DisplayError(exError.Message);
			}
			
		}
		
		public void FilterResults(int FilterMinimum = 0, int FilterMaximum = 0)
		{
			// Clear the listview and display only the results in the required range
			//======================================================================
			string strDescription = "";
			ListViewItem lviItem = default(ListViewItem);
			Color colOriginalColour = modMain.asAppSettings.ListItemColour;
			
			
			//== If there are no changes then exit the sub ==
			if (intFilterMin == FilterMinimum & intFilterMax == FilterMaximum)
			{
				return;
			}
			
			//== Store new settings ==
			intFilterMin = FilterMinimum;
			intFilterMax = FilterMaximum;
			
			if (intFilterMax == CodeIssue.CRITICAL & intFilterMin == CodeIssue.POSSIBLY_SAFE)
			{
				blnIsFiltered = false;
			}
			else
			{
				blnIsFiltered = true;
			}
			
			
			//== Remove any previous results ==
			rtbResults.Text = "";
			lvResults.Items.Clear();
			
			//== Write out the new results ==
			foreach (var srResultItem in modMain.rtResultsTracker.ScanResults)
			{
				if (Convert.ToInt32(srResultItem.Severity) >= FilterMaximum && Convert.ToInt32(srResultItem.Severity) <= FilterMinimum)
				{
					
					modMain.asAppSettings.ListItemColour = colOriginalColour;
					
					//== Add to rich text ==
					strDescription = Convert.ToString(srResultItem.Description);
					WriteResult(Convert.ToString(srResultItem.Title) + Constants.vbNewLine, Convert.ToString(strDescription) + Constants.vbNewLine + "Line: " + Convert.ToString(srResultItem.LineNumber) + " - Filename: " + Convert.ToString(srResultItem.FileName) + Constants.vbNewLine, Convert.ToString(srResultItem.CodeLine.Trim), Convert.ToInt32(srResultItem.Severity));
					
					//== Add to listview ==
					lviItem = new ListViewItem();
					lviItem.Name = Convert.ToString(srResultItem.ItemKey);
					lviItem.Text = Convert.ToString(srResultItem.Severity);
					lviItem.SubItems.Add(srResultItem.SeverityDesc.ToString());
					lviItem.SubItems.Add(srResultItem.Title.ToString());
					lviItem.SubItems.Add(srResultItem.Description.ToString());
					lviItem.SubItems.Add(srResultItem.FileName.ToString());
					lviItem.SubItems.Add(srResultItem.LineNumber.ToString());
					
					
					if (srResultItem.IsChecked == true)
					{
						modMain.asAppSettings.ListItemColour = Color.FromName(Convert.ToString(srResultItem.CheckColour));
						lviItem.Checked = Convert.ToBoolean(srResultItem.IsChecked);
					}
					
					lvResults.Items.Add(lviItem);
				}
			}
			
		}
		
		public void lvResults_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			// Allow user to mark items
			//=========================
			
			//== Set item state for saves/sorts/etc. ==
			SetCheckState(Convert.ToInt32(lvResults.Items[e.Index].Name), Convert.ToBoolean(e.NewValue), modMain.asAppSettings.ListItemColour);
			
		}
		
		private void SetCheckState(int Index, bool Value, Color SelectedColour)
		{
			// Locate the item in results collection and assign appropriate check status
			//==========================================================================
			int intIndex = 0;
			
			
			foreach (var itmItem in modMain.rtResultsTracker.ScanResults)
			{
				if (itmItem.ItemKey == Index)
				{
					itmItem.IsChecked = Value;
					
					// If the listview items have been sorted then they should be identified with their name, not index
					intIndex = lvResults.Items.IndexOfKey(Convert.ToString(Index));
					
					// Apply the appropriate colour to the listview item
					if (Value)
					{
						lvResults.Items[intIndex].BackColor = SelectedColour;
						itmItem.CheckColour = SelectedColour;
					}
					else
					{
						lvResults.Items[intIndex].BackColor = lvResults.BackColor;
					}
					
					break;
				}
			}
			
		}
		
		private void SetSeverity(System.Object sender, System.EventArgs e)
		{
			// Loop through selected items and set new severity value
			//=======================================================
			ArrayList arlIndexes = new ArrayList();
			frmNewSeverity frmNewSev = new frmNewSeverity();
			
			
			//== Display dialog to get new value ==
			frmNewSev.ShowDialog(this);
			if (modMain.intNewSeverity == -1)
			{
				return;
			}
			
			//== Modify values for result set and listview ==
			foreach (var itmListItem in lvResults.SelectedItems)
			{
				arlIndexes.Add(itmListItem.Name);
			}
			
			SetNewSeverity(arlIndexes, modMain.intNewSeverity, arlIndexes.Count);
			
		}
		
		private void SetNewSeverity(ArrayList Indexes, int Value, int ItemCount)
		{
			// Locate the item in results collection and assign appropriate check status
			//==========================================================================
			int intIndex = 0;
			int intItemCount = 0;
			
			
			foreach (ScanResult itmItem in modMain.rtResultsTracker.ScanResults)
			{
				if (Indexes.Contains((itmItem.ItemKey).ToString()))
				{
					
					// Set new severity
					itmItem.SetSeverity(Value);
					
					// If the listview items have been sorted then they should be identified with their name, not index
					intIndex = lvResults.Items.IndexOfKey((itmItem.ItemKey).ToString());
					
					// Apply the new value to the listview item
					lvResults.Items[intIndex].Text = Value.ToString();
					lvResults.Items[intIndex].SubItems[1].Text = itmItem.SeverityDesc();
					
					// Increase the count and exit loop if we've modified all results
					intItemCount++;
					if (intItemCount == ItemCount)
					{
						break;
					}
				}
			}
			
		}
		
		private void OrderOnMultColumns(System.Object sender, System.EventArgs e)
		{
			// Show dialog and then order on selected columns
			//===============================================
			ListViewItem lviItem = default(ListViewItem);
			Color colOriginalColour = modMain.asAppSettings.ListItemColour;
			
			
			frmSort.Default.ShowDialog(this);
			if (modMain.dicColumns.Count == 3)
			{
				lvResults.Items.Clear();
				SortResultsOnMultColumns();
				
				foreach (var itmItem in modMain.rtResultsTracker.ScanResults)
				{
					
					if (!blnIsFiltered || (blnIsFiltered && Convert.ToInt32(itmItem.Severity) >= intFilterMax && Convert.ToInt32(itmItem.Severity) <= intFilterMin))
					{
						lviItem = new ListViewItem();
						
						modMain.asAppSettings.ListItemColour = colOriginalColour;
						
						//== Add to listview ==
						lviItem.Name = Convert.ToString(itmItem.ItemKey);
						lviItem.Text = Convert.ToString(itmItem.Severity);
						lviItem.SubItems.Add(itmItem.SeverityDesc.ToString());
						lviItem.SubItems.Add(itmItem.Title.ToString());
						lviItem.SubItems.Add(itmItem.Description.ToString());
						lviItem.SubItems.Add(itmItem.FileName.ToString());
						lviItem.SubItems.Add(itmItem.LineNumber.ToString());
						
						if (itmItem.IsChecked == true)
						{
							modMain.asAppSettings.ListItemColour = (Color) itmItem.CheckColour;
							lviItem.Checked = Convert.ToBoolean(itmItem.IsChecked);
						}
						
						lvResults.Items.Add(lviItem);
					}
				}
			}
			
		}
		
		//================================================================================================
		//== All code/classes below used for sorting the summary table according to the selected column ==
		//------------------------------------------------------------------------------------------------
		
		private void SortResultsOnMultColumns()
		{
			// Sort the collection on multiple columns
			//========================================
			MultiComparer mcsMultiCol = new MultiComparer();
			
			mcsMultiCol.PrimaryField = modMain.dicColumns["Primary"];
			mcsMultiCol.SecondaryField = modMain.dicColumns["Secondary"];
			mcsMultiCol.TertiaryField = modMain.dicColumns["Tertiary"];
			
			modMain.rtResultsTracker.ScanResults.Sort(mcsMultiCol);
			
		}
		
		private void SortResults(int SortItem)
		{
			// Sort the collection - use specified item for comparison basis
			//==============================================================
			
			SeverityComparer scSevComp = new SeverityComparer();
			TitleComparer tcTitleComp = new TitleComparer();
			DescComparer dcDescComp = new DescComparer();
			FileComparer fcFileComp = new FileComparer();
			
			
			switch (SortItem)
			{
				case PRIORITY_COL:
					// Severity
					blnIsAscendingSeverity = !blnIsAscendingSeverity;
					modMain.rtResultsTracker.ScanResults.Sort(scSevComp);
					break;
				case SEVERITY_COL:
					// Severity
					blnIsAscendingSeverity = !blnIsAscendingSeverity;
					modMain.rtResultsTracker.ScanResults.Sort(scSevComp);
					break;
				case TITLE_COL:
					// Issue name/title
					blnIsAscendingTitle = !blnIsAscendingTitle;
					modMain.rtResultsTracker.ScanResults.Sort(tcTitleComp);
					break;
				case DESC_COL:
					// Issue description
					blnIsAscendingDescription = !blnIsAscendingDescription;
					modMain.rtResultsTracker.ScanResults.Sort(dcDescComp);
					break;
				case FILE_COL:
					// Filename
					blnIsAscendingFile = !blnIsAscendingFile;
					modMain.rtResultsTracker.ScanResults.Sort(fcFileComp);
					break;
			}
			
		}
		
		private class SeverityComparer : IComparer
		{
			
			public int Compare(dynamic x, dynamic y)
			{
				//Compare results by severity
				//===========================
				
				ScanResult scLeftResult = new ScanResult();
				ScanResult scRightResult = new ScanResult();
				scLeftResult = (ScanResult) x;
				scRightResult = (ScanResult) y;
				
				if (frmMain.Default.blnIsAscendingSeverity)
				{
					return new CaseInsensitiveComparer().Compare(x.Severity, y.Severity);
				}
				else
				{
					return new CaseInsensitiveComparer().Compare(y.Severity, x.Severity);
				}
				
				//== Avoid the GUI locking or hanging during processing ==
				Application.DoEvents();
				
			}
		}
		
		
		public class ThreadedFileCollector
		{
			public List<string> CollectFiles(DirectoryInfo directory, string pattern)
			{
				ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
				InternalCollectFiles(directory, pattern, queue);
				return queue.AsEnumerable().ToList();
			}
			
			private void InternalCollectFiles(DirectoryInfo directory, string pattern, ConcurrentQueue<string> queue)
			{
				try
				{
					foreach (string result in directory.GetFiles(pattern).Select(file => file.FullName))
					{
						if (modMain.asAppSettings.IsAllFileTypes || CheckFileType(result) == true)
						{
							queue.Enqueue(result);
						}
					}
					Task.WaitAll(directory.GetDirectories().Select(dir => Task.Factory.StartNew(() => InternalCollectFiles((DirectoryInfo) ((object) (FileSystem.Dir())), pattern, queue))).ToArray());
				}
				catch (System.IO.PathTooLongException ex)
				{
					//suppress
					queue.Enqueue(ex.Message.ToString());
				}
			}
			
			
			private bool CheckFileType(object TargetFile)
			{
				// Check file type is consistent with required language
				//=====================================================
				bool blnRetVal = false;
				
				
				//== Iterate through suffix array and compare to the end of current filename ==
				for (var intIndex = 0; intIndex <= modMain.asAppSettings.NumSuffixes; intIndex++)
				{
					string sCurSuffix = System.Convert.ToString(modMain.asAppSettings.FileSuffixes.GetValue(intIndex).ToString());
					if (!string.IsNullOrEmpty(sCurSuffix.Trim()) && TargetFile.ToString().ToLower().EndsWith(sCurSuffix))
					{
						blnRetVal = true;
						break;
					}
				}
				
				return blnRetVal;
				
			}
			
		}
		
		private class TitleComparer : IComparer
		{
			
			public int Compare(dynamic x, dynamic y)
			{
				//Compare results by severity
				//===========================
				
				ScanResult scLeftResult = new ScanResult();
				ScanResult scRightResult = new ScanResult();
				scLeftResult = (ScanResult) x;
				scRightResult = (ScanResult) y;
				
				if (frmMain.Default.blnIsAscendingTitle)
				{
					return new CaseInsensitiveComparer().Compare(x.Title, y.Title);
				}
				else
				{
					return new CaseInsensitiveComparer().Compare(y.Title, x.Title);
				}
				
				//== Avoid the GUI locking or hanging during processing ==
				Application.DoEvents();
				
			}
		}
		
		private class DescComparer : IComparer
		{
			
			public int Compare(dynamic x, dynamic y)
			{
				//Compare results by severity
				//===========================
				
				ScanResult scLeftResult = new ScanResult();
				ScanResult scRightResult = new ScanResult();
				scLeftResult = (ScanResult) x;
				scRightResult = (ScanResult) y;
				
				if (frmMain.Default.blnIsAscendingDescription)
				{
					return new CaseInsensitiveComparer().Compare(x.Description, y.Description);
				}
				else
				{
					return new CaseInsensitiveComparer().Compare(y.Description, x.Description);
				}
				
				//== Avoid the GUI locking or hanging during processing ==
				Application.DoEvents();
				
			}
		}
		
		private class FileComparer : IComparer
		{
			
			public int Compare(dynamic x, dynamic y)
			{
				//Compare results by severity
				//===========================
				
				ScanResult scLeftResult = new ScanResult();
				ScanResult scRightResult = new ScanResult();
				scLeftResult = (ScanResult) x;
				scRightResult = (ScanResult) y;
				
				if (frmMain.Default.blnIsAscendingFile)
				{
					return new CaseInsensitiveComparer().Compare(x.FileName, y.FileName);
				}
				else
				{
					return new CaseInsensitiveComparer().Compare(y.FileName, x.FileName);
				}
				
				//== Avoid the GUI locking or hanging during processing ==
				Application.DoEvents();
				
			}
		}
		
		private class MultiComparer : IComparer
		{
			
			public int PrimaryField;
			public int SecondaryField;
			public int TertiaryField;
			
			public int Compare(object x, object y)
			{
				//Compare results by multiple fields
				//==================================
				int intRetVal = 0;
				
				ScanResult scLeftResult = new ScanResult();
				ScanResult scRightResult = new ScanResult();
				
				scLeftResult = (ScanResult) x;
				scRightResult = (ScanResult) y;
				
				intRetVal = CompareFields(x, y, PrimaryField);
				if (intRetVal == 0)
				{
					intRetVal = CompareFields(x, y, SecondaryField);
					if (intRetVal == 0)
					{
						intRetVal = CompareFields(x, y, TertiaryField);
					}
				}
				
				//== Avoid the GUI locking or hanging during processing ==
				Application.DoEvents();
				
				return intRetVal;
				
			}
			
			private int CompareFields(dynamic x, dynamic y, int FieldID)
			{
				
				int intRetVal = 0;
				
				switch (FieldID)
				{
					case SEVERITY_COL:
						// Severity
						intRetVal = new CaseInsensitiveComparer().Compare(x.Severity, y.Severity);
						break;
					case TITLE_COL:
						// Issue name/title
						intRetVal = new CaseInsensitiveComparer().Compare(x.Title, y.Title);
						break;
					case DESC_COL:
						// Issue description
						intRetVal = new CaseInsensitiveComparer().Compare(x.Description, y.Description);
						break;
					case FILE_COL:
						// Filename
						intRetVal = new CaseInsensitiveComparer().Compare(x.FileName, y.FileName);
						break;
				}
				
				//== Avoid the GUI locking or hanging during processing ==
				Application.DoEvents();
				
				return intRetVal;
				
			}
			
		}
		
		public void VisualBadFuncBreakdownToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Scan the code only for items listed in the associated config file
			//==================================================================
			int intIndex = 0;
			
			
			modMain.asAppSettings.IsConfigOnly = true;
			
			//== If no data available then scan files in 'code only' mode ==
			if (modMain.rtResultsTracker.OverallLineCount == 0)
			{
				ScanFiles(false, true);
			}
			else
			{
				for (intIndex = 0; intIndex <= modMain.rtResultsTracker.FileDataList.Count - 1; intIndex++)
				{
					UpdateFileView(modMain.rtResultsTracker.FileDataList[intIndex] as FileData, intIndex, false);
				}
			}
			
			ShowResults();
			
		}
		
		public void btnSelectDir_Click(object sender, EventArgs e)
		{
			ShowSelectDirectoryDialog();
		}
	}
	
	
	
}
