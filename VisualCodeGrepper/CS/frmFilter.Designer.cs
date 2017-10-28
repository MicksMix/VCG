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
	partial class frmFilter : System.Windows.Forms.Form
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
			this.gbFilter = new System.Windows.Forms.GroupBox();
			base.Load += new System.EventHandler(frmFilter_Load);
			this.cboMaximum = new System.Windows.Forms.ComboBox();
			this.lblTo = new System.Windows.Forms.Label();
			this.rbRange = new System.Windows.Forms.RadioButton();
			this.rbRange.CheckedChanged += new System.EventHandler(this.rbRange_CheckedChanged);
			this.cboMinimum = new System.Windows.Forms.ComboBox();
			this.rbBelow = new System.Windows.Forms.RadioButton();
			this.rbBelow.CheckedChanged += new System.EventHandler(this.rbBelow_CheckedChanged);
			this.cboBelow = new System.Windows.Forms.ComboBox();
			this.rbAbove = new System.Windows.Forms.RadioButton();
			this.rbAbove.CheckedChanged += new System.EventHandler(this.rbAbove_CheckedChanged);
			this.cboAbove = new System.Windows.Forms.ComboBox();
			this.gbExport = new System.Windows.Forms.GroupBox();
			this.cbExport = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.gbFilter.SuspendLayout();
			this.gbExport.SuspendLayout();
			this.SuspendLayout();
			//
			//gbFilter
			//
			this.gbFilter.Controls.Add(this.cboMaximum);
			this.gbFilter.Controls.Add(this.lblTo);
			this.gbFilter.Controls.Add(this.rbRange);
			this.gbFilter.Controls.Add(this.cboMinimum);
			this.gbFilter.Controls.Add(this.rbBelow);
			this.gbFilter.Controls.Add(this.cboBelow);
			this.gbFilter.Controls.Add(this.rbAbove);
			this.gbFilter.Controls.Add(this.cboAbove);
			this.gbFilter.Location = new System.Drawing.Point(7, 4);
			this.gbFilter.Name = "gbFilter";
			this.gbFilter.Size = new System.Drawing.Size(551, 113);
			this.gbFilter.TabIndex = 0;
			this.gbFilter.TabStop = false;
			this.gbFilter.Text = "Filter Options";
			//
			//cboMaximum
			//
			this.cboMaximum.Enabled = false;
			this.cboMaximum.FormattingEnabled = true;
			this.cboMaximum.Items.AddRange(new object[] {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"});
			this.cboMaximum.Location = new System.Drawing.Point(385, 73);
			this.cboMaximum.Name = "cboMaximum";
			this.cboMaximum.Size = new System.Drawing.Size(157, 21);
			this.cboMaximum.TabIndex = 10;
			//
			//lblTo
			//
			this.lblTo.AutoSize = true;
			this.lblTo.Location = new System.Drawing.Point(360, 76);
			this.lblTo.Name = "lblTo";
			this.lblTo.Size = new System.Drawing.Size(19, 13);
			this.lblTo.TabIndex = 9;
			this.lblTo.Text = "to:";
			//
			//rbRange
			//
			this.rbRange.AutoSize = true;
			this.rbRange.Location = new System.Drawing.Point(6, 73);
			this.rbRange.Name = "rbRange";
			this.rbRange.Size = new System.Drawing.Size(164, 17);
			this.rbRange.TabIndex = 8;
			this.rbRange.TabStop = true;
			this.rbRange.Text = "Display Results in the Range:";
			this.rbRange.UseVisualStyleBackColor = true;
			//
			//cboMinimum
			//
			this.cboMinimum.Enabled = false;
			this.cboMinimum.FormattingEnabled = true;
			this.cboMinimum.Items.AddRange(new object[] {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"});
			this.cboMinimum.Location = new System.Drawing.Point(196, 73);
			this.cboMinimum.Name = "cboMinimum";
			this.cboMinimum.Size = new System.Drawing.Size(157, 21);
			this.cboMinimum.TabIndex = 7;
			//
			//rbBelow
			//
			this.rbBelow.AutoSize = true;
			this.rbBelow.Location = new System.Drawing.Point(6, 46);
			this.rbBelow.Name = "rbBelow";
			this.rbBelow.Size = new System.Drawing.Size(186, 17);
			this.rbBelow.TabIndex = 6;
			this.rbBelow.TabStop = true;
			this.rbBelow.Text = "Display Results Equal to or Below:";
			this.rbBelow.UseVisualStyleBackColor = true;
			//
			//cboBelow
			//
			this.cboBelow.Enabled = false;
			this.cboBelow.FormattingEnabled = true;
			this.cboBelow.Items.AddRange(new object[] {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"});
			this.cboBelow.Location = new System.Drawing.Point(196, 46);
			this.cboBelow.Name = "cboBelow";
			this.cboBelow.Size = new System.Drawing.Size(157, 21);
			this.cboBelow.TabIndex = 5;
			//
			//rbAbove
			//
			this.rbAbove.AutoSize = true;
			this.rbAbove.Checked = true;
			this.rbAbove.Location = new System.Drawing.Point(6, 19);
			this.rbAbove.Name = "rbAbove";
			this.rbAbove.Size = new System.Drawing.Size(188, 17);
			this.rbAbove.TabIndex = 4;
			this.rbAbove.TabStop = true;
			this.rbAbove.Text = "Display Results Equal to or Above:";
			this.rbAbove.UseVisualStyleBackColor = true;
			//
			//cboAbove
			//
			this.cboAbove.FormattingEnabled = true;
			this.cboAbove.Items.AddRange(new object[] {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"});
			this.cboAbove.Location = new System.Drawing.Point(196, 19);
			this.cboAbove.Name = "cboAbove";
			this.cboAbove.Size = new System.Drawing.Size(157, 21);
			this.cboAbove.TabIndex = 3;
			//
			//gbExport
			//
			this.gbExport.Controls.Add(this.cbExport);
			this.gbExport.Location = new System.Drawing.Point(7, 124);
			this.gbExport.Name = "gbExport";
			this.gbExport.Size = new System.Drawing.Size(551, 44);
			this.gbExport.TabIndex = 1;
			this.gbExport.TabStop = false;
			this.gbExport.Text = "Export";
			//
			//cbExport
			//
			this.cbExport.AutoSize = true;
			this.cbExport.Location = new System.Drawing.Point(6, 19);
			this.cbExport.Name = "cbExport";
			this.cbExport.Size = new System.Drawing.Size(158, 17);
			this.cbExport.TabIndex = 0;
			this.cbExport.Text = "Export Results After Filtering";
			this.cbExport.UseVisualStyleBackColor = true;
			//
			//btnOK
			//
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(483, 174);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(402, 174);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			//frmFilter
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(565, 203);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.gbExport);
			this.Controls.Add(this.gbFilter);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmFilter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Results Filter";
			this.gbFilter.ResumeLayout(false);
			this.gbFilter.PerformLayout();
			this.gbExport.ResumeLayout(false);
			this.gbExport.PerformLayout();
			this.ResumeLayout(false);
			
		}
		internal System.Windows.Forms.GroupBox gbFilter;
		internal System.Windows.Forms.ComboBox cboAbove;
		internal System.Windows.Forms.RadioButton rbBelow;
		internal System.Windows.Forms.ComboBox cboBelow;
		internal System.Windows.Forms.RadioButton rbAbove;
		internal System.Windows.Forms.RadioButton rbRange;
		internal System.Windows.Forms.ComboBox cboMinimum;
		internal System.Windows.Forms.ComboBox cboMaximum;
		internal System.Windows.Forms.Label lblTo;
		internal System.Windows.Forms.GroupBox gbExport;
		internal System.Windows.Forms.CheckBox cbExport;
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.Button btnCancel;
	}
	
}
