using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class UserMaster : Page
    {
        readonly ClsUserMaster objUserMaster = new ClsUserMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadRoles();
                BindGridView();
                ClearControls();
            }
        }

        private void LoadRoles()
        {
            try
            {
                DataTable dt = objUserMaster.GetRoles();
                ddlRole.DataSource = dt;
                ddlRole.DataTextField = "role_name";
                ddlRole.DataValueField = "role_id";
                ddlRole.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading roles: " + ex.Message, "danger");
            }
        }

        private void BindGridView()
        {
            try
            {
                DataTable dt = objUserMaster.GetUserMaster();
                grdUserMaster.DataSource = dt;
                grdUserMaster.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ShowMessage("Username and Password are required.", "warning");
                    return;
                }

                var objUser = new ClsUserMaster
                {
                    USER_ID = "U" + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8),
                    USERNAME = txtUsername.Text.Trim(),
                    PASSWORD = txtPassword.Text,
                    FULL_NAME = txtFullName.Text.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    ROLE_ID = ddlRole.SelectedValue,
                    IS_ACTIVE = chkIsActive.Checked,
                    CREATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    CREATED_AT = DateTime.Now
                };

                int result = objUserMaster.CreateUser(objUser);
                ShowResult(result);
                if (result > 0)
                {
                    ClearControls();
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e) => ClearControls();

        protected void grdUserMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdUserMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdUserMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string userId = grdUserMaster.DataKeys[e.RowIndex].Value.ToString();
                var objUser = new ClsUserMaster
                {
                    USER_ID = userId,
                    FULL_NAME = ((TextBox)grdUserMaster.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim(),
                    UPDATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int result = objUserMaster.UpdateUser(objUser);
                ShowResult(result);
                grdUserMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void grdUserMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdUserMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdUserMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string userId = grdUserMaster.DataKeys[e.RowIndex].Value.ToString();
                var objUser = new ClsUserMaster { USER_ID = userId };
                int result = objUserMaster.DeleteUser(objUser);
                ShowResult(result);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        private void ClearControls()
        {
            txtUsername.Text = txtPassword.Text = txtFullName.Text = txtEmail.Text = "";
            chkIsActive.Checked = true;
            lblMessage.Style["display"] = "none";
        }

        private void ShowResult(int rowsAffected)
        {
            ShowMessage(rowsAffected > 0 ? "Success" : "No records affected.", rowsAffected > 0 ? "success" : "warning");
        }

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }
    }
}
