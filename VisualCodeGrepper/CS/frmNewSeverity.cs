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
	public partial class frmNewSeverity
	{
		public frmNewSeverity()
		{
			InitializeComponent();
		}
		
		public void btnCancel_Click(System.Object sender, System.EventArgs e)
		{
			// Set severity marker to 'do nothing' and close form
			//===================================================
			
			modMain.intNewSeverity = -1;
			this.Close();
			
		}
		
		public void btnOK_Click(System.Object sender, System.EventArgs e)
		{
			// Set new severity marker and close form
			//=======================================
			
			modMain.intNewSeverity = 7 - cboNewLevel.SelectedIndex;
			this.Close();
			
		}
		
		public void frmNewSeverity_Load(System.Object sender, System.EventArgs e)
		{
			
			cboNewLevel.SelectedIndex = CodeIssue.STANDARD;
			
		}
		
	}
}
