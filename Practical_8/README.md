# Practical_8 - Inventory Management Module

Complete ASP.NET MVC 5 project for **.NET Framework 4.8**, following the supplied Lab Experiment 8 reference.

## Open and run

1. Extract the ZIP file.
2. Open `Practical_8.sln` in Visual Studio (not the `.csproj` as a text file).
3. Right-click the solution and select **Restore NuGet Packages**. The project requires the MVC/Razor package DLLs; they are not bundled in this ZIP.
4. If restore does not populate the package folder, open **Tools → NuGet Package Manager → Package Manager Console**, select `Practical_8` as the Default project, and run `Update-Package -Reinstall`.
5. Select **Build → Clean Solution**, then **Build → Rebuild Solution**.
6. Before running, confirm that `Practical_8\bin\System.Web.WebPages.Razor.dll` exists. The `bin` folder is populated by the build.
7. Set `Practical_8` as the Startup Project if needed, then press **F5** or **Ctrl+F5**.

## Requirements

- Visual Studio with the **ASP.NET and web development** workload.
- .NET Framework 4.8 Developer Pack / Targeting Pack.
- SQL Server LocalDB with the instance name `(localdb)\\MSSQLLocalDB`.

## Database

At application startup, `Data/DatabaseInitializer.cs` creates `InventoryDB` and its `Products` table if they do not already exist. The database connection is configured in `Practical_8/Web.config`.

The equivalent manual SQL setup is included at `Practical_8/Database/InventoryDB.sql`. It can be run from SQL Server Object Explorer or SSMS if you prefer to create the database manually.

## Fix for a missing `System.Web.WebPages.Razor` assembly

The MVC 5 Razor/WebPages NuGet packages are version `3.2.9`, while their .NET assembly identities are version `3.0.0.0`. The project references now use the correct assembly identity while retaining the `3.2.9` package HintPaths. Restore the NuGet packages and rebuild so Visual Studio copies `System.Web.WebPages.Razor.dll`, `System.Web.Razor.dll`, and the related WebPages assemblies into `bin`. Do not change the binding redirect to `3.2.9.0`.

## Features

- Add a product (Create)
- Display all products (Read)
- Edit product details (Update)
- Delete a product (Delete)
- Search products by name

The table follows the reference model: `ProductId`, `ProductName`, `Quantity`, and `Price`. `ProductId` is an identity/auto-generated primary key, so it is not entered on the Create form.

## Project structure

```text
Practical_8_Full_Project/
├── Practical_8.sln
├── README.md
└── Practical_8/
    ├── App_Start/
    ├── Content/
    ├── Controllers/
    ├── Data/
    ├── Database/
    ├── Models/
    ├── Properties/
    ├── Views/
    ├── Global.asax
    ├── Global.asax.cs
    ├── Web.config
    ├── packages.config
    └── Practical_8.csproj
```

## If LocalDB is not found

Install SQL Server Express LocalDB, or change the `Data Source` value in both connection strings in `Practical_8/Web.config` and in `Data/DatabaseInitializer.cs` only if you have hard-coded a different SQL Server instance. The supplied project is configured for the same LocalDB instance shown in the reference PDF.
