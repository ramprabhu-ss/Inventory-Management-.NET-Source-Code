using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class CategoryMaster : Page
    {
        readonly ClsProductCategoryMaster objCategoryMaster = new ClsProductCategoryMaster();
        DataTable dtCategoryMaster = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGridView();
                ClearControls();
            }
        }

        private void BindGridView()
        {
            try
            {
                dtCategoryMaster = objCategoryMaster.GetProductCategory();
                grdCategoryMaster.DataSource = dtCategoryMaster;
                grdCategoryMaster.DataBind();
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
                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    ShowMessage("Category Name is required.", "warning");
                    return;
                }

                var objCategory = new ClsProductCategoryMaster
                {
                    CATEGORY_NAME = txtCategoryName.Text.Trim(),
                    CREATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    CREATED_AT = DateTime.Now
                };

                int rowsAffected = objCategoryMaster.CreateCategory(objCategory);
                ShowResult(rowsAffected);
                if (rowsAffected > 0)
                {
                    ClearControls();
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving category: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void grdCategoryMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdCategoryMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdCategoryMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int categoryId = (int)grdCategoryMaster.DataKeys[e.RowIndex].Value;
                GridViewRow row = grdCategoryMaster.Rows[e.RowIndex];

                var objCategory = new ClsProductCategoryMaster
                {
                    CATEGORY_ID = categoryId,
                    CATEGORY_NAME = ((TextBox)row.Cells[1].Controls[0]).Text.Trim(),
                    UPDATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int rowsAffected = objCategoryMaster.UpdateCategory(objCategory);
                ShowResult(rowsAffected);

                grdCategoryMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating category: " + ex.Message, "danger");
            }
        }

        protected void grdCategoryMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdCategoryMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdCategoryMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int categoryId = (int)grdCategoryMaster.DataKeys[e.RowIndex].Value;
                var objCategory = new ClsProductCategoryMaster { CATEGORY_ID = categoryId };

                int rowsAffected = objCategoryMaster.DeleteCategory(objCategory);
                ShowResult(rowsAffected);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting category: " + ex.Message, "danger");
            }
        }

        private void ClearControls()
        {
            txtCategoryName.Text = "";
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
