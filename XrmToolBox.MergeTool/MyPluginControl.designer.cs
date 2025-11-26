using System;
using System.Drawing;
using System.Windows.Forms;

namespace XrmToolBox.MergeTool
{
    partial class MyPluginControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton btnLoadEntities;
        private System.Windows.Forms.ToolStripButton btnLoadExcel;
        private System.Windows.Forms.ToolStripButton btnValidateExcel;
        private System.Windows.Forms.ToolStripButton btnMerge;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox grpEntities;
        private System.Windows.Forms.GroupBox grpExcel;
        private System.Windows.Forms.TableLayoutPanel tableEntityLayout;
        private System.Windows.Forms.Label lblGuideLoadEntities;
        private System.Windows.Forms.Label lblGuideSearch;
        private System.Windows.Forms.Panel panelEntitySearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblGuideStatus;
        private System.Windows.Forms.TableLayoutPanel tableStateStatus;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStateOptions;
        private System.Windows.Forms.ComboBox cmbStatusOptions;
        private System.Windows.Forms.DataGridView dataGridViewEntities;
        private System.Windows.Forms.TableLayoutPanel tableExcelLayout;
        private System.Windows.Forms.Label lblGuideLoadExcel;
        private System.Windows.Forms.Panel panelExcelPath;
        private System.Windows.Forms.Label lblExcelFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label lblGuideValidate;
        private System.Windows.Forms.Panel panelExcelGrid;
        private System.Windows.Forms.DataGridView dataGridViewExcel;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Panel footerInfoPanel;
        private System.Windows.Forms.TextBox txtErrorCount;
        private System.Windows.Forms.TextBox txtTotalCount;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.TextBox txtProgressLog;
        private System.Windows.Forms.ProgressBar progressBarMerge;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.btnLoadEntities = new System.Windows.Forms.ToolStripButton();
            this.btnLoadExcel = new System.Windows.Forms.ToolStripButton();
            this.btnValidateExcel = new System.Windows.Forms.ToolStripButton();
            this.btnMerge = new System.Windows.Forms.ToolStripButton();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.grpEntities = new System.Windows.Forms.GroupBox();
            this.tableEntityLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblGuideLoadEntities = new System.Windows.Forms.Label();
            this.lblGuideSearch = new System.Windows.Forms.Label();
            this.panelEntitySearch = new System.Windows.Forms.Panel();
            this.dataGridViewEntities = new System.Windows.Forms.DataGridView();
            this.tableStateStatus = new System.Windows.Forms.TableLayoutPanel();
            this.lblState = new System.Windows.Forms.Label();
            this.cmbStateOptions = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatusOptions = new System.Windows.Forms.ComboBox();
            this.lblGuideStatus = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.grpExcel = new System.Windows.Forms.GroupBox();
            this.tableExcelLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblGuideLoadExcel = new System.Windows.Forms.Label();
            this.panelExcelPath = new System.Windows.Forms.Panel();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblExcelFile = new System.Windows.Forms.Label();
            this.lblGuideValidate = new System.Windows.Forms.Label();
            this.panelExcelGrid = new System.Windows.Forms.Panel();
            this.dataGridViewExcel = new System.Windows.Forms.DataGridView();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.txtProgressLog = new System.Windows.Forms.TextBox();
            this.progressBarMerge = new System.Windows.Forms.ProgressBar();
            this.footerInfoPanel = new System.Windows.Forms.Panel();
            this.txtTotalCount = new System.Windows.Forms.TextBox();
            this.txtErrorCount = new System.Windows.Forms.TextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblReport = new System.Windows.Forms.Label();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.grpEntities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).BeginInit();
            this.tableStateStatus.SuspendLayout();
            this.grpExcel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).BeginInit();
            this.panelExcelPath.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.footerInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadEntities,
            this.btnLoadExcel,
            this.btnValidateExcel,
            this.btnMerge,
            this.btnHelp});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(553, 31);
            this.toolStripMenu.TabIndex = 0;
            this.toolStripMenu.Text = "toolStripMenu";
            // 
            // btnLoadEntities
            // 
            this.btnLoadEntities.Image = global::XrmToolBox.MergeTool.Properties.Resources.load_entities;
            this.btnLoadEntities.Name = "btnLoadEntities";
            this.btnLoadEntities.Size = new System.Drawing.Size(102, 28);
            this.btnLoadEntities.Text = "Load Entities";
            this.btnLoadEntities.Click += new System.EventHandler(this.btnLoadEntities_Click);
            // 
            // btnLoadExcel
            // 
            this.btnLoadExcel.Enabled = false;
            this.btnLoadExcel.Image = global::XrmToolBox.MergeTool.Properties.Resources.load_excel;
            this.btnLoadExcel.Name = "btnLoadExcel";
            this.btnLoadExcel.Size = new System.Drawing.Size(90, 28);
            this.btnLoadExcel.Text = "Load Excel";
            this.btnLoadExcel.Click += new System.EventHandler(this.btnLoadExcel_Click);
            // 
            // btnValidateExcel
            // 
            this.btnValidateExcel.Enabled = false;
            this.btnValidateExcel.Image = global::XrmToolBox.MergeTool.Properties.Resources.validate_excel;
            this.btnValidateExcel.Name = "btnValidateExcel";
            this.btnValidateExcel.Size = new System.Drawing.Size(106, 28);
            this.btnValidateExcel.Text = "Validate Excel";
            this.btnValidateExcel.Click += new System.EventHandler(this.btnValidateExcel_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Enabled = false;
            this.btnMerge.Image = global::XrmToolBox.MergeTool.Properties.Resources.merge;
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(69, 28);
            this.btnMerge.Text = "Merge";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Image = global::XrmToolBox.MergeTool.Properties.Resources.help_icon;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(53, 28);
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 31);
            this.splitContainerMain.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Size = new System.Drawing.Size(553, 329);
            this.splitContainerMain.SplitterDistance = 262;
            this.splitContainerMain.TabIndex = 1;
            // 
            // grpEntities
            // 
            this.grpEntities.Controls.Add(this.tableEntityLayout);
            this.grpEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEntities.Location = new System.Drawing.Point(0, 0);
            this.grpEntities.Margin = new System.Windows.Forms.Padding(8);
            this.grpEntities.Name = "grpEntities";
            this.grpEntities.Padding = new System.Windows.Forms.Padding(8);
            this.grpEntities.Size = new System.Drawing.Size(262, 329);
            this.grpEntities.TabIndex = 0;
            this.grpEntities.TabStop = false;
            this.grpEntities.Text = "Entities";
            // 
            // dataGridViewEntities
            // 
            this.dataGridViewEntities.AllowUserToAddRows = false;
            this.dataGridViewEntities.AllowUserToDeleteRows = false;
            this.dataGridViewEntities.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEntities.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewEntities.Name = "dataGridViewEntities";
            this.dataGridViewEntities.ReadOnly = true;
            this.dataGridViewEntities.RowHeadersWidth = 62;
            this.dataGridViewEntities.RowTemplate.Height = 28;
            this.dataGridViewEntities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewEntities.TabIndex = 5;
            this.dataGridViewEntities.SelectionChanged += new System.EventHandler(this.dataGridViewEntities_SelectionChanged);
            // 
            // tableEntityLayout
            // 
            this.tableEntityLayout.ColumnCount = 1;
            this.tableEntityLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEntityLayout.Controls.Add(this.lblGuideLoadEntities, 0, 0);
            this.tableEntityLayout.Controls.Add(this.lblGuideSearch, 0, 1);
            this.tableEntityLayout.Controls.Add(this.panelEntitySearch, 0, 2);
            this.tableEntityLayout.Controls.Add(this.dataGridViewEntities, 0, 3);
            this.tableEntityLayout.Controls.Add(this.lblGuideStatus, 0, 4);
            this.tableEntityLayout.Controls.Add(this.tableStateStatus, 0, 5);
            this.tableEntityLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEntityLayout.Location = new System.Drawing.Point(8, 23);
            this.tableEntityLayout.Name = "tableEntityLayout";
            this.tableEntityLayout.RowCount = 6;
            this.tableEntityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableEntityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableEntityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableEntityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEntityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableEntityLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableEntityLayout.Size = new System.Drawing.Size(246, 298);
            this.tableEntityLayout.TabIndex = 0;
            // 
            // lblGuideLoadEntities
            // 
            this.lblGuideLoadEntities.AutoSize = true;
            this.lblGuideLoadEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGuideLoadEntities.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGuideLoadEntities.Location = new System.Drawing.Point(3, 0);
            this.lblGuideLoadEntities.Name = "lblGuideLoadEntities";
            this.lblGuideLoadEntities.Size = new System.Drawing.Size(240, 24);
            this.lblGuideLoadEntities.TabIndex = 0;
            this.lblGuideLoadEntities.Text = "1. Load entities.";
            this.lblGuideLoadEntities.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGuideLoadEntities.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            // 
            // lblGuideSearch
            // 
            this.lblGuideSearch.AutoSize = true;
            this.lblGuideSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGuideSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGuideSearch.Location = new System.Drawing.Point(3, 24);
            this.lblGuideSearch.Name = "lblGuideSearch";
            this.lblGuideSearch.Size = new System.Drawing.Size(240, 24);
            this.lblGuideSearch.TabIndex = 1;
            this.lblGuideSearch.Text = "2. Search and highlight an entity.";
            this.lblGuideSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGuideSearch.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            // 
            // panelEntitySearch
            // 
            this.panelEntitySearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEntitySearch.Controls.Add(this.txtSearch);
            this.panelEntitySearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEntitySearch.Location = new System.Drawing.Point(3, 51);
            this.panelEntitySearch.Name = "panelEntitySearch";
            this.panelEntitySearch.Padding = new System.Windows.Forms.Padding(3);
            this.panelEntitySearch.Size = new System.Drawing.Size(240, 29);
            this.panelEntitySearch.TabIndex = 2;
            // 
            // lblGuideStatus
            // 
            this.lblGuideStatus.AutoSize = true;
            this.lblGuideStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGuideStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGuideStatus.Name = "lblGuideStatus";
            this.lblGuideStatus.TabIndex = 3;
            this.lblGuideStatus.Text = "3. Choose state/status for merged-from.";
            this.lblGuideStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGuideStatus.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Enabled = false;
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(232, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "Search...";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.GotFocus += new System.EventHandler(this.RemoveText);
            this.txtSearch.LostFocus += new System.EventHandler(this.AddText);
            // 
            // tableStateStatus
            // 
            this.tableStateStatus.ColumnCount = 2;
            this.tableStateStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableStateStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableStateStatus.Controls.Add(this.lblState, 0, 0);
            this.tableStateStatus.Controls.Add(this.cmbStateOptions, 1, 0);
            this.tableStateStatus.Controls.Add(this.lblStatus, 0, 1);
            this.tableStateStatus.Controls.Add(this.cmbStatusOptions, 1, 1);
            this.tableStateStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableStateStatus.Margin = new System.Windows.Forms.Padding(3);
            this.tableStateStatus.Name = "tableStateStatus";
            this.tableStateStatus.RowCount = 2;
            this.tableStateStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableStateStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableStateStatus.TabIndex = 1;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblState.Location = new System.Drawing.Point(2, 0);
            this.lblState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(33, 28);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "State:";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStateOptions
            // 
            this.cmbStateOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStateOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStateOptions.Enabled = false;
            this.cmbStateOptions.FormattingEnabled = true;
            this.cmbStateOptions.Location = new System.Drawing.Point(39, 2);
            this.cmbStateOptions.Margin = new System.Windows.Forms.Padding(2);
            this.cmbStateOptions.Name = "cmbStateOptions";
            this.cmbStateOptions.Size = new System.Drawing.Size(205, 21);
            this.cmbStateOptions.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(2, 28);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(33, 30);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStatusOptions
            // 
            this.cmbStatusOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatusOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusOptions.Enabled = false;
            this.cmbStatusOptions.FormattingEnabled = true;
            this.cmbStatusOptions.Location = new System.Drawing.Point(39, 30);
            this.cmbStatusOptions.Margin = new System.Windows.Forms.Padding(2);
            this.cmbStatusOptions.Name = "cmbStatusOptions";
            this.cmbStatusOptions.Size = new System.Drawing.Size(205, 21);
            this.cmbStatusOptions.TabIndex = 3;
            // 
            // 
            // grpExcel
            // 
            this.grpExcel.Controls.Add(this.tableExcelLayout);
            this.grpExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpExcel.Location = new System.Drawing.Point(0, 0);
            this.grpExcel.Margin = new System.Windows.Forms.Padding(8);
            this.grpExcel.Name = "grpExcel";
            this.grpExcel.Padding = new System.Windows.Forms.Padding(8);
            this.grpExcel.Size = new System.Drawing.Size(287, 329);
            this.grpExcel.TabIndex = 0;
            this.grpExcel.TabStop = false;
            this.grpExcel.Text = "Excel Validation";
            // 
            // tableExcelLayout
            // 
            this.tableExcelLayout.ColumnCount = 1;
            this.tableExcelLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableExcelLayout.Controls.Add(this.lblGuideLoadExcel, 0, 0);
            this.tableExcelLayout.Controls.Add(this.panelExcelPath, 0, 1);
            this.tableExcelLayout.Controls.Add(this.lblGuideValidate, 0, 2);
            this.tableExcelLayout.Controls.Add(this.panelExcelGrid, 0, 3);
            this.tableExcelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableExcelLayout.Location = new System.Drawing.Point(8, 23);
            this.tableExcelLayout.Name = "tableExcelLayout";
            this.tableExcelLayout.RowCount = 4;
            this.tableExcelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableExcelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableExcelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableExcelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableExcelLayout.Size = new System.Drawing.Size(271, 298);
            this.tableExcelLayout.TabIndex = 0;
            // 
            // lblGuideLoadExcel
            // 
            this.lblGuideLoadExcel.AutoSize = true;
            this.lblGuideLoadExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGuideLoadExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGuideLoadExcel.Location = new System.Drawing.Point(3, 0);
            this.lblGuideLoadExcel.Name = "lblGuideLoadExcel";
            this.lblGuideLoadExcel.Size = new System.Drawing.Size(265, 24);
            this.lblGuideLoadExcel.TabIndex = 0;
            this.lblGuideLoadExcel.Text = "4. Load the Excel file.";
            this.lblGuideLoadExcel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelExcelPath
            // 
            this.panelExcelPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelExcelPath.Controls.Add(this.txtFilePath);
            this.panelExcelPath.Controls.Add(this.lblExcelFile);
            this.panelExcelPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExcelPath.Location = new System.Drawing.Point(3, 27);
            this.panelExcelPath.Name = "panelExcelPath";
            this.panelExcelPath.Padding = new System.Windows.Forms.Padding(3);
            this.panelExcelPath.Size = new System.Drawing.Size(265, 54);
            this.panelExcelPath.TabIndex = 1;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.Location = new System.Drawing.Point(76, 3);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(184, 20);
            this.txtFilePath.TabIndex = 1;
            // 
            // lblExcelFile
            // 
            this.lblExcelFile.AutoSize = true;
            this.lblExcelFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblExcelFile.Location = new System.Drawing.Point(3, 3);
            this.lblExcelFile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExcelFile.Name = "lblExcelFile";
            this.lblExcelFile.Padding = new System.Windows.Forms.Padding(0, 4, 6, 0);
            this.lblExcelFile.Size = new System.Drawing.Size(73, 17);
            this.lblExcelFile.TabIndex = 0;
            this.lblExcelFile.Text = "Excel path:";
            // 
            // lblGuideValidate
            // 
            this.lblGuideValidate.AutoSize = true;
            this.lblGuideValidate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGuideValidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblGuideValidate.Location = new System.Drawing.Point(3, 84);
            this.lblGuideValidate.Name = "lblGuideValidate";
            this.lblGuideValidate.Size = new System.Drawing.Size(265, 24);
            this.lblGuideValidate.TabIndex = 2;
            this.lblGuideValidate.Text = "5. Validate Excel before merging.";
            this.lblGuideValidate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelExcelGrid
            // 
            this.panelExcelGrid.AutoScroll = true;
            this.panelExcelGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelExcelGrid.Controls.Add(this.dataGridViewExcel);
            this.panelExcelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExcelGrid.Location = new System.Drawing.Point(3, 111);
            this.panelExcelGrid.Name = "panelExcelGrid";
            this.panelExcelGrid.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panelExcelGrid.Size = new System.Drawing.Size(265, 184);
            this.panelExcelGrid.TabIndex = 3;
            // 
            // dataGridViewExcel
            // 
            this.dataGridViewExcel.AllowUserToAddRows = false;
            this.dataGridViewExcel.AllowUserToDeleteRows = false;
            this.dataGridViewExcel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExcel.Enabled = true;
            this.dataGridViewExcel.ReadOnly = true;
            this.dataGridViewExcel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewExcel.Location = new System.Drawing.Point(0, 5);
            this.dataGridViewExcel.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewExcel.Name = "dataGridViewExcel";
            this.dataGridViewExcel.RowHeadersWidth = 62;
            this.dataGridViewExcel.RowTemplate.Height = 28;
            this.dataGridViewExcel.Size = new System.Drawing.Size(263, 177);
            this.dataGridViewExcel.TabIndex = 0;
            // 
            // footerPanel
            // 
            this.footerPanel.Controls.Add(this.txtProgressLog);
            this.footerPanel.Controls.Add(this.progressBarMerge);
            this.footerPanel.Controls.Add(this.footerInfoPanel);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 360);
            this.footerPanel.Margin = new System.Windows.Forms.Padding(2);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(5);
            this.footerPanel.Size = new System.Drawing.Size(553, 140);
            this.footerPanel.TabIndex = 2;
            // 
            // txtProgressLog
            // 
            this.txtProgressLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProgressLog.Location = new System.Drawing.Point(5, 45);
            this.txtProgressLog.Margin = new System.Windows.Forms.Padding(2);
            this.txtProgressLog.Multiline = true;
            this.txtProgressLog.Name = "txtProgressLog";
            this.txtProgressLog.ReadOnly = true;
            this.txtProgressLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProgressLog.Size = new System.Drawing.Size(543, 70);
            this.txtProgressLog.TabIndex = 2;
            // 
            // progressBarMerge
            // 
            this.progressBarMerge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarMerge.Location = new System.Drawing.Point(5, 115);
            this.progressBarMerge.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarMerge.Name = "progressBarMerge";
            this.progressBarMerge.Size = new System.Drawing.Size(543, 20);
            this.progressBarMerge.TabIndex = 1;
            // 
            // footerInfoPanel
            // 
            this.footerInfoPanel.Controls.Add(this.txtTotalCount);
            this.footerInfoPanel.Controls.Add(this.txtErrorCount);
            this.footerInfoPanel.Controls.Add(this.lblProgress);
            this.footerInfoPanel.Controls.Add(this.lblReport);
            this.footerInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.footerInfoPanel.Location = new System.Drawing.Point(5, 5);
            this.footerInfoPanel.Margin = new System.Windows.Forms.Padding(2);
            this.footerInfoPanel.Name = "footerInfoPanel";
            this.footerInfoPanel.Size = new System.Drawing.Size(543, 40);
            this.footerInfoPanel.TabIndex = 0;
            // 
            // txtTotalCount
            // 
            this.txtTotalCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtTotalCount.Location = new System.Drawing.Point(480, 5);
            this.txtTotalCount.Margin = new System.Windows.Forms.Padding(2);
            this.txtTotalCount.Name = "txtTotalCount";
            this.txtTotalCount.ReadOnly = true;
            this.txtTotalCount.Size = new System.Drawing.Size(50, 26);
            this.txtTotalCount.TabIndex = 4;
            // 
            // txtErrorCount
            // 
            this.txtErrorCount.BackColor = System.Drawing.Color.LightCoral;
            this.txtErrorCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtErrorCount.Location = new System.Drawing.Point(420, 5);
            this.txtErrorCount.Margin = new System.Windows.Forms.Padding(2);
            this.txtErrorCount.Name = "txtErrorCount";
            this.txtErrorCount.ReadOnly = true;
            this.txtErrorCount.Size = new System.Drawing.Size(50, 26);
            this.txtErrorCount.TabIndex = 3;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(5, 23);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 2;
            // 
            // lblReport
            // 
            this.lblReport.AutoSize = true;
            this.lblReport.Location = new System.Drawing.Point(5, 5);
            this.lblReport.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(0, 13);
            this.lblReport.TabIndex = 1;
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStripMenu);
            this.Controls.Add(this.footerPanel);
            this.splitContainerMain.Panel1.Controls.Add(this.grpEntities);
            this.splitContainerMain.Panel2.Controls.Add(this.grpExcel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(553, 500);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.grpEntities.ResumeLayout(false);
            this.grpEntities.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).EndInit();
            this.tableStateStatus.ResumeLayout(false);
            this.tableStateStatus.PerformLayout();
            this.grpExcel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).EndInit();
            this.panelExcelPath.ResumeLayout(false);
            this.panelExcelPath.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.footerPanel.PerformLayout();
            this.footerInfoPanel.ResumeLayout(false);
            this.footerInfoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

