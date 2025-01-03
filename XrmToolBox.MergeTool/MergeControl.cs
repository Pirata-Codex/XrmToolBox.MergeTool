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
using Microsoft.Xrm.Sdk.Query;

namespace XrmToolBox.MergeTool
{
    public partial class MergeControl : UserControl
    {
        private IOrganizationService orgService;
        private DataTable excelTable;
        private List<string> errorRows;

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

                        try
                        {
                            MergeRequest mergeRequest = new MergeRequest(logicalName, sourceId, targetId, orgService, true);
                            mergeRequest.DoMerge();
                        }
                        catch (Exception ex)
                        {
                            errorRows.Add($"Row {row}: {ex.Message}");
                        }
                    }
                }

                if (errorRows.Any())
                {
                    GenerateErrorReport();
                    MessageBox.Show("Bulk merge completed with errors. Please check the error report.");
                }
                else
                {
                    MessageBox.Show("Bulk merge completed successfully.");
                }
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

        private void btnValidateExcel_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid Excel file.");
                return;
            }

            lblValidationStatus.Text = "Validating...";
            excelTable = new DataTable();
            errorRows = new List<string>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                excelTable.Columns.Add("SourceId");
                excelTable.Columns.Add("TargetId");
                excelTable.Columns.Add("Status");

                for (int row = 2; row <= rowCount; row++)
                {
                    var sourceId = worksheet.Cells[row, 1].Text;
                    var targetId = worksheet.Cells[row, 2].Text;

                    var rowToAdd = excelTable.NewRow();
                    rowToAdd["SourceId"] = sourceId;
                    rowToAdd["TargetId"] = targetId;

                    if (Guid.TryParse(sourceId, out _) && Guid.TryParse(targetId, out _))
                    {
                        if (EntityExists(sourceId) && EntityExists(targetId))
                        {
                            rowToAdd["Status"] = "Valid";
                        }
                        else
                        {
                            rowToAdd["Status"] = "Invalid";
                            errorRows.Add($"Row {row}: Entity does not exist.");
                        }
                    }
                    else
                    {
                        rowToAdd["Status"] = "Invalid";
                        errorRows.Add($"Row {row}: Invalid GUID format.");
                    }

                    excelTable.Rows.Add(rowToAdd);
                }
            }

            dataGridViewExcel.DataSource = excelTable;
            HighlightRows();
            ShowReport();
            lblValidationStatus.Text = "Validation complete";
            btnMerge.Enabled = true;
        }

        private bool EntityExists(string id)
        {
            QueryExpression query = new QueryExpression(txtLogicalName.Text);
            query.Criteria.AddCondition($"{txtLogicalName.Text}id", ConditionOperator.Equal, new Guid(id));
            query.NoLock = true;
            var entity = orgService.RetrieveMultiple(query);
            return entity.Entities.Count == 1;
        }

        private void HighlightRows()
        {
            foreach (DataGridViewRow row in dataGridViewExcel.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "Valid")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void ShowReport()
        {
            int totalRows = excelTable.Rows.Count;
            int errorRowsCount = errorRows.Count;

            lblReport.Text = $"Total Rows: {totalRows}, Rows with Errors: {errorRowsCount}";
        }

        private void GenerateErrorReport()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Error Report");

                worksheet.Cells[1, 1].Value = "Row";
                worksheet.Cells[1, 2].Value = "Error";

                for (int i = 0; i < errorRows.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = errorRows[i].Split(':')[0];
                    worksheet.Cells[i + 2, 2].Value = errorRows[i].Split(':')[1];
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx;*.xls",
                    FileName = $"ErrorReport-{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    package.SaveAs(new FileInfo(saveFileDialog.FileName));
                }
            }
        }
    }
}
