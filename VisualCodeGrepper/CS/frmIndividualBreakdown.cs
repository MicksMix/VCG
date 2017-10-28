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


namespace VisualCodeGrepper
{
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
	
	public partial class frmIndividualBreakdown
	{
		public frmIndividualBreakdown()
		{
			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
		
#region Default Instance
		
		private static frmIndividualBreakdown defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmIndividualBreakdown Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmIndividualBreakdown();
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
		
		private void ExitToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		public void OpenWithNotepadToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			modMain.LaunchNPP(modMain.strCurrentFileName);
		}
		
		public void CopyUnsafeMethodsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Loop throught comments, format them and add to clipboard
			//=========================================================
			string strCode = "";
			
			
			if (modMain.intCodeIssues > 0)
			{
				foreach (ScanResult srItem in modMain.rtResultsTracker.ScanResults)
				{
					if (srItem.FileName == modMain.strCurrentFileName && srItem.Severity() != CodeIssue.INFO)
					{
						strCode += "File: " + srItem.FileName + Constants.vbNewLine;
						strCode += "Line: " + System.Convert.ToString(srItem.LineNumber) + Constants.vbNewLine;
						strCode += "Issue: " + srItem.Title + Constants.vbNewLine + srItem.Description + Constants.vbNewLine + srItem.CodeLine + Constants.vbNewLine + Constants.vbNewLine;
					}
				}
				Clipboard.SetText(strCode);
			}
			
		}
		
		public void CopyCommentsToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			// Loop throught comments, format them and add to clipboard
			//=========================================================
			string strComments = "";
			
			
			if (modMain.intComments > 0)
			{
				foreach (ScanResult srItem in modMain.rtResultsTracker.FixMeList)
				{
					if (srItem.FileName == modMain.strCurrentFileName)
					{
						strComments += "File: " + srItem.FileName + Constants.vbNewLine;
						strComments += "Line: " + System.Convert.ToString(srItem.LineNumber) + Constants.vbNewLine + "Contains: '" + srItem.CodeLine + "'" + Constants.vbNewLine + Constants.vbNewLine;
					}
				}
				Clipboard.SetText(strComments);
			}
			
		}
		
	}
}
