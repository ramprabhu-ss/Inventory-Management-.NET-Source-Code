using InventoryManagement.IL;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class DeliveryInformation : System.Web.UI.Page
    {
        readonly ClsDeliveryInformation objDeliveryInformation = new ClsDeliveryInformation();
        DataTable dtDeliveryInfo = new DataTable();
        double footerTotalQuantity = 0;
        double footerTotalAmount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TxtDeliveryDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    TxtDeliveryDate.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");

                    // Load product details for product dropdown in gridview
                    ViewState["dtProducts"] = objDeliveryInformation.GetProducts();
                    BindGridView();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList arrListDeliveryInfo = new ArrayList();
                ArrayList arrListDeliveryDetail = new ArrayList();
                StringBuilder sqlQueryBuilder = new StringBuilder();
                string CreatedBy = "1"; // Session["UserId"].ToString();
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (GridViewRow row in GrdDeliveryInfo.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        // Access data from each row
                        //string customerId = row.Cells[0].Text; // For BoundFields
                        DropDownList GrdDdlProduct = (DropDownList)row.FindControl("GrdDdlProduct"); // For TemplateFields
                        TextBox GrdTxtQuantity = (TextBox)row.FindControl("GrdTxtQuantity");
                        TextBox GrdTxtPrice = (TextBox)row.FindControl("GrdTxtPrice");
                        TextBox GrdTxtTotalAmount = (TextBox)row.FindControl("GrdTxtTotalAmount");
                        TextBox GrdTxtRemarks = (TextBox)row.FindControl("GrdTxtRemarks");

                        double quantity, price, totalAmount = 0;
                        if (GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            quantity = Convert.ToDouble(GrdTxtQuantity.Text);
                            price = Convert.ToDouble(GrdTxtPrice.Text);
                            totalAmount = (quantity * price);
                        }

                        // Process each row (e.g., add to a list or update database)
                        /*
                        objDeliveryInformation.Param_ProductId = GrdDdlProduct.SelectedValue;
                        objDeliveryInformation.Param_Quantity = GrdTxtQuantity.Text;
                        objDeliveryInformation.Param_Price = GrdTxtPrice.Text;
                        objDeliveryInformation.Param_TotalAmount = GrdTxtTotalAmount.Text;
                        objDeliveryInformation.Param_Remarks = GrdTxtRemarks.Text;
                        objDeliveryInformation.Param_CreatedBy = "user";
                        objDeliveryInformation.Param_created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        */

                        /*sqlQueryBuilder.Append("INSERT INTO delivery_inf (Delivery_ID, DeliveryDate, EmployeeID, Total_Amount, ");
                        sqlQueryBuilder.Append("Total_Quantity, CreatedBy, created_at) VALUES ");
                        sqlQueryBuilder.Append("(@Delivery_ID, @DeliveryDate, @EmployeeID, @Total_Amount, @Total_Quantity, @CreatedBy, @created_at)");*/

                        /*MySqlCommand sqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                        sqlCommand.Parameters.AddWithValue("@Delivery_ID", "1");
                        sqlCommand.Parameters.AddWithValue("@DeliveryDate", TxtDeliveryDate.Text);
                        sqlCommand.Parameters.AddWithValue("@EmployeeID", CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@Total_Amount", GrdTxtTotalAmount.Text);
                        sqlCommand.Parameters.AddWithValue("@Total_Quantity", GrdTxtQuantity.Text);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@created_at", CreatedOn);*/

                        if (GrdDdlProduct.SelectedValue != "" && GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            arrListDeliveryInfo.Add("@Delivery_ID,'" + TxtDeliveryDate.Text + "','" + CreatedBy + "','" + totalAmount + "','"
                                + GrdTxtQuantity.Text + "','" + CreatedBy + "','" + CreatedOn + "'");

                            arrListDeliveryDetail.Add("@Detail_ID,@Delivery_ID,'" + TxtDeliveryDate.Text + "','" + CreatedBy + "','"
                                + GrdDdlProduct.SelectedValue + "','" + "','" + CreatedOn + "'");
                        }
                    }
                }

                ShowResult(objDeliveryInformation.InitiateTransaction(arrListDeliveryInfo, arrListDeliveryDetail));
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["dtDeliveryInfo"] = null;
                Response.Redirect("~/DeliveryInformation.aspx");
            }
            catch (Exception)
            {
                throw;
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

                        TextBox GrdTxtTotalAmount = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtTotalAmount");
                        if (GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            double quantity = Convert.ToDouble(GrdTxtQuantity.Text);
                            double price = Convert.ToDouble(GrdTxtPrice.Text);
                            double totalAmount = (quantity * price);
                            dt.Rows[i]["TotalAmount"] = totalAmount;
                        }
                        else
                        {
                            dt.Rows[i]["TotalAmount"] = "0.00";
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
                        GrdDdlProduct.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ProductId").ToString();
                    }

                    TextBox GrdTxtQuantity = (TextBox)e.Row.FindControl("GrdTxtQuantity");
                    if (GrdTxtQuantity != null)
                    {
                        GrdTxtQuantity.Text = DataBinder.Eval(e.Row.DataItem, "Quantity").ToString();
                        if (GrdTxtQuantity.Text != "")
                            footerTotalQuantity = footerTotalQuantity + Convert.ToDouble(GrdTxtQuantity.Text);
                    }

                    TextBox GrdTxtPrice = (TextBox)e.Row.FindControl("GrdTxtPrice");
                    if (GrdTxtPrice != null)
                    {
                        GrdTxtPrice.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();
                    }

                    TextBox GrdTxtTotalAmount = (TextBox)e.Row.FindControl("GrdTxtTotalAmount");
                    if (GrdTxtTotalAmount != null)
                    {
                        //GrdTxtTotalAmount.Text = DataBinder.Eval(e.Row.DataItem, "TotalAmount").ToString();
                        double quantity, price, totalAmount = 0;
                        if (GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            quantity = Convert.ToDouble(GrdTxtQuantity.Text);
                            price = Convert.ToDouble(GrdTxtPrice.Text);
                            totalAmount = (quantity * price);
                            footerTotalAmount = footerTotalAmount + totalAmount;
                            GrdTxtTotalAmount.Text = Convert.ToString(totalAmount);
                        }
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
                    e.Row.Cells[2].Text = Convert.ToString(footerTotalQuantity);
                    e.Row.Cells[4].Text = Convert.ToString(footerTotalAmount); // You can use a calculated variable here
                }
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        private void BindGridView()
        {
            try
            {
                if (ViewState["dtDeliveryInfo"] == null)
                {
                    dtDeliveryInfo = objDeliveryInformation.GetDeliveryDetails(TxtDeliveryDate.Text);
                    ViewState["dtDeliveryInfo"] = dtDeliveryInfo;
                }

                // Set initial row structure for gridview if no records found for the selected date
                if (dtDeliveryInfo == null || dtDeliveryInfo.Rows.Count == 0)
                {
                    dtDeliveryInfo = AddNewRow();
                }

                GrdDeliveryInfo.DataSource = dtDeliveryInfo;
                GrdDeliveryInfo.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ShowResult(int transactionStatus)
        {
            try
            {
                if (transactionStatus == 1)
                {
                    successAlert.Visible = true;
                    // Execute server logic here, then show the popup
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "$('#successMessageModal').modal('show');", true);
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#successMessageModal').modal('show');", true);
                }
                else
                {
                    successAlert.Visible = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["dtDeliveryInfo"] = null;
                BindGridView();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}