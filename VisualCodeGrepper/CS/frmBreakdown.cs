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
	
	public partial class frmBreakdown
	{
		public frmBreakdown()
		{
			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
		
#region Default Instance
		
		private static frmBreakdown defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmBreakdown Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmBreakdown();
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
		
		public void dgvResults_CellContentClick(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			// When user clicks a cell show the individual results for that file
			// using the Individual Breakdown form
			//==================================================================
			
			modMain.intComments = 0;
			modMain.intCodeIssues = 0;
			
			frmIndividualBreakdown.Default.Close();
			frmIndividualBreakdown.Default.Text = System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[0].Value);
			
			
			frmIndividualBreakdown.Default.chtResults.Series[0].Points.AddY(System.Convert.ToDouble(this.dgvResults.Rows[e.RowIndex].Cells[3].Value));
			frmIndividualBreakdown.Default.chtResults.Series[0].Points.AddY(System.Convert.ToDouble(this.dgvResults.Rows[e.RowIndex].Cells[4].Value));
			frmIndividualBreakdown.Default.chtResults.Series[0].Points.AddY(System.Convert.ToDouble(this.dgvResults.Rows[e.RowIndex].Cells[5].Value));
			if (this.dgvResults.Rows[e.RowIndex].Cells[6].Value != null)
			{
				frmIndividualBreakdown.Default.chtResults.Series[0].Points.AddY(System.Convert.ToDouble(this.dgvResults.Rows[e.RowIndex].Cells[6].Value));
				frmIndividualBreakdown.Default.chtResults.Series[0].Points[3].LegendText = "Potentially Unfinished Code (" + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[6].Value) + " counts)";
				this.pnlResults.Visible = true;
				modMain.intComments = System.Convert.ToInt32(this.dgvResults.Rows[e.RowIndex].Cells[6].Value);
			}
			if (this.dgvResults.Rows[e.RowIndex].Cells[7].Value != null)
			{
				frmIndividualBreakdown.Default.chtResults.Series[0].Points.AddY(System.Convert.ToDouble(this.dgvResults.Rows[e.RowIndex].Cells[7].Value));
				if (modMain.intComments > 0)
				{
					frmIndividualBreakdown.Default.chtResults.Series[0].Points[4].LegendText = "Potentially Bad Code (" + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[7].Value) + " counts)";
				}
				else
				{
					frmIndividualBreakdown.Default.chtResults.Series[0].Points[3].LegendText = "Potentially Bad Code (" + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[7].Value) + " counts)";
				}
				this.pnlResults.Visible = true;
				modMain.intCodeIssues = System.Convert.ToInt32(this.dgvResults.Rows[e.RowIndex].Cells[7].Value);
			}
			frmIndividualBreakdown.Default.chtResults.Series[0].Points[0].LegendText = "Overall Code (including comment-appended code) (" + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[3].Value) + " lines)";
			frmIndividualBreakdown.Default.chtResults.Series[0].Points[1].LegendText = "Overall Comments (" + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[4].Value) + " comments)";
			frmIndividualBreakdown.Default.chtResults.Series[0].Points[2].LegendText = "Overall Whitespace (" + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[5].Value) + " lines)";
			frmIndividualBreakdown.Default.chtResults.Series["Series1"]["BarLabelStyle"] = "Right";
			frmIndividualBreakdown.Default.chtResults.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
			frmIndividualBreakdown.Default.chtResults.Series["Series1"]["DrawingStyle"] = "Cylinder";
			
			
			modMain.strCurrentFileName = System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[8].Value);
			frmMain.CountFixMeComments(System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[8].Value));
			
			dynamic with_2 = frmIndividualBreakdown;
			with_2.lblResults.Text = "File: " + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[0].Value) + Constants.vbNewLine + "Total Line Count: " + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[1].Value) 
				+ Constants.vbNewLine + "\t" + "Number of Lines of Code (including comment-appended lines):" + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[3].Value) + Constants.vbNewLine + "\t" 
				+ "Number of Comments: " + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[4].Value) + Constants.vbNewLine + "\t" + "Lines of Whitespace: " + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[5].Value) 
				+ Constants.vbNewLine + Constants.vbNewLine + "Full Path: " + System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[8].Value);
			
			with_2.Show();
			
		}
		
		public void OpenInNotepadToolStripMenuItem_Click(System.Object Sender, System.EventArgs GridArgs)
		{
			
			if (!string.IsNullOrEmpty(modMain.strCurrentFileName))
			{
				modMain.LaunchNPP(modMain.strCurrentFileName);
			}
			
		}
		
		private void dgvResults_CellContentClick_1(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			
			if (!(e.RowIndex == -1))
			{
				modMain.strCurrentFileName = System.Convert.ToString(this.dgvResults.Rows[e.RowIndex].Cells[8].Value); //== Bodge for ordering columns ==
			}
			
		}
		
		private void ExitToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		public void ExportToClipboardToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
		{
			if (frmMain.Default.rtbResults.Text != "")
			{
				Clipboard.SetText(frmMain.Default.rtbResults.Text);
			}
		}
		
		public void ExitToolStripMenuItem_Click_1(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		public void btnApplyFilter_Click(System.Object sender, System.EventArgs e)
		{
			// Filter filename column, based on content of filter textbox
			//===========================================================
			string strText = "";
			
			
			//== Hide any rows where the filename does not contain the filter text ==
			foreach (DataGridViewRow itmItem in dgvResults.Rows)
			{
				if (itmItem.Cells[0].Value != null)
				{
					strText = System.Convert.ToString(itmItem.Cells[0].Value);
					if ((!strText.Contains(txtFilter.Text)) && (!(txtFilter.Text.Trim() == "")))
					{
						itmItem.Visible = false;
					}
					else
					{
						itmItem.Visible = true;
					}
				}
			}
			
		}
		
	}
	
	
	
}
