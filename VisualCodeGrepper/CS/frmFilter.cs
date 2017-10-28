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
	
	public partial class frmFilter
	{
		public frmFilter()
		{
			InitializeComponent();
		}
		
		public void frmFilter_Load(System.Object sender, System.EventArgs e)
		{
			// Start with a valid option selected
			//===================================
			cboAbove.SelectedIndex = 0;
			cboBelow.SelectedIndex = 0;
			cboMinimum.SelectedIndex = 0;
			cboMaximum.SelectedIndex = 6;
		}
		
		public void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		public void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			// Conceal the form and apply the required updates
			//================================================
			int intMinimum = CodeIssue.POSSIBLY_SAFE;
			int intMaximum = CodeIssue.CRITICAL;
			
			
			//== Set report levels ==
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
					return;
				}
			}
			
			//== Hide the dialog ==
			this.Hide();
			
			//== Filter and export ==
			frmMain.Default.FilterResults(intMinimum, intMaximum);
			if (cbExport.Checked == true)
			{
				frmMain.Default.ExportResultsXML(intMinimum, intMaximum);
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
		
	}
}
