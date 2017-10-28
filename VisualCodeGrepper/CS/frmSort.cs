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
	
	public partial class frmSort
	{
		public frmSort()
		{
			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
		
#region Default Instance
		
		private static frmSort defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmSort Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmSort();
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
		
		public void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			this.Close();
		}
		
		public void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			// Store sort order on main form and exit
			//=======================================
			
			if ((cboPrimary.SelectedIndex == cboSecondary.SelectedIndex) || (cboPrimary.SelectedIndex == cboTertiary.SelectedIndex) || (cboSecondary.SelectedIndex == cboTertiary.SelectedIndex))
			{
				Interaction.MsgBox("Please select at least two different columns.", MsgBoxStyle.OkOnly, "Incorrect Column Selection");
				return;
			}
			
			modMain.dicColumns.Clear();
			
			// Need a primary and secondary sort for any of it to work...
			if (cboPrimary.SelectedIndex > -1 & cboSecondary.SelectedIndex > -1)
			{
				modMain.dicColumns.Add("Primary", cboPrimary.SelectedIndex + 1);
				modMain.dicColumns.Add("Secondary", cboSecondary.SelectedIndex + 1);
				if (cboTertiary.SelectedIndex != -1)
				{
					modMain.dicColumns.Add("Tertiary", cboTertiary.SelectedIndex + 1);
				}
				else
				{
					modMain.dicColumns.Add("Tertiary", -1);
				}
			}
			
			this.Close();
			
		}
		
	}
}
