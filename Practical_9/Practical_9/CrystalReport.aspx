<%@ Page Title="Crystal Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CrystalReport.aspx.cs" Inherits="Practical_9.CrystalReportPage" %>
<asp:Content ID="TitleBlock" ContentPlaceHolderID="TitleContent" runat="server">Crystal Report</asp:Content>
<asp:Content ID="MainBlock" ContentPlaceHolderID="MainContent" runat="server">
<div class="page-wrap">
    <div class="page-heading">
        <div><p class="eyebrow">REPORTING & ANALYTICS</p><h1>Placement Summary Report</h1><p>Crystal Reports viewer for placed students, departments, companies and package details.</p></div>
        <div class="heading-actions"><a class="btn btn-secondary" href="Default.aspx">← Back to Dashboard</a></div>
    </div>
    <div class="report-shell">
        <asp:Literal ID="litReportMessage" runat="server" />
        <asp:PlaceHolder ID="phCrystalViewer" runat="server" />
    </div>
</div>
</asp:Content>
