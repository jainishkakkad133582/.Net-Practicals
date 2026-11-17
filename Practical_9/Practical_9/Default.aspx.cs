using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Practical_9.Data;

namespace Practical_9
{
    public partial class _Default : Page
    {
        private readonly PlacementRepository repository = new PlacementRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DatabaseInitializer.EnsureCreated();
                if (!IsPostBack)
                {
                    BindFilterLists();
                    BindDashboard();
                }
            }
            catch (Exception ex)
            {
                pnlDashboard.Visible = false;
                pnlError.Visible = true;
                litError.Text = Server.HtmlEncode(ex.Message);
            }
        }

        private void BindFilterLists()
        {
            DataTable departments = repository.GetDepartmentSummary();
            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new ListItem("All departments", ""));
            foreach (DataRow row in departments.Rows)
                ddlDepartment.Items.Add(new ListItem(Convert.ToString(row["Department"]), Convert.ToString(row["Department"])));

            ddlBatchYear.Items.Clear();
            ddlBatchYear.Items.Add(new ListItem("All batches", "0"));
            DataTable batches = repository.GetBatchYears();
            foreach (DataRow row in batches.Rows)
                ddlBatchYear.Items.Add(new ListItem(Convert.ToString(row["BatchYear"]), Convert.ToString(row["BatchYear"])));
        }

        private void BindDashboard()
        {
            DashboardStats stats = repository.GetDashboardStats();
            litTotalStudents.Text = stats.TotalStudents.ToString("N0");
            litPlacedStudents.Text = stats.PlacedStudents.ToString("N0");
            litPlacementRate.Text = stats.PlacementRate.ToString("0.0");
            litAveragePackage.Text = stats.AveragePackage.ToString("0.00");
            litHighestPackage.Text = stats.HighestPackage.ToString("0.00");
            rptDepartments.DataSource = repository.GetDepartmentSummary();
            rptDepartments.DataBind();
            BindGrid();
        }

        private void BindGrid()
        {
            int batchYear = 0;
            Int32.TryParse(ddlBatchYear.SelectedValue, out batchYear);
            string sortExpression = Convert.ToString(ViewState["SortExpression"]);
            string sortDirection = Convert.ToString(ViewState["SortDirection"]);
            DataTable records = repository.GetPlacements(ddlDepartment.SelectedValue, batchYear, txtSearch.Text, sortExpression, sortDirection);
            gvPlacements.DataSource = records;
            gvPlacements.DataBind();
            litResultCount.Text = records.Rows.Count.ToString("N0");
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvPlacements.PageIndex = 0;
            pnlStudentDetails.Visible = false;
            BindDashboard();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
            ddlDepartment.SelectedIndex = 0;
            ddlBatchYear.SelectedIndex = 0;
            gvPlacements.PageIndex = 0;
            ViewState["SortExpression"] = "StudentName";
            ViewState["SortDirection"] = "ASC";
            pnlStudentDetails.Visible = false;
            BindDashboard();
        }

        protected void gvPlacements_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlacements.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvPlacements_Sorting(object sender, GridViewSortEventArgs e)
        {
            string oldExpression = Convert.ToString(ViewState["SortExpression"]);
            string oldDirection = Convert.ToString(ViewState["SortDirection"]);
            ViewState["SortDirection"] = oldExpression == e.SortExpression && oldDirection == "ASC" ? "DESC" : "ASC";
            ViewState["SortExpression"] = e.SortExpression;
            BindGrid();
        }

        protected void gvPlacements_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvPlacements.SelectedDataKey == null) return;
            int id = Convert.ToInt32(gvPlacements.SelectedDataKey.Value);
            DataTable selectedStudent = repository.GetStudentById(id);
            dvStudent.DataSource = selectedStudent;
            dvStudent.DataBind();
            pnlStudentDetails.Visible = selectedStudent.Rows.Count > 0;
        }

        protected void gvPlacements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.ToolTip = "Select View details to open the student's complete record.";
        }

        protected void btnHideDetails_Click(object sender, EventArgs e)
        {
            pnlStudentDetails.Visible = false;
            gvPlacements.SelectedIndex = -1;
        }
    }
}
