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
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class frmReminder : System.Windows.Forms.Form
	{
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			this.btnOK = new System.Windows.Forms.Button();
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.lblReminder = new System.Windows.Forms.Label();
			this.lblReminder.Click += new System.EventHandler(this.lblReminder_Click);
			this.chkNotAgain = new System.Windows.Forms.CheckBox();
			this.chkNotAgain.CheckedChanged += new System.EventHandler(this.chkNotAgain_CheckedChanged);
			this.SuspendLayout();
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(260, 72);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//lblReminder
			//
			this.lblReminder.Location = new System.Drawing.Point(5, 9);
			this.lblReminder.Name = "lblReminder";
			this.lblReminder.Size = new System.Drawing.Size(330, 40);
			this.lblReminder.TabIndex = 1;
			this.lblReminder.Text = "Remember to select the appropriate language from the 'Settings' menu before choos" + 
				"ing a direcory.";
			//
			//chkNotAgain
			//
			this.chkNotAgain.AutoSize = true;
			this.chkNotAgain.Location = new System.Drawing.Point(8, 53);
			this.chkNotAgain.Name = "chkNotAgain";
			this.chkNotAgain.Size = new System.Drawing.Size(180, 17);
			this.chkNotAgain.TabIndex = 2;
			this.chkNotAgain.Text = "Do not show this reminder again.";
			this.chkNotAgain.UseVisualStyleBackColor = true;
			//
			//frmReminder
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(345, 101);
			this.ControlBox = false;
			this.Controls.Add(this.chkNotAgain);
			this.Controls.Add(this.lblReminder);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmReminder";
			this.Text = "Select Language";
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Label lblReminder;
		internal System.Windows.Forms.CheckBox chkNotAgain;
	}
	
}
