<%@ Page Title="Area Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AreaMaster.aspx.cs" Inherits="InventoryManagement.AreaMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Load jQuery first -->
    <script src="Scripts/jquery-3.7.0.min.js"></script>
    <!-- Load Popper.js (required for Bootstrap dropdowns) -->
    <script src="Scripts/popper.min.js"></script>
    <!-- Load Bootstrap JavaScript -->
    <script src="Scripts/bootstrap.bundle.min.js"></script>

    <style type="text/css">
        .pageBody {
            width: 98%;
            margin: 1% 0% 0% 1%;
        }
    </style>

    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

        <div class="table-responsive">
            <asp:GridView ID="grdAreaMaster" runat="server" AutoGenerateColumns="false" EnableViewState="true"
                AllowSorting="true" AllowPaging="true" PageSize="5" DataKeyNames="area_id,area_name"
                OnRowEditing="grdAreaMaster_RowEditing" OnRowUpdating="grdAreaMaster_RowUpdating"
                OnRowCancelingEdit="grdAreaMaster_RowCancelingEdit" OnRowDeleting="grdAreaMaster_RowDeleting"
                OnRowDataBound="grdAreaMaster_RowDataBound" OnPageIndexChanging="grdAreaMaster_PageIndexChanging"
                CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="Area Id" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <%# Eval("area_id") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtAreaId" runat="server" class="form-control" Text='<%# Bind("area_id") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <%# Eval("area_name") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtAreaName" runat="server" class="form-control" Text='<%# Bind("area_name") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Zip Code" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <%# Eval("ZIPCODE") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtZipCode" runat="server" class="form-control" Text='<%# Bind("ZIPCODE") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" HeaderStyle-CssClass="bg-primary text-white" />
                </Columns>
                <PagerSettings Visible="true" Mode="NumericFirstLast" Position="Bottom" />
                <PagerStyle CssClass="table-secondary text-white" HorizontalAlign="Right" />
            </asp:GridView>
        </div>

        <div class="container">
            <div class="row p-2">
                <div class="col-2">
                    <asp:Label ID="lblAreaId" runat="server" Text="Area Id" class="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtAreaId" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="col-6">
                </div>
            </div>
            <div class="row p-2">
                <div class="col-2">
                    <asp:Label ID="lblAreaName" runat="server" Text="Area Name" class="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtAreaName" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="col-6">
                </div>
            </div>
            <div class="row p-2">
                <div class="col-2">
                    <asp:Label ID="lblZipCode" runat="server" Text="Zip Code" class="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtZipCode" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="col-6">
                </div>
            </div>
            <div class="row p-2">
                <div class="col-2">
                </div>
                <div class="col-4">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="btn btn-primary" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn btn-secondary" />
                </div>
                <div class="col-6">
                </div>
            </div>
        </div>
    </main>
</asp:Content>
