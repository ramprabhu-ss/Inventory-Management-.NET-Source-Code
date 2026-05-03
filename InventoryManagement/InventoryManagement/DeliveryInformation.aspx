<%@ Page Title="Delivery Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryInformation.aspx.cs" Inherits="InventoryManagement.DeliveryInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/jquery-3.7.0.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.bundle.min.js"></script>

    <script>
        function calculateQuantity() {

            $(document).ready(function () {
                // Select all textboxes within the GridView by partial ID match
                $("[id*=GrdDeliveryInfo] [id*=GrdTxtQuantity]").on("keyup", function () {

                    var GrdTxtQuantity = $(this).val();
                    //if (GrdTxtQuantity == "")
                    //GrdTxtQuantity = "0.00";

                    // 1. Get the current row of the triggered textbox
                    var row = $(this).closest("tr");

                    // 2. Find the other control in the same row
                    var GrdTxtPrice = row.find("[id*=GrdTxtPrice]");
                    //if (GrdTxtPrice.val() == "")
                    //GrdTxtPrice.val("0.00");

                    // 3. Update the target control value
                    var totalAmount = 0.00;
                    if (GrdTxtQuantity != "" && GrdTxtPrice.val() != "") {
                        var quantity = GrdTxtQuantity;
                        var price = parseFloat(GrdTxtPrice.val());
                        totalAmount = Math.round((quantity * price), 2);
                    }

                    var GrdTxtTotalAmount = row.find("[id*=GrdTxtTotalAmount]");
                    GrdTxtTotalAmount.val(totalAmount);

                    // Use .val() if the target is another TextBox
                    // row.find("[id*=txtPrice]").val(newValue);
                });
            });
        }

        function calculatePrice() {

            $(document).ready(function () {
                // Select all textboxes within the GridView by partial ID match
                $("[id*=GrdDeliveryInfo] [id*=GrdTxtPrice]").on("keyup", function () {
                    //$(this).on("keyup", function () {

                    var GrdTxtPrice = $(this).val();
                    //if (GrdTxtPrice == "")
                    //GrdTxtPrice = "0.00";

                    // 1. Get the current row of the triggered textbox
                    var row = $(this).closest("tr");

                    // 2. Find the other control in the same row
                    var GrdTxtQuantity = row.find("[id*=GrdTxtQuantity]");
                    //if (GrdTxtQuantity.val() == "")
                    //GrdTxtQuantity.val("0.00");


                    // 3. Update the target control value
                    var totalAmount = 0.00;
                    if (GrdTxtQuantity.val() != "" && GrdTxtPrice != "") {
                        var quantity = GrdTxtQuantity.val();
                        var price = parseFloat(GrdTxtPrice);
                        totalAmount = Math.round((quantity * price), 2);
                    }

                    var GrdTxtTotalAmount = row.find("[id*=GrdTxtTotalAmount]");
                    GrdTxtTotalAmount.val(totalAmount);

                    // Use .val() if the target is another TextBox
                    // row.find("[id*=txtPrice]").val(newValue);
                });
            });
        }

    </script>

    <style type="text/css">
        .pageBody {
            width: 98%;
            margin: 1% 0% 0% 1%;
        }

        .alignRight {
            text-align: right;
        }

        .removeGridFooterBorder {
            border-width: 0px !important;
            border-style: none !important;
        }
    </style>

    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

        <div class="">
            <div class="row p-2">
                <div class="col-2" style="width: 10%;">
                    <asp:Label ID="LblDate" runat="server" Text="Delivery Date:" class="form-label" Style="vertical-align: middle;"></asp:Label>
                </div>
                <div class="col-2" style="width: 15%;">
                    <asp:TextBox ID="TxtDeliveryDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm"></asp:TextBox>

                </div>
                <div class="col-2" style="width: 15%;">
                    <asp:Button ID="BtnView" runat="server" Text="View" OnClick="BtnView_Click" class="btn btn-primary btn-sm"></asp:Button>
                </div>
            </div>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="GrdDeliveryInfo" runat="server" AutoGenerateColumns="false" EnableViewState="true" ShowHeaderWhenEmpty="true"
                ShowFooter="true" OnRowDataBound="GrdDeliveryInfo_RowDataBound" CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="bg-primary text-white" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <div class="form-check">
                                <input id="GrdCheckBoxSelect" runat="server" class="form-check-input" type="checkbox" value="">
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product Name" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <asp:DropDownList ID="GrdDdlProduct" runat="server" AutoPostBack="false" EnableViewState="true" class="form-select" TabIndex="1">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnAddRow" runat="server" Text="Add Row" OnClick="btnAddRow_Click" class="btn btn-secondary btn-sm" />
                            <asp:Button ID="btnDeleteRow" runat="server" Text="Delete Row" OnClick="btnDeleteRow_Click" class="btn btn-secondary btn-sm" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="bg-primary text-white" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtQuantity" runat="server" TextMode="Number" EnableViewState="true" onkeypress="calculateQuantity();" class="form-control alignRight" TabIndex="2"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="LblFooterTotalQuantity" runat="server" Text="Total Quantity: " class="form-label"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtPrice" runat="server" TextMode="Number" EnableViewState="true" onkeypress="calculatePrice();" class="form-control alignRight" TabIndex="3"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="bg-primary text-white" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%--<asp:Label ID="GrdLblTotalAmount" runat="server" Text="0.00" EnableViewState="true" class="form-label" TabIndex="4"></asp:Label>--%>
                            <asp:TextBox ID="GrdTxtTotalAmount" runat="server" TextMode="Number" EnableViewState="true" ReadOnly="true" class="form-control alignRight" TabIndex="4"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="LblFooterTotalAmount" runat="server" Text="Total Amount: " class="form-label"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtRemarks" runat="server" EnableViewState="true" class="form-control" TabIndex="5"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="removeGridFooterBorder" />
            </asp:GridView>


        </div>

        <div class="text-center">
            <div class="row p-2">
                <div class="col-12">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" class="btn btn-primary btn-sm" />
                    <asp:Button ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" class="btn btn-secondary btn-sm" />
                </div>

            </div>
        </div>

        <div id="successAlert" runat="server" class="alert alert-dismissible alert-success" visible="false">
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            <strong>Submitted Successfully!</strong>
        </div>

        <div class="modal" id="successMessageModal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Success</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true"></span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p>Submitted Successfully!</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success">Ok</button>
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </main>

</asp:Content>
