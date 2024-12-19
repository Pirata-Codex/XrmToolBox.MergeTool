using System;

namespace XrmToolBox.MergeTool
{
    partial class MyPluginControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbMerge;
        private System.Windows.Forms.Panel panelMergeControl;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPluginControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbMerge = new System.Windows.Forms.ToolStripButton();
            this.panelMergeControl = new System.Windows.Forms.Panel();
            this.toolStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tsbMerge});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripMenu.Size = new System.Drawing.Size(839, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";

            // 
            // tsbMerge
            // 
            this.tsbMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMerge.Name = "tsbMerge";
            this.tsbMerge.Size = new System.Drawing.Size(28, 28);
            this.tsbMerge.Text = "Merge Records";
            this.tsbMerge.Click += new System.EventHandler(this.TsbMerge_Click);
            // 
            // panelMergeControl
            // 
            this.panelMergeControl.Location = new System.Drawing.Point(0, 31);
            this.panelMergeControl.Name = "panelMergeControl";
            this.panelMergeControl.Size = new System.Drawing.Size(839, 431);
            this.panelMergeControl.TabIndex = 5;
            this.panelMergeControl.Visible = false;
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMergeControl);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(839, 462);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
