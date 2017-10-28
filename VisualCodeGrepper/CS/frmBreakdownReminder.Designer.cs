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
	partial class frmBreakdownReminder : System.Windows.Forms.Form
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
			this.chkNotAgain = new System.Windows.Forms.CheckBox();
			this.lblReminder = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.chkAlwaysShow = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			//
			//chkNotAgain
			//
			this.chkNotAgain.AutoSize = true;
			this.chkNotAgain.Location = new System.Drawing.Point(5, 89);
			this.chkNotAgain.Name = "chkNotAgain";
			this.chkNotAgain.Size = new System.Drawing.Size(180, 17);
			this.chkNotAgain.TabIndex = 5;
			this.chkNotAgain.Text = "Do not show this reminder again.";
			this.chkNotAgain.UseVisualStyleBackColor = true;
			//
			//lblReminder
			//
			this.lblReminder.Location = new System.Drawing.Point(2, 9);
			this.lblReminder.Name = "lblReminder";
			this.lblReminder.Size = new System.Drawing.Size(367, 40);
			this.lblReminder.TabIndex = 4;
			this.lblReminder.Text = "To see a visual breakdown of the LOC, number of comments, number of potential iss" + 
				"ues, etc. select 'Visual Code/Comment Breakdown' from the 'Scan' menu or or clic" + 
				"k the 'Always display' option below.";
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(287, 111);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//chkAlwaysShow
			//
			this.chkAlwaysShow.AutoSize = true;
			this.chkAlwaysShow.Location = new System.Drawing.Point(5, 66);
			this.chkAlwaysShow.Name = "chkAlwaysShow";
			this.chkAlwaysShow.Size = new System.Drawing.Size(264, 17);
			this.chkAlwaysShow.TabIndex = 6;
			this.chkAlwaysShow.Text = "Always display Visual Breakdown after every scan.";
			this.chkAlwaysShow.UseVisualStyleBackColor = true;
			//
			//frmBreakdownReminder
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(367, 140);
			this.ControlBox = false;
			this.Controls.Add(this.chkAlwaysShow);
			this.Controls.Add(this.chkNotAgain);
			this.Controls.Add(this.lblReminder);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmBreakdownReminder";
			this.Text = "Visual Breakdown";
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.CheckBox chkNotAgain;
		internal System.Windows.Forms.Label lblReminder;
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.CheckBox chkAlwaysShow;
	}
	
}
