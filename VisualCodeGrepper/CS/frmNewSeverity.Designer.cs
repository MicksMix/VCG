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
	partial class frmNewSeverity : System.Windows.Forms.Form
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
			base.Load += new System.EventHandler(frmNewSeverity_Load);
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.cboNewLevel = new System.Windows.Forms.ComboBox();
			this.lblNewLevel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			//
			//btnOK
			//
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(188, 39);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(107, 38);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			//cboNewLevel
			//
			this.cboNewLevel.FormattingEnabled = true;
			this.cboNewLevel.Items.AddRange(new object[] {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"});
			this.cboNewLevel.Location = new System.Drawing.Point(108, 9);
			this.cboNewLevel.Name = "cboNewLevel";
			this.cboNewLevel.Size = new System.Drawing.Size(154, 21);
			this.cboNewLevel.TabIndex = 8;
			//
			//lblNewLevel
			//
			this.lblNewLevel.AutoSize = true;
			this.lblNewLevel.Location = new System.Drawing.Point(3, 12);
			this.lblNewLevel.Name = "lblNewLevel";
			this.lblNewLevel.Size = new System.Drawing.Size(102, 13);
			this.lblNewLevel.TabIndex = 9;
			this.lblNewLevel.Text = "New Severity Level:";
			//
			//frmNewSeverity
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(272, 69);
			this.Controls.Add(this.lblNewLevel);
			this.Controls.Add(this.cboNewLevel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmNewSeverity";
			this.Text = "Set Severity";
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.ComboBox cboNewLevel;
		internal System.Windows.Forms.Label lblNewLevel;
	}
	
}
