<%@ Page Title="Delivery Information Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryInformationReport.aspx.cs" Inherits="InventoryManagement.DeliveryInformationReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Load jQuery first -->
    <script src="Scripts/jquery-3.7.0.min.js"></script>
    <!-- Load Popper.js (required for Bootstrap dropdowns) -->
    <script src="Scripts/popper.min.js"></script>
    <!-- Load Bootstrap JavaScript -->
    <script src="Scripts/bootstrap.bundle.min.js"></script>

    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

        <div class="filter-section">
            <label for="fromDate">From Date:</label>
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="yyyy-mm-dd"></asp:TextBox>

            <label for="toDate">To Date:</label>
            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" placeholder="yyyy-mm-dd"></asp:TextBox>

            <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" />
        </div>

        <script>
            document.addEventListener('DOMContentLoaded', function () {
                const today = new Date().toISOString().split('T')[0];
                document.getElementById('<%= txtFromDate.ClientID %>').setAttribute('max', today);
                document.getElementById('<%= txtToDate.ClientID %>').setAttribute('max', today);
            });
        </script>
    </main>
</asp:Content>
