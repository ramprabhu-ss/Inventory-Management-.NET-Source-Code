using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class ProductMaster : Page
    {
        readonly ClsProductMaster objProductMaster = new ClsProductMaster();
        readonly ClsProductCategoryMaster objCategoryMaster = new ClsProductCategoryMaster();
        DataTable dtProductMaster = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCategoryDropdown();
                BindGridView();
                ClearControls();
            }
        }

        private void LoadCategoryDropdown()
        {
            try
            {
                DataTable dtCategories = objCategoryMaster.GetProductCategory();
                ddlCategory.DataSource = dtCategories;
                ddlCategory.DataTextField = "category_name";
                ddlCategory.DataValueField = "category_id";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading categories: " + ex.Message, "danger");
            }
        }

        private void BindGridView()
        {
            try
            {
                dtProductMaster = objProductMaster.GetProductMaster();
                grdProductMaster.DataSource = dtProductMaster;
                grdProductMaster.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error binding grid: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var objProd = new ClsProductMaster
                {
                    PRODUCT_NAME = txtProductName.Text.Trim(),
                    CATEGORY_ID = int.Parse(ddlCategory.SelectedValue),
                    DESCRIPTION = txtDescription.Text.Trim(),
                    UNIT_PRICE = decimal.Parse(txtUnitPrice.Text),
                    STOCK_QUANTITY = int.Parse(txtStockQuantity.Text),
                    REORDER_LEVEL = string.IsNullOrEmpty(txtReorderLevel.Text) ? 0 : int.Parse(txtReorderLevel.Text),
                    WEIGHT = string.IsNullOrEmpty(txtWeight.Text) ? (decimal?)null : decimal.Parse(txtWeight.Text),
                    GAS_TYPE = txtGasType.Text.Trim(),
                    IS_ACTIVE = chkIsActive.Checked,
                    AVAILABLE_OUT_DELIVERY = chkAvailableOutDelivery.Checked ? "YES" : "NO",
                    FLEXI_PRICE = chkFlexiPrice.Checked ? "YES" : "NO",
                    CREATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    CREATED_AT = DateTime.Now
                };

                int rowsAffected = objProductMaster.CreateProduct(objProd);
                ShowResult(rowsAffected);
                if (rowsAffected > 0)
                {
                    ClearControls();
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving product: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void grdProductMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdProductMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdProductMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int productId = (int)grdProductMaster.DataKeys[e.RowIndex].Value;
                GridViewRow row = grdProductMaster.Rows[e.RowIndex];

                var objProd = new ClsProductMaster
                {
                    PRODUCT_ID = productId,
                    PRODUCT_NAME = ((TextBox)row.Cells[1].Controls[0]).Text.Trim(),
                    UNIT_PRICE = decimal.Parse(((TextBox)row.Cells[4].Controls[0]).Text),
                    STOCK_QUANTITY = int.Parse(((TextBox)row.Cells[5].Controls[0]).Text),
                    UPDATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int rowsAffected = objProductMaster.UpdateProduct(objProd);
                ShowResult(rowsAffected);

                grdProductMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating product: " + ex.Message, "danger");
            }
        }

        protected void grdProductMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdProductMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdProductMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int productId = (int)grdProductMaster.DataKeys[e.RowIndex].Value;
                var objProd = new ClsProductMaster { PRODUCT_ID = productId };

                int rowsAffected = objProductMaster.DeleteProduct(objProd);
                ShowResult(rowsAffected);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting product: " + ex.Message, "danger");
            }
        }

        protected void grdProductMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                // Optional: Add client-side validation or formatting for edit mode
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                ShowMessage("Product Name is required.", "warning");
                return false;
            }

            if (ddlCategory.SelectedValue == "")
            {
                ShowMessage("Please select a Category.", "warning");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUnitPrice.Text) || !decimal.TryParse(txtUnitPrice.Text, out _))
            {
                ShowMessage("Valid Unit Price is required.", "warning");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStockQuantity.Text) || !int.TryParse(txtStockQuantity.Text, out _))
            {
                ShowMessage("Valid Stock Quantity is required.", "warning");
                return false;
            }

            return true;
        }

        private void ClearControls()
        {
            txtProductName.Text = "";
            ddlCategory.SelectedValue = "";
            txtDescription.Text = "";
            txtUnitPrice.Text = "";
            txtStockQuantity.Text = "";
            txtReorderLevel.Text = "";
            txtWeight.Text = "";
            txtGasType.Text = "";
            chkIsActive.Checked = true;
            chkAvailableOutDelivery.Checked = false;
            chkFlexiPrice.Checked = false;
            lblMessage.Style["display"] = "none";
        }

        private void ShowResult(int rowsAffected)
        {
            if (rowsAffected > 0)
            {
                ShowMessage("Operation completed successfully.", "success");
            }
            else
            {
                ShowMessage("No records affected.", "warning");
            }
        }

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }
    }
}
