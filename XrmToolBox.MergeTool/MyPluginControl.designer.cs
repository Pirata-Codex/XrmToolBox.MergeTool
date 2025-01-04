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
        private System.Windows.Forms.DataGridView dataGridViewEntities;
        private System.Windows.Forms.DataGridView dataGridViewExcel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.TextBox txtErrorCount;
        private System.Windows.Forms.TextBox txtTotalCount;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.ToolStripButton btnHelp;




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
            this.dataGridViewEntities = new System.Windows.Forms.DataGridView();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewExcel = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblReport = new System.Windows.Forms.Label();
            this.txtErrorCount = new System.Windows.Forms.TextBox();
            this.txtTotalCount = new System.Windows.Forms.TextBox();
            this.footerPanel = new System.Windows.Forms.Panel();

            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).BeginInit();
            this.footerPanel.SuspendLayout();
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
            // btnHelp
            // 
            this.btnHelp.Image = global::XrmToolBox.MergeTool.Properties.Resources.help_icon; // Add an appropriate image
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(53, 28);
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
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
            // dataGridViewEntities
            // 
            this.dataGridViewEntities.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEntities.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Select});
            this.dataGridViewEntities.Enabled = false;
            this.dataGridViewEntities.Location = new System.Drawing.Point(8, 63);
            this.dataGridViewEntities.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewEntities.Name = "dataGridViewEntities";
            this.dataGridViewEntities.RowHeadersWidth = 62;
            this.dataGridViewEntities.RowTemplate.Height = 28;
            this.dataGridViewEntities.Size = new System.Drawing.Size(267, 260);
            this.dataGridViewEntities.TabIndex = 1;
            this.dataGridViewEntities.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewEntities_CellContentClick);
            // 
            // Select
            // 
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            // 
            // dataGridViewExcel
            // 
            this.dataGridViewExcel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExcel.Enabled = false;
            this.dataGridViewExcel.Location = new System.Drawing.Point(279, 63);
            this.dataGridViewExcel.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewExcel.Name = "dataGridViewExcel";
            this.dataGridViewExcel.RowHeadersWidth = 62;
            this.dataGridViewExcel.RowTemplate.Height = 28;
            this.dataGridViewExcel.Size = new System.Drawing.Size(267, 260);
            this.dataGridViewExcel.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Enabled = false;
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Location = new System.Drawing.Point(8, 42);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(135, 20);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.Text = "Search...";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.GotFocus += new System.EventHandler(this.RemoveText);
            this.txtSearch.LostFocus += new System.EventHandler(this.AddText);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(279, 42);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(268, 20);
            this.txtFilePath.TabIndex = 4;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(2, 19);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 6;
            // 
            // lblReport
            // 
            this.lblReport.AutoSize = true;
            this.lblReport.Location = new System.Drawing.Point(0, 6);
            this.lblReport.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(0, 13);
            this.lblReport.TabIndex = 7;
            // 
            // txtErrorCount
            // 
            this.txtErrorCount.BackColor = System.Drawing.Color.LightCoral;
            this.txtErrorCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtErrorCount.Location = new System.Drawing.Point(300, 2);
            this.txtErrorCount.Margin = new System.Windows.Forms.Padding(2);
            this.txtErrorCount.Name = "txtErrorCount";
            this.txtErrorCount.ReadOnly = true;
            this.txtErrorCount.Size = new System.Drawing.Size(50, 26);
            this.txtErrorCount.TabIndex = 8;
            // 
            // txtTotalCount
            // 
            this.txtTotalCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtTotalCount.Location = new System.Drawing.Point(365, 2);
            this.txtTotalCount.Margin = new System.Windows.Forms.Padding(2);
            this.txtTotalCount.Name = "txtTotalCount";
            this.txtTotalCount.ReadOnly = true;
            this.txtTotalCount.Size = new System.Drawing.Size(50, 26);
            this.txtTotalCount.TabIndex = 9;
            // 
            // footerPanel
            // 
            this.footerPanel.Controls.Add(this.txtErrorCount);
            this.footerPanel.Controls.Add(this.txtTotalCount);
            this.footerPanel.Controls.Add(this.lblProgress);
            this.footerPanel.Controls.Add(this.lblReport);
            this.footerPanel.Location = new System.Drawing.Point(8, 327);
            this.footerPanel.Margin = new System.Windows.Forms.Padding(2);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Size = new System.Drawing.Size(533, 67);
            this.footerPanel.TabIndex = 10;
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dataGridViewExcel);
            this.Controls.Add(this.dataGridViewEntities);
            this.Controls.Add(this.toolStripMenu);
            this.Controls.Add(this.footerPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(553, 396);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.Resize += new System.EventHandler(this.MyPluginControl_Resize);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).EndInit();
            this.footerPanel.ResumeLayout(false);
            this.footerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MyPluginControl_Resize(object sender, EventArgs e)
        {
            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            dataGridViewEntities.Width = (width / 2) - 20;
            dataGridViewEntities.Height = height - footerPanel.Height - 50;

            dataGridViewExcel.Width = (width / 2) - 20;
            dataGridViewExcel.Height = height - footerPanel.Height - 50;

            dataGridViewExcel.Left = dataGridViewEntities.Right + 10;

            txtFilePath.Width = dataGridViewExcel.Width;
            txtFilePath.Top = dataGridViewExcel.Top - 35;

            footerPanel.Top = height - (footerPanel.Height / 2);
            footerPanel.Width = width - 20;

            // Distribute footer panel controls evenly
            int footerWidth = footerPanel.Width;
            int controlWidth = footerWidth / 4;
        }

        private void dataGridViewEntities_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) // Assuming the "Select" column is at index 0
            {
                foreach (DataGridViewRow row in dataGridViewEntities.Rows)
                {
                    if (row.Index != e.RowIndex)
                    {
                        DataGridViewCheckBoxCell checkBox = (DataGridViewCheckBoxCell)row.Cells[0];
                        checkBox.Value = false;
                    }
                }
            }
        }

        private DataGridViewCheckBoxColumn Select;
    }
}
