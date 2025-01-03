namespace XrmToolBox.MergeTool
{
    partial class MergeControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtLogicalName;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnValidateExcel;
        private System.Windows.Forms.DataGridView dataGridViewEntities;
        private System.Windows.Forms.DataGridView dataGridViewExcel;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.Label lblValidationStatus;

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
            this.txtLogicalName = new System.Windows.Forms.TextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnValidateExcel = new System.Windows.Forms.Button();
            this.dataGridViewEntities = new System.Windows.Forms.DataGridView();
            this.dataGridViewExcel = new System.Windows.Forms.DataGridView();
            this.lblReport = new System.Windows.Forms.Label();
            this.lblValidationStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLogicalName
            // 
            this.txtLogicalName.Location = new System.Drawing.Point(12, 12);
            this.txtLogicalName.Name = "txtLogicalName";
            this.txtLogicalName.Size = new System.Drawing.Size(260, 20);
            this.txtLogicalName.TabIndex = 0;
            this.txtLogicalName.Text = "Logical Name";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(12, 38);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(260, 20);
            this.txtFilePath.TabIndex = 1;
            this.txtFilePath.Text = "Excel File Path";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(278, 36);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnValidateExcel
            // 
            this.btnValidateExcel.Location = new System.Drawing.Point(12, 64);
            this.btnValidateExcel.Name = "btnValidateExcel";
            this.btnValidateExcel.Size = new System.Drawing.Size(341, 23);
            this.btnValidateExcel.TabIndex = 3;
            this.btnValidateExcel.Text = "Validate Excel File";
            this.btnValidateExcel.UseVisualStyleBackColor = true;
            this.btnValidateExcel.Click += new System.EventHandler(this.btnValidateExcel_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(12, 93);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(341, 23);
            this.btnMerge.TabIndex = 4;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            this.btnMerge.Enabled = false;
            // 
            // dataGridViewEntities
            // 
            this.dataGridViewEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEntities.Location = new System.Drawing.Point(12, 122);
            this.dataGridViewEntities.Name = "dataGridViewEntities";
            this.dataGridViewEntities.RowHeadersWidth = 62;
            this.dataGridViewEntities.RowTemplate.Height = 28;
            this.dataGridViewEntities.Size = new System.Drawing.Size(400, 150);
            this.dataGridViewEntities.TabIndex = 5;
            this.dataGridViewEntities.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            // 
            // dataGridViewExcel
            // 
            this.dataGridViewExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExcel.Location = new System.Drawing.Point(418, 122);
            this.dataGridViewExcel.Name = "dataGridViewExcel";
            this.dataGridViewExcel.RowHeadersWidth = 62;
            this.dataGridViewExcel.RowTemplate.Height = 28;
            this.dataGridViewExcel.Size = new System.Drawing.Size(400, 150);
            this.dataGridViewExcel.TabIndex = 6;
            this.dataGridViewExcel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            // 
            // lblReport
            // 
            this.lblReport.AutoSize = true;
            this.lblReport.Location = new System.Drawing.Point(12, 275);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(0, 13);
            this.lblReport.TabIndex = 7;
            // 
            // lblValidationStatus
            // 
            this.lblValidationStatus.AutoSize = true;
            this.lblValidationStatus.Location = new System.Drawing.Point(12, 300);
            this.lblValidationStatus.Name = "lblValidationStatus";
            this.lblValidationStatus.Size = new System.Drawing.Size(0, 13);
            this.lblValidationStatus.TabIndex = 8;
            // 
            // MergeControl
            // 
            this.Controls.Add(this.lblValidationStatus);
            this.Controls.Add(this.lblReport);
            this.Controls.Add(this.dataGridViewExcel);
            this.Controls.Add(this.dataGridViewEntities);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.btnValidateExcel);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.txtLogicalName);
            this.Name = "MergeControl";
            this.Size = new System.Drawing.Size(830, 320);
            this.ResumeLayout(false);
            this.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExcel)).EndInit();
        }

        private void RemoveText(object sender)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            if (textBox.Text == "Logical Name" || textBox.Text == "Excel File Path")
            {
                textBox.Text = "";
            }
        }

        private void AddText(object sender)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox == txtLogicalName)
                {
                    textBox.Text = "Logical Name";
                }
                else if (textBox == txtFilePath)
                {
                    textBox.Text = "Excel File Path";
                }
            }
        }
    }
}
