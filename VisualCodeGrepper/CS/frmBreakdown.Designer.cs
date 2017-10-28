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
	partial class frmBreakdown : System.Windows.Forms.Form
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
			this.OpenInNotepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenInNotepadToolStripMenuItem.Click += new System.EventHandler(this.OpenInNotepadToolStripMenuItem_Click);
			this.dgvResults = new System.Windows.Forms.DataGridView();
			this.dgvResults.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellContentClick);
			this.clmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TotalLines = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PercentageTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LinesofCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CommentLines = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Whitespace = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmFixme = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CodeIssues = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click_1);
			this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExportToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExportToClipboardToolStripMenuItem.Click += new System.EventHandler(this.ExportToClipboardToolStripMenuItem_Click);
			this.mnuBreakdown = new System.Windows.Forms.MenuStrip();
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.lblDoubleClick = new System.Windows.Forms.Label();
			this.lblResults = new System.Windows.Forms.Label();
			this.pnlResults = new System.Windows.Forms.Panel();
			this.btnApplyFilter = new System.Windows.Forms.Button();
			this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click);
			this.chtResults = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize) this.dgvResults).BeginInit();
			this.mnuBreakdown.SuspendLayout();
			this.pnlResults.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.chtResults).BeginInit();
			this.SuspendLayout();
			//
			//OpenInNotepadToolStripMenuItem
			//
			this.OpenInNotepadToolStripMenuItem.Name = "OpenInNotepadToolStripMenuItem";
			this.OpenInNotepadToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
			this.OpenInNotepadToolStripMenuItem.Text = "Open Selected File in Notepad";
			//
			//dgvResults
			//
			this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {this.clmName, this.TotalLines, this.PercentageTotal, this.LinesofCode, this.CommentLines, this.Whitespace, this.clmFixme, this.CodeIssues, this.FullPath});
			this.dgvResults.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dgvResults.Location = new System.Drawing.Point(0, 476);
			this.dgvResults.Name = "dgvResults";
			this.dgvResults.Size = new System.Drawing.Size(1225, 445);
			this.dgvResults.TabIndex = 7;
			//
			//clmName
			//
			this.clmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.clmName.HeaderText = "Name";
			this.clmName.Name = "clmName";
			this.clmName.ReadOnly = true;
			this.clmName.Width = 60;
			//
			//TotalLines
			//
			this.TotalLines.HeaderText = "Total Lines";
			this.TotalLines.Name = "TotalLines";
			//
			//PercentageTotal
			//
			this.PercentageTotal.HeaderText = "Percentage of Total";
			this.PercentageTotal.Name = "PercentageTotal";
			this.PercentageTotal.ReadOnly = true;
			//
			//LinesofCode
			//
			this.LinesofCode.HeaderText = "Lines Of Code";
			this.LinesofCode.Name = "LinesofCode";
			//
			//CommentLines
			//
			this.CommentLines.HeaderText = "Commented Lines";
			this.CommentLines.Name = "CommentLines";
			this.CommentLines.ReadOnly = true;
			//
			//Whitespace
			//
			this.Whitespace.HeaderText = "Whitespace";
			this.Whitespace.Name = "Whitespace";
			//
			//clmFixme
			//
			this.clmFixme.HeaderText = "Potentially Unsafe Flags";
			this.clmFixme.Name = "clmFixme";
			this.clmFixme.ToolTipText = "occurances of todo, fixme, etc.";
			//
			//CodeIssues
			//
			this.CodeIssues.HeaderText = "Potentially Unsafe Code";
			this.CodeIssues.Name = "CodeIssues";
			//
			//FullPath
			//
			this.FullPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.FullPath.HeaderText = "Full Path";
			this.FullPath.Name = "FullPath";
			this.FullPath.ReadOnly = true;
			this.FullPath.Width = 68;
			//
			//FileToolStripMenuItem
			//
			this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.OpenInNotepadToolStripMenuItem, this.ToolStripMenuItem1, this.ExitToolStripMenuItem});
			this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
			this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.FileToolStripMenuItem.Text = "&File";
			//
			//ToolStripMenuItem1
			//
			this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
			this.ToolStripMenuItem1.Size = new System.Drawing.Size(230, 6);
			//
			//ExitToolStripMenuItem
			//
			this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
			this.ExitToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
			this.ExitToolStripMenuItem.Text = "Exit";
			//
			//ExportToolStripMenuItem
			//
			this.ExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.ExportToClipboardToolStripMenuItem});
			this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
			this.ExportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.ExportToolStripMenuItem.Text = "Export";
			//
			//ExportToClipboardToolStripMenuItem
			//
			this.ExportToClipboardToolStripMenuItem.Name = "ExportToClipboardToolStripMenuItem";
			this.ExportToClipboardToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
			this.ExportToClipboardToolStripMenuItem.Text = "Copy All Results to Clipboard";
			//
			//mnuBreakdown
			//
			this.mnuBreakdown.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.FileToolStripMenuItem, this.ExportToolStripMenuItem});
			this.mnuBreakdown.Location = new System.Drawing.Point(0, 0);
			this.mnuBreakdown.Name = "mnuBreakdown";
			this.mnuBreakdown.Size = new System.Drawing.Size(1225, 24);
			this.mnuBreakdown.TabIndex = 11;
			this.mnuBreakdown.Text = "MenuStrip1";
			//
			//txtFilter
			//
			this.txtFilter.Location = new System.Drawing.Point(607, 401);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(304, 20);
			this.txtFilter.TabIndex = 12;
			this.txtFilter.Text = "Filter....";
			//
			//lblDoubleClick
			//
			this.lblDoubleClick.AutoSize = true;
			this.lblDoubleClick.Location = new System.Drawing.Point(602, 372);
			this.lblDoubleClick.Name = "lblDoubleClick";
			this.lblDoubleClick.Size = new System.Drawing.Size(334, 13);
			this.lblDoubleClick.TabIndex = 10;
			this.lblDoubleClick.Text = "Double click on an item below to view an individual code breakdown.";
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
			//pnlResults
			//
			this.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlResults.Controls.Add(this.lblResults);
			this.pnlResults.Location = new System.Drawing.Point(12, 372);
			this.pnlResults.Name = "pnlResults";
			this.pnlResults.Size = new System.Drawing.Size(584, 80);
			this.pnlResults.TabIndex = 9;
			//
			//btnApplyFilter
			//
			this.btnApplyFilter.Location = new System.Drawing.Point(915, 400);
			this.btnApplyFilter.Name = "btnApplyFilter";
			this.btnApplyFilter.Size = new System.Drawing.Size(82, 20);
			this.btnApplyFilter.TabIndex = 13;
			this.btnApplyFilter.Text = "Apply Filter";
			this.btnApplyFilter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnApplyFilter.UseVisualStyleBackColor = true;
			//
			//chtResults
			//
			this.chtResults.BackColor = System.Drawing.Color.Transparent;
			this.chtResults.BackImageTransparentColor = System.Drawing.Color.White;
			this.chtResults.BorderlineColor = System.Drawing.Color.Black;
			this.chtResults.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
			ChartArea1.Name = "ChartArea1";
			this.chtResults.ChartAreas.Add(ChartArea1);
			this.chtResults.Dock = System.Windows.Forms.DockStyle.Top;
			Legend1.Name = "Legend1";
			this.chtResults.Legends.Add(Legend1);
			this.chtResults.Location = new System.Drawing.Point(0, 24);
			this.chtResults.Name = "chtResults";
			this.chtResults.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
			Series1.ChartArea = "ChartArea1";
			Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
			Series1.IsValueShownAsLabel = true;
			Series1.Legend = "Legend1";
			Series1.Name = "Series1";
			this.chtResults.Series.Add(Series1);
			this.chtResults.Size = new System.Drawing.Size(1225, 345);
			this.chtResults.TabIndex = 14;
			this.chtResults.Text = "Chart1";
			//
			//frmBreakdown
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1225, 921);
			this.Controls.Add(this.chtResults);
			this.Controls.Add(this.dgvResults);
			this.Controls.Add(this.mnuBreakdown);
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.lblDoubleClick);
			this.Controls.Add(this.pnlResults);
			this.Controls.Add(this.btnApplyFilter);
			this.Name = "frmBreakdown";
			this.Text = "Code Breakdown";
			((System.ComponentModel.ISupportInitialize) this.dgvResults).EndInit();
			this.mnuBreakdown.ResumeLayout(false);
			this.mnuBreakdown.PerformLayout();
			this.pnlResults.ResumeLayout(false);
			this.pnlResults.PerformLayout();
			((System.ComponentModel.ISupportInitialize) this.chtResults).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.ToolStripMenuItem OpenInNotepadToolStripMenuItem;
		internal System.Windows.Forms.DataGridView dgvResults;
		internal System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ExportToClipboardToolStripMenuItem;
		internal System.Windows.Forms.MenuStrip mnuBreakdown;
		internal System.Windows.Forms.TextBox txtFilter;
		internal System.Windows.Forms.Label lblDoubleClick;
		internal System.Windows.Forms.Label lblResults;
		internal System.Windows.Forms.Panel pnlResults;
		internal System.Windows.Forms.Button btnApplyFilter;
		internal System.Windows.Forms.DataVisualization.Charting.Chart chtResults;
		internal System.Windows.Forms.DataGridViewTextBoxColumn clmName;
		internal System.Windows.Forms.DataGridViewTextBoxColumn TotalLines;
		internal System.Windows.Forms.DataGridViewTextBoxColumn PercentageTotal;
		internal System.Windows.Forms.DataGridViewTextBoxColumn LinesofCode;
		internal System.Windows.Forms.DataGridViewTextBoxColumn CommentLines;
		internal System.Windows.Forms.DataGridViewTextBoxColumn Whitespace;
		internal System.Windows.Forms.DataGridViewTextBoxColumn clmFixme;
		internal System.Windows.Forms.DataGridViewTextBoxColumn CodeIssues;
		internal System.Windows.Forms.DataGridViewTextBoxColumn FullPath;
	}
	
}
