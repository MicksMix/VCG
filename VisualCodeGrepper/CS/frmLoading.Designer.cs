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
	partial class frmLoading : System.Windows.Forms.Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoading));
			this.lblProgress = new System.Windows.Forms.Label();
			this.pbProgressBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			//
			//lblProgress
			//
			this.lblProgress.AutoSize = true;
			this.lblProgress.Location = new System.Drawing.Point(12, 50);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(39, 13);
			this.lblProgress.TabIndex = 3;
			this.lblProgress.Text = "Label1";
			//
			//pbProgressBar
			//
			this.pbProgressBar.Location = new System.Drawing.Point(12, 12);
			this.pbProgressBar.Name = "pbProgressBar";
			this.pbProgressBar.Size = new System.Drawing.Size(577, 26);
			this.pbProgressBar.TabIndex = 2;
			//
			//frmLoading
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 75);
			this.Controls.Add(this.lblProgress);
			this.Controls.Add(this.pbProgressBar);
			this.Icon = (System.Drawing.Icon) (resources.GetObject("$this.Icon"));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmLoading";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Processing...";
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.Label lblProgress;
		internal System.Windows.Forms.ProgressBar pbProgressBar;
	}
	
}
