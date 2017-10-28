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
	partial class frmMain : System.Windows.Forms.Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMain_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(frmMain_KeyDown);
			this.Load += new System.EventHandler(frmMain_Load);
			this.Shown += new System.EventHandler(frmMain_Shown);
			this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewTargetToolStripMenuItem.Click += new System.EventHandler(this.NewTargetToolStripMenuItem_Click);
			this.NewTargetFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewTargetFileToolStripMenuItem.Click += new System.EventHandler(this.NewTargetFileToolStripMenuItem_Click);
			this.SaveResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveResultsToolStripMenuItem.Click += new System.EventHandler(this.SaveResultsToolStripMenuItem_Click);
			this.ToolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem9.Click += new System.EventHandler(this.ToolStripMenuItem9_Click);
			this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem10.Click += new System.EventHandler(this.ToolStripMenuItem10_Click);
			this.ToolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem11.Click += new System.EventHandler(this.ToolStripMenuItem11_Click);
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem14.Click += new System.EventHandler(this.ToolStripMenuItem14_Click);
			this.ToolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripMenuItem15.Click += new System.EventHandler(this.ToolStripMenuItem15_Click);
			this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
			this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CutToolStripMenuItem.Click += new System.EventHandler(this.CutToolStripMenuItem_Click);
			this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
			this.PasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PasteToolStripMenuItem.Click += new System.EventHandler(this.PasteToolStripMenuItem_Click);
			this.ToolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.FindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FindToolStripMenuItem.Click += new System.EventHandler(this.FindToolStripMenuItem_Click);
			this.ToolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.SelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SelectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItem_Click);
			this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GroupRichTextResultsByIssueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GroupRichTextResultsByIssueToolStripMenuItem.Click += new System.EventHandler(this.GroupRichTextResultsByIssueToolStripMenuItem_Click);
			this.GroupRichTextResultsByFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GroupRichTextResultsByFileToolStripMenuItem.Click += new System.EventHandler(this.GroupRichTextResultsByFileToolStripMenuItem_Click);
			this.ShowIndividualRichTextResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ShowIndividualRichTextResultsToolStripMenuItem.Click += new System.EventHandler(this.ShowIndividualRichTextResultsToolStripMenuItem_Click);
			this.ToolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.StatusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StatusBarToolStripMenuItem.Click += new System.EventHandler(this.StatusBarToolStripMenuItem_Click);
			this.ScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StartScanningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StartScanningToolStripMenuItem.Click += new System.EventHandler(this.StartScanningToolStripMenuItem_Click);
			this.ToolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.VisualCodeBreakdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.VisualCodeBreakdownToolStripMenuItem.Click += new System.EventHandler(this.VisualCodeBreakdownToolStripMenuItem_Click);
			this.VisualSecurityBreakdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.VisualSecurityBreakdownToolStripMenuItem.Click += new System.EventHandler(this.VisualSecurityBreakdownToolStripMenuItem_Click);
			this.VisualBadFuncBreakdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.VisualBadFuncBreakdownToolStripMenuItem.Click += new System.EventHandler(this.VisualBadFuncBreakdownToolStripMenuItem_Click);
			this.VisualCommentBreakdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.VisualCommentBreakdownToolStripMenuItem.Click += new System.EventHandler(this.VisualCommentBreakdownToolStripMenuItem_Click);
			this.ToolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.ExportFixMeCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExportFixMeCommentsToolStripMenuItem.Click += new System.EventHandler(this.ExportFixMeCommentsToolStripMenuItem_Click);
			this.ToolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.SortRichTextResultsOnSeverityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SortRichTextResultsOnSeverityToolStripMenuItem.Click += new System.EventHandler(this.SortRichTextResultsOnSeverityToolStripMenuItem_Click);
			this.SortRichTextResultsOnFileNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SortRichTextResultsOnFileNameToolStripMenuItem.Click += new System.EventHandler(this.SortRichTextResultsOnFileNameToolStripMenuItem_Click);
			this.ToolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.FilterResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FilterResultsToolStripMenuItem.Click += new System.EventHandler(this.FilterResultsToolStripMenuItem_Click);
			this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.DeleteItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeleteItemToolStripMenuItem.Click += new System.EventHandler(this.DeleteItemToolStripMenuItem_Click);
			this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.BannedFunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.BannedFunctionsToolStripMenuItem.Click += new System.EventHandler(this.BannedFunctionsToolStripMenuItem_Click);
			this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.CCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CCToolStripMenuItem.Click += new System.EventHandler(this.CCToolStripMenuItem_Click);
			this.JavaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.JavaToolStripMenuItem.Click += new System.EventHandler(this.JavaToolStripMenuItem_Click);
			this.PLSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PLSQLToolStripMenuItem.Click += new System.EventHandler(this.PLSQLToolStripMenuItem_Click);
			this.CSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CSToolStripMenuItem.Click += new System.EventHandler(this.CSToolStripMenuItem_Click);
			this.VBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.VBToolStripMenuItem.Click += new System.EventHandler(this.VBToolStripMenuItem_Click);
			this.PHPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PHPToolStripMenuItem.Click += new System.EventHandler(this.PHPToolStripMenuItem_Click);
			this.COBOLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.COBOLToolStripMenuItem.Click += new System.EventHandler(this.COBOLToolStripMenuItem_Click);
			this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OptionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
			this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutVCGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutVCGToolStripMenuItem.Click += new System.EventHandler(this.AboutVCGToolStripMenuItem_Click);
			this.fbFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.ssStatusStrip = new System.Windows.Forms.StatusStrip();
			this.sslLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.spMain = new System.Windows.Forms.SplitContainer();
			this.cboTargetDir = new System.Windows.Forms.ComboBox();
			this.cboTargetDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboTargetDir_DragDrop);
			this.cboTargetDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboTargetDir_DragEnter);
			this.cboTargetDir.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboTargetDir_KeyDown);
			this.tcMain = new System.Windows.Forms.TabControl();
			this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
			this.tabTargetFiles = new System.Windows.Forms.TabPage();
			this.lbTargets = new System.Windows.Forms.ListBox();
			this.lbTargets.SelectedIndexChanged += new System.EventHandler(this.lbTargets_SelectedIndexChanged);
			this.tabResults = new System.Windows.Forms.TabPage();
			this.rtbResults = new System.Windows.Forms.RichTextBox();
			this.tabResultsTable = new System.Windows.Forms.TabPage();
			this.lvResults = new System.Windows.Forms.ListView();
			this.lvResults.Click += new System.EventHandler(this.lvResults_Click);
			this.lvResults.DoubleClick += new System.EventHandler(this.lvResults_DoubleClick);
			this.lvResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvResults_ColumnClick);
			this.lvResults.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvResults_ItemCheck);
			this.chSeverityRanking = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.chSeverity = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.chTitle = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.chDescription = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.chFileName = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.chLine = (System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader());
			this.sfdSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.ofdOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.cdColorDialog = new System.Windows.Forms.ColorDialog();
			this.btnSelectDir = new System.Windows.Forms.Button();
			this.btnSelectDir.Click += new System.EventHandler(this.btnSelectDir_Click);
			this.mnuMain.SuspendLayout();
			this.ssStatusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.spMain).BeginInit();
			this.spMain.Panel1.SuspendLayout();
			this.spMain.Panel2.SuspendLayout();
			this.spMain.SuspendLayout();
			this.tcMain.SuspendLayout();
			this.tabTargetFiles.SuspendLayout();
			this.tabResults.SuspendLayout();
			this.tabResultsTable.SuspendLayout();
			this.SuspendLayout();
			//
			//mnuMain
			//
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.FileToolStripMenuItem, this.EditToolStripMenuItem, this.ViewToolStripMenuItem, this.ScanToolStripMenuItem, this.ToolsToolStripMenuItem, this.HelpToolStripMenuItem});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Size = new System.Drawing.Size(824, 24);
			this.mnuMain.TabIndex = 0;
			this.mnuMain.Text = "MenuStrip1";
			//
			//FileToolStripMenuItem
			//
			this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.NewTargetToolStripMenuItem, this.NewTargetFileToolStripMenuItem, this.SaveResultsToolStripMenuItem, this.ToolStripMenuItem5, this.ToolStripMenuItem9, this.ToolStripMenuItem1, this.ToolStripMenuItem10, this.ToolStripMenuItem11, this.ToolStripSeparator1, this.ToolStripMenuItem14, this.ToolStripMenuItem15, this.ToolStripSeparator2, this.ExitToolStripMenuItem});
			this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
			this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.FileToolStripMenuItem.Text = "File";
			//
			//NewTargetToolStripMenuItem
			//
			this.NewTargetToolStripMenuItem.Name = "NewTargetToolStripMenuItem";
			this.NewTargetToolStripMenuItem.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N);
			this.NewTargetToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			this.NewTargetToolStripMenuItem.Text = "New Target Directory...";
			//
			//NewTargetFileToolStripMenuItem
			//
			this.NewTargetFileToolStripMenuItem.Name = "NewTargetFileToolStripMenuItem";
			this.NewTargetFileToolStripMenuItem.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T);
			this.NewTargetFileToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			this.NewTargetFileToolStripMenuItem.Text = "New Target File...";
			//
			//SaveResultsToolStripMenuItem
			//
			this.SaveResultsToolStripMenuItem.Name = "SaveResultsToolStripMenuItem";
			this.SaveResultsToolStripMenuItem.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S);
			this.SaveResultsToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			this.SaveResultsToolStripMenuItem.Text = "Save Results as Text...";
			//
			//ToolStripMenuItem5
			//
			this.ToolStripMenuItem5.Name = "ToolStripMenuItem5";
			this.ToolStripMenuItem5.Size = new System.Drawing.Size(234, 6);
			//
			//ToolStripMenuItem9
			//
			this.ToolStripMenuItem9.Name = "ToolStripMenuItem9";
			this.ToolStripMenuItem9.Size = new System.Drawing.Size(237, 22);
			this.ToolStripMenuItem9.Text = "Clear";
			//
			//ToolStripMenuItem1
			//
			this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
			this.ToolStripMenuItem1.Size = new System.Drawing.Size(234, 6);
			//
			//ToolStripMenuItem10
			//
			this.ToolStripMenuItem10.Name = "ToolStripMenuItem10";
			this.ToolStripMenuItem10.Size = new System.Drawing.Size(237, 22);
			this.ToolStripMenuItem10.Text = "Export Results as XML...";
			//
			//ToolStripMenuItem11
			//
			this.ToolStripMenuItem11.Name = "ToolStripMenuItem11";
			this.ToolStripMenuItem11.Size = new System.Drawing.Size(237, 22);
			this.ToolStripMenuItem11.Text = "Import Results from XML File...";
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(234, 6);
			//
			//ToolStripMenuItem14
			//
			this.ToolStripMenuItem14.Name = "ToolStripMenuItem14";
			this.ToolStripMenuItem14.Size = new System.Drawing.Size(237, 22);
			this.ToolStripMenuItem14.Text = "Export Results to CSV File..";
			//
			//ToolStripMenuItem15
			//
			this.ToolStripMenuItem15.Name = "ToolStripMenuItem15";
			this.ToolStripMenuItem15.Size = new System.Drawing.Size(237, 22);
			this.ToolStripMenuItem15.Text = "Import Results from CSV File...";
			//
			//ToolStripSeparator2
			//
			this.ToolStripSeparator2.Name = "ToolStripSeparator2";
			this.ToolStripSeparator2.Size = new System.Drawing.Size(234, 6);
			//
			//ExitToolStripMenuItem
			//
			this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
			this.ExitToolStripMenuItem.ShortcutKeys = (System.Windows.Forms.Keys) (System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4);
			this.ExitToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
			this.ExitToolStripMenuItem.Text = "Exit";
			//
			//EditToolStripMenuItem
			//
			this.EditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.CutToolStripMenuItem, this.CopyToolStripMenuItem, this.PasteToolStripMenuItem, this.ToolStripMenuItem8, this.FindToolStripMenuItem, this.ToolStripMenuItem6, this.SelectAllToolStripMenuItem});
			this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
			this.EditToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.EditToolStripMenuItem.Text = "Edit";
			//
			//CutToolStripMenuItem
			//
			this.CutToolStripMenuItem.Name = "CutToolStripMenuItem";
			this.CutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.CutToolStripMenuItem.Text = "Cut";
			//
			//CopyToolStripMenuItem
			//
			this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
			this.CopyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.CopyToolStripMenuItem.Text = "Copy";
			//
			//PasteToolStripMenuItem
			//
			this.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem";
			this.PasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.PasteToolStripMenuItem.Text = "Paste";
			//
			//ToolStripMenuItem8
			//
			this.ToolStripMenuItem8.Name = "ToolStripMenuItem8";
			this.ToolStripMenuItem8.Size = new System.Drawing.Size(149, 6);
			//
			//FindToolStripMenuItem
			//
			this.FindToolStripMenuItem.Name = "FindToolStripMenuItem";
			this.FindToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.FindToolStripMenuItem.Text = "Find";
			//
			//ToolStripMenuItem6
			//
			this.ToolStripMenuItem6.Name = "ToolStripMenuItem6";
			this.ToolStripMenuItem6.Size = new System.Drawing.Size(149, 6);
			//
			//SelectAllToolStripMenuItem
			//
			this.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem";
			this.SelectAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.SelectAllToolStripMenuItem.Text = "Select All";
			//
			//ViewToolStripMenuItem
			//
			this.ViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.GroupRichTextResultsByIssueToolStripMenuItem, this.GroupRichTextResultsByFileToolStripMenuItem, this.ShowIndividualRichTextResultsToolStripMenuItem, this.ToolStripMenuItem16, this.StatusBarToolStripMenuItem});
			this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
			this.ViewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.ViewToolStripMenuItem.Text = "View";
			//
			//GroupRichTextResultsByIssueToolStripMenuItem
			//
			this.GroupRichTextResultsByIssueToolStripMenuItem.CheckOnClick = true;
			this.GroupRichTextResultsByIssueToolStripMenuItem.Name = "GroupRichTextResultsByIssueToolStripMenuItem";
			this.GroupRichTextResultsByIssueToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
			this.GroupRichTextResultsByIssueToolStripMenuItem.Text = "Group Rich Text Results by Issue";
			//
			//GroupRichTextResultsByFileToolStripMenuItem
			//
			this.GroupRichTextResultsByFileToolStripMenuItem.CheckOnClick = true;
			this.GroupRichTextResultsByFileToolStripMenuItem.Name = "GroupRichTextResultsByFileToolStripMenuItem";
			this.GroupRichTextResultsByFileToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
			this.GroupRichTextResultsByFileToolStripMenuItem.Text = "Group Rich Text Results by File";
			//
			//ShowIndividualRichTextResultsToolStripMenuItem
			//
			this.ShowIndividualRichTextResultsToolStripMenuItem.Checked = true;
			this.ShowIndividualRichTextResultsToolStripMenuItem.CheckOnClick = true;
			this.ShowIndividualRichTextResultsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ShowIndividualRichTextResultsToolStripMenuItem.Name = "ShowIndividualRichTextResultsToolStripMenuItem";
			this.ShowIndividualRichTextResultsToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
			this.ShowIndividualRichTextResultsToolStripMenuItem.Text = "Show Individual Rich Text Results";
			//
			//ToolStripMenuItem16
			//
			this.ToolStripMenuItem16.Name = "ToolStripMenuItem16";
			this.ToolStripMenuItem16.Size = new System.Drawing.Size(245, 6);
			//
			//StatusBarToolStripMenuItem
			//
			this.StatusBarToolStripMenuItem.Checked = true;
			this.StatusBarToolStripMenuItem.CheckOnClick = true;
			this.StatusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem";
			this.StatusBarToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
			this.StatusBarToolStripMenuItem.Text = "Status Bar";
			//
			//ScanToolStripMenuItem
			//
			this.ScanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.StartScanningToolStripMenuItem, this.ToolStripMenuItem4, this.VisualCodeBreakdownToolStripMenuItem, this.VisualSecurityBreakdownToolStripMenuItem, this.VisualBadFuncBreakdownToolStripMenuItem, this.VisualCommentBreakdownToolStripMenuItem, this.ToolStripMenuItem7, this.ExportFixMeCommentsToolStripMenuItem, this.ToolStripMenuItem12, this.SortRichTextResultsOnSeverityToolStripMenuItem, this.SortRichTextResultsOnFileNameToolStripMenuItem, this.ToolStripMenuItem13, this.FilterResultsToolStripMenuItem, this.ToolStripSeparator3, this.DeleteItemToolStripMenuItem});
			this.ScanToolStripMenuItem.Name = "ScanToolStripMenuItem";
			this.ScanToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.ScanToolStripMenuItem.Text = "Scan";
			//
			//StartScanningToolStripMenuItem
			//
			this.StartScanningToolStripMenuItem.Enabled = false;
			this.StartScanningToolStripMenuItem.Name = "StartScanningToolStripMenuItem";
			this.StartScanningToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.StartScanningToolStripMenuItem.Text = "Full Scan";
			//
			//ToolStripMenuItem4
			//
			this.ToolStripMenuItem4.Name = "ToolStripMenuItem4";
			this.ToolStripMenuItem4.Size = new System.Drawing.Size(363, 6);
			//
			//VisualCodeBreakdownToolStripMenuItem
			//
			this.VisualCodeBreakdownToolStripMenuItem.Enabled = false;
			this.VisualCodeBreakdownToolStripMenuItem.Name = "VisualCodeBreakdownToolStripMenuItem";
			this.VisualCodeBreakdownToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.VisualCodeBreakdownToolStripMenuItem.Text = "Visual Code/Comment Breakdown";
			//
			//VisualSecurityBreakdownToolStripMenuItem
			//
			this.VisualSecurityBreakdownToolStripMenuItem.Enabled = false;
			this.VisualSecurityBreakdownToolStripMenuItem.Name = "VisualSecurityBreakdownToolStripMenuItem";
			this.VisualSecurityBreakdownToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.VisualSecurityBreakdownToolStripMenuItem.Text = "Scan Code Only (Ignore Comments)";
			//
			//VisualBadFuncBreakdownToolStripMenuItem
			//
			this.VisualBadFuncBreakdownToolStripMenuItem.Enabled = false;
			this.VisualBadFuncBreakdownToolStripMenuItem.Name = "VisualBadFuncBreakdownToolStripMenuItem";
			this.VisualBadFuncBreakdownToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.VisualBadFuncBreakdownToolStripMenuItem.Text = "Scan For Bad Functions Only (As Defined in Config File)";
			//
			//VisualCommentBreakdownToolStripMenuItem
			//
			this.VisualCommentBreakdownToolStripMenuItem.Enabled = false;
			this.VisualCommentBreakdownToolStripMenuItem.Name = "VisualCommentBreakdownToolStripMenuItem";
			this.VisualCommentBreakdownToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.VisualCommentBreakdownToolStripMenuItem.Text = "Scan Comments Only";
			//
			//ToolStripMenuItem7
			//
			this.ToolStripMenuItem7.Name = "ToolStripMenuItem7";
			this.ToolStripMenuItem7.Size = new System.Drawing.Size(363, 6);
			//
			//ExportFixMeCommentsToolStripMenuItem
			//
			this.ExportFixMeCommentsToolStripMenuItem.Enabled = false;
			this.ExportFixMeCommentsToolStripMenuItem.Name = "ExportFixMeCommentsToolStripMenuItem";
			this.ExportFixMeCommentsToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.ExportFixMeCommentsToolStripMenuItem.Text = "Show All 'FixMe' Comments";
			//
			//ToolStripMenuItem12
			//
			this.ToolStripMenuItem12.Name = "ToolStripMenuItem12";
			this.ToolStripMenuItem12.Size = new System.Drawing.Size(363, 6);
			//
			//SortRichTextResultsOnSeverityToolStripMenuItem
			//
			this.SortRichTextResultsOnSeverityToolStripMenuItem.Name = "SortRichTextResultsOnSeverityToolStripMenuItem";
			this.SortRichTextResultsOnSeverityToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.SortRichTextResultsOnSeverityToolStripMenuItem.Text = "Sort Rich Text Results on Severity";
			//
			//SortRichTextResultsOnFileNameToolStripMenuItem
			//
			this.SortRichTextResultsOnFileNameToolStripMenuItem.Name = "SortRichTextResultsOnFileNameToolStripMenuItem";
			this.SortRichTextResultsOnFileNameToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.SortRichTextResultsOnFileNameToolStripMenuItem.Text = "Sort Rich Text Results on FileName";
			//
			//ToolStripMenuItem13
			//
			this.ToolStripMenuItem13.Name = "ToolStripMenuItem13";
			this.ToolStripMenuItem13.Size = new System.Drawing.Size(363, 6);
			//
			//FilterResultsToolStripMenuItem
			//
			this.FilterResultsToolStripMenuItem.Name = "FilterResultsToolStripMenuItem";
			this.FilterResultsToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.FilterResultsToolStripMenuItem.Text = "Filter Results...";
			//
			//ToolStripSeparator3
			//
			this.ToolStripSeparator3.Name = "ToolStripSeparator3";
			this.ToolStripSeparator3.Size = new System.Drawing.Size(363, 6);
			//
			//DeleteItemToolStripMenuItem
			//
			this.DeleteItemToolStripMenuItem.Name = "DeleteItemToolStripMenuItem";
			this.DeleteItemToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.DeleteItemToolStripMenuItem.Size = new System.Drawing.Size(366, 22);
			this.DeleteItemToolStripMenuItem.Text = "Delete Selected Item(s)";
			//
			//ToolsToolStripMenuItem
			//
			this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.BannedFunctionsToolStripMenuItem, this.ToolStripMenuItem2, this.CCToolStripMenuItem, this.JavaToolStripMenuItem, this.PLSQLToolStripMenuItem, this.CSToolStripMenuItem, this.VBToolStripMenuItem, this.PHPToolStripMenuItem, this.COBOLToolStripMenuItem, this.ToolStripMenuItem3, this.OptionsToolStripMenuItem});
			this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
			this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.ToolsToolStripMenuItem.Text = "Settings";
			//
			//BannedFunctionsToolStripMenuItem
			//
			this.BannedFunctionsToolStripMenuItem.Name = "BannedFunctionsToolStripMenuItem";
			this.BannedFunctionsToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.BannedFunctionsToolStripMenuItem.Text = "Banned/Insecure Functions...";
			//
			//ToolStripMenuItem2
			//
			this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
			this.ToolStripMenuItem2.Size = new System.Drawing.Size(224, 6);
			//
			//CCToolStripMenuItem
			//
			this.CCToolStripMenuItem.Checked = true;
			this.CCToolStripMenuItem.CheckOnClick = true;
			this.CCToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CCToolStripMenuItem.Name = "CCToolStripMenuItem";
			this.CCToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.CCToolStripMenuItem.Text = "C/C++";
			//
			//JavaToolStripMenuItem
			//
			this.JavaToolStripMenuItem.CheckOnClick = true;
			this.JavaToolStripMenuItem.Name = "JavaToolStripMenuItem";
			this.JavaToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.JavaToolStripMenuItem.Text = "Java";
			//
			//PLSQLToolStripMenuItem
			//
			this.PLSQLToolStripMenuItem.CheckOnClick = true;
			this.PLSQLToolStripMenuItem.Name = "PLSQLToolStripMenuItem";
			this.PLSQLToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.PLSQLToolStripMenuItem.Text = "PL/SQL";
			//
			//CSToolStripMenuItem
			//
			this.CSToolStripMenuItem.CheckOnClick = true;
			this.CSToolStripMenuItem.Name = "CSToolStripMenuItem";
			this.CSToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.CSToolStripMenuItem.Text = "C#";
			//
			//VBToolStripMenuItem
			//
			this.VBToolStripMenuItem.CheckOnClick = true;
			this.VBToolStripMenuItem.Name = "VBToolStripMenuItem";
			this.VBToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.VBToolStripMenuItem.Text = "VB";
			//
			//PHPToolStripMenuItem
			//
			this.PHPToolStripMenuItem.CheckOnClick = true;
			this.PHPToolStripMenuItem.Name = "PHPToolStripMenuItem";
			this.PHPToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.PHPToolStripMenuItem.Text = "PHP";
			//
			//COBOLToolStripMenuItem
			//
			this.COBOLToolStripMenuItem.CheckOnClick = true;
			this.COBOLToolStripMenuItem.Name = "COBOLToolStripMenuItem";
			this.COBOLToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.COBOLToolStripMenuItem.Text = "COBOL";
			this.COBOLToolStripMenuItem.Visible = false;
			//
			//ToolStripMenuItem3
			//
			this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
			this.ToolStripMenuItem3.Size = new System.Drawing.Size(224, 6);
			//
			//OptionsToolStripMenuItem
			//
			this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
			this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(227, 22);
			this.OptionsToolStripMenuItem.Text = "Options...";
			//
			//HelpToolStripMenuItem
			//
			this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {this.AboutVCGToolStripMenuItem});
			this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
			this.HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.HelpToolStripMenuItem.Text = "Help";
			//
			//AboutVCGToolStripMenuItem
			//
			this.AboutVCGToolStripMenuItem.Name = "AboutVCGToolStripMenuItem";
			this.AboutVCGToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.AboutVCGToolStripMenuItem.Text = "About VCG";
			//
			//ssStatusStrip
			//
			this.ssStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.sslLabel});
			this.ssStatusStrip.Location = new System.Drawing.Point(0, 452);
			this.ssStatusStrip.Name = "ssStatusStrip";
			this.ssStatusStrip.Size = new System.Drawing.Size(824, 22);
			this.ssStatusStrip.TabIndex = 1;
			//
			//sslLabel
			//
			this.sslLabel.Name = "sslLabel";
			this.sslLabel.Size = new System.Drawing.Size(102, 17);
			this.sslLabel.Text = "Language: C/C++";
			//
			//spMain
			//
			this.spMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spMain.IsSplitterFixed = true;
			this.spMain.Location = new System.Drawing.Point(0, 24);
			this.spMain.Name = "spMain";
			this.spMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			//
			//spMain.Panel1
			//
			this.spMain.Panel1.Controls.Add(this.btnSelectDir);
			this.spMain.Panel1.Controls.Add(this.cboTargetDir);
			//
			//spMain.Panel2
			//
			this.spMain.Panel2.Controls.Add(this.tcMain);
			this.spMain.Size = new System.Drawing.Size(824, 428);
			this.spMain.SplitterDistance = 25;
			this.spMain.TabIndex = 2;
			//
			//cboTargetDir
			//
			this.cboTargetDir.AllowDrop = true;
			this.cboTargetDir.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.cboTargetDir.FormattingEnabled = true;
			this.cboTargetDir.Location = new System.Drawing.Point(3, 0);
			this.cboTargetDir.Name = "cboTargetDir";
			this.cboTargetDir.Size = new System.Drawing.Size(724, 21);
			this.cboTargetDir.TabIndex = 0;
			//
			//tcMain
			//
			this.tcMain.Controls.Add(this.tabTargetFiles);
			this.tcMain.Controls.Add(this.tabResults);
			this.tcMain.Controls.Add(this.tabResultsTable);
			this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMain.Location = new System.Drawing.Point(0, 0);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(824, 399);
			this.tcMain.TabIndex = 1;
			//
			//tabTargetFiles
			//
			this.tabTargetFiles.Controls.Add(this.lbTargets);
			this.tabTargetFiles.Location = new System.Drawing.Point(4, 22);
			this.tabTargetFiles.Name = "tabTargetFiles";
			this.tabTargetFiles.Padding = new System.Windows.Forms.Padding(3);
			this.tabTargetFiles.Size = new System.Drawing.Size(816, 373);
			this.tabTargetFiles.TabIndex = 0;
			this.tabTargetFiles.Text = "Target Files";
			this.tabTargetFiles.UseVisualStyleBackColor = true;
			//
			//lbTargets
			//
			this.lbTargets.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTargets.FormattingEnabled = true;
			this.lbTargets.Location = new System.Drawing.Point(3, 3);
			this.lbTargets.Name = "lbTargets";
			this.lbTargets.Size = new System.Drawing.Size(810, 367);
			this.lbTargets.TabIndex = 1;
			//
			//tabResults
			//
			this.tabResults.Controls.Add(this.rtbResults);
			this.tabResults.Location = new System.Drawing.Point(4, 22);
			this.tabResults.Name = "tabResults";
			this.tabResults.Padding = new System.Windows.Forms.Padding(3);
			this.tabResults.Size = new System.Drawing.Size(816, 373);
			this.tabResults.TabIndex = 1;
			this.tabResults.Text = "Results";
			this.tabResults.UseVisualStyleBackColor = true;
			//
			//rtbResults
			//
			this.rtbResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbResults.Location = new System.Drawing.Point(3, 3);
			this.rtbResults.Name = "rtbResults";
			this.rtbResults.Size = new System.Drawing.Size(810, 367);
			this.rtbResults.TabIndex = 0;
			this.rtbResults.Text = "";
			//
			//tabResultsTable
			//
			this.tabResultsTable.Controls.Add(this.lvResults);
			this.tabResultsTable.Location = new System.Drawing.Point(4, 22);
			this.tabResultsTable.Name = "tabResultsTable";
			this.tabResultsTable.Padding = new System.Windows.Forms.Padding(3);
			this.tabResultsTable.Size = new System.Drawing.Size(816, 373);
			this.tabResultsTable.TabIndex = 2;
			this.tabResultsTable.Text = "Summary Table";
			this.tabResultsTable.UseVisualStyleBackColor = true;
			//
			//lvResults
			//
			this.lvResults.AllowColumnReorder = true;
			this.lvResults.CheckBoxes = true;
			this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.chSeverityRanking, this.chSeverity, this.chTitle, this.chDescription, this.chFileName, this.chLine});
			this.lvResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvResults.FullRowSelect = true;
			this.lvResults.HideSelection = false;
			this.lvResults.Location = new System.Drawing.Point(3, 3);
			this.lvResults.Name = "lvResults";
			this.lvResults.Size = new System.Drawing.Size(810, 367);
			this.lvResults.TabIndex = 0;
			this.lvResults.UseCompatibleStateImageBehavior = false;
			this.lvResults.View = System.Windows.Forms.View.Details;
			//
			//chSeverityRanking
			//
			this.chSeverityRanking.Text = "Priority";
			this.chSeverityRanking.Width = 43;
			//
			//chSeverity
			//
			this.chSeverity.Text = "Severity";
			this.chSeverity.Width = 75;
			//
			//chTitle
			//
			this.chTitle.Text = "Title";
			this.chTitle.Width = 229;
			//
			//chDescription
			//
			this.chDescription.Text = "Description";
			this.chDescription.Width = 399;
			//
			//chFileName
			//
			this.chFileName.Text = "FileName";
			this.chFileName.Width = 378;
			//
			//chLine
			//
			this.chLine.Text = "Line";
			//
			//ofdOpenFileDialog
			//
			this.ofdOpenFileDialog.FileName = "XmlResults";
			//
			//btnSelectDir
			//
			this.btnSelectDir.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnSelectDir.Location = new System.Drawing.Point(733, 0);
			this.btnSelectDir.Name = "btnSelectDir";
			this.btnSelectDir.Size = new System.Drawing.Size(87, 23);
			this.btnSelectDir.TabIndex = 1;
			this.btnSelectDir.Text = "Select Dir...";
			this.btnSelectDir.UseVisualStyleBackColor = true;
			//
			//frmMain
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(824, 474);
			this.Controls.Add(this.spMain);
			this.Controls.Add(this.ssStatusStrip);
			this.Controls.Add(this.mnuMain);
			this.Icon = (System.Drawing.Icon) (resources.GetObject("$this.Icon"));
			this.KeyPreview = true;
			this.MainMenuStrip = this.mnuMain;
			this.Name = "frmMain";
			this.Text = "VCG";
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.ssStatusStrip.ResumeLayout(false);
			this.ssStatusStrip.PerformLayout();
			this.spMain.Panel1.ResumeLayout(false);
			this.spMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize) this.spMain).EndInit();
			this.spMain.ResumeLayout(false);
			this.tcMain.ResumeLayout(false);
			this.tabTargetFiles.ResumeLayout(false);
			this.tabResults.ResumeLayout(false);
			this.tabResultsTable.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
			
		}
		internal System.Windows.Forms.MenuStrip mnuMain;
		internal System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem NewTargetToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem SaveResultsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem CutToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem CopyToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem PasteToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem BannedFunctionsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem2;
		internal System.Windows.Forms.ToolStripMenuItem CCToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem JavaToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem PLSQLToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem3;
		internal System.Windows.Forms.ToolStripMenuItem OptionsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem AboutVCGToolStripMenuItem;
		internal System.Windows.Forms.FolderBrowserDialog fbFolderBrowser;
		internal System.Windows.Forms.StatusStrip ssStatusStrip;
		internal System.Windows.Forms.SplitContainer spMain;
		internal System.Windows.Forms.ToolStripMenuItem ScanToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem StartScanningToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem4;
		internal System.Windows.Forms.ToolStripMenuItem VisualCodeBreakdownToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem VisualSecurityBreakdownToolStripMenuItem;
		internal System.Windows.Forms.ComboBox cboTargetDir;
		internal System.Windows.Forms.TabControl tcMain;
		internal System.Windows.Forms.TabPage tabTargetFiles;
		internal System.Windows.Forms.ListBox lbTargets;
		internal System.Windows.Forms.TabPage tabResults;
		internal System.Windows.Forms.ToolStripMenuItem FindToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem6;
		internal System.Windows.Forms.ToolStripMenuItem SelectAllToolStripMenuItem;
		internal System.Windows.Forms.RichTextBox rtbResults;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem7;
		internal System.Windows.Forms.ToolStripMenuItem ExportFixMeCommentsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripStatusLabel sslLabel;
		internal System.Windows.Forms.SaveFileDialog sfdSaveFileDialog;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem8;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem5;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem9;
		internal System.Windows.Forms.TabPage tabResultsTable;
		internal System.Windows.Forms.ListView lvResults;
		internal System.Windows.Forms.ColumnHeader chSeverity;
		internal System.Windows.Forms.ColumnHeader chTitle;
		internal System.Windows.Forms.ColumnHeader chDescription;
		internal System.Windows.Forms.ColumnHeader chFileName;
		internal System.Windows.Forms.ColumnHeader chSeverityRanking;
		internal System.Windows.Forms.OpenFileDialog ofdOpenFileDialog;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem10;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem11;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
		internal System.Windows.Forms.ColumnHeader chLine;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem12;
		internal System.Windows.Forms.ToolStripMenuItem SortRichTextResultsOnSeverityToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem SortRichTextResultsOnFileNameToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem CSToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem13;
		internal System.Windows.Forms.ToolStripMenuItem FilterResultsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem VBToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem PHPToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem COBOLToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem VisualCommentBreakdownToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem NewTargetFileToolStripMenuItem;
		internal System.Windows.Forms.ColorDialog cdColorDialog;
		internal System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem GroupRichTextResultsByIssueToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem GroupRichTextResultsByFileToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem14;
		internal System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem15;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
		internal System.Windows.Forms.ToolStripMenuItem ShowIndividualRichTextResultsToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
		internal System.Windows.Forms.ToolStripMenuItem DeleteItemToolStripMenuItem;
		internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem16;
		internal System.Windows.Forms.ToolStripMenuItem StatusBarToolStripMenuItem;
		internal System.Windows.Forms.ToolStripMenuItem VisualBadFuncBreakdownToolStripMenuItem;
		internal Button btnSelectDir;
	}
	
}
