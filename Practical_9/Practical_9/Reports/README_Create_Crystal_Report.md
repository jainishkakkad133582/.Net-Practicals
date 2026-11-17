# Create the native Crystal report

The `.rpt` format is a native SAP Crystal Reports designer file. This project deliberately does not include a fake or empty `.rpt` file.

1. Install SAP Crystal Reports developer version for Visual Studio (64-bit) and restart Visual Studio.
2. Right-click `Reports` → Add → New Item → Crystal Reports.
3. Name the new item `PlacementSummary.rpt`.
4. Choose the Report Wizard and select **Project Data → ADO.NET DataSets → PlacementDataSet → PlacementRecords**.
5. Add `EnrollmentNo`, `StudentName`, `Department`, `BatchYear`, `CGPA`, `CompanyName`, `JobRole`, `PackageLPA`, and `PlacementDate` to the report.
6. Add a report title. Group by `Department` or `CompanyName` if desired. Finish and save.
7. Select x64 as the project platform and run `CrystalReport.aspx`.
8. If the Crystal viewer area loads but its generated images are blank, merge the handler mapping from `CrystalImageHandler.config.snippet` into the matching `<system.web>` and `<system.webServer>` sections of `Web.config`. The snippet targets the Crystal .NET assembly identity `13.0.4000.0`; confirm the installed `CrystalDecisions.Web` assembly version before using it.

The application supplies a `DataSet` named `PlacementDataSet` with a table named `PlacementRecords`. The data source passed to Crystal Reports uses those names and the columns in `Data/PlacementDataSet.xsd`.
