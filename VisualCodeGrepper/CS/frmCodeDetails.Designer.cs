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
	partial class frmCodeDetails : System.Windows.Forms.Form
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
			this.rtbCodeDetails = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			//
			//rtbCodeDetails
			//
			this.rtbCodeDetails.BackColor = System.Drawing.SystemColors.Window;
			this.rtbCodeDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbCodeDetails.EnableAutoDragDrop = true;
			this.rtbCodeDetails.Font = new System.Drawing.Font("Century Gothic", (float) (9.75F), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.rtbCodeDetails.ForeColor = System.Drawing.SystemColors.MenuText;
			this.rtbCodeDetails.Location = new System.Drawing.Point(0, 0);
			this.rtbCodeDetails.Name = "rtbCodeDetails";
			this.rtbCodeDetails.Size = new System.Drawing.Size(1250, 527);
			this.rtbCodeDetails.TabIndex = 2;
			this.rtbCodeDetails.Text = "";
			//
			//frmCodeDetails
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1250, 527);
			this.Controls.Add(this.rtbCodeDetails);
			this.Name = "frmCodeDetails";
			this.Text = "frmCodeDetails";
			this.ResumeLayout(false);
			
		}
		internal System.Windows.Forms.RichTextBox rtbCodeDetails;
	}
	
}
