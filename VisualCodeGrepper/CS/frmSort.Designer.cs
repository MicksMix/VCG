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
	partial class frmSort : System.Windows.Forms.Form
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
			this.lblSort = new System.Windows.Forms.Label();
			this.cboPrimary = new System.Windows.Forms.ComboBox();
			this.lblPrimary = new System.Windows.Forms.Label();
			this.lblSecondary = new System.Windows.Forms.Label();
			this.cboSecondary = new System.Windows.Forms.ComboBox();
			this.lblTertiary = new System.Windows.Forms.Label();
			this.cboTertiary = new System.Windows.Forms.ComboBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.SuspendLayout();
			//
			//lblSort
			//
			this.lblSort.AutoSize = true;
			this.lblSort.Location = new System.Drawing.Point(3, 3);
			this.lblSort.Name = "lblSort";
			this.lblSort.Size = new System.Drawing.Size(141, 13);
			this.lblSort.TabIndex = 0;
			this.lblSort.Text = "Choose columns to sort on...";
			//
			//cboPrimary
			//
			this.cboPrimary.FormattingEnabled = true;
			this.cboPrimary.Items.AddRange(new object[] {"Severity", "Title", "Description", "Filename"});
			this.cboPrimary.Location = new System.Drawing.Point(103, 29);
			this.cboPrimary.Name = "cboPrimary";
			this.cboPrimary.Size = new System.Drawing.Size(169, 21);
			this.cboPrimary.TabIndex = 1;
			this.cboPrimary.Text = "Choose...";
			//
			//lblPrimary
			//
			this.lblPrimary.AutoSize = true;
			this.lblPrimary.Location = new System.Drawing.Point(6, 32);
			this.lblPrimary.Name = "lblPrimary";
			this.lblPrimary.Size = new System.Drawing.Size(66, 13);
			this.lblPrimary.TabIndex = 2;
			this.lblPrimary.Text = "Primary Sort:";
			//
			//lblSecondary
			//
			this.lblSecondary.AutoSize = true;
			this.lblSecondary.Location = new System.Drawing.Point(6, 61);
			this.lblSecondary.Name = "lblSecondary";
			this.lblSecondary.Size = new System.Drawing.Size(83, 13);
			this.lblSecondary.TabIndex = 4;
			this.lblSecondary.Text = "Secondary Sort:";
			//
			//cboSecondary
			//
			this.cboSecondary.FormattingEnabled = true;
			this.cboSecondary.Items.AddRange(new object[] {"Severity", "Title", "Description", "Filename"});
			this.cboSecondary.Location = new System.Drawing.Point(103, 58);
			this.cboSecondary.Name = "cboSecondary";
			this.cboSecondary.Size = new System.Drawing.Size(169, 21);
			this.cboSecondary.TabIndex = 3;
			this.cboSecondary.Text = "Choose...";
			//
			//lblTertiary
			//
			this.lblTertiary.AutoSize = true;
			this.lblTertiary.Location = new System.Drawing.Point(6, 88);
			this.lblTertiary.Name = "lblTertiary";
			this.lblTertiary.Size = new System.Drawing.Size(67, 13);
			this.lblTertiary.TabIndex = 6;
			this.lblTertiary.Text = "Tertiary Sort:";
			//
			//cboTertiary
			//
			this.cboTertiary.FormattingEnabled = true;
			this.cboTertiary.Items.AddRange(new object[] {"Severity", "Title", "Description", "Filename"});
			this.cboTertiary.Location = new System.Drawing.Point(103, 85);
			this.cboTertiary.Name = "cboTertiary";
			this.cboTertiary.Size = new System.Drawing.Size(169, 21);
			this.cboTertiary.TabIndex = 5;
			this.cboTertiary.Text = "Choose...";
			//
			//btnOK
			//
			this.btnOK.Location = new System.Drawing.Point(196, 115);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//btnCancel
			//
			this.btnCancel.Location = new System.Drawing.Point(115, 115);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			//frmSort
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(279, 147);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblTertiary);
			this.Controls.Add(this.cboTertiary);
			this.Controls.Add(this.lblSecondary);
			this.Controls.Add(this.cboSecondary);
			this.Controls.Add(this.lblPrimary);
			this.Controls.Add(this.cboPrimary);
			this.Controls.Add(this.lblSort);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSort";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sort";
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.Label lblSort;
		internal System.Windows.Forms.ComboBox cboPrimary;
		internal System.Windows.Forms.Label lblPrimary;
		internal System.Windows.Forms.Label lblSecondary;
		internal System.Windows.Forms.ComboBox cboSecondary;
		internal System.Windows.Forms.Label lblTertiary;
		internal System.Windows.Forms.ComboBox cboTertiary;
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Button btnCancel;
	}
	
}
