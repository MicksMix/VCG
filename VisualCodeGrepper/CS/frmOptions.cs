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
	
	
	public partial class frmOptions
	{
		public frmOptions()
		{
			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
		
#region Default Instance
		
		private static frmOptions defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmOptions Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmOptions();
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
		
		public void btnCPPEdit_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.asAppSettings.CConfFile);
		}
		
		public void btnJavaEdit_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.asAppSettings.JavaConfFile);
		}
		
		public void btnSQLEdit_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.asAppSettings.PLSQLConfFile);
		}
		
		public void btnCSharpEdit_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.asAppSettings.CSharpConfFile);
		}
		
		public void btnVBEdit_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.asAppSettings.VBConfFile);
		}
		
		public void btnPHPEdit_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.asAppSettings.PHPConfFile);
		}
		
		public void btnCobolEdit_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.asAppSettings.COBOLConfFile);
		}
		
		public void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			// Apply all new settings and exit
			//================================
			
			// Assign the new file suffixes to the appropriate language
			AssignFileSuffixes();
			
			// Set test language
			modMain.SelectLanguage(cboCurrentLanguage.SelectedIndex);
			modMain.asAppSettings.StartType = cboStartUpLanguage.SelectedIndex;
			
			// Set conf file locations
			if (txtCPP.Text.Trim() != "")
			{
				modMain.asAppSettings.CConfFile = txtCPP.Text.Trim();
			}
			if (txtJava.Text.Trim() != "")
			{
				modMain.asAppSettings.JavaConfFile = txtJava.Text.Trim();
			}
			if (txtPLSQL.Text.Trim() != "")
			{
				modMain.asAppSettings.PLSQLConfFile = txtPLSQL.Text.Trim();
			}
			if (txtCSharp.Text.Trim() != "")
			{
				modMain.asAppSettings.CSharpConfFile = txtCSharp.Text.Trim();
			}
			if (txtVB.Text.Trim() != "")
			{
				modMain.asAppSettings.VBConfFile = txtVB.Text.Trim();
			}
			if (txtPHP.Text.Trim() != "")
			{
				modMain.asAppSettings.PHPConfFile = txtPHP.Text.Trim();
			}
			if (txtCobol.Text.Trim() != "")
			{
				modMain.asAppSettings.COBOLConfFile = txtCobol.Text.Trim();
			}
			
			// Set reporting level - the reverse order in the dropdown list necessitates "7 - selected val"
			modMain.asAppSettings.OutputLevel = 7 - cboReporting.SelectedIndex;
			
			
			//======= Java settings =======
			// Set OWASP compliance
			modMain.asAppSettings.IsFinalizeCheck = cbFinalize.Checked;
			modMain.asAppSettings.IsInnerClassCheck = cbInnerClass.Checked;
			//Android checks
			modMain.asAppSettings.IsAndroid = cbAndroid.Checked;
			//----------------------------------------------
			
			// Set output file
			if (modMain.asAppSettings.IsOutputFile == true && txtOutput.Text.Trim() != "")
			{
				modMain.asAppSettings.OutputFile = txtOutput.Text.Trim();
			}
			
			// Set XML Export options
			frmMain.Default.SaveCheckState = cbSaveState.Checked;
			frmMain.Default.SaveFiltered = rbFiltered.Checked;
			
			// Include any required beta functionality
			SetBetaDetails(cbSigned.Checked, cbCobol.Checked);
			
			// Load contents of temporary grep box into bad function array
			if (txtTempGrep.Text.Trim() == "")
			{
				modMain.asAppSettings.TempGrepText = "";
				modMain.LoadUnsafeFunctionList(modMain.asAppSettings.TestType);
				return;
			}
			else
			{
				modMain.asAppSettings.TempGrepText = txtTempGrep.Text;
				LoadTempGrepContent(txtTempGrep.Text);
			}
			
			
			if (rbFiltered.Checked && CheckFilters() == false)
			{
				return;
			}
			
			this.Close();
			
		}
		
		private void cbOutput_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			modMain.asAppSettings.IsOutputFile = System.Convert.ToBoolean(cbOutput.CheckState);
		}
		
		private void AssignFileSuffixes()
		{
			// Assign the chosen suffixes to the relevant language
			//====================================================
			
			switch (cboFileTypes.SelectedIndex)
			{
				case AppSettings.C:
					modMain.asAppSettings.CSuffixes = txtFileTypes.Text;
					break;
				case AppSettings.JAVA:
					modMain.asAppSettings.JavaSuffixes = txtFileTypes.Text;
					break;
				case AppSettings.SQL:
					modMain.asAppSettings.PLSQLSuffixes = txtFileTypes.Text;
					break;
				case AppSettings.CSHARP:
					modMain.asAppSettings.CSharpSuffixes = txtFileTypes.Text;
					break;
				case AppSettings.VB:
					modMain.asAppSettings.VBSuffixes = txtFileTypes.Text;
					break;
				case AppSettings.PHP:
					modMain.asAppSettings.PHPSuffixes = txtFileTypes.Text;
					break;
				case AppSettings.COBOL:
					modMain.asAppSettings.COBOLSuffixes = txtFileTypes.Text;
					break;
			}
			
		}
		
		public void frmOptions_Load(System.Object sender, System.EventArgs e)
		{
			// Get current settings and load values into controls
			//===================================================
			
			// Current language
			cboCurrentLanguage.SelectedIndex = modMain.asAppSettings.TestType;
			cboStartUpLanguage.SelectedIndex = modMain.asAppSettings.StartType;
			
			// File suffixes
			cboFileTypes.SelectedIndex = modMain.asAppSettings.TestType;
			
			// Config files
			txtCPP.Text = modMain.asAppSettings.CConfFile;
			txtJava.Text = modMain.asAppSettings.JavaConfFile;
			txtPLSQL.Text = modMain.asAppSettings.PLSQLConfFile;
			txtCSharp.Text = modMain.asAppSettings.CSharpConfFile;
			txtVB.Text = modMain.asAppSettings.VBConfFile;
			txtPHP.Text = modMain.asAppSettings.PHPConfFile;
			txtCobol.Text = modMain.asAppSettings.COBOLConfFile;
			
			// Output settings
			cboReporting.SelectedIndex = 7 - modMain.asAppSettings.OutputLevel;
			
			// OWASP compliance
			cbFinalize.Checked = modMain.asAppSettings.IsFinalizeCheck;
			cbInnerClass.Checked = modMain.asAppSettings.IsInnerClassCheck;
			
			// Android checks
			cbAndroid.Checked = modMain.asAppSettings.IsAndroid;
			
			// Output file
			cbOutput.Checked = modMain.asAppSettings.IsOutputFile;
			txtOutput.Text = modMain.asAppSettings.OutputFile;
			
			// XML file and filter
			cbSaveState.Checked = frmMain.Default.SaveCheckState;
			rbFiltered.Checked = frmMain.Default.SaveFiltered;
			rbAll.Checked = !frmMain.Default.SaveFiltered;
			SetFilterDetails();
			
			// Beta functionality
			cbSigned.Checked = modMain.asAppSettings.IncludeSigned;
			cbCobol.Checked = modMain.asAppSettings.IncludeCobol;
			SetBetaDetails(modMain.asAppSettings.IncludeSigned, modMain.asAppSettings.IncludeCobol);
			
			// Temporary Grep text
			txtTempGrep.Text = modMain.asAppSettings.TempGrepText;
			
		}
		
		public void cboFileTypes_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			// Display suffixes for selected language
			//=======================================
			
			switch (cboFileTypes.SelectedIndex)
			{
				case AppSettings.C:
					txtFileTypes.Text = modMain.asAppSettings.CSuffixes;
					break;
				case AppSettings.JAVA:
					txtFileTypes.Text = modMain.asAppSettings.JavaSuffixes;
					break;
				case AppSettings.SQL:
					txtFileTypes.Text = modMain.asAppSettings.PLSQLSuffixes;
					break;
				case AppSettings.CSHARP:
					txtFileTypes.Text = modMain.asAppSettings.CSharpSuffixes;
					break;
				case AppSettings.VB:
					txtFileTypes.Text = modMain.asAppSettings.VBSuffixes;
					break;
				case AppSettings.PHP:
					txtFileTypes.Text = modMain.asAppSettings.PHPSuffixes;
					break;
				case AppSettings.COBOL:
					txtFileTypes.Text = modMain.asAppSettings.COBOLSuffixes;
					break;
			}
			
		}
		
		public void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		public void btnCPPBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new C++ config file
			//========================================
			
			// Get target directory
			ofdOpenFileDialog.ShowDialog();
			
			if (!System.Convert.ToBoolean(System.Windows.Forms.DialogResult.Cancel & (System.Windows.Forms.DialogResult) ((object) (ofdOpenFileDialog.FileName != ""))))
			{
				txtCPP.Text = ofdOpenFileDialog.FileName;
				modMain.asAppSettings.CConfFile = ofdOpenFileDialog.FileName;
			}
			
		}
		
		public void btnJavaBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new Java config file
			//========================================
			
			// Get target directory
			ofdOpenFileDialog.ShowDialog();
			
			if (!System.Convert.ToBoolean(System.Windows.Forms.DialogResult.Cancel & (System.Windows.Forms.DialogResult) ((object) (ofdOpenFileDialog.FileName != ""))))
			{
				txtJava.Text = ofdOpenFileDialog.FileName;
				modMain.asAppSettings.JavaConfFile = ofdOpenFileDialog.FileName;
			}
			
		}
		
		public void btnSQLBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new PL/SQL config file
			//===========================================
			
			// Get target directory
			ofdOpenFileDialog.ShowDialog();
			
			if (!System.Convert.ToBoolean(System.Windows.Forms.DialogResult.Cancel & (System.Windows.Forms.DialogResult) ((object) (ofdOpenFileDialog.FileName != ""))))
			{
				txtPLSQL.Text = ofdOpenFileDialog.FileName;
				modMain.asAppSettings.PLSQLConfFile = ofdOpenFileDialog.FileName;
			}
			
		}
		
		public void btnVBBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new VB config file
			//=======================================
			
			// Get target directory
			ofdOpenFileDialog.ShowDialog();
			
			if (!System.Convert.ToBoolean(System.Windows.Forms.DialogResult.Cancel & (System.Windows.Forms.DialogResult) ((object) (ofdOpenFileDialog.FileName != ""))))
			{
				txtVB.Text = ofdOpenFileDialog.FileName;
				modMain.asAppSettings.VBConfFile = ofdOpenFileDialog.FileName;
			}
		}
		
		public void btnPHPBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new PHP config file
			//=======================================
			
			// Get target directory
			ofdOpenFileDialog.ShowDialog();
			
			if (!System.Convert.ToBoolean(System.Windows.Forms.DialogResult.Cancel & (System.Windows.Forms.DialogResult) ((object) (ofdOpenFileDialog.FileName != ""))))
			{
				txtPHP.Text = ofdOpenFileDialog.FileName;
				modMain.asAppSettings.PHPConfFile = ofdOpenFileDialog.FileName;
			}
		}
		
		public void btnCobolBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new COBOL config file
			//==========================================
			
			// Get target directory
			ofdOpenFileDialog.ShowDialog();
			
			if (!System.Convert.ToBoolean(System.Windows.Forms.DialogResult.Cancel & (System.Windows.Forms.DialogResult) ((object) (ofdOpenFileDialog.FileName != ""))))
			{
				txtCobol.Text = ofdOpenFileDialog.FileName;
				modMain.asAppSettings.COBOLConfFile = ofdOpenFileDialog.FileName;
			}
			
		}
		
		private void btnOutputBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new output file
			//====================================
			
			// Get target directory
			sfdSaveFileDialog.ShowDialog();
			
			if (System.Convert.ToBoolean(~System.Convert.ToInt32(System.Windows.Forms.DialogResult.Cancel)))
			{
				txtOutput.Text = sfdSaveFileDialog.FileName;
				modMain.asAppSettings.CConfFile = sfdSaveFileDialog.FileName;
			}
			
		}
		
		public void btnCSharpBrowse_Click(System.Object sender, System.EventArgs e)
		{
			// Show dialog box for new C++ config file
			//========================================
			
			// Get target directory
			ofdOpenFileDialog.ShowDialog();
			
			if (!System.Convert.ToBoolean(System.Windows.Forms.DialogResult.Cancel & (System.Windows.Forms.DialogResult) ((object) (ofdOpenFileDialog.FileName != ""))))
			{
				txtCSharp.Text = ofdOpenFileDialog.FileName;
				modMain.asAppSettings.CSharpConfFile = ofdOpenFileDialog.FileName;
			}
			
		}
		
		public void rbAbove_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			// Enable/disable relevant controls
			//=================================
			cboAbove.Enabled = true;
			cboBelow.Enabled = false;
			cboMinimum.Enabled = false;
			cboMaximum.Enabled = false;
		}
		
		public void rbBelow_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			// Enable/disable relevant controls
			//=================================
			cboBelow.Enabled = true;
			cboAbove.Enabled = false;
			cboMinimum.Enabled = false;
			cboMaximum.Enabled = false;
		}
		
		public void rbRange_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			// Enable/disable relevant controls
			//=================================
			cboMinimum.Enabled = true;
			cboMaximum.Enabled = true;
			cboBelow.Enabled = false;
			cboAbove.Enabled = false;
		}
		
		public void btnExport_Click(System.Object sender, System.EventArgs e)
		{
			// Assign new values and then export to XML file
			//==============================================
			CheckFilters();
			frmMain.Default.ExportResultsXML();
		}
		
		private bool CheckFilters()
		{
			//Check filter values are valid
			//=============================
			bool blnRetVal = true;
			int intMinimum = CodeIssue.POSSIBLY_SAFE;
			int intMaximum = CodeIssue.CRITICAL;
			
			if (rbAbove.Checked == true)
			{
				intMinimum = 7 - cboAbove.SelectedIndex;
			}
			else if (rbBelow.Checked == true)
			{
				intMaximum = 7 - cboBelow.SelectedIndex;
			}
			else
			{
				intMinimum = 7 - cboMinimum.SelectedIndex;
				intMaximum = 7 - cboMaximum.SelectedIndex;
				if (intMaximum > intMinimum)
				{
					Interaction.MsgBox("Maximum value cannot be less than minmum value.", (MsgBoxStyle) Constants.vbOK, "Invalid Values");
					blnRetVal = false;
				}
			}
			
			if (blnRetVal)
			{
				frmMain.Default.FilterResults(intMinimum, intMaximum);
			}
			
			return blnRetVal;
			
		}
		
		private void SetFilterDetails()
		{
			// Start with a correct and valid option selected
			//===============================================
			
			cboAbove.SelectedIndex = 0;
			cboBelow.SelectedIndex = 0;
			cboMinimum.SelectedIndex = 0;
			cboMaximum.SelectedIndex = 6;
			
			if (frmMain.Default.SaveFiltered)
			{
				rbRange.Checked = true;
				cboMinimum.SelectedIndex = 7 - frmMain.Default.intFilterMin;
				cboMaximum.SelectedIndex = 7 - frmMain.Default.intFilterMax;
			}
			
		}
		
		private void SetBetaDetails(bool IncludeSigned, bool IncludeCobol)
		{
			//Implement any beta functionality that the user requires
			//=======================================================
			
			// C/C++ signed/unsigned comparison
			modMain.asAppSettings.IncludeSigned = IncludeSigned;
			
			// COBOL scanning
			lblCobol.Visible = IncludeCobol;
			txtCobol.Visible = IncludeCobol;
			btnCobolBrowse.Visible = IncludeCobol;
			btnCobolEdit.Visible = IncludeCobol;
			
			// Enable/disable controls on main form
			frmMain.Default.COBOLToolStripMenuItem.Visible = IncludeCobol;
			
			if (IncludeCobol == true)
			{
				cboCurrentLanguage.Items.Add("COBOL");
				cboStartUpLanguage.Items.Add("COBOL");
			}
			else
			{
				cboCurrentLanguage.Items.Remove("COBOL");
				cboStartUpLanguage.Items.Remove("COBOL");
			}
			
		}
		
		public void btnColour_Click(System.Object sender, System.EventArgs e)
		{
			// Allow user to modify the colour for checked listbox items
			//==========================================================
			
			if (cdColorDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
			{
				modMain.asAppSettings.ListItemColour = cdColorDialog.Color;
			}
			
		}
		
		public void LoadTempGrepContent(string TempGrepText)
		{
			// Take content of temp grep box and add to the list of bad functions
			//===================================================================
			string[] arrTempGrepContent = null;
			string strDescription = "";
			string[] arrFuncList = null;
			
			
			arrTempGrepContent = TempGrepText.Split(Constants.vbNewLine.ToCharArray());
			
			try
			{
				foreach (var strLine in arrTempGrepContent)
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
						
						if (!modMain.asAppSettings.BadFunctions.Contains(ciCodeIssue))
						{
							modMain.asAppSettings.BadFunctions.Add(ciCodeIssue);
						}
					}
				}
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			
		}
		
	}
}
