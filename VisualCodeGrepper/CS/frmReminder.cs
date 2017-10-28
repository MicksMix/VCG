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
	
	public partial class frmReminder
	{
		public frmReminder()
		{
			InitializeComponent();
			
			//Added to support default instance behavour in C#
			if (defaultInstance == null)
				defaultInstance = this;
		}
		
#region Default Instance
		
		private static frmReminder defaultInstance;
		
		/// <summary>
		/// Added by the VB.Net to C# Converter to support default instance behavour in C#
		/// </summary>
		public static frmReminder Default
		{
			get
			{
				if (defaultInstance == null)
				{
					defaultInstance = new frmReminder();
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
		
		public void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			// Write user choice to registry
			//==============================
			
			if (chkNotAgain.Checked)
			{
				Interaction.SaveSetting("VisualCodeGrepper", "Startup", "LangReminder", "0");
			}
			this.Hide();
			
		}
		
		public void lblReminder_Click(System.Object sender, System.EventArgs e)
		{
			
		}
		public void chkNotAgain_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			
		}
	}
}
