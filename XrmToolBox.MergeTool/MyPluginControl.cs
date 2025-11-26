using Dynamics365.Merge;
using Dynamics365.Merge.Common;
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
        private List<MergeError> mergeErrors = new List<MergeError>();
        private readonly Dictionary<string, Dictionary<int, string>> optionSetLabelCache = new Dictionary<string, Dictionary<int, string>>();
        private readonly Dictionary<string, StateStatusInfo> recordStateCache = new Dictionary<string, StateStatusInfo>();
        private string currentEntityLogicalName;
        private ConnectionDetail connectionDetail;

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
            ExecuteMethod(LoadEntities);
        }

        private void LoadEntities()
        {
            if (Service == null)
            {
                MessageBox.Show("Please connect to an organization service first.", "No Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                            dataGridViewEntities.ReadOnly = true;
                            dataGridViewEntities.Enabled = true;
                            if (dataGridViewEntities.Rows.Count > 0)
                            {
                                dataGridViewEntities.Rows[0].Selected = true;
                                dataGridViewEntities.CurrentCell = dataGridViewEntities.Rows[0].Cells["Name"];
                            }
                            LoadEntityStateStatusPicklists(EntityLogicalName());
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
                if (dataGridViewEntities.Rows.Count > 0)
                {
                    dataGridViewEntities.Rows[0].Selected = true;
                    dataGridViewEntities.CurrentCell = dataGridViewEntities.Rows[0].Cells["Name"];
                }
            }
            else
            {
                dataGridViewEntities.DataSource = entitiesTable.Clone(); // Clear the DataGridView if no rows are found
            }

            LoadEntityStateStatusPicklists(EntityLogicalName());
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
            var proceed = MessageBox.Show(
                "Make sure the workbook you want to use is closed in Excel before continuing.\n\nDo you want to select an Excel file now?",
                "Load Excel File",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (proceed != DialogResult.Yes)
            {
                return;
            }

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
            recordStateCache.Clear();
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
                        excelTable.Columns.Add("SourceState");
                        excelTable.Columns.Add("SourceStatus");
                        excelTable.Columns.Add("TargetState");
                        excelTable.Columns.Add("TargetStatus");

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
                                    PopulateStateStatusColumns(rowToAdd, sourceGuid, targetGuid);
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
                        SetupCrmLinks();
                        HighlightRows();
                        ShowReport();
                        btnMerge.Enabled = true;
                    }
                }
            });
        }

        private string EntityLogicalName()
        {
            if (dataGridViewEntities.CurrentRow != null && dataGridViewEntities.CurrentRow.Cells["Logical Name"] != null)
            {
                var value = dataGridViewEntities.CurrentRow.Cells["Logical Name"].Value;
                return value?.ToString();
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

        private void AppendProgressLog(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (txtProgressLog.TextLength > 4000)
            {
                txtProgressLog.Clear();
            }

            txtProgressLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void UpdateProgressUI(MergeProgressDetail detail, int percentage)
        {
            if (detail == null)
            {
                return;
            }

            lblProgress.Text = detail.Message;
            if (percentage >= 0 && percentage <= 100)
            {
                progressBarMerge.Value = percentage;
            }
            AppendProgressLog($"{detail.Stage}: {detail.Message}");
        }

        private string GetOptionSetLabel(string entityLogicalName, string attributeLogicalName, int optionValue)
        {
            var labels = GetOrLoadOptionSetLabels(entityLogicalName, attributeLogicalName);
            return labels.TryGetValue(optionValue, out var label) ? label : optionValue.ToString();
        }

        private Dictionary<int, string> GetOrLoadOptionSetLabels(string entityLogicalName, string attributeLogicalName)
        {
            if (Service == null || string.IsNullOrEmpty(entityLogicalName) || string.IsNullOrEmpty(attributeLogicalName))
            {
                return new Dictionary<int, string>();
            }

            string cacheKey = $"{entityLogicalName}:{attributeLogicalName}";
            if (!optionSetLabelCache.TryGetValue(cacheKey, out var labels))
            {
                try
                {
                    var request = new RetrieveAttributeRequest
                    {
                        EntityLogicalName = entityLogicalName,
                        LogicalName = attributeLogicalName,
                        RetrieveAsIfPublished = true
                    };

                    var response = (RetrieveAttributeResponse)Service.Execute(request);

                    if (response.AttributeMetadata is StateAttributeMetadata stateMetadata)
                    {
                        labels = stateMetadata.OptionSet.Options
                            .Where(o => o.Value.HasValue)
                            .ToDictionary(o => o.Value.Value, o => o.Label.UserLocalizedLabel?.Label ?? o.Value.Value.ToString());
                    }
                    else if (response.AttributeMetadata is StatusAttributeMetadata statusMetadata)
                    {
                        labels = statusMetadata.OptionSet.Options
                            .Where(o => o.Value.HasValue)
                            .ToDictionary(o => o.Value.Value, o => o.Label.UserLocalizedLabel?.Label ?? o.Value.Value.ToString());
                    }
                    else if (response.AttributeMetadata is PicklistAttributeMetadata picklistMetadata)
                    {
                        labels = picklistMetadata.OptionSet.Options
                            .Where(o => o.Value.HasValue)
                            .ToDictionary(o => o.Value.Value, o => o.Label.UserLocalizedLabel?.Label ?? o.Value.Value.ToString());
                    }
                    else
                    {
                        labels = new Dictionary<int, string>();
                    }

                    optionSetLabelCache[cacheKey] = labels;
                }
                catch (Exception ex)
                {
                    AppendProgressLog($"Unable to load option labels for {attributeLogicalName}: {ex.Message}");
                    labels = new Dictionary<int, string>();
                    optionSetLabelCache[cacheKey] = labels;
                }
            }

            return labels ?? new Dictionary<int, string>();
        }

        private List<OptionItem> GetOptionItemsForAttribute(string entityLogicalName, string attributeLogicalName)
        {
            var labels = GetOrLoadOptionSetLabels(entityLogicalName, attributeLogicalName);
            return labels
                .Select(kvp => new OptionItem
                {
                    Value = kvp.Key,
                    Label = string.IsNullOrEmpty(kvp.Value) ? kvp.Key.ToString() : $"{kvp.Value} ({kvp.Key})"
                })
                .OrderBy(item => item.Label)
                .ToList();
        }

        private StateStatusInfo GetRecordStateStatus(Guid recordId)
        {
            var logicalName = EntityLogicalName();
            if (logicalName == null)
            {
                return new StateStatusInfo();
            }

            var cacheKey = $"{logicalName}:{recordId}";
            if (recordStateCache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            try
            {
                var entity = Service.Retrieve(logicalName, recordId, new ColumnSet("statecode", "statuscode"));
                string stateLabel = string.Empty;
                string statusLabel = string.Empty;

                var stateOption = entity.GetAttributeValue<OptionSetValue>("statecode");
                if (stateOption != null)
                {
                    var label = GetOptionSetLabel(logicalName, "statecode", stateOption.Value);
                    stateLabel = string.IsNullOrEmpty(label) ? stateOption.Value.ToString() : $"{label} ({stateOption.Value})";
                }

                var statusOption = entity.GetAttributeValue<OptionSetValue>("statuscode");
                if (statusOption != null)
                {
                    var label = GetOptionSetLabel(logicalName, "statuscode", statusOption.Value);
                    statusLabel = string.IsNullOrEmpty(label) ? statusOption.Value.ToString() : $"{label} ({statusOption.Value})";
                }

                var info = new StateStatusInfo
                {
                    StateLabel = stateLabel,
                    StatusLabel = statusLabel
                };

                recordStateCache[cacheKey] = info;
                return info;
            }
            catch (Exception ex)
            {
                AppendProgressLog($"Unable to retrieve state/status for record {recordId}: {ex.Message}");
                return new StateStatusInfo();
            }
        }

        private void PopulateStateStatusColumns(DataRow rowToAdd, Guid sourceGuid, Guid targetGuid)
        {
            var sourceInfo = GetRecordStateStatus(sourceGuid);
            rowToAdd["SourceState"] = sourceInfo.StateLabel;
            rowToAdd["SourceStatus"] = sourceInfo.StatusLabel;

            var targetInfo = GetRecordStateStatus(targetGuid);
            rowToAdd["TargetState"] = targetInfo.StateLabel;
            rowToAdd["TargetStatus"] = targetInfo.StatusLabel;
        }

        private void LoadEntityStateStatusPicklists(string logicalName)
        {
            if (string.IsNullOrEmpty(logicalName))
            {
                cmbStateOptions.DataSource = null;
                cmbStatusOptions.DataSource = null;
                cmbStateOptions.Enabled = false;
                cmbStatusOptions.Enabled = false;
                currentEntityLogicalName = null;
                recordStateCache.Clear();
                return;
            }

            if (!string.Equals(currentEntityLogicalName, logicalName, StringComparison.OrdinalIgnoreCase))
            {
                currentEntityLogicalName = logicalName;
                recordStateCache.Clear();
            }

            var stateItems = GetOptionItemsForAttribute(logicalName, "statecode");
            cmbStateOptions.DataSource = stateItems;
            cmbStateOptions.DisplayMember = "Label";
            cmbStateOptions.ValueMember = "Value";
            cmbStateOptions.Enabled = stateItems.Any();

            var statusItems = GetOptionItemsForAttribute(logicalName, "statuscode");
            cmbStatusOptions.DataSource = statusItems;
            cmbStatusOptions.DisplayMember = "Label";
            cmbStatusOptions.ValueMember = "Value";
            cmbStatusOptions.Enabled = statusItems.Any();
        }

        private void GenerateErrorReport()
        {
            if (!errorRows.Any() && !mergeErrors.Any())
            {
                MessageBox.Show("There are no errors to include in the report.", "No Errors", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Do you want to save the error report?", "Save Error Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (var package = new ExcelPackage())
                {
                    if (errorRows.Any())
                    {
                        var validationSheet = package.Workbook.Worksheets.Add("Validation Errors");
                        validationSheet.Cells[1, 1].Value = "Row No.";
                        validationSheet.Cells[1, 2].Value = "Error Message";

                        for (int i = 0; i < errorRows.Count; i++)
                        {
                            var errorRow = errorRows[i].Split(':');
                            validationSheet.Cells[i + 2, 1].Value = errorRow[0]; // Original row number
                            validationSheet.Cells[i + 2, 2].Value = errorRow[1]; // Error message
                        }
                    }

                    if (mergeErrors.Any())
                    {
                        var mergeSheet = package.Workbook.Worksheets.Add("Merge Errors");
                        mergeSheet.Cells[1, 1].Value = "Row No.";
                        mergeSheet.Cells[1, 2].Value = "Source Id";
                        mergeSheet.Cells[1, 3].Value = "Target Id";
                        mergeSheet.Cells[1, 4].Value = "Stage";
                        mergeSheet.Cells[1, 5].Value = "Operation";
                        mergeSheet.Cells[1, 6].Value = "Relationship";
                        mergeSheet.Cells[1, 7].Value = "Related Record";
                        mergeSheet.Cells[1, 8].Value = "Message";
                        mergeSheet.Cells[1, 9].Value = "Details";

                        for (int i = 0; i < mergeErrors.Count; i++)
                        {
                            var error = mergeErrors[i];
                            mergeSheet.Cells[i + 2, 1].Value = error.RowNumber;
                            mergeSheet.Cells[i + 2, 2].Value = error.SourceId;
                            mergeSheet.Cells[i + 2, 3].Value = error.TargetId;
                            mergeSheet.Cells[i + 2, 4].Value = error.Stage;
                            mergeSheet.Cells[i + 2, 5].Value = error.Operation;
                            mergeSheet.Cells[i + 2, 6].Value = error.RelationshipSchemaName;
                            mergeSheet.Cells[i + 2, 7].Value = error.RelatedRecordId?.ToString();
                            mergeSheet.Cells[i + 2, 8].Value = error.Message;
                            mergeSheet.Cells[i + 2, 9].Value = error.Details;
                        }
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
                var reminder = MessageBox.Show("Reminder: the tool cannot bypass Dynamics 365 processes, workflows, or plugins that may block a merge. Do you still want to continue?", "Processes May Block Merge", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (reminder != DialogResult.Yes)
                {
                    return;
                }

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

                // Capture selected state/status values before async work (thread-safe)
                int? selectedState = null;
                int? selectedStatus = null;

                if (cmbStateOptions.SelectedValue != null && cmbStateOptions.SelectedValue is int stateValue)
                {
                    selectedState = stateValue;
                }

                if (cmbStatusOptions.SelectedValue != null && cmbStatusOptions.SelectedValue is int statusValue)
                {
                    selectedStatus = statusValue;
                }

                mergeErrors = new List<MergeError>();
                txtProgressLog.Clear();
                progressBarMerge.Value = 0;

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

                                int progressPercent = (row) * 100 / (rowCount);
                                worker.ReportProgress(progressPercent, $"Processing row {row} of {rowCount}");

                                MergeRequest mergeRequest = new MergeRequest(logicalName, sourceId, targetId, Service, true, row);
                                mergeRequest.OnFunctionCalled += MergeRequest_OnFunctionCalled;
                                EventHandler<MergeProgressDetail> progressHandler = (s, detail) =>
                                {
                                    worker.ReportProgress(progressPercent, detail);
                                };
                                mergeRequest.OnProgressChanged += progressHandler;
                                mergeRequest.DoMerge();
                                mergeRequest.OnProgressChanged -= progressHandler;

                                // Update source record status if user has selected state/status
                                if (Guid.TryParse(sourceId, out var sourceGuid) && (selectedState.HasValue || selectedStatus.HasValue))
                                {
                                    try
                                    {
                                        UpdateSourceRecordStatus(logicalName, sourceGuid, selectedState, selectedStatus);
                                        worker.ReportProgress(progressPercent, $"Updated source record status for {sourceId}");
                                    }
                                    catch (Exception statusEx)
                                    {
                                        mergeErrors.Add(new MergeError
                                        {
                                            RowNumber = row,
                                            SourceId = sourceId,
                                            TargetId = targetId,
                                            Stage = "UpdateSourceStatus",
                                            Operation = "UpdateStatus",
                                            Message = $"Failed updating source record {sourceId} status",
                                            Details = statusEx?.ToString(),
                                            Timestamp = DateTime.UtcNow
                                        });
                                    }
                                }

                                if (mergeRequest.Errors.Any())
                                {
                                    foreach (var error in mergeRequest.Errors)
                                    {
                                        if (!error.RowNumber.HasValue)
                                        {
                                            error.RowNumber = row;
                                        }
                                        mergeErrors.Add(error);
                                    }
                                }
                            }
                        }
                    },
                    ProgressChanged = (args) =>
                    {
                        if (args.UserState is MergeProgressDetail detail)
                        {
                            UpdateProgressUI(detail, args.ProgressPercentage);
                        }
                        else if (args.UserState is string message)
                        {
                            lblProgress.Text = message;
                            if (args.ProgressPercentage >= 0 && args.ProgressPercentage <= 100)
                            {
                                progressBarMerge.Value = args.ProgressPercentage;
                            }
                            AppendProgressLog(message);
                        }
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (mergeErrors.Any())
                            {
                                MessageBox.Show($"Bulk merge completed with {mergeErrors.Count} merge errors. Review the report for details.", "Merge Completed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Bulk merge completed successfully.", "Merge Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
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

        private void UpdateSourceRecordStatus(string entityLogicalName, Guid sourceId, int? selectedState, int? selectedStatus)
        {
            if (Service == null || string.IsNullOrEmpty(entityLogicalName) || sourceId == Guid.Empty)
            {
                return;
            }

            // Only update if user has made a selection
            if (!selectedState.HasValue && !selectedStatus.HasValue)
            {
                return;
            }

            Entity sourceEntity = new Entity(entityLogicalName)
            {
                Id = sourceId
            };

            if (selectedState.HasValue)
            {
                sourceEntity["statecode"] = new OptionSetValue(selectedState.Value);
            }

            if (selectedStatus.HasValue)
            {
                sourceEntity["statuscode"] = new OptionSetValue(selectedStatus.Value);
            }

            Service.Update(sourceEntity);
        }

        private string GetCrmBaseUrl()
        {
            if (connectionDetail == null || string.IsNullOrEmpty(connectionDetail.WebApplicationUrl))
            {
                // Try to get from settings as fallback
                if (mySettings != null && !string.IsNullOrEmpty(mySettings.LastUsedOrganizationWebappUrl))
                {
                    return ExtractBaseUrl(mySettings.LastUsedOrganizationWebappUrl);
                }
                return null;
            }

            return ExtractBaseUrl(connectionDetail.WebApplicationUrl);
        }

        private string ExtractBaseUrl(string serviceUrl)
        {
            if (string.IsNullOrEmpty(serviceUrl))
            {
                return null;
            }

            try
            {
                Uri uri = new Uri(serviceUrl);
                // Remove /XRMServices/2011/Organization.svc or similar paths
                string baseUrl = $"{uri.Scheme}://{uri.Host}";
                if (!string.IsNullOrEmpty(uri.AbsolutePath))
                {
                    // Extract the organization path (e.g., /Marketing from /Marketing/XRMServices/2011/Organization.svc)
                    var pathParts = uri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (pathParts.Length > 0)
                    {
                        baseUrl += "/" + pathParts[0];
                    }
                }
                return baseUrl;
            }
            catch
            {
                return null;
            }
        }

        private string GetCrmRecordUrl(string entityLogicalName, Guid recordId)
        {
            string baseUrl = GetCrmBaseUrl();
            if (string.IsNullOrEmpty(baseUrl))
            {
                return null;
            }

            return $"{baseUrl}/main.aspx?etn={entityLogicalName}&id={recordId}&pagetype=entityrecord";
        }

        private void SetupCrmLinks()
        {
            if (dataGridViewExcel.Columns.Count == 0)
            {
                return;
            }

            var logicalName = EntityLogicalName();
            if (string.IsNullOrEmpty(logicalName))
            {
                return;
            }

            // Ensure grid is enabled and selectable but read-only
            dataGridViewExcel.Enabled = true;
            dataGridViewExcel.ReadOnly = true;
            dataGridViewExcel.SelectionMode = DataGridViewSelectionMode.CellSelect;

            // Remove any existing event handlers to avoid duplicates
            dataGridViewExcel.CellContentClick -= DataGridViewExcel_CellContentClick;

            // Handle cell click to open both source and target links
            // Use only CellContentClick to avoid duplicate events
            dataGridViewExcel.CellContentClick += DataGridViewExcel_CellContentClick;

            // Format SourceId and TargetId columns to show as links
            foreach (DataGridViewColumn column in dataGridViewExcel.Columns)
            {
                if (column.Name == "SourceId" || column.Name == "TargetId")
                {
                    column.DefaultCellStyle.ForeColor = Color.Blue;
                    column.DefaultCellStyle.Font = new Font(dataGridViewExcel.DefaultCellStyle.Font, FontStyle.Underline);
                }
            }
        }

        private void DataGridViewExcel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var columnName = dataGridViewExcel.Columns[e.ColumnIndex].Name;
            // If clicking on SourceId or TargetId, open both source and target links
            if (columnName == "SourceId" || columnName == "TargetId")
            {
                OpenBothCrmLinks(e.RowIndex);
            }
        }

        private void OpenBothCrmLinks(int rowIndex)
        {
            var logicalName = EntityLogicalName();
            if (string.IsNullOrEmpty(logicalName))
            {
                return;
            }

            var sourceIdColumn = dataGridViewExcel.Columns["SourceId"];
            var targetIdColumn = dataGridViewExcel.Columns["TargetId"];

            if (sourceIdColumn == null || targetIdColumn == null)
            {
                return;
            }

            // Open source link
            var sourceValue = dataGridViewExcel.Rows[rowIndex].Cells["SourceId"].Value?.ToString();
            if (Guid.TryParse(sourceValue, out var sourceId))
            {
                string sourceUrl = GetCrmRecordUrl(logicalName, sourceId);
                if (!string.IsNullOrEmpty(sourceUrl))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(sourceUrl);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to open source CRM link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            // Small delay to avoid opening both at exactly the same time
            System.Threading.Thread.Sleep(100);

            // Open target link
            var targetValue = dataGridViewExcel.Rows[rowIndex].Cells["TargetId"].Value?.ToString();
            if (Guid.TryParse(targetValue, out var targetId))
            {
                string targetUrl = GetCrmRecordUrl(logicalName, targetId);
                if (!string.IsNullOrEmpty(targetUrl))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(targetUrl);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to open target CRM link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void dataGridViewEntities_SelectionChanged(object sender, EventArgs e)
        {
            LoadEntityStateStatusPicklists(EntityLogicalName());
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

            connectionDetail = detail;

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private class StateStatusInfo
        {
            public string StateLabel { get; set; }
            public string StatusLabel { get; set; }
        }

        private class OptionItem
        {
            public int Value { get; set; }
            public string Label { get; set; }
        }
    }
}