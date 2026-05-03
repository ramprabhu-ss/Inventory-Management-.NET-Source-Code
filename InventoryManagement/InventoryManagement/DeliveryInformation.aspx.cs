using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class DeliveryInformation : System.Web.UI.Page
    {
        readonly ClsDeliveryInformation objDeliveryInformation = new ClsDeliveryInformation();
        DataTable dtDeliveryInfo = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TxtDeliveryDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");

                    // Load product details for product dropdown in gridview
                    ViewState["dtProducts"] = objDeliveryInformation.GetProducts();

                    // Set initial row structure for gridview
                    /*
                    DataTable dt = AddNewRow_1();
                    GrdDeliveryInfo.DataSource = dt;
                    GrdDeliveryInfo.DataBind();
                    */
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void TxtDeliveryDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (TxtDeliveryDate.Text == DateTime.Now.Date.ToString("yyyy-MM-dd"))
                {
                    ViewState["dtDeliveryInfo"] = null;
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in GrdDeliveryInfo.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        // Access data from each row
                        //string customerId = row.Cells[0].Text; // For BoundFields
                        DropDownList GrdDdlProduct = (DropDownList)row.FindControl("GrdDdlProduct"); // For TemplateFields
                        TextBox GrdTxtQuantity = (TextBox)row.FindControl("GrdTxtQuantity");
                        TextBox GrdTxtPrice = (TextBox)row.FindControl("GrdTxtPrice");
                        Label GrdLblTotalAmount = (Label)row.FindControl("GrdLblTotalAmount");
                        TextBox GrdTxtRemarks = (TextBox)row.FindControl("GrdTxtRemarks");

                        // Process each row (e.g., add to a list or update database)
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["dtDeliveryInfo"] = null;
                Response.Redirect("~/DeliveryInformation.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (ViewState["dtDeliveryInfo"] != null) // Retrieve the existing DataTable from ViewState
                {
                    // Capturing values before adding a new row
                    dt = (DataTable)ViewState["dtDeliveryInfo"];

                    for (int i = 0; i < GrdDeliveryInfo.Rows.Count; i++)
                    {
                        DropDownList GrdDdlProduct = (DropDownList)GrdDeliveryInfo.Rows[i].FindControl("GrdDdlProduct");
                        dt.Rows[i]["ProductId"] = GrdDdlProduct.SelectedValue;

                        TextBox GrdTxtQuantity = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtQuantity");
                        dt.Rows[i]["Quantity"] = (GrdTxtQuantity.Text == "") ? "" : GrdTxtQuantity.Text;

                        TextBox GrdTxtPrice = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtPrice");
                        dt.Rows[i]["Price"] = (GrdTxtPrice.Text == "") ? "" : GrdTxtPrice.Text;

                        Label GrdLblTotalAmount = (Label)GrdDeliveryInfo.Rows[i].FindControl("GrdLblTotalAmount");
                        if (GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            Double quantity = Convert.ToDouble(GrdTxtQuantity.Text);
                            Double price = Convert.ToDouble(GrdTxtPrice.Text);
                            Double totalAmount = (quantity * price);
                            dt.Rows[i]["TotalAmount"] = totalAmount;
                        }
                        else
                        {
                            dt.Rows[i]["TotalAmount"] = "";
                        }

                        TextBox GrdTxtRemarks = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtRemarks");
                        dt.Rows[i]["Remarks"] = GrdTxtRemarks.Text;
                    }

                    // Now add the new blank row to dt and rebind
                    dt = AddNewRow();
                }

                GrdDeliveryInfo.DataSource = (DataTable)ViewState["dtDeliveryInfo"];
                GrdDeliveryInfo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (ViewState["dtDeliveryInfo"] != null) // Retrieve the existing DataTable from ViewState
                {
                    // Capturing values before adding a new row
                    dt = (DataTable)ViewState["dtDeliveryInfo"];

                    for (int i = 0; i < GrdDeliveryInfo.Rows.Count; i++)
                    {
                        HtmlInputCheckBox GrdCheckBoxSelect = (HtmlInputCheckBox)GrdDeliveryInfo.Rows[i].FindControl("GrdCheckBoxSelect");
                        if (GrdCheckBoxSelect != null && GrdCheckBoxSelect.Checked == true)
                        {
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                        }
                    }

                    ViewState["dtDeliveryInfo"] = dt;
                }

                GrdDeliveryInfo.DataSource = dt;
                GrdDeliveryInfo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GrdDeliveryInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Check if the current row is a data row (not header/footer)
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Find the DropDownList using its ID
                    DropDownList GrdDdlProduct = (DropDownList)e.Row.FindControl("GrdDdlProduct");
                    if (GrdDdlProduct != null)
                    {
                        // Populate the DropDownList from your data source
                        GrdDdlProduct.DataSource = (DataTable)ViewState["dtProducts"]; // Method to fetch your data
                        GrdDdlProduct.DataTextField = "ProductName";
                        GrdDdlProduct.DataValueField = "ProductID";
                        GrdDdlProduct.DataBind();

                        // Optional: Add a default "Select" item
                        GrdDdlProduct.Items.Insert(0, new ListItem("-- Select --", ""));

                        // Set the selected value based on the current row data
                        GrdDdlProduct.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ProductID").ToString();
                    }

                    TextBox GrdTxtQuantity = (TextBox)e.Row.FindControl("GrdTxtQuantity");
                    if (GrdTxtQuantity != null)
                    {
                        GrdTxtQuantity.Text = DataBinder.Eval(e.Row.DataItem, "Quantity").ToString();
                    }

                    TextBox GrdTxtPrice = (TextBox)e.Row.FindControl("GrdTxtPrice");
                    if (GrdTxtPrice != null)
                    {
                        GrdTxtPrice.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();
                    }

                    Label GrdLblTotalAmount = (Label)e.Row.FindControl("GrdLblTotalAmount");
                    if (GrdLblTotalAmount != null)
                    {
                        GrdLblTotalAmount.Text = DataBinder.Eval(e.Row.DataItem, "TotalAmount").ToString();
                    }

                    TextBox GrdTxtRemarks = (TextBox)e.Row.FindControl("GrdTxtRemarks");
                    if (GrdTxtRemarks != null)
                    {
                        GrdTxtRemarks.Text = DataBinder.Eval(e.Row.DataItem, "Remarks").ToString();
                    }
                }

                // Check if the current row being bound is the footer
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    // Access cells by index and set text
                    //Label GrdLblTotalAmount = (Label)e.Row.FindControl("GrdLblTotalAmount");

                    //e.Row.Cells[2].Text = "Total Amount:";
                    //e.Row.Cells[4].Text = GrdLblTotalAmount.Text; // You can use a calculated variable here
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable AddNewRow()
        {
            // 1. Retrieve the existing DataTable from ViewState
            DataTable dt = (DataTable)ViewState["dtDeliveryInfo"];
            DataRow dr;

            try
            {
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("ProductId");
                    dt.Columns.Add("Quantity");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("TotalAmount");
                    dt.Columns.Add("Remarks");
                }

                // 2. Add a new row
                dr = dt.NewRow();
                dr["ProductId"] = "";
                dr["Quantity"] = "";
                dr["Price"] = "";
                dr["TotalAmount"] = "";
                dr["Remarks"] = "";
                dt.Rows.Add(dr);

                // 3. Save back to ViewState and rebind
                ViewState["dtDeliveryInfo"] = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        private void BindGridView()
        {
            try
            {
                if (ViewState["dtDeliveryInfo"] == null)
                {
                    dtDeliveryInfo = objDeliveryInformation.GetDeliveryInformation(TxtDeliveryDate.Text);
                    ViewState["dtDeliveryInfo"] = dtDeliveryInfo;
                }
                /*else
                 {
                     dtDeliveryInfo = (DataTable)ViewState["dtDeliveryInfo"];
                 }*/

                // Set initial row structure for gridview
                if (dtDeliveryInfo == null || dtDeliveryInfo.Rows.Count == 0)
                {
                    dtDeliveryInfo = AddNewRow();
                }

                GrdDeliveryInfo.DataSource = dtDeliveryInfo;
                GrdDeliveryInfo.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowResult(int transactionStatus)
        {
            try
            {
                if (transactionStatus == 1)
                {

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {

        }
    }
}