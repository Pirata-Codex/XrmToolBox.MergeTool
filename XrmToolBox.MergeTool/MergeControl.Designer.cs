namespace XrmToolBox.MergeTool
{
    partial class MergeControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtLogicalName;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnSelectFile;

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
            this.SuspendLayout();
            // 
            // txtLogicalName
            // 
            this.txtLogicalName.Location = new System.Drawing.Point(12, 12);
            this.txtLogicalName.Name = "txtLogicalName";
            this.txtLogicalName.Size = new System.Drawing.Size(260, 20);
            this.txtLogicalName.TabIndex = 0;
            this.txtLogicalName.Text = "Logical Name"; // Changed from PlaceholderText to Text
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(12, 38);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(260, 20);
            this.txtFilePath.TabIndex = 1;
            this.txtFilePath.Text = "Excel File Path"; // Changed from PlaceholderText to Text
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
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(12, 64);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(341, 23);
            this.btnMerge.TabIndex = 3;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // MergeControl
            // 
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.txtLogicalName);
            this.Name = "MergeControl";
            this.Size = new System.Drawing.Size(365, 100);
            this.ResumeLayout(false);
            this.PerformLayout();
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
