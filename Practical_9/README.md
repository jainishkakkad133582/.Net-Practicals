# Practical_9 — Placement Dashboard using Data Controls & Crystal Reports

This is a complete **ASP.NET Web Forms (.NET Framework 4.8)** Visual Studio solution. It uses ADO.NET and SQL Server LocalDB for placement data, ASP.NET data controls for the dashboard, and SAP Crystal Reports for the report viewer integration.

## Important Crystal Reports requirement

The dashboard and database pages are designed to build without a compile-time reference to Crystal assemblies. Crystal Reports itself is an external SAP component and must be installed on the Windows development machine before the report viewer can render a `.rpt` report.

1. Install **SAP Crystal Reports, developer version for Microsoft Visual Studio (64-bit)** using SAP's current supported download. SAP's current platform information lists SP40 and Visual Studio 2022 support. The 32-bit .NET runtime was discontinued after December 2025; use a 64-bit application platform.
2. Run the installer as Administrator, then restart Visual Studio.
3. In Visual Studio, right-click `Practical_9/Reports` → **Add → New Item** → **Crystal Reports**. Name the file `PlacementSummary.rpt`.
4. Choose **Using the Report Wizard** (or Standard Report).
5. In Database Expert, choose **Project Data → ADO.NET DataSets** and select `Data/PlacementDataSet.xsd`.
6. Expand `PlacementDataSet` and select the `PlacementRecords` table.
7. Add the fields `EnrollmentNo`, `StudentName`, `Department`, `BatchYear`, `CGPA`, `CompanyName`, `JobRole`, `PackageLPA`, and `PlacementDate`. Add a report title such as **Campus Placement Summary**. Group by `Department` or `CompanyName`, then finish and save the file as `Reports/PlacementSummary.rpt`.
8. Set the project platform to **x64** (not Any CPU) and run the project. Open **Crystal Report** from the navigation bar.

Official SAP references:
- Crystal Reports for Visual Studio developer download: https://www.sap.com/products/data-cloud/crystal-reports.html#visual-studio
- SAP's Crystal Report Viewer documentation: https://help.sap.com/docs/SAP_CRYSTAL_REPORTS%2C_DEVELOPER_VERSION_FOR_MICROSOFT_VISUAL_STUDIO
- SAP installation best practices: https://userapps.support.sap.com/sap/support/knowledge/en/2806940

The report page checks for the Crystal assemblies and `PlacementSummary.rpt`. If either is missing, it displays an instruction panel rather than an unhandled server exception. The report file is deliberately not represented by a dummy/invalid `.rpt` file; it must be saved by the actual SAP Crystal Reports designer. The optional `Reports/CrystalImageHandler.config.snippet` contains the image-handler mapping used by the Crystal web viewer; merge it into `Web.config` only after the matching Crystal runtime is installed and confirm the assembly version.

## Open and run the dashboard

1. Extract the ZIP.
2. Open `Practical_9.sln` in Visual Studio 2022.
3. Confirm the **.NET Framework 4.8 Developer Pack** and **ASP.NET and web development** workload are installed.
4. Confirm SQL Server Express LocalDB is installed and the instance `(localdb)\MSSQLLocalDB` is available.
5. Build the solution, set `Practical_9` as the startup project, and press `Ctrl + F5`.
6. The first visit to the dashboard creates `PlacementDB` and `dbo.PlacementStudents` if they do not exist, and seeds sample records only when the table is empty.

No NuGet package restore is required for the dashboard. It uses .NET Framework BCL assemblies and ADO.NET.

## Included dashboard features

- Summary cards: total students, students placed, placement rate, and average package.
- Department-wise placement summary using an ASP.NET Repeater.
- Search by student name, enrollment number, company, or role.
- Filters by department and batch year.
- Sortable and pageable ASP.NET GridView.
- Selected-record detail panel using DetailsView.
- Crystal Reports viewer page (requires the SAP component and the `.rpt` file described above).
- SQL setup/seed script at `Practical_9/Database/PlacementDB.sql`.

## Database details

Database: `PlacementDB`

Table: `dbo.PlacementStudents`

Columns: `StudentId`, `EnrollmentNo`, `StudentName`, `Department`, `BatchYear`, `CGPA`, `CompanyName`, `JobRole`, `PackageLPA`, `PlacementDate`, and `PlacementStatus`.

The names and placement figures are fictional sample data for an academic demonstration. They are not actual Marwadi University placement records.

If LocalDB is installed under a different instance name, change the `MasterDb` and `PlacementDb` connection strings in `Practical_9/Web.config`.
