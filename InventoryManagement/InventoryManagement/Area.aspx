<%@ Page Title="Area Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Area.aspx.cs" Inherits="InventoryManagement.Area" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<head>--%>
    <%--<meta charset="utf-8" />--%>
    <%--<meta name="viewport" content="width=device-width, initial-scale=1" />--%>
    <%--<title>Area Master</title>--%>

    <script src="Scripts/jquery-3.7.0.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function checkMandatoryFields() {
            return true;
        }

        $(document).ready(function () {
            $('#grdAreaMaster').DataTable();
        })
    </script>
    <style type="text/css">
        .pageBody {
            width: 90%;
            margin: 1% 5% 0% 5%;
        }
    </style>
    <%--</head>--%>
    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h2 id="title"><%: Title %></h2>
        <%--<form id="form1" runat="server">--%>

        <div class="table-responsive">
            <asp:GridView ID="grdAreaMaster" runat="server" AutoGenerateColumns="false" EnableViewState="true"
                AllowSorting="true" AllowPaging="true" PageSize="5" DataKeyNames="area_id,area_name"
                OnRowEditing="grdAreaMaster_RowEditing" OnRowUpdating="grdAreaMaster_RowUpdating"
                OnRowCancelingEdit="grdAreaMaster_RowCancelingEdit" OnRowDeleting="grdAreaMaster_RowDeleting"
                OnRowDataBound="grdAreaMaster_RowDataBound" OnPageIndexChanging="grdAreaMaster_PageIndexChanging"
                CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <%--<asp:BoundField DataField="area_id" HeaderText="AREA ID" ReadOnly="true" />--%>
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

        <%--<div class="clearfix"></div>--%>

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
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="checkMandatoryFields();" class="btn btn-primary" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn btn-secondary" />
                </div>
                <div class="col-6">
                </div>
            </div>
        </div>
        <%--</form>--%>
    </main>

</asp:Content>
