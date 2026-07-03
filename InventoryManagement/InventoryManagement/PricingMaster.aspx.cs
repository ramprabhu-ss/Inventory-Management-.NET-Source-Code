using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class PricingMaster : Page
    {
        readonly ClsPricingMaster objPricingMaster = new ClsPricingMaster();
        readonly ClsProductMaster objProductMaster = new ClsProductMaster();
        readonly ClsGSTMaster objGSTMaster = new ClsGSTMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDropdowns();
                BindGridView();
            }
        }

        private void LoadDropdowns()
        {
            try
            {
                // Load products
                DataTable dtProducts = objProductMaster.GetProductMaster();
                ddlProduct.DataSource = dtProducts;
                ddlProduct.DataTextField = "ProductName";
                ddlProduct.DataValueField = "ProductID";
                ddlProduct.DataBind();
                ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));

                // Load GST
                DataTable dtGST = objGSTMaster.GetGSTMaster();
                ddlGST.DataSource = dtGST;
                ddlGST.DataTextField = "gst_percentage";
                ddlGST.DataValueField = "gst_id";
                ddlGST.DataBind();
                ddlGST.Items.Insert(0, new ListItem("-- Select GST --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading dropdowns: " + ex.Message, "danger");
            }
        }

        private void BindGridView()
        {
            try
            {
                DataTable dt = objPricingMaster.GetPricing();
                grdPricingMaster.DataSource = dt;
                grdPricingMaster.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading pricing: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlProduct.SelectedValue) || string.IsNullOrEmpty(ddlGST.SelectedValue))
                {
                    ShowMessage("Please select Product and GST.", "warning");
                    return;
                }

                var objPricing = new ClsPricingMaster
                {
                    PRODUCT_ID = int.Parse(ddlProduct.SelectedValue),
                    BASE_PRICE = decimal.Parse(txtBasePrice.Text),
                    GST_ID = int.Parse(ddlGST.SelectedValue),
                    EFFECTIVE_FROM = DateTime.Now,
                    EFFECTIVE_STATUS = "ACTIVE",
                    CREATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    CREATED_AT = DateTime.Now
                };

                int result = objPricingMaster.CreatePricing(objPricing);
                ShowResult(result);
                if (result > 0)
                {
                    txtBasePrice.Text = "";
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void grdPricingMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdPricingMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdPricingMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int pricingId = (int)grdPricingMaster.DataKeys[e.RowIndex].Value;
                var objPricing = new ClsPricingMaster
                {
                    PRICING_ID = pricingId,
                    UPDATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int result = objPricingMaster.UpdatePricing(objPricing);
                ShowResult(result);
                grdPricingMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating: " + ex.Message, "danger");
            }
        }

        protected void grdPricingMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdPricingMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdPricingMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int pricingId = (int)grdPricingMaster.DataKeys[e.RowIndex].Value;
                var objPricing = new ClsPricingMaster { PRICING_ID = pricingId };
                int result = objPricingMaster.DeletePricing(objPricing);
                ShowResult(result);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting: " + ex.Message, "danger");
            }
        }

        private void ShowResult(int rowsAffected)
        {
            ShowMessage(rowsAffected > 0 ? "Operation completed successfully." : "No records affected.", rowsAffected > 0 ? "success" : "warning");
        }

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }
    }
}
