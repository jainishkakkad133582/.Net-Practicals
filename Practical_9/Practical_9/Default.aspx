<%@ Page Title="Placement Dashboard"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="Practical_9._Default"
    ResponseEncoding="utf-8" %><asp:Content ID="TitleBlock" ContentPlaceHolderID="TitleContent" runat="server">Placement Dashboard</asp:Content>
<asp:Content ID="MainBlock" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-wrap">
    <div class="page-heading">
        <div>
            <p class="eyebrow">CAREER & PLACEMENT CELL</p>
            <h1>Placement Dashboard</h1>
            <p>Track student outcomes, hiring companies and placement performance.</p>
        </div>
        <div class="heading-actions">
            <a class="btn btn-primary" href="CrystalReport.aspx">
                <svg class="button-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" aria-hidden="true" focusable="false">
                    <path d="M7 3.75h7l4.25 4.25v12.25H7z" />
                    <path d="M14 3.75V8h4.25M10 12h5.5M10 15.5h5.5M10 19h3.5" />
                </svg>
                Generate Crystal Report
            </a>
        </div>
    </div>

    <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="alert alert-error">
        <strong>Dashboard could not load.</strong><br />
        <asp:Literal ID="litError" runat="server" />
    </asp:Panel>

    <asp:Panel ID="pnlDashboard" runat="server">
        <div class="stats-grid">
            <div class="stat-card">
                <div class="stat-top"><span>Total Students</span><span class="stat-icon" aria-hidden="true">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><circle cx="9" cy="8" r="3.2"/><path d="M3.5 20c.4-3.2 2.2-5 5.5-5s5.1 1.8 5.5 5M16 5.5a3 3 0 0 1 0 5.8M17 15.2c2 .7 3.2 2.3 3.5 4.8"/></svg>
                </span></div>
                <div class="stat-value"><asp:Literal ID="litTotalStudents" runat="server" /></div>
                <div class="stat-foot">Students in the placement database</div>
            </div>
            <div class="stat-card">
                <div class="stat-top"><span>Students Placed</span><span class="stat-icon" aria-hidden="true">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><rect x="3.5" y="7" width="17" height="13" rx="2"/><path d="M8.5 7V5.5A1.5 1.5 0 0 1 10 4h4a1.5 1.5 0 0 1 1.5 1.5V7M3.5 12h17M9.5 15l1.8 1.8 3.5-3.7"/></svg>
                </span></div>
                <div class="stat-value"><asp:Literal ID="litPlacedStudents" runat="server" /></div>
                <div class="stat-foot">Offers recorded in the placement database</div>
            </div>
            <div class="stat-card">
                <div class="stat-top"><span>Placement Rate</span><span class="stat-icon" aria-hidden="true">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M4 19.5h16M5 16l5-5 3.5 3.5L20 8"/><path d="M15.5 8H20v4.5"/></svg>
                </span></div>
                <div class="stat-value"><asp:Literal ID="litPlacementRate" runat="server" />%</div>
                <div class="stat-foot">Placed students as a share of total students</div>
            </div>
            <div class="stat-card">
                <div class="stat-top"><span>Average Package</span><span class="stat-icon" aria-hidden="true">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><rect x="3" y="5.5" width="18" height="13" rx="2"/><path d="M3 10h18M7 14.5h3M16.5 14.5h.01"/></svg>
                </span></div>
                <div class="stat-value"><asp:Literal ID="litAveragePackage" runat="server" /> <span class="stat-unit">LPA</span></div>
                <div class="stat-foot">Highest package: <asp:Literal ID="litHighestPackage" runat="server" /> LPA</div>
            </div>
        </div>

        <div class="content-grid">
            <section class="panel">
                <div class="panel-head"><div><h2>Student Placement Records</h2><p>Search, filter, sort and inspect the student records.</p></div><span class="muted" style="font-size:11px">LIVE DATABASE</span></div>
                <div class="filters">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="input search-input" placeholder="Search student, company or role..." MaxLength="100" />
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="select" />
                    <asp:DropDownList ID="ddlBatchYear" runat="server" CssClass="select" />
                    <asp:Button ID="btnFilter" runat="server" Text="Apply" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-quiet" OnClick="btnClear_Click" CausesValidation="false" />
                    <span class="result-count"><asp:Literal ID="litResultCount" runat="server" /> record(s)</span>
                </div>
                <div class="grid-wrap">
                    <asp:GridView ID="gvPlacements" runat="server" CssClass="data-grid" AutoGenerateColumns="false" AllowPaging="true" PageSize="8" AllowSorting="true" DataKeyNames="StudentId" GridLines="None" OnPageIndexChanging="gvPlacements_PageIndexChanging" OnSorting="gvPlacements_Sorting" OnSelectedIndexChanged="gvPlacements_SelectedIndexChanged" OnRowDataBound="gvPlacements_RowDataBound" EmptyDataText="No placement records match the selected filters.">
                        <Columns>
                            <asp:CommandField ShowSelectButton="true" SelectText="View details" ButtonType="Link" HeaderText="Details" />
                            <asp:BoundField DataField="EnrollmentNo" HeaderText="Enrollment No." SortExpression="EnrollmentNo" />
                            <asp:BoundField DataField="StudentName" HeaderText="Student Name" SortExpression="StudentName" />
                            <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName" />
                            <asp:BoundField DataField="PackageLPA" HeaderText="Package (LPA)" SortExpression="PackageLPA" DataFormatString="{0:0.00}" NullDisplayText="-" />
                            <asp:TemplateField HeaderText="Status" SortExpression="PlacementStatus"><ItemTemplate><span class='status-pill <%# Convert.ToString(Eval("PlacementStatus")) == "Placed" ? "status-placed" : "status-pending" %>'><span class="status-dot"></span><%# Eval("PlacementStatus") %></span></ItemTemplate></asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pager" />
                        <SelectedRowStyle BackColor="#f3f6ff" />
                    </asp:GridView>
                </div>
            </section>

            <section class="panel">
                <div class="panel-head"><div><h2>Department Performance</h2><p>Placement rate by department.</p></div></div>
                <div class="panel-body">
                    <asp:Repeater ID="rptDepartments" runat="server">
                        <ItemTemplate>
                            <div class="dept-row">
                                <div><div class="dept-name"><%# Eval("Department") %></div><div class="dept-sub"><%# Eval("PlacedStudents") %> / <%# Eval("TotalStudents") %> placed</div></div>
                                <div class="dept-track"><div class="dept-fill" style='width:<%# Eval("PlacementRate") %>%'></div></div>
                                <div class="dept-rate"><%# Eval("PlacementRate", "{0:0}%") %></div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="quick-note"><strong>Dashboard note</strong><p>All figures are calculated from the sample placement records stored in SQL Server LocalDB. Replace the sample rows with your institution's authorized data for real reporting.</p></div>
                </div>
            </section>
        </div>

        <asp:Panel ID="pnlStudentDetails" runat="server" Visible="false" CssClass="panel details-panel">
            <div class="panel-head"><div><h2>Selected Student Details</h2><p>Record selected from the GridView data control.</p></div><asp:Button ID="btnHideDetails" runat="server" Text="Hide details" CssClass="btn btn-secondary" OnClick="btnHideDetails_Click" CausesValidation="false" /></div>
            <div class="panel-body">
                <asp:DetailsView ID="dvStudent" runat="server" CssClass="details-view" AutoGenerateRows="false" GridLines="None">
                    <Fields>
                        <asp:BoundField DataField="EnrollmentNo" HeaderText="Enrollment No." />
                        <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                        <asp:BoundField DataField="Department" HeaderText="Department" />
                        <asp:BoundField DataField="BatchYear" HeaderText="Batch Year" />
                        <asp:BoundField DataField="CGPA" HeaderText="CGPA" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" NullDisplayText="-" />
                        <asp:BoundField DataField="JobRole" HeaderText="Job Role" NullDisplayText="-" />
                        <asp:BoundField DataField="PackageLPA" HeaderText="Package (LPA)" DataFormatString="{0:0.00}" NullDisplayText="-" />
                        <asp:BoundField DataField="PlacementDate" HeaderText="Placement Date" DataFormatString="{0:dd MMM yyyy}" NullDisplayText="-" />
                        <asp:BoundField DataField="PlacementStatus" HeaderText="Placement Status" />
                    </Fields>
                </asp:DetailsView>
            </div>
        </asp:Panel>
    </asp:Panel>
</div>
</asp:Content>
