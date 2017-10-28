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
	partial class frmIndividualBreakdown : System.Windows.Forms.Form
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea ChartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend Legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series Series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.chtResults = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.CopyCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyCommentsToolStripMenuItem.Click += new System.EventHandler(this.CopyCommentsToolStripMenuItem_Click);
			this.pnlMethods = new System.Windows.Forms.Panel();
			this.lblBreakdown = new System.Windows.Forms.TextBox();
			this.lblMethods = new System.Windows.Forms.TextBox();
			this.CopyUnsafeMethodsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyUnsafeMethodsToolStripMenuItem.Click += new System.EventHandler(this.CopyUnsafeMethodsToolStripMenuItem_Click);
			this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblResults = new System.Windows.Forms.Label();
			this.mnuMenuStrip = new System.Windows.Forms.MenuStrip();
			this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenWithNotepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenWithNotepadToolStripMenuItem.Click += new System.EventHandler(this.OpenWithNotepadToolStripMenuItem_Click);
			this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlResults = new System.Windows.Forms.Panel();
			this.Label2 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize) this.chtResults).BeginInit();
			this.pnlMethods.SuspendLayout();
			this.mnuMenuStrip.SuspendLayout();
			this.pnlResults.SuspendLayout();
			this.SuspendLayout();
			//
			//chtResults
			//
			this.chtResults.BackColor = System.Drawing.Color.Transparent;
			this.chtResults.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.chtResults.BackImageTransparentColor = System.Drawing.Color.Transparent;
			this.chtResults.BorderlineColor = System.Drawing.Color.Black;
			this.chtResults.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
			ChartArea1.Name = "ChartArea1";
			this.chtResults.ChartAreas.Add(ChartArea1);
			Legend1.Name = "Legend1";
			this.chtResults.Legends.Add(Legend1);
			this.chtResults.Location = new System.Drawing.Point(0, 26);
			this.chtResults.Name = "chtResults";
			this.chtResults.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
			Series1.ChartArea = "ChartArea1";
			Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
			Series1.IsValueShownAsLabel = true;
			Series1.Legend = "Legend1";
			Series1.Name = "Series1";
			this.chtResults.Series.Add(Series1);
			this.chtResults.Size = new System.Drawing.Size(880, 299);
			this.chtResults.TabIndex = 6;
			this.chtResults.Text = "Chart1";
			//
			//CopyCommentsToolStripMenuItem
			//
			this.CopyCommentsToolStripMenuItem.Name = "CopyCommentsToolStripMenuItem";
			this.CopyCommentsToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
			this.CopyCommentsToolStripMenuItem.Text = "Copy Comments to Clipboard";
			//
			//pnlMethods
			//
			this.pnlMethods.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlMethods.Controls.Add(this.lblBreakdown);
			this.pnlMethods.Controls.Add(this.lblMethods);
			this.pnlMethods.Location = new System.Drawing.Point(648, 138);
			this.pnlMethods.Name = "pnlMethods";
			this.pnlMethods.Size = new System.Drawing.Size(232, 170);
			this.pnlMethods.TabIndex = 5;
			this.pnlMethods.Visible = false;
			//
			//lblBreakdown
			//
			this.lblBreakdown.Location = new System.Drawing.Point(65, 23);
			this.lblBreakdown.Multiline = true;
			this.lblBreakdown.Name = "lblBreakdown";
			this.lblBreakdown.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.lblBreakdown.Size = new System.Drawing.Size(226, 164);
			this.lblBreakdown.TabIndex = 9;
			//
			//lblMethods
			//
			this.lblMethods.Location = new System.Drawing.Point(3, 3);
			this.lblMethods.Multiline = true;
			this.lblMethods.Name = "lblMethods";
			this.lblMethods.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.lblMethods.Size = new System.Drawing.Size(226, 164);
			this.lblMethods.TabIndex = 2;
			//
			//CopyUnsafeMethodsToolStripMenuItem
			//
			this.CopyUnsafeMethodsToolStripMenuItem.Name = "CopyUnsafeMethodsToolStripMenuItem";
			this.CopyUnsafeMethodsToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
			this.CopyUnsafeMethodsToolStripMenuItem.Text = "Copy Code Issues to Clipboard";
			//
			//ExportToolStripMenuItem
			//
			this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.CopyUnsafeMethodsToolStripMenuItem, this.CopyCommentsToolStripMenuItem});
			this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
			this.ExportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.ExportToolStripMenuItem.Text = "Export";
			//
			//lblResults
			//
			this.lblResults.AutoSize = true;
			this.lblResults.Location = new System.Drawing.Point(14, 13);
			this.lblResults.Name = "lblResults";
			this.lblResults.Size = new System.Drawing.Size(39, 13);
			this.lblResults.TabIndex = 0;
			this.lblResults.Text = "Label1";
			//
			//mnuMenuStrip
			//
			this.mnuMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.FileToolStripMenuItem, this.ExportToolStripMenuItem});
			this.mnuMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mnuMenuStrip.Name = "mnuMenuStrip";
			this.mnuMenuStrip.Size = new System.Drawing.Size(939, 24);
			this.mnuMenuStrip.TabIndex = 8;
			this.mnuMenuStrip.Text = "MenuStrip1";
			//
			//FileToolStripMenuItem
			//
			this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.OpenWithNotepadToolStripMenuItem, this.ToolStripMenuItem1, this.ExitToolStripMenuItem});
			this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
			this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.FileToolStripMenuItem.Text = "File";
			//
			//OpenWithNotepadToolStripMenuItem
			//
			this.OpenWithNotepadToolStripMenuItem.Name = "OpenWithNotepadToolStripMenuItem";
			this.OpenWithNotepadToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.OpenWithNotepadToolStripMenuItem.Text = "Open With Notepad...";
			//
			//ToolStripMenuItem1
			//
			this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
			this.ToolStripMenuItem1.Size = new System.Drawing.Size(186, 6);
			//
			//ExitToolStripMenuItem
			//
			this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
			this.ExitToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
			this.ExitToolStripMenuItem.Text = "Exit";
			//
			//pnlResults
			//
			this.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlResults.Controls.Add(this.lblResults);
			this.pnlResults.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlResults.Location = new System.Drawing.Point(0, 343);
			this.pnlResults.Name = "pnlResults";
			this.pnlResults.Size = new System.Drawing.Size(939, 127);
			this.pnlResults.TabIndex = 7;
			//
			//Label2
			//
			this.Label2.Location = new System.Drawing.Point(3, 3);
			this.Label2.Multiline = true;
			this.Label2.Name = "Label2";
			this.Label2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.Label2.Size = new System.Drawing.Size(226, 164);
			this.Label2.TabIndex = 2;
			//
			//frmIndividualBreakdown
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(939, 470);
			this.Controls.Add(this.chtResults);
			this.Controls.Add(this.pnlMethods);
			this.Controls.Add(this.mnuMenuStrip);
			this.Controls.Add(this.pnlResults);
			this.Name = "frmIndividualBreakdown";
			this.Text = "Individual Breakdown";
			((System.ComponentModel.ISupportInitialize) this.chtResults).EndInit();
			this.pnlMethods.ResumeLayout(false);
			this.pnlMethods.PerformLayout();
			this.mnuMenuStrip.ResumeLayout(false);
			this.mnuMenuStrip.PerformLayout();
			this.pnlResults.ResumeLayout(false);
			this.pnlResults.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.DataVisualization.Charting.Chart chtResults;
		internal System.Windows.Forms.ToolStripMenuItem CopyCommentsToolStripMenuItem;
		internal System.Windows.Forms.Panel pnlMethods;
		internal System.Windows.Forms.TextBox lblMethods;
		internal System.Windows.Forms.ToolStripMenuItem CopyUnsafeMethodsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
		internal System.Windows.Forms.Label lblResults;
		internal System.Windows.Forms.MenuStrip mnuMenuStrip;
		internal System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem OpenWithNotepadToolStripMenuItem;
		internal System.Windows.Forms.Panel pnlResults;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
		internal System.Windows.Forms.TextBox Label2;
		internal System.Windows.Forms.TextBox lblBreakdown;
	}
	
}
