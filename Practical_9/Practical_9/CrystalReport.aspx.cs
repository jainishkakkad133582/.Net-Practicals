using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.UI;
using Practical_9.Data;

namespace Practical_9
{
    public partial class CrystalReportPage : Page
    {
        private object reportDocument;
        private Type reportDocumentType;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                DatabaseInitializer.EnsureCreated();
                string configuredPath = System.Configuration.ConfigurationManager.AppSettings["CrystalReportPath"];
                string reportPath = Server.MapPath(String.IsNullOrWhiteSpace(configuredPath) ? "~/Reports/PlacementSummary.rpt" : configuredPath);

                if (!File.Exists(reportPath))
                {
                    ShowReportInstructions("The report file <strong>Reports/PlacementSummary.rpt</strong> has not been added yet.");
                    return;
                }

                Assembly webAssembly;
                Assembly engineAssembly;
                try
                {
                    webAssembly = Assembly.Load("CrystalDecisions.Web");
                    engineAssembly = Assembly.Load("CrystalDecisions.CrystalReports.Engine");
                }
                catch (Exception)
                {
                    ShowReportInstructions("SAP Crystal Reports for Visual Studio is not installed or its runtime assemblies are not available to this application.");
                    return;
                }

                Type viewerType = webAssembly.GetType("CrystalDecisions.Web.CrystalReportViewer", false);
                reportDocumentType = engineAssembly.GetType("CrystalDecisions.CrystalReports.Engine.ReportDocument", false);
                if (viewerType == null || reportDocumentType == null)
                {
                    ShowReportInstructions("The installed Crystal Reports runtime does not expose the expected ASP.NET viewer assemblies.");
                    return;
                }

                reportDocument = Activator.CreateInstance(reportDocumentType);
                reportDocumentType.GetMethod("Load", new Type[] { typeof(string) }).Invoke(reportDocument, new object[] { reportPath });

                DataSet data = new PlacementRepository().GetReportDataSet();
                MethodInfo setDataSource = reportDocumentType.GetMethod("SetDataSource", new Type[] { typeof(DataSet) });
                if (setDataSource != null)
                    setDataSource.Invoke(reportDocument, new object[] { data });
                else
                {
                    MethodInfo tableMethod = reportDocumentType.GetMethod("SetDataSource", new Type[] { typeof(DataTable) });
                    if (tableMethod == null) throw new InvalidOperationException("The Crystal Reports runtime does not contain a compatible SetDataSource method.");
                    tableMethod.Invoke(reportDocument, new object[] { data.Tables["PlacementRecords"] });
                }

                Control viewer = Activator.CreateInstance(viewerType) as Control;
                if (viewer == null) throw new InvalidOperationException("CrystalReportViewer could not be created as an ASP.NET control.");
                viewer.ID = "crystalPlacementViewer";
                PropertyInfo reportSource = viewerType.GetProperty("ReportSource");
                if (reportSource == null) throw new InvalidOperationException("The CrystalReportViewer ReportSource property was not found.");
                reportSource.SetValue(viewer, reportDocument, null);
                PropertyInfo prompt = viewerType.GetProperty("EnableDatabaseLogonPrompt");
                if (prompt != null && prompt.CanWrite) prompt.SetValue(viewer, false, null);
                PropertyInfo reuse = viewerType.GetProperty("ReuseParameterValuesOnRefresh");
                if (reuse != null && reuse.CanWrite) reuse.SetValue(viewer, true, null);
                phCrystalViewer.Controls.Add(viewer);
            }
            catch (TargetInvocationException ex)
            {
                Exception cause = ex.InnerException ?? ex;
                ShowReportInstructions("Crystal Reports could not load the report data: " + Server.HtmlEncode(cause.Message));
            }
            catch (Exception ex)
            {
                ShowReportInstructions("Crystal Reports is not ready: " + Server.HtmlEncode(ex.Message));
            }
        }

        private void ShowReportInstructions(string message)
        {
            litReportMessage.Text = @"<div class='alert alert-info'>" + message + @"</div>
<div class='report-instructions'>
  <p class='eyebrow'>CRYSTAL REPORTS SETUP</p>
  <h2>Finish the Crystal Reports connection</h2>
  <p>The placement dashboard and database are available. To render the native Crystal report, complete these one-time setup steps:</p>
  <ol>
    <li>Install <strong>SAP Crystal Reports, developer version for Microsoft Visual Studio (64-bit)</strong> and restart Visual Studio.</li>
    <li>Right-click the project's <code>Reports</code> folder and add a new <strong>Crystal Reports</strong> item named <code>PlacementSummary.rpt</code>.</li>
    <li>Choose the ADO.NET DataSet source <code>Data/PlacementDataSet.xsd</code>, then select the <code>PlacementRecords</code> table.</li>
    <li>Add the placement fields to the report, save it, and run this page again using the x64 platform.</li>
  </ol>
  <p><a href='https://www.sap.com/products/data-cloud/crystal-reports.html#visual-studio' target='_blank' rel='noopener'>Open SAP Crystal Reports for Visual Studio download information ↗</a></p>
</div>";
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (reportDocument != null && reportDocumentType != null)
            {
                IDisposable disposable = reportDocument as IDisposable;
                if (disposable != null) disposable.Dispose();
                else
                {
                    MethodInfo close = reportDocumentType.GetMethod("Close", Type.EmptyTypes);
                    if (close != null) close.Invoke(reportDocument, null);
                }
            }
        }
    }
}
