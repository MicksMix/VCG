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
	partial class frmOptions : System.Windows.Forms.Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
			this.btnCancel = new System.Windows.Forms.Button();
			base.Load += new System.EventHandler(frmOptions_Load);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnOK = new System.Windows.Forms.Button();
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.ofdOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.sfdSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.tabOptions = new System.Windows.Forms.TabControl();
			this.tpGeneral = new System.Windows.Forms.TabPage();
			this.gbReporting = new System.Windows.Forms.GroupBox();
			this.cboReporting = new System.Windows.Forms.ComboBox();
			this.lblReporting = new System.Windows.Forms.Label();
			this.gbOutput = new System.Windows.Forms.GroupBox();
			this.cbOutput = new System.Windows.Forms.CheckBox();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.btnOutputBrowse = new System.Windows.Forms.Button();
			this.gbFileTypes = new System.Windows.Forms.GroupBox();
			this.lblExplain = new System.Windows.Forms.Label();
			this.txtFileTypes = new System.Windows.Forms.TextBox();
			this.cboFileTypes = new System.Windows.Forms.ComboBox();
			this.cboFileTypes.SelectedIndexChanged += new System.EventHandler(this.cboFileTypes_SelectedIndexChanged);
			this.gbLanguage = new System.Windows.Forms.GroupBox();
			this.cboStartUpLanguage = new System.Windows.Forms.ComboBox();
			this.cboCurrentLanguage = new System.Windows.Forms.ComboBox();
			this.lblStartUpLanguage = new System.Windows.Forms.Label();
			this.lblCurrentLanguage = new System.Windows.Forms.Label();
			this.tpConfigFiles = new System.Windows.Forms.TabPage();
			this.gbConfigFiles = new System.Windows.Forms.GroupBox();
			this.btnCobolEdit = new System.Windows.Forms.Button();
			this.btnCobolEdit.Click += new System.EventHandler(this.btnCobolEdit_Click);
			this.btnCobolBrowse = new System.Windows.Forms.Button();
			this.btnCobolBrowse.Click += new System.EventHandler(this.btnCobolBrowse_Click);
			this.txtCobol = new System.Windows.Forms.TextBox();
			this.lblCobol = new System.Windows.Forms.Label();
			this.btnPHPEdit = new System.Windows.Forms.Button();
			this.btnPHPEdit.Click += new System.EventHandler(this.btnPHPEdit_Click);
			this.btnPHPBrowse = new System.Windows.Forms.Button();
			this.btnPHPBrowse.Click += new System.EventHandler(this.btnPHPBrowse_Click);
			this.txtPHP = new System.Windows.Forms.TextBox();
			this.lblPHP = new System.Windows.Forms.Label();
			this.btnVBEdit = new System.Windows.Forms.Button();
			this.btnVBEdit.Click += new System.EventHandler(this.btnVBEdit_Click);
			this.btnVBBrowse = new System.Windows.Forms.Button();
			this.btnVBBrowse.Click += new System.EventHandler(this.btnVBBrowse_Click);
			this.txtVB = new System.Windows.Forms.TextBox();
			this.lblVB = new System.Windows.Forms.Label();
			this.btnCSharpEdit = new System.Windows.Forms.Button();
			this.btnCSharpEdit.Click += new System.EventHandler(this.btnCSharpEdit_Click);
			this.btnCSharpBrowse = new System.Windows.Forms.Button();
			this.btnCSharpBrowse.Click += new System.EventHandler(this.btnCSharpBrowse_Click);
			this.txtCSharp = new System.Windows.Forms.TextBox();
			this.lblCSharp = new System.Windows.Forms.Label();
			this.btnCPPBrowse = new System.Windows.Forms.Button();
			this.btnCPPBrowse.Click += new System.EventHandler(this.btnCPPBrowse_Click);
			this.txtCPP = new System.Windows.Forms.TextBox();
			this.btnSQLEdit = new System.Windows.Forms.Button();
			this.btnSQLEdit.Click += new System.EventHandler(this.btnSQLEdit_Click);
			this.btnSQLBrowse = new System.Windows.Forms.Button();
			this.btnSQLBrowse.Click += new System.EventHandler(this.btnSQLBrowse_Click);
			this.txtPLSQL = new System.Windows.Forms.TextBox();
			this.btnJavaEdit = new System.Windows.Forms.Button();
			this.btnJavaEdit.Click += new System.EventHandler(this.btnJavaEdit_Click);
			this.btnJavaBrowse = new System.Windows.Forms.Button();
			this.btnJavaBrowse.Click += new System.EventHandler(this.btnJavaBrowse_Click);
			this.txtJava = new System.Windows.Forms.TextBox();
			this.btnCPPEdit = new System.Windows.Forms.Button();
			this.btnCPPEdit.Click += new System.EventHandler(this.btnCPPEdit_Click);
			this.lblSQL = new System.Windows.Forms.Label();
			this.lblJava = new System.Windows.Forms.Label();
			this.lblCPP = new System.Windows.Forms.Label();
			this.tpJava = new System.Windows.Forms.TabPage();
			this.gbAndroid = new System.Windows.Forms.GroupBox();
			this.cbAndroid = new System.Windows.Forms.CheckBox();
			this.gbOWASP = new System.Windows.Forms.GroupBox();
			this.cbInnerClass = new System.Windows.Forms.CheckBox();
			this.cbFinalize = new System.Windows.Forms.CheckBox();
			this.tpXMLExport = new System.Windows.Forms.TabPage();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			this.gbFilter = new System.Windows.Forms.GroupBox();
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
			this.gbExportMode = new System.Windows.Forms.GroupBox();
			this.cbSaveState = new System.Windows.Forms.CheckBox();
			this.rbFiltered = new System.Windows.Forms.RadioButton();
			this.rbAll = new System.Windows.Forms.RadioButton();
			this.tpDisplay = new System.Windows.Forms.TabPage();
			this.gbDisplay = new System.Windows.Forms.GroupBox();
			this.cbShowStatusBar = new System.Windows.Forms.CheckBox();
			this.lblColour = new System.Windows.Forms.Label();
			this.btnColour = new System.Windows.Forms.Button();
			this.btnColour.Click += new System.EventHandler(this.btnColour_Click);
			this.cbReminder = new System.Windows.Forms.CheckBox();
			this.cbShowChart = new System.Windows.Forms.CheckBox();
			this.tpBeta = new System.Windows.Forms.TabPage();
			this.gbBeta = new System.Windows.Forms.GroupBox();
			this.cbSigned = new System.Windows.Forms.CheckBox();
			this.cbCobol = new System.Windows.Forms.CheckBox();
			this.tpGrep = new System.Windows.Forms.TabPage();
			this.txtTempGrepTitle = new System.Windows.Forms.TextBox();
			this.txtTempGrep = new System.Windows.Forms.TextBox();
			this.cdColorDialog = new System.Windows.Forms.ColorDialog();
			this.tabOptions.SuspendLayout();
			this.tpGeneral.SuspendLayout();
			this.gbReporting.SuspendLayout();
			this.gbOutput.SuspendLayout();
			this.gbFileTypes.SuspendLayout();
			this.gbLanguage.SuspendLayout();
			this.tpConfigFiles.SuspendLayout();
			this.gbConfigFiles.SuspendLayout();
			this.tpJava.SuspendLayout();
			this.gbAndroid.SuspendLayout();
			this.gbOWASP.SuspendLayout();
			this.tpXMLExport.SuspendLayout();
			this.gbFilter.SuspendLayout();
			this.gbExportMode.SuspendLayout();
			this.tpDisplay.SuspendLayout();
			this.gbDisplay.SuspendLayout();
			this.tpBeta.SuspendLayout();
			this.gbBeta.SuspendLayout();
			this.tpGrep.SuspendLayout();
			this.SuspendLayout();
			//
			//btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(402, 310);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			//btnOK
			//
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(483, 310);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			//
			//tabOptions
			//
			this.tabOptions.Controls.Add(this.tpGeneral);
			this.tabOptions.Controls.Add(this.tpConfigFiles);
			this.tabOptions.Controls.Add(this.tpJava);
			this.tabOptions.Controls.Add(this.tpXMLExport);
			this.tabOptions.Controls.Add(this.tpDisplay);
			this.tabOptions.Controls.Add(this.tpBeta);
			this.tabOptions.Controls.Add(this.tpGrep);
			this.tabOptions.Location = new System.Drawing.Point(1, 2);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			this.tabOptions.Size = new System.Drawing.Size(560, 302);
			this.tabOptions.TabIndex = 8;
			//
			//tpGeneral
			//
			this.tpGeneral.Controls.Add(this.gbReporting);
			this.tpGeneral.Controls.Add(this.gbOutput);
			this.tpGeneral.Controls.Add(this.gbFileTypes);
			this.tpGeneral.Controls.Add(this.gbLanguage);
			this.tpGeneral.Location = new System.Drawing.Point(4, 22);
			this.tpGeneral.Name = "tpGeneral";
			this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
			this.tpGeneral.Size = new System.Drawing.Size(552, 276);
			this.tpGeneral.TabIndex = 0;
			this.tpGeneral.Text = "General";
			this.tpGeneral.UseVisualStyleBackColor = true;
			//
			//gbReporting
			//
			this.gbReporting.Controls.Add(this.cboReporting);
			this.gbReporting.Controls.Add(this.lblReporting);
			this.gbReporting.Location = new System.Drawing.Point(8, 141);
			this.gbReporting.Name = "gbReporting";
			this.gbReporting.Size = new System.Drawing.Size(536, 46);
			this.gbReporting.TabIndex = 10;
			this.gbReporting.TabStop = false;
			this.gbReporting.Text = "Results/Reporting";
			//
			//cboReporting
			//
			this.cboReporting.FormattingEnabled = true;
			this.cboReporting.Items.AddRange(new object[] {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"});
			this.cboReporting.Location = new System.Drawing.Point(245, 17);
			this.cboReporting.Name = "cboReporting";
			this.cboReporting.Size = new System.Drawing.Size(199, 21);
			this.cboReporting.TabIndex = 1;
			//
			//lblReporting
			//
			this.lblReporting.AutoSize = true;
			this.lblReporting.Location = new System.Drawing.Point(12, 20);
			this.lblReporting.Name = "lblReporting";
			this.lblReporting.Size = new System.Drawing.Size(215, 13);
			this.lblReporting.TabIndex = 0;
			this.lblReporting.Text = "Display/Scan for Results Equal to or Above:";
			//
			//gbOutput
			//
			this.gbOutput.Controls.Add(this.cbOutput);
			this.gbOutput.Controls.Add(this.txtOutput);
			this.gbOutput.Controls.Add(this.btnOutputBrowse);
			this.gbOutput.Location = new System.Drawing.Point(8, 190);
			this.gbOutput.Name = "gbOutput";
			this.gbOutput.Size = new System.Drawing.Size(536, 72);
			this.gbOutput.TabIndex = 8;
			this.gbOutput.TabStop = false;
			this.gbOutput.Text = "Output";
			//
			//cbOutput
			//
			this.cbOutput.AutoSize = true;
			this.cbOutput.Location = new System.Drawing.Point(12, 18);
			this.cbOutput.Name = "cbOutput";
			this.cbOutput.Size = new System.Drawing.Size(117, 17);
			this.cbOutput.TabIndex = 0;
			this.cbOutput.Text = "Write to Output File";
			this.cbOutput.UseVisualStyleBackColor = true;
			//
			//txtOutput
			//
			this.txtOutput.Location = new System.Drawing.Point(9, 41);
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.Size = new System.Drawing.Size(491, 20);
			this.txtOutput.TabIndex = 3;
			//
			//btnOutputBrowse
			//
			this.btnOutputBrowse.Location = new System.Drawing.Point(506, 40);
			this.btnOutputBrowse.Name = "btnOutputBrowse";
			this.btnOutputBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnOutputBrowse.TabIndex = 4;
			this.btnOutputBrowse.Text = "...";
			this.btnOutputBrowse.UseVisualStyleBackColor = true;
			//
			//gbFileTypes
			//
			this.gbFileTypes.Controls.Add(this.lblExplain);
			this.gbFileTypes.Controls.Add(this.txtFileTypes);
			this.gbFileTypes.Controls.Add(this.cboFileTypes);
			this.gbFileTypes.Location = new System.Drawing.Point(8, 68);
			this.gbFileTypes.Name = "gbFileTypes";
			this.gbFileTypes.Size = new System.Drawing.Size(536, 67);
			this.gbFileTypes.TabIndex = 5;
			this.gbFileTypes.TabStop = false;
			this.gbFileTypes.Text = "File Types";
			//
			//lblExplain
			//
			this.lblExplain.AutoSize = true;
			this.lblExplain.Location = new System.Drawing.Point(6, 44);
			this.lblExplain.Name = "lblExplain";
			this.lblExplain.Size = new System.Drawing.Size(478, 13);
			this.lblExplain.TabIndex = 2;
			this.lblExplain.Text = "(Use .* to scan all file types but this will impact speed and potentially include" + 
				" non-code files in results)";
			//
			//txtFileTypes
			//
			this.txtFileTypes.Location = new System.Drawing.Point(121, 20);
			this.txtFileTypes.Name = "txtFileTypes";
			this.txtFileTypes.Size = new System.Drawing.Size(409, 20);
			this.txtFileTypes.TabIndex = 1;
			//
			//cboFileTypes
			//
			this.cboFileTypes.FormattingEnabled = true;
			this.cboFileTypes.Items.AddRange(new object[] {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP", "COBOL"});
			this.cboFileTypes.Location = new System.Drawing.Point(9, 20);
			this.cboFileTypes.Name = "cboFileTypes";
			this.cboFileTypes.Size = new System.Drawing.Size(105, 21);
			this.cboFileTypes.TabIndex = 0;
			//
			//gbLanguage
			//
			this.gbLanguage.Controls.Add(this.cboStartUpLanguage);
			this.gbLanguage.Controls.Add(this.cboCurrentLanguage);
			this.gbLanguage.Controls.Add(this.lblStartUpLanguage);
			this.gbLanguage.Controls.Add(this.lblCurrentLanguage);
			this.gbLanguage.Location = new System.Drawing.Point(8, 10);
			this.gbLanguage.Name = "gbLanguage";
			this.gbLanguage.Size = new System.Drawing.Size(536, 52);
			this.gbLanguage.TabIndex = 4;
			this.gbLanguage.TabStop = false;
			this.gbLanguage.Text = "Language";
			//
			//cboStartUpLanguage
			//
			this.cboStartUpLanguage.FormattingEnabled = true;
			this.cboStartUpLanguage.Items.AddRange(new object[] {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP", "COBOL"});
			this.cboStartUpLanguage.Location = new System.Drawing.Point(364, 20);
			this.cboStartUpLanguage.Name = "cboStartUpLanguage";
			this.cboStartUpLanguage.Size = new System.Drawing.Size(100, 21);
			this.cboStartUpLanguage.TabIndex = 3;
			//
			//cboCurrentLanguage
			//
			this.cboCurrentLanguage.FormattingEnabled = true;
			this.cboCurrentLanguage.Items.AddRange(new object[] {"C/C++", "Java", "PL/SQL", "C#", "VB", "PHP", "COBOL"});
			this.cboCurrentLanguage.Location = new System.Drawing.Point(118, 20);
			this.cboCurrentLanguage.Name = "cboCurrentLanguage";
			this.cboCurrentLanguage.Size = new System.Drawing.Size(100, 21);
			this.cboCurrentLanguage.TabIndex = 2;
			//
			//lblStartUpLanguage
			//
			this.lblStartUpLanguage.AutoSize = true;
			this.lblStartUpLanguage.Location = new System.Drawing.Point(258, 23);
			this.lblStartUpLanguage.Name = "lblStartUpLanguage";
			this.lblStartUpLanguage.Size = new System.Drawing.Size(100, 13);
			this.lblStartUpLanguage.TabIndex = 1;
			this.lblStartUpLanguage.Text = "Start Up Language:";
			//
			//lblCurrentLanguage
			//
			this.lblCurrentLanguage.AutoSize = true;
			this.lblCurrentLanguage.Location = new System.Drawing.Point(9, 23);
			this.lblCurrentLanguage.Name = "lblCurrentLanguage";
			this.lblCurrentLanguage.Size = new System.Drawing.Size(95, 13);
			this.lblCurrentLanguage.TabIndex = 0;
			this.lblCurrentLanguage.Text = "Current Language:";
			//
			//tpConfigFiles
			//
			this.tpConfigFiles.Controls.Add(this.gbConfigFiles);
			this.tpConfigFiles.Location = new System.Drawing.Point(4, 22);
			this.tpConfigFiles.Name = "tpConfigFiles";
			this.tpConfigFiles.Padding = new System.Windows.Forms.Padding(3);
			this.tpConfigFiles.Size = new System.Drawing.Size(552, 276);
			this.tpConfigFiles.TabIndex = 1;
			this.tpConfigFiles.Text = "Config Files";
			this.tpConfigFiles.UseVisualStyleBackColor = true;
			//
			//gbConfigFiles
			//
			this.gbConfigFiles.Controls.Add(this.btnCobolEdit);
			this.gbConfigFiles.Controls.Add(this.btnCobolBrowse);
			this.gbConfigFiles.Controls.Add(this.txtCobol);
			this.gbConfigFiles.Controls.Add(this.lblCobol);
			this.gbConfigFiles.Controls.Add(this.btnPHPEdit);
			this.gbConfigFiles.Controls.Add(this.btnPHPBrowse);
			this.gbConfigFiles.Controls.Add(this.txtPHP);
			this.gbConfigFiles.Controls.Add(this.lblPHP);
			this.gbConfigFiles.Controls.Add(this.btnVBEdit);
			this.gbConfigFiles.Controls.Add(this.btnVBBrowse);
			this.gbConfigFiles.Controls.Add(this.txtVB);
			this.gbConfigFiles.Controls.Add(this.lblVB);
			this.gbConfigFiles.Controls.Add(this.btnCSharpEdit);
			this.gbConfigFiles.Controls.Add(this.btnCSharpBrowse);
			this.gbConfigFiles.Controls.Add(this.txtCSharp);
			this.gbConfigFiles.Controls.Add(this.lblCSharp);
			this.gbConfigFiles.Controls.Add(this.btnCPPBrowse);
			this.gbConfigFiles.Controls.Add(this.txtCPP);
			this.gbConfigFiles.Controls.Add(this.btnSQLEdit);
			this.gbConfigFiles.Controls.Add(this.btnSQLBrowse);
			this.gbConfigFiles.Controls.Add(this.txtPLSQL);
			this.gbConfigFiles.Controls.Add(this.btnJavaEdit);
			this.gbConfigFiles.Controls.Add(this.btnJavaBrowse);
			this.gbConfigFiles.Controls.Add(this.txtJava);
			this.gbConfigFiles.Controls.Add(this.btnCPPEdit);
			this.gbConfigFiles.Controls.Add(this.lblSQL);
			this.gbConfigFiles.Controls.Add(this.lblJava);
			this.gbConfigFiles.Controls.Add(this.lblCPP);
			this.gbConfigFiles.Location = new System.Drawing.Point(6, 6);
			this.gbConfigFiles.Name = "gbConfigFiles";
			this.gbConfigFiles.Size = new System.Drawing.Size(536, 216);
			this.gbConfigFiles.TabIndex = 5;
			this.gbConfigFiles.TabStop = false;
			this.gbConfigFiles.Text = "Config Files";
			//
			//btnCobolEdit
			//
			this.btnCobolEdit.Location = new System.Drawing.Point(482, 176);
			this.btnCobolEdit.Name = "btnCobolEdit";
			this.btnCobolEdit.Size = new System.Drawing.Size(48, 23);
			this.btnCobolEdit.TabIndex = 29;
			this.btnCobolEdit.Text = "Edit";
			this.btnCobolEdit.UseVisualStyleBackColor = true;
			this.btnCobolEdit.Visible = false;
			//
			//btnCobolBrowse
			//
			this.btnCobolBrowse.Location = new System.Drawing.Point(452, 176);
			this.btnCobolBrowse.Name = "btnCobolBrowse";
			this.btnCobolBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnCobolBrowse.TabIndex = 28;
			this.btnCobolBrowse.Text = "...";
			this.btnCobolBrowse.UseVisualStyleBackColor = true;
			this.btnCobolBrowse.Visible = false;
			//
			//txtCobol
			//
			this.txtCobol.Location = new System.Drawing.Point(66, 178);
			this.txtCobol.Name = "txtCobol";
			this.txtCobol.Size = new System.Drawing.Size(380, 20);
			this.txtCobol.TabIndex = 27;
			this.txtCobol.Visible = false;
			//
			//lblCobol
			//
			this.lblCobol.AutoSize = true;
			this.lblCobol.Location = new System.Drawing.Point(9, 184);
			this.lblCobol.Name = "lblCobol";
			this.lblCobol.Size = new System.Drawing.Size(46, 13);
			this.lblCobol.TabIndex = 26;
			this.lblCobol.Text = "COBOL:";
			this.lblCobol.Visible = false;
			//
			//btnPHPEdit
			//
			this.btnPHPEdit.Location = new System.Drawing.Point(482, 147);
			this.btnPHPEdit.Name = "btnPHPEdit";
			this.btnPHPEdit.Size = new System.Drawing.Size(48, 23);
			this.btnPHPEdit.TabIndex = 25;
			this.btnPHPEdit.Text = "Edit";
			this.btnPHPEdit.UseVisualStyleBackColor = true;
			//
			//btnPHPBrowse
			//
			this.btnPHPBrowse.Location = new System.Drawing.Point(452, 147);
			this.btnPHPBrowse.Name = "btnPHPBrowse";
			this.btnPHPBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnPHPBrowse.TabIndex = 24;
			this.btnPHPBrowse.Text = "...";
			this.btnPHPBrowse.UseVisualStyleBackColor = true;
			//
			//txtPHP
			//
			this.txtPHP.Location = new System.Drawing.Point(66, 149);
			this.txtPHP.Name = "txtPHP";
			this.txtPHP.Size = new System.Drawing.Size(380, 20);
			this.txtPHP.TabIndex = 23;
			//
			//lblPHP
			//
			this.lblPHP.AutoSize = true;
			this.lblPHP.Location = new System.Drawing.Point(9, 155);
			this.lblPHP.Name = "lblPHP";
			this.lblPHP.Size = new System.Drawing.Size(32, 13);
			this.lblPHP.TabIndex = 22;
			this.lblPHP.Text = "PHP:";
			//
			//btnVBEdit
			//
			this.btnVBEdit.Location = new System.Drawing.Point(482, 120);
			this.btnVBEdit.Name = "btnVBEdit";
			this.btnVBEdit.Size = new System.Drawing.Size(48, 23);
			this.btnVBEdit.TabIndex = 21;
			this.btnVBEdit.Text = "Edit";
			this.btnVBEdit.UseVisualStyleBackColor = true;
			//
			//btnVBBrowse
			//
			this.btnVBBrowse.Location = new System.Drawing.Point(452, 120);
			this.btnVBBrowse.Name = "btnVBBrowse";
			this.btnVBBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnVBBrowse.TabIndex = 20;
			this.btnVBBrowse.Text = "...";
			this.btnVBBrowse.UseVisualStyleBackColor = true;
			//
			//txtVB
			//
			this.txtVB.Location = new System.Drawing.Point(66, 122);
			this.txtVB.Name = "txtVB";
			this.txtVB.Size = new System.Drawing.Size(380, 20);
			this.txtVB.TabIndex = 19;
			//
			//lblVB
			//
			this.lblVB.AutoSize = true;
			this.lblVB.Location = new System.Drawing.Point(9, 128);
			this.lblVB.Name = "lblVB";
			this.lblVB.Size = new System.Drawing.Size(24, 13);
			this.lblVB.TabIndex = 18;
			this.lblVB.Text = "VB:";
			//
			//btnCSharpEdit
			//
			this.btnCSharpEdit.Location = new System.Drawing.Point(482, 93);
			this.btnCSharpEdit.Name = "btnCSharpEdit";
			this.btnCSharpEdit.Size = new System.Drawing.Size(48, 23);
			this.btnCSharpEdit.TabIndex = 17;
			this.btnCSharpEdit.Text = "Edit";
			this.btnCSharpEdit.UseVisualStyleBackColor = true;
			//
			//btnCSharpBrowse
			//
			this.btnCSharpBrowse.Location = new System.Drawing.Point(452, 93);
			this.btnCSharpBrowse.Name = "btnCSharpBrowse";
			this.btnCSharpBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnCSharpBrowse.TabIndex = 16;
			this.btnCSharpBrowse.Text = "...";
			this.btnCSharpBrowse.UseVisualStyleBackColor = true;
			//
			//txtCSharp
			//
			this.txtCSharp.Location = new System.Drawing.Point(66, 95);
			this.txtCSharp.Name = "txtCSharp";
			this.txtCSharp.Size = new System.Drawing.Size(380, 20);
			this.txtCSharp.TabIndex = 15;
			//
			//lblCSharp
			//
			this.lblCSharp.AutoSize = true;
			this.lblCSharp.Location = new System.Drawing.Point(9, 101);
			this.lblCSharp.Name = "lblCSharp";
			this.lblCSharp.Size = new System.Drawing.Size(24, 13);
			this.lblCSharp.TabIndex = 14;
			this.lblCSharp.Text = "C#:";
			//
			//btnCPPBrowse
			//
			this.btnCPPBrowse.Location = new System.Drawing.Point(452, 15);
			this.btnCPPBrowse.Name = "btnCPPBrowse";
			this.btnCPPBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnCPPBrowse.TabIndex = 13;
			this.btnCPPBrowse.Text = "...";
			this.btnCPPBrowse.UseVisualStyleBackColor = true;
			//
			//txtCPP
			//
			this.txtCPP.Location = new System.Drawing.Point(66, 17);
			this.txtCPP.Name = "txtCPP";
			this.txtCPP.Size = new System.Drawing.Size(380, 20);
			this.txtCPP.TabIndex = 12;
			//
			//btnSQLEdit
			//
			this.btnSQLEdit.Location = new System.Drawing.Point(482, 67);
			this.btnSQLEdit.Name = "btnSQLEdit";
			this.btnSQLEdit.Size = new System.Drawing.Size(48, 23);
			this.btnSQLEdit.TabIndex = 11;
			this.btnSQLEdit.Text = "Edit";
			this.btnSQLEdit.UseVisualStyleBackColor = true;
			//
			//btnSQLBrowse
			//
			this.btnSQLBrowse.Location = new System.Drawing.Point(452, 67);
			this.btnSQLBrowse.Name = "btnSQLBrowse";
			this.btnSQLBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnSQLBrowse.TabIndex = 10;
			this.btnSQLBrowse.Text = "...";
			this.btnSQLBrowse.UseVisualStyleBackColor = true;
			//
			//txtPLSQL
			//
			this.txtPLSQL.Location = new System.Drawing.Point(66, 69);
			this.txtPLSQL.Name = "txtPLSQL";
			this.txtPLSQL.Size = new System.Drawing.Size(380, 20);
			this.txtPLSQL.TabIndex = 9;
			//
			//btnJavaEdit
			//
			this.btnJavaEdit.Location = new System.Drawing.Point(482, 41);
			this.btnJavaEdit.Name = "btnJavaEdit";
			this.btnJavaEdit.Size = new System.Drawing.Size(48, 23);
			this.btnJavaEdit.TabIndex = 8;
			this.btnJavaEdit.Text = "Edit";
			this.btnJavaEdit.UseVisualStyleBackColor = true;
			//
			//btnJavaBrowse
			//
			this.btnJavaBrowse.Location = new System.Drawing.Point(452, 41);
			this.btnJavaBrowse.Name = "btnJavaBrowse";
			this.btnJavaBrowse.Size = new System.Drawing.Size(24, 23);
			this.btnJavaBrowse.TabIndex = 7;
			this.btnJavaBrowse.Text = "...";
			this.btnJavaBrowse.UseVisualStyleBackColor = true;
			//
			//txtJava
			//
			this.txtJava.Location = new System.Drawing.Point(66, 43);
			this.txtJava.Name = "txtJava";
			this.txtJava.Size = new System.Drawing.Size(380, 20);
			this.txtJava.TabIndex = 6;
			//
			//btnCPPEdit
			//
			this.btnCPPEdit.Location = new System.Drawing.Point(482, 15);
			this.btnCPPEdit.Name = "btnCPPEdit";
			this.btnCPPEdit.Size = new System.Drawing.Size(48, 23);
			this.btnCPPEdit.TabIndex = 5;
			this.btnCPPEdit.Text = "Edit";
			this.btnCPPEdit.UseVisualStyleBackColor = true;
			//
			//lblSQL
			//
			this.lblSQL.AutoSize = true;
			this.lblSQL.Location = new System.Drawing.Point(9, 75);
			this.lblSQL.Name = "lblSQL";
			this.lblSQL.Size = new System.Drawing.Size(49, 13);
			this.lblSQL.TabIndex = 2;
			this.lblSQL.Text = "PL/SQL:";
			//
			//lblJava
			//
			this.lblJava.AutoSize = true;
			this.lblJava.Location = new System.Drawing.Point(9, 45);
			this.lblJava.Name = "lblJava";
			this.lblJava.Size = new System.Drawing.Size(33, 13);
			this.lblJava.TabIndex = 1;
			this.lblJava.Text = "Java:";
			//
			//lblCPP
			//
			this.lblCPP.AutoSize = true;
			this.lblCPP.Location = new System.Drawing.Point(9, 20);
			this.lblCPP.Name = "lblCPP";
			this.lblCPP.Size = new System.Drawing.Size(41, 13);
			this.lblCPP.TabIndex = 0;
			this.lblCPP.Text = "C/C++:";
			//
			//tpJava
			//
			this.tpJava.Controls.Add(this.gbAndroid);
			this.tpJava.Controls.Add(this.gbOWASP);
			this.tpJava.Location = new System.Drawing.Point(4, 22);
			this.tpJava.Name = "tpJava";
			this.tpJava.Size = new System.Drawing.Size(552, 276);
			this.tpJava.TabIndex = 5;
			this.tpJava.Text = "Java";
			this.tpJava.UseVisualStyleBackColor = true;
			//
			//gbAndroid
			//
			this.gbAndroid.Controls.Add(this.cbAndroid);
			this.gbAndroid.Location = new System.Drawing.Point(8, 70);
			this.gbAndroid.Name = "gbAndroid";
			this.gbAndroid.Size = new System.Drawing.Size(536, 50);
			this.gbAndroid.TabIndex = 12;
			this.gbAndroid.TabStop = false;
			this.gbAndroid.Text = "Mobile Applications";
			//
			//cbAndroid
			//
			this.cbAndroid.AutoSize = true;
			this.cbAndroid.Location = new System.Drawing.Point(11, 21);
			this.cbAndroid.Name = "cbAndroid";
			this.cbAndroid.Size = new System.Drawing.Size(139, 17);
			this.cbAndroid.TabIndex = 12;
			this.cbAndroid.Text = "Include Android Checks";
			this.cbAndroid.UseVisualStyleBackColor = true;
			//
			//gbOWASP
			//
			this.gbOWASP.Controls.Add(this.cbInnerClass);
			this.gbOWASP.Controls.Add(this.cbFinalize);
			this.gbOWASP.Location = new System.Drawing.Point(8, 14);
			this.gbOWASP.Name = "gbOWASP";
			this.gbOWASP.Size = new System.Drawing.Size(536, 46);
			this.gbOWASP.TabIndex = 10;
			this.gbOWASP.TabStop = false;
			this.gbOWASP.Text = "OWASP Recommendations";
			//
			//cbInnerClass
			//
			this.cbInnerClass.AutoSize = true;
			this.cbInnerClass.Location = new System.Drawing.Point(251, 18);
			this.cbInnerClass.Name = "cbInnerClass";
			this.cbInnerClass.Size = new System.Drawing.Size(174, 17);
			this.cbInnerClass.TabIndex = 1;
			this.cbInnerClass.Text = "Check for Nested Java Classes";
			this.cbInnerClass.UseVisualStyleBackColor = true;
			//
			//cbFinalize
			//
			this.cbFinalize.AutoSize = true;
			this.cbFinalize.Location = new System.Drawing.Point(12, 18);
			this.cbFinalize.Name = "cbFinalize";
			this.cbFinalize.Size = new System.Drawing.Size(204, 17);
			this.cbFinalize.TabIndex = 0;
			this.cbFinalize.Text = "Check for Finalization of Java Classes";
			this.cbFinalize.UseVisualStyleBackColor = true;
			//
			//tpXMLExport
			//
			this.tpXMLExport.Controls.Add(this.btnExport);
			this.tpXMLExport.Controls.Add(this.gbFilter);
			this.tpXMLExport.Controls.Add(this.gbExportMode);
			this.tpXMLExport.Location = new System.Drawing.Point(4, 22);
			this.tpXMLExport.Name = "tpXMLExport";
			this.tpXMLExport.Padding = new System.Windows.Forms.Padding(3);
			this.tpXMLExport.Size = new System.Drawing.Size(552, 276);
			this.tpXMLExport.TabIndex = 2;
			this.tpXMLExport.Text = "Result Filter and XML Export";
			this.tpXMLExport.UseVisualStyleBackColor = true;
			//
			//btnExport
			//
			this.btnExport.Location = new System.Drawing.Point(451, 220);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(92, 23);
			this.btnExport.TabIndex = 2;
			this.btnExport.Text = "Export Now...";
			this.btnExport.UseVisualStyleBackColor = true;
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
			this.gbFilter.Location = new System.Drawing.Point(7, 106);
			this.gbFilter.Name = "gbFilter";
			this.gbFilter.Size = new System.Drawing.Size(536, 108);
			this.gbFilter.TabIndex = 1;
			this.gbFilter.TabStop = false;
			this.gbFilter.Text = "Result Filter Options";
			//
			//cboMaximum
			//
			this.cboMaximum.Enabled = false;
			this.cboMaximum.FormattingEnabled = true;
			this.cboMaximum.Items.AddRange(new object[] {"Potentially Unsafe", "Suspicious Comment", "Low", "Standard", "Medium", "High", "Critical"});
			this.cboMaximum.Location = new System.Drawing.Point(372, 73);
			this.cboMaximum.Name = "cboMaximum";
			this.cboMaximum.Size = new System.Drawing.Size(158, 21);
			this.cboMaximum.TabIndex = 10;
			//
			//lblTo
			//
			this.lblTo.AutoSize = true;
			this.lblTo.Location = new System.Drawing.Point(351, 76);
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
			this.cboMinimum.Location = new System.Drawing.Point(194, 73);
			this.cboMinimum.Name = "cboMinimum";
			this.cboMinimum.Size = new System.Drawing.Size(154, 21);
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
			this.cboBelow.Location = new System.Drawing.Point(194, 46);
			this.cboBelow.Name = "cboBelow";
			this.cboBelow.Size = new System.Drawing.Size(154, 21);
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
			this.cboAbove.Location = new System.Drawing.Point(194, 19);
			this.cboAbove.Name = "cboAbove";
			this.cboAbove.Size = new System.Drawing.Size(154, 21);
			this.cboAbove.TabIndex = 3;
			//
			//gbExportMode
			//
			this.gbExportMode.Controls.Add(this.cbSaveState);
			this.gbExportMode.Controls.Add(this.rbFiltered);
			this.gbExportMode.Controls.Add(this.rbAll);
			this.gbExportMode.Location = new System.Drawing.Point(7, 7);
			this.gbExportMode.Name = "gbExportMode";
			this.gbExportMode.Size = new System.Drawing.Size(536, 92);
			this.gbExportMode.TabIndex = 0;
			this.gbExportMode.TabStop = false;
			this.gbExportMode.Text = "Export Mode";
			//
			//cbSaveState
			//
			this.cbSaveState.AutoSize = true;
			this.cbSaveState.Checked = true;
			this.cbSaveState.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSaveState.Location = new System.Drawing.Point(7, 68);
			this.cbSaveState.Name = "cbSaveState";
			this.cbSaveState.Size = new System.Drawing.Size(439, 17);
			this.cbSaveState.TabIndex = 2;
			this.cbSaveState.Text = "Save CheckBox State of Marked Items (Summary Table State will be Preserved in XML" + 
				")";
			this.cbSaveState.UseVisualStyleBackColor = true;
			//
			//rbFiltered
			//
			this.rbFiltered.AutoSize = true;
			this.rbFiltered.Location = new System.Drawing.Point(7, 44);
			this.rbFiltered.Name = "rbFiltered";
			this.rbFiltered.Size = new System.Drawing.Size(130, 17);
			this.rbFiltered.TabIndex = 1;
			this.rbFiltered.Text = "Export Filtered Results";
			this.rbFiltered.UseVisualStyleBackColor = true;
			//
			//rbAll
			//
			this.rbAll.AutoSize = true;
			this.rbAll.Checked = true;
			this.rbAll.Location = new System.Drawing.Point(7, 20);
			this.rbAll.Name = "rbAll";
			this.rbAll.Size = new System.Drawing.Size(107, 17);
			this.rbAll.TabIndex = 0;
			this.rbAll.TabStop = true;
			this.rbAll.Text = "Export All Results";
			this.rbAll.UseVisualStyleBackColor = true;
			//
			//tpDisplay
			//
			this.tpDisplay.Controls.Add(this.gbDisplay);
			this.tpDisplay.Location = new System.Drawing.Point(4, 22);
			this.tpDisplay.Name = "tpDisplay";
			this.tpDisplay.Padding = new System.Windows.Forms.Padding(3);
			this.tpDisplay.Size = new System.Drawing.Size(552, 276);
			this.tpDisplay.TabIndex = 3;
			this.tpDisplay.Text = "Display";
			this.tpDisplay.UseVisualStyleBackColor = true;
			//
			//gbDisplay
			//
			this.gbDisplay.Controls.Add(this.cbShowStatusBar);
			this.gbDisplay.Controls.Add(this.lblColour);
			this.gbDisplay.Controls.Add(this.btnColour);
			this.gbDisplay.Controls.Add(this.cbReminder);
			this.gbDisplay.Controls.Add(this.cbShowChart);
			this.gbDisplay.Location = new System.Drawing.Point(6, 6);
			this.gbDisplay.Name = "gbDisplay";
			this.gbDisplay.Size = new System.Drawing.Size(540, 303);
			this.gbDisplay.TabIndex = 0;
			this.gbDisplay.TabStop = false;
			this.gbDisplay.Text = "Display Options";
			//
			//cbShowStatusBar
			//
			this.cbShowStatusBar.AutoSize = true;
			this.cbShowStatusBar.Location = new System.Drawing.Point(8, 86);
			this.cbShowStatusBar.Name = "cbShowStatusBar";
			this.cbShowStatusBar.Size = new System.Drawing.Size(105, 17);
			this.cbShowStatusBar.TabIndex = 4;
			this.cbShowStatusBar.Text = "Show Status Bar";
			this.cbShowStatusBar.UseVisualStyleBackColor = true;
			//
			//lblColour
			//
			this.lblColour.AutoSize = true;
			this.lblColour.Location = new System.Drawing.Point(4, 116);
			this.lblColour.Name = "lblColour";
			this.lblColour.Size = new System.Drawing.Size(197, 13);
			this.lblColour.TabIndex = 3;
			this.lblColour.Text = "Set Colour of Selected Items in ListView:";
			//
			//btnColour
			//
			this.btnColour.Location = new System.Drawing.Point(201, 111);
			this.btnColour.Name = "btnColour";
			this.btnColour.Size = new System.Drawing.Size(30, 22);
			this.btnColour.TabIndex = 2;
			this.btnColour.Text = "...";
			this.btnColour.UseVisualStyleBackColor = true;
			//
			//cbReminder
			//
			this.cbReminder.AutoSize = true;
			this.cbReminder.Location = new System.Drawing.Point(8, 29);
			this.cbReminder.Name = "cbReminder";
			this.cbReminder.Size = new System.Drawing.Size(281, 17);
			this.cbReminder.TabIndex = 1;
			this.cbReminder.Text = "Show me a reminder to choose a language on start-up";
			this.cbReminder.UseVisualStyleBackColor = true;
			//
			//cbShowChart
			//
			this.cbShowChart.AutoSize = true;
			this.cbShowChart.Location = new System.Drawing.Point(8, 59);
			this.cbShowChart.Name = "cbShowChart";
			this.cbShowChart.Size = new System.Drawing.Size(282, 17);
			this.cbShowChart.TabIndex = 0;
			this.cbShowChart.Text = "Show charts and code breakdown when scan finishes";
			this.cbShowChart.UseVisualStyleBackColor = true;
			//
			//tpBeta
			//
			this.tpBeta.Controls.Add(this.gbBeta);
			this.tpBeta.Location = new System.Drawing.Point(4, 22);
			this.tpBeta.Name = "tpBeta";
			this.tpBeta.Size = new System.Drawing.Size(552, 276);
			this.tpBeta.TabIndex = 4;
			this.tpBeta.Text = "Beta Functionality";
			this.tpBeta.UseVisualStyleBackColor = true;
			//
			//gbBeta
			//
			this.gbBeta.Controls.Add(this.cbSigned);
			this.gbBeta.Controls.Add(this.cbCobol);
			this.gbBeta.Location = new System.Drawing.Point(5, 6);
			this.gbBeta.Name = "gbBeta";
			this.gbBeta.Size = new System.Drawing.Size(540, 303);
			this.gbBeta.TabIndex = 1;
			this.gbBeta.TabStop = false;
			this.gbBeta.Text = "Beta Functionality Options";
			//
			//cbSigned
			//
			this.cbSigned.AutoSize = true;
			this.cbSigned.Location = new System.Drawing.Point(8, 29);
			this.cbSigned.Name = "cbSigned";
			this.cbSigned.Size = new System.Drawing.Size(240, 17);
			this.cbSigned.TabIndex = 1;
			this.cbSigned.Text = "Include signed/unsigned comparison (C/C++)";
			this.cbSigned.UseVisualStyleBackColor = true;
			//
			//cbCobol
			//
			this.cbCobol.AutoSize = true;
			this.cbCobol.Location = new System.Drawing.Point(8, 59);
			this.cbCobol.Name = "cbCobol";
			this.cbCobol.Size = new System.Drawing.Size(205, 17);
			this.cbCobol.TabIndex = 0;
			this.cbCobol.Text = "Include COBOL scanning functionality";
			this.cbCobol.UseVisualStyleBackColor = true;
			//
			//tpGrep
			//
			this.tpGrep.Controls.Add(this.txtTempGrepTitle);
			this.tpGrep.Controls.Add(this.txtTempGrep);
			this.tpGrep.Location = new System.Drawing.Point(4, 22);
			this.tpGrep.Name = "tpGrep";
			this.tpGrep.Size = new System.Drawing.Size(552, 276);
			this.tpGrep.TabIndex = 6;
			this.tpGrep.Text = "Temporary Grep";
			this.tpGrep.UseVisualStyleBackColor = true;
			//
			//txtTempGrepTitle
			//
			this.txtTempGrepTitle.Location = new System.Drawing.Point(8, 8);
			this.txtTempGrepTitle.Multiline = true;
			this.txtTempGrepTitle.Name = "txtTempGrepTitle";
			this.txtTempGrepTitle.ReadOnly = true;
			this.txtTempGrepTitle.Size = new System.Drawing.Size(531, 54);
			this.txtTempGrepTitle.TabIndex = 1;
			this.txtTempGrepTitle.Text = resources.GetString("txtTempGrepTitle.Text");
			//
			//txtTempGrep
			//
			this.txtTempGrep.Location = new System.Drawing.Point(8, 68);
			this.txtTempGrep.Multiline = true;
			this.txtTempGrep.Name = "txtTempGrep";
			this.txtTempGrep.Size = new System.Drawing.Size(531, 196);
			this.txtTempGrep.TabIndex = 0;
			//
			//frmOptions
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(565, 341);
			this.Controls.Add(this.tabOptions);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Icon = (System.Drawing.Icon) (resources.GetObject("$this.Icon"));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.tabOptions.ResumeLayout(false);
			this.tpGeneral.ResumeLayout(false);
			this.gbReporting.ResumeLayout(false);
			this.gbReporting.PerformLayout();
			this.gbOutput.ResumeLayout(false);
			this.gbOutput.PerformLayout();
			this.gbFileTypes.ResumeLayout(false);
			this.gbFileTypes.PerformLayout();
			this.gbLanguage.ResumeLayout(false);
			this.gbLanguage.PerformLayout();
			this.tpConfigFiles.ResumeLayout(false);
			this.gbConfigFiles.ResumeLayout(false);
			this.gbConfigFiles.PerformLayout();
			this.tpJava.ResumeLayout(false);
			this.gbAndroid.ResumeLayout(false);
			this.gbAndroid.PerformLayout();
			this.gbOWASP.ResumeLayout(false);
			this.gbOWASP.PerformLayout();
			this.tpXMLExport.ResumeLayout(false);
			this.gbFilter.ResumeLayout(false);
			this.gbFilter.PerformLayout();
			this.gbExportMode.ResumeLayout(false);
			this.gbExportMode.PerformLayout();
			this.tpDisplay.ResumeLayout(false);
			this.gbDisplay.ResumeLayout(false);
			this.gbDisplay.PerformLayout();
			this.tpBeta.ResumeLayout(false);
			this.gbBeta.ResumeLayout(false);
			this.gbBeta.PerformLayout();
			this.tpGrep.ResumeLayout(false);
			this.tpGrep.PerformLayout();
			this.ResumeLayout(false);
			
		}
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.Button btnOK;
		internal System.Windows.Forms.OpenFileDialog ofdOpenFileDialog;
		internal System.Windows.Forms.SaveFileDialog sfdSaveFileDialog;
		internal System.Windows.Forms.TabControl tabOptions;
		internal System.Windows.Forms.TabPage tpGeneral;
		internal System.Windows.Forms.GroupBox gbReporting;
		internal System.Windows.Forms.ComboBox cboReporting;
		internal System.Windows.Forms.Label lblReporting;
		internal System.Windows.Forms.GroupBox gbOutput;
		internal System.Windows.Forms.CheckBox cbOutput;
		internal System.Windows.Forms.TextBox txtOutput;
		internal System.Windows.Forms.Button btnOutputBrowse;
		internal System.Windows.Forms.GroupBox gbFileTypes;
		internal System.Windows.Forms.Label lblExplain;
		internal System.Windows.Forms.TextBox txtFileTypes;
		internal System.Windows.Forms.ComboBox cboFileTypes;
		internal System.Windows.Forms.GroupBox gbLanguage;
		internal System.Windows.Forms.ComboBox cboStartUpLanguage;
		internal System.Windows.Forms.ComboBox cboCurrentLanguage;
		internal System.Windows.Forms.Label lblStartUpLanguage;
		internal System.Windows.Forms.Label lblCurrentLanguage;
		internal System.Windows.Forms.TabPage tpConfigFiles;
		internal System.Windows.Forms.GroupBox gbConfigFiles;
		internal System.Windows.Forms.Button btnCSharpEdit;
		internal System.Windows.Forms.Button btnCSharpBrowse;
		internal System.Windows.Forms.TextBox txtCSharp;
		internal System.Windows.Forms.Label lblCSharp;
		internal System.Windows.Forms.Button btnCPPBrowse;
		internal System.Windows.Forms.TextBox txtCPP;
		internal System.Windows.Forms.Button btnSQLEdit;
		internal System.Windows.Forms.Button btnSQLBrowse;
		internal System.Windows.Forms.TextBox txtPLSQL;
		internal System.Windows.Forms.Button btnJavaEdit;
		internal System.Windows.Forms.Button btnJavaBrowse;
		internal System.Windows.Forms.TextBox txtJava;
		internal System.Windows.Forms.Button btnCPPEdit;
		internal System.Windows.Forms.Label lblSQL;
		internal System.Windows.Forms.Label lblJava;
		internal System.Windows.Forms.Label lblCPP;
		internal System.Windows.Forms.TabPage tpXMLExport;
		internal System.Windows.Forms.GroupBox gbExportMode;
		internal System.Windows.Forms.CheckBox cbSaveState;
		internal System.Windows.Forms.RadioButton rbFiltered;
		internal System.Windows.Forms.RadioButton rbAll;
		internal System.Windows.Forms.GroupBox gbFilter;
		internal System.Windows.Forms.ComboBox cboMaximum;
		internal System.Windows.Forms.Label lblTo;
		internal System.Windows.Forms.RadioButton rbRange;
		internal System.Windows.Forms.ComboBox cboMinimum;
		internal System.Windows.Forms.RadioButton rbBelow;
		internal System.Windows.Forms.ComboBox cboBelow;
		internal System.Windows.Forms.RadioButton rbAbove;
		internal System.Windows.Forms.ComboBox cboAbove;
		internal System.Windows.Forms.Button btnExport;
		internal System.Windows.Forms.Button btnVBEdit;
		internal System.Windows.Forms.Button btnVBBrowse;
		internal System.Windows.Forms.TextBox txtVB;
		internal System.Windows.Forms.Label lblVB;
		internal System.Windows.Forms.Button btnPHPEdit;
		internal System.Windows.Forms.Button btnPHPBrowse;
		internal System.Windows.Forms.TextBox txtPHP;
		internal System.Windows.Forms.Label lblPHP;
		internal System.Windows.Forms.Button btnCobolEdit;
		internal System.Windows.Forms.Button btnCobolBrowse;
		internal System.Windows.Forms.TextBox txtCobol;
		internal System.Windows.Forms.Label lblCobol;
		internal System.Windows.Forms.TabPage tpDisplay;
		internal System.Windows.Forms.GroupBox gbDisplay;
		internal System.Windows.Forms.CheckBox cbReminder;
		internal System.Windows.Forms.CheckBox cbShowChart;
		internal System.Windows.Forms.Label lblColour;
		internal System.Windows.Forms.Button btnColour;
		internal System.Windows.Forms.ColorDialog cdColorDialog;
		internal System.Windows.Forms.CheckBox cbShowStatusBar;
		internal System.Windows.Forms.TabPage tpBeta;
		internal System.Windows.Forms.GroupBox gbBeta;
		internal System.Windows.Forms.CheckBox cbSigned;
		internal System.Windows.Forms.CheckBox cbCobol;
		internal System.Windows.Forms.TabPage tpJava;
		internal System.Windows.Forms.GroupBox gbOWASP;
		internal System.Windows.Forms.CheckBox cbInnerClass;
		internal System.Windows.Forms.CheckBox cbFinalize;
		internal System.Windows.Forms.TabPage tpGrep;
		internal System.Windows.Forms.TextBox txtTempGrep;
		internal System.Windows.Forms.TextBox txtTempGrepTitle;
		internal System.Windows.Forms.GroupBox gbAndroid;
		internal System.Windows.Forms.CheckBox cbAndroid;
	}
	
}
