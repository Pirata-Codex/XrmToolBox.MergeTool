# Record Merge Tool for Dynamics 365

## Overview

The Record Merge Tool is a comprehensive plugin for XrmToolBox designed to facilitate the merging of out-of-the-box and custom entity records in Dynamics 365. This tool allows users to load entities, validate records from an Excel file, and merge records efficiently while providing detailed progress and error reporting.

## Features

- **Load Entities**: Load all entities from the connected Dynamics 365 organization.
- **Search Entities**: Filter entities by name or logical name.
- **Load Excel File**: Load records from an Excel file for validation and merging.
- **Validate Records**: Validate the existence of records in Dynamics 365.
- **Merge Records**: Merge records based on the validated data.
- **Error Reporting**: Generate detailed error reports for invalid records.

## Prerequisites

- Visual Studio 2022
- .NET Framework 4.8
- XrmToolBox
- Dynamics 365 SDK

## Installation

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio 2022.
3. Build the solution to restore the necessary NuGet packages.
4. Deploy the plugin to XrmToolBox.

## Usage

### Loading Entities

1. Connect to your Dynamics 365 organization using XrmToolBox.
2. Click on the "Load Entities" button to load all entities from the organization.
3. Use the search box to filter entities by name or logical name.

### Loading and Validating Excel File

1. Click on the "Load Excel" button to select an Excel file containing the records to be merged.
2. Click on the "Validate Excel" button to validate the records in the Excel file.
3. The tool will check the existence of the records in Dynamics 365 and display the validation status.

### Merging Records

1. Ensure that the records in the Excel file are validated and marked as "Valid".
2. Click on the "Merge" button to start the merge process.
3. The tool will merge the records and provide progress updates.
4. After the merge process is complete, an error report can be generated if there are any invalid records.

### Error Reporting

1. If there are any invalid records, the tool will prompt you to save an error report.
2. The error report will contain details of the invalid records and the corresponding error messages.

## Code Structure

### MyPlugin.cs

This file contains the main plugin class `MyPlugin` which inherits from `PluginBase`. It includes metadata attributes for the plugin and handles the assembly resolution for external dependencies.

### MyPluginControl.cs

This file contains the main control class `MyPluginControl` which inherits from `PluginControlBase`. It includes methods for loading entities, validating records, merging records, and generating error reports.

### Merge.cs

This file contains the `MergeRequest` class which handles the merging of records. It includes methods for retrieving entity metadata, merging one-to-many and many-to-many relationships, and merging fields.

### Settings.cs

This file contains the `Settings` class which is used to store settings for the plugin. It includes properties for storing the last used organization web app URL.

## Contributing

Contributions are welcome! Please fork the repository and submit pull requests for any enhancements or bug fixes.

## License

This project is licensed under the GPL License. See the [LICENSE](https://github.com/Pirata-Codex/XrmToolBox.MergeTool#GPL-3.0-1-ov-file) file for details.

## Contact

For any questions or feedback, please contact me at r.xaleghi[at]gmail.com.

---

Thank you for using the Record Merge Tool for Dynamics 365!
