using System;
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
        private System.Windows.Forms.DataGridView dataGridViewEntities;
        private System.Windows.Forms.DataGridView dataGridViewExcel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.Label lblValidationStatus;
        private System.Windows.Forms.TextBox txtLogicalName;

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
            this.dataGridViewEntities = new System.Windows.Forms.DataGridView();
            this.dataGridViewExcel = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblReport = new System.Windows.Forms.Label();
            this.lblValidationStatus = new System.Windows.Forms.Label();
            this.txtLogicalName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).BeginInit();
            this.toolStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.btnLoadEntities,
                    this.btnLoadExcel,
                    this.btnValidateExcel,
                    this.btnMerge});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(839, 31);
            this.toolStripMenu.TabIndex = 0;
            this.toolStripMenu.Text = "toolStripMenu";
            // 
            // btnLoadEntities
            // 
            this.btnLoadEntities.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.btnLoadEntities.Image = global::XrmToolBox.MergeTool.Properties.Resources.load_entities;
            this.btnLoadEntities.Name = "btnLoadEntities";
            this.btnLoadEntities.Size = new System.Drawing.Size(100, 28);
            this.btnLoadEntities.Text = "Load Entities";
            this.btnLoadEntities.Click += new System.EventHandler(this.btnLoadEntities_Click);
            // 
            // btnLoadExcel
            // 
            this.btnLoadExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.btnLoadExcel.Image = global::XrmToolBox.MergeTool.Properties.Resources.load_excel;
            this.btnLoadExcel.Name = "btnLoadExcel";
            this.btnLoadExcel.Size = new System.Drawing.Size(100, 28);
            this.btnLoadExcel.Text = "Load Excel";
            this.btnLoadExcel.Click += new System.EventHandler(this.btnLoadExcel_Click);
            this.btnLoadExcel.Enabled = false;
            // 
            // btnValidateExcel
            // 
            this.btnValidateExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.btnValidateExcel.Image = global::XrmToolBox.MergeTool.Properties.Resources.validate_excel;
            this.btnValidateExcel.Name = "btnValidateExcel";
            this.btnValidateExcel.Size = new System.Drawing.Size(100, 28);
            this.btnValidateExcel.Text = "Validate Excel";
            this.btnValidateExcel.Click += new System.EventHandler(this.btnValidateExcel_Click);
            this.btnValidateExcel.Enabled = false;
            // 
            // btnMerge
            // 
            this.btnMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.btnMerge.Image = global::XrmToolBox.MergeTool.Properties.Resources.merge;
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(100, 28);
            this.btnMerge.Text = "Merge";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            this.btnMerge.Enabled = false;
            // 
            // dataGridViewEntities
            // 
            this.dataGridViewEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEntities.Location = new System.Drawing.Point(12, 72);
            this.dataGridViewEntities.Name = "dataGridViewEntities";
            this.dataGridViewEntities.RowHeadersWidth = 62;
            this.dataGridViewEntities.RowTemplate.Height = 28;
            this.dataGridViewEntities.Size = new System.Drawing.Size(400, 150);
            this.dataGridViewEntities.TabIndex = 1;
            this.dataGridViewEntities.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            // 
            // dataGridViewExcel
            // 
            this.dataGridViewExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExcel.Location = new System.Drawing.Point(418, 72);
            this.dataGridViewExcel.Name = "dataGridViewExcel";
            this.dataGridViewExcel.RowHeadersWidth = 62;
            this.dataGridViewExcel.RowTemplate.Height = 28;
            this.dataGridViewExcel.Size = new System.Drawing.Size(400, 150);
            this.dataGridViewExcel.TabIndex = 2;
            this.dataGridViewExcel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 26);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.Text = "Search...";
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.GotFocus += new System.EventHandler(this.RemoveText);
            this.txtSearch.LostFocus += new System.EventHandler(this.AddText);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Enabled = false;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(12, 228);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(200, 26);
            this.txtFilePath.TabIndex = 4;
            this.txtFilePath.ReadOnly = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 260);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(815, 23);
            this.progressBar.TabIndex = 5;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(12, 286);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 20);
            this.lblProgress.TabIndex = 6;
            // 
            // lblReport
            // 
            this.lblReport.AutoSize = true;
            this.lblReport.Location = new System.Drawing.Point(12, 310);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(0, 20);
            this.lblReport.TabIndex = 7;
            // 
            // lblValidationStatus
            // 
            this.lblValidationStatus.AutoSize = true;
            this.lblValidationStatus.Location = new System.Drawing.Point(12, 334);
            this.lblValidationStatus.Name = "lblValidationStatus";
            this.lblValidationStatus.Size = new System.Drawing.Size(0, 20);
            this.lblValidationStatus.TabIndex = 8;
            // 
            // txtLogicalName
            // 
            this.txtLogicalName.Location = new System.Drawing.Point(12, 12);
            this.txtLogicalName.Name = "txtLogicalName";
            this.txtLogicalName.Size = new System.Drawing.Size(260, 26);
            this.txtLogicalName.TabIndex = 9;
            this.txtLogicalName.Text = "Logical Name";
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtLogicalName);
            this.Controls.Add(this.lblValidationStatus);
            this.Controls.Add(this.lblReport);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dataGridViewExcel);
            this.Controls.Add(this.dataGridViewEntities);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(830, 360);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).EndInit();
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
