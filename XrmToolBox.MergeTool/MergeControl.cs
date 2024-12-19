using Microsoft.Xrm.Sdk;
using OfficeOpenXml;
using Dynamics365.Merge;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XrmToolBox.MergeTool
{
    public partial class MergeControl : UserControl
    {
        private IOrganizationService orgService;

        public MergeControl(IOrganizationService service)
        {
            InitializeComponent();
            orgService = service;
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid Excel file.");
                return;
            }

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        string logicalName = txtLogicalName.Text;
                        string sourceId = worksheet.Cells[row, 1].Text;
                        string targetId = worksheet.Cells[row, 2].Text;

                        if (string.IsNullOrEmpty(sourceId) || string.IsNullOrEmpty(targetId))
                        {
                            continue;
                        }

                        MergeRequest mergeRequest = new MergeRequest(logicalName, sourceId, targetId, orgService, true);
                        mergeRequest.DoMerge();
                    }
                }

                MessageBox.Show("Bulk merge completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }
        
    }
}
