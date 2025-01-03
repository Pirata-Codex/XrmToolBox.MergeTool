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

        public MyPluginControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is my first tool published on XrmToolBox. Feel free to send me your feedback", new Uri("mailto:r.xaleghi@gmail.com"));

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

            // Load entities into the DataGridView
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
                        }
                    }
                }
            });
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
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

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (dataGridViewEntities.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an entity.");
                return;
            }

            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Please select a valid Excel file.");
                return;
            }

            var selectedRow = dataGridViewEntities.SelectedRows[0];
            var logicalName = selectedRow.Cells["Logical Name"].Value.ToString();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Merging records...",
                Work = (worker, args) =>
                {
                    using (var package = new ExcelPackage(new FileInfo(txtFilePath.Text)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        args.Result = rowCount;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            string sourceId = worksheet.Cells[row, 1].Text;
                            string targetId = worksheet.Cells[row, 2].Text;

                            if (string.IsNullOrEmpty(sourceId) || string.IsNullOrEmpty(targetId))
                            {
                                continue;
                            }

                            worker.ReportProgress((row - 1) * 100 / (rowCount - 1), $"Processing row {row - 1} of {rowCount - 1}");

                            MergeRequest mergeRequest = new MergeRequest(logicalName, sourceId, targetId, Service, true);
                            mergeRequest.OnFunctionCalled += MergeRequest_OnFunctionCalled;
                            mergeRequest.DoMerge();
                        }
                    }
                },
                ProgressChanged = (args) =>
                {
                    progressBar.Value = args.ProgressPercentage;
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
                }
            });
        }

        private void MergeRequest_OnFunctionCalled(object sender, string functionName)
        {
            lblProgress.Text = $"Calling function: {functionName}";
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(GetAccounts);
        }

        private void GetAccounts()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting accounts",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(new QueryExpression("account")
                    {
                        TopCount = 50
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;
                    if (result != null)
                    {
                        MessageBox.Show($"Found {result.Entities.Count} accounts");
                    }
                }
            });
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param="e"></param>
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