using Dynamics365.Merge;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using OfficeOpenXml;
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
using XrmToolBox.Extensibility;

namespace XrmToolBox.MergeTool
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;
        private DataTable entitiesTable;
        private DataTable excelTable;
        private List<string> errorRows;
        private int _startingrow = 1;

        public MyPluginControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            //ShowInfoNotification("This is my first tool published on XrmToolBox. Feel free to send me your feedback", new Uri("mailto:r.xaleghi@gmail.com"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void btnLoadEntities_Click(object sender, EventArgs e)
        {
            if (Service == null)
            {
                MessageBox.Show("Please connect to an organization service first.", "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Enable the DataGridView for editing after loading entities

            LoadEntities();
        }

        private void LoadEntities()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading entities...",
                Work = (worker, args) =>
                {
                    var request = new RetrieveAllEntitiesRequest
                    {
                        EntityFilters = EntityFilters.Entity,
                        RetrieveAsIfPublished = true
                    };
                    var response = (RetrieveAllEntitiesResponse)Service.Execute(request);
                    args.Result = response.EntityMetadata.OrderBy(e => e.DisplayName.UserLocalizedLabel?.Label ?? e.LogicalName).ToList();
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var entities = args.Result as List<EntityMetadata>;
                        if (entities != null)
                        {
                            entitiesTable = new DataTable();
                            entitiesTable.Columns.Add("Name");
                            entitiesTable.Columns.Add("Logical Name");

                            foreach (var entity in entities)
                            {
                                var row = entitiesTable.NewRow();
                                row["Name"] = entity.DisplayName.UserLocalizedLabel?.Label ?? entity.LogicalName;
                                row["Logical Name"] = entity.LogicalName;
                                entitiesTable.Rows.Add(row);
                            }

                            dataGridViewEntities.DataSource = entitiesTable;
                            txtSearch.Enabled = true;
                            btnLoadExcel.Enabled = true;
                            dataGridViewEntities.ReadOnly = false;
                            dataGridViewEntities.Enabled = true;
                        }
                    }
                }
            });
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (entitiesTable == null) return;

            // Remove rows with empty "Logical Name" values
            var rowsToRemove = entitiesTable.AsEnumerable()
                .Where(row => string.IsNullOrEmpty(row.Field<string>("Logical Name")))
                .ToList();

            foreach (var row in rowsToRemove)
            {
                entitiesTable.Rows.Remove(row);
            }

            var searchText = txtSearch.Text.ToLower();
            var filteredRows = entitiesTable.AsEnumerable()
                .Where(row => row.Field<string>("Name").ToLower().Contains(searchText) || row.Field<string>("Logical Name").ToLower().Contains(searchText));

            if (filteredRows.Any())
            {
                dataGridViewEntities.DataSource = filteredRows.CopyToDataTable();
            }
            else
            {
                dataGridViewEntities.DataSource = entitiesTable.Clone(); // Clear the DataGridView if no rows are found
            }
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search...";
                txtSearch.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                    btnValidateExcel.Enabled = true;
                }
            }
        }
        private HashSet<Guid> CheckEntityExistence(HashSet<Guid> guids)
        {
            var logicalName = EntityLogicalName();
            if (logicalName == null)
            {
                return new HashSet<Guid>();
            }

            var query = new QueryExpression(logicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions =
            {
                new ConditionExpression($"{logicalName}id", ConditionOperator.In, guids.ToArray())
            }
                }
            };

            var entities = Service.RetrieveMultiple(query).Entities;
            return new HashSet<Guid>(entities.Select(e => e.Id));
        }
        private void btnValidateExcel_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid Excel file.");
                return;
            }

            if (EntityLogicalName() == null)
            {
                MessageBox.Show("Please select an entity.");
                return;
            }

            excelTable = new DataTable();
            errorRows = new List<string>();
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Validating Excel file...",
                Work = (worker, args) =>
                {
                    worker.WorkerReportsProgress = true;
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        excelTable.Columns.Add("SourceId");
                        excelTable.Columns.Add("TargetId");
                        excelTable.Columns.Add("Status");

                        var guidsToCheck = new HashSet<Guid>();

                        for (int row = _startingrow; row <= rowCount; row++)
                        {
                            var sourceId = worksheet.Cells[row, 1].Text;
                            var targetId = worksheet.Cells[row, 2].Text;

                            if (Guid.TryParse(sourceId, out var sourceGuid))
                            {
                                guidsToCheck.Add(sourceGuid);
                            }
                            if (Guid.TryParse(targetId, out var targetGuid))
                            {
                                guidsToCheck.Add(targetGuid);
                            }
                        }

                        var existingGuids = CheckEntityExistence(guidsToCheck);

                        for (int row = _startingrow; row <= rowCount; row++)
                        {
                            var sourceId = worksheet.Cells[row, 1].Text;
                            var targetId = worksheet.Cells[row, 2].Text;

                            var rowToAdd = excelTable.NewRow();
                            rowToAdd["SourceId"] = sourceId;
                            rowToAdd["TargetId"] = targetId;
                            rowToAdd["Status"] = "Invalid";

                            if (Guid.TryParse(sourceId, out var sourceGuid) && Guid.TryParse(targetId, out var targetGuid))
                            {
                                if (existingGuids.Contains(sourceGuid) && existingGuids.Contains(targetGuid))
                                {
                                    rowToAdd["Status"] = "Valid";
                                }
                                else
                                {
                                    rowToAdd["Status"] = "Invalid";
                                    errorRows.Add($"Row {row}: Record does not exist.");
                                }
                            }
                            else
                            {
                                rowToAdd["Status"] = "Invalid";
                                errorRows.Add($"Row {row}: Invalid GUID format.");
                            }

                            excelTable.Rows.Add(rowToAdd);
                            worker.ReportProgress((row) * 100 / (rowCount), $"Validating row {row} of {rowCount}");
                        }
                    }
                    args.Result = excelTable;
                },
                ProgressChanged = (args) =>
                {
                    lblProgress.Text = args.UserState.ToString();
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        dataGridViewExcel.DataSource = args.Result;
                        HighlightRows();
                        ShowReport();
                        btnMerge.Enabled = true;
                    }
                }
            });
        }

        private string EntityLogicalName()
        {
            foreach (DataGridViewRow row in dataGridViewEntities.Rows)
            {
                var cell = row.Cells["Select"] as DataGridViewCheckBoxCell;
                if (cell != null && cell.Selected)
                {
                    return row.Cells["Logical Name"].Value.ToString();
                }
            }

            return null;
        }

        private bool EntityExists(string id)
        {

            var logicalName = EntityLogicalName();
            if (logicalName == null)
            {
                return false;
            }
            QueryExpression query = new QueryExpression(logicalName);
            query.Criteria.AddCondition($"{logicalName}id", ConditionOperator.Equal, new Guid(id));
            query.NoLock = true;
            var entity = Service.RetrieveMultiple(query);
            return entity.Entities.Count == 1;
        }

        private void HighlightRows()
        {
            foreach (DataGridViewRow row in dataGridViewExcel.Rows)
            {
                try
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
                catch (Exception)
                {
                    continue;
                    throw;
                }
                
            }
        }

        private void ShowReport()
        {
            int totalRows = excelTable.Rows.Count;
            int errorRowsCount = errorRows.Count;

            lblReport.Text = $"Total Rows: {totalRows}, Rows with Errors: {errorRowsCount}";
            txtTotalCount.Text = totalRows.ToString();
            txtErrorCount.Text = errorRowsCount.ToString();
        }

        private void GenerateErrorReport()
        {
            var result = MessageBox.Show("Do you want to save the error report?", "Save Error Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Error Report");

                    worksheet.Cells[1, 1].Value = "Row No.";
                    worksheet.Cells[1, 2].Value = "Error Message";

                    for (int i = 0; i < errorRows.Count; i++)
                    {
                        var errorRow = errorRows[i].Split(':');
                        worksheet.Cells[i + 2, 1].Value = errorRow[0]; // Original row number
                        worksheet.Cells[i + 2, 2].Value = errorRow[1]; // Error message
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
        private void btnHelp_Click(object sender, EventArgs e)
        {
            string tutorialMessage = "Tutorial on how to use the tool:\n\n" +
                                     "1. Load Entities: Click to load the entities.\n" +
                                     "2. Load Excel: Click to load the Excel file.\n" +
                                     "3. Validate Excel: Click to validate the loaded Excel data.\n" +
                                     "4. Merge: Click to merge the data.\n" +
                                     "Use the search box to filter entities.\n" +
                                     "Check the progress and report in the footer panel.\n\n" +
                                     "Standard Excel Format (Add no header or table):\n" +
                                     "1. Column A: source GUID\n" +
                                     "2. Column B: mergeTo GUID\n" +
                                     "Press Ok to see an example";
            MessageBox.Show(tutorialMessage, "Help - Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Save the image from resources to a temporary file and open it
            string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "tutorial_image.png");
            try
            {
                using (var image = Properties.Resources.help) // Replace with your actual resource name
                {
                    image.Save(tempFilePath);
                }
                System.Diagnostics.Process.Start(tempFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure to start the merge process? Note that log for every row will be reported.", "Merge Process", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {


                if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
                {
                    MessageBox.Show("Please select a valid Excel file.");
                    return;
                }

                var logicalName = EntityLogicalName();
                if (logicalName == null)
                {
                    return;
                }

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Merging records...",
                    Work = (worker, args) =>
                    {
                        worker.WorkerReportsProgress = true;
                        using (var package = new ExcelPackage(new FileInfo(txtFilePath.Text)))
                        {
                            var worksheet = package.Workbook.Worksheets[0];
                            int rowCount = worksheet.Dimension.Rows;
                            args.Result = rowCount;
                            for (int row = _startingrow; row <= rowCount; row++)
                            {
                                string sourceId = worksheet.Cells[row, 1].Text;
                                string targetId = worksheet.Cells[row, 2].Text;

                                // Only process rows that are marked as "Valid"
                                var status = excelTable.Rows[row - _startingrow]["Status"].ToString();
                                if (status != "Valid")
                                {
                                    continue;
                                }

                                if (string.IsNullOrEmpty(sourceId) || string.IsNullOrEmpty(targetId))
                                {
                                    continue;
                                }

                                worker.ReportProgress((row) * 100 / (rowCount), $"Processing row {row} of {rowCount}");

                                MergeRequest mergeRequest = new MergeRequest(logicalName, sourceId, targetId, Service, true);
                                mergeRequest.OnFunctionCalled += MergeRequest_OnFunctionCalled;
                                mergeRequest.DoMerge();
                            }
                        }
                    },
                    ProgressChanged = (args) =>
                    {
                        lblProgress.Text = args.UserState.ToString();
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Bulk merge completed successfully.");
                        }
                        GenerateErrorReport();
                    }
                });
            }
        }

        private void MergeRequest_OnFunctionCalled(object sender, string functionName)
        {
            lblProgress.Text = $"Calling function: {functionName}";
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

    }
}