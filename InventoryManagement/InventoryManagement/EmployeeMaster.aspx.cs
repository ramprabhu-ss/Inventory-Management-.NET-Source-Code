using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class EmployeeMaster : Page
    {
        readonly ClsEmployeeMaster objEmployeeMaster = new ClsEmployeeMaster();
        DataTable dtEmployeeMaster = new DataTable();

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
                dtEmployeeMaster = objEmployeeMaster.GetEmployeeMaster();
                grdEmployeeMaster.DataSource = dtEmployeeMaster;
                grdEmployeeMaster.DataBind();
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

                var objEmp = new ClsEmployeeMaster
                {
                    EMP_NAME = txtEmployeeName.Text.Trim(),
                    DESIGNATION = txtDesignation.Text.Trim(),
                    DEPARTMENT = txtDepartment.Text.Trim(),
                    MOBILE_NO = txtMobileNo.Text.Trim(),
                    EMAIL_ID = txtEmailId.Text.Trim(),
                    ADDRESS = txtAddress.Text.Trim(),
                    CITY = txtCity.Text.Trim(),
                    STATE = txtState.Text.Trim(),
                    PINCODE = txtPincode.Text.Trim(),
                    JOIN_DATE = string.IsNullOrEmpty(dtJoinDate.Text) ? (DateTime?)null : DateTime.Parse(dtJoinDate.Text),
                    SALARY = string.IsNullOrEmpty(txtSalary.Text) ? (decimal?)null : decimal.Parse(txtSalary.Text),
                    STATUS = ddlStatus.SelectedValue,
                    CREATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    CREATED_AT = DateTime.Now
                };

                int rowsAffected = objEmployeeMaster.CreateEmployee(objEmp);
                ShowResult(rowsAffected);
                if (rowsAffected > 0)
                {
                    ClearControls();
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving employee: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void grdEmployeeMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdEmployeeMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdEmployeeMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int empCode = (int)grdEmployeeMaster.DataKeys[e.RowIndex].Value;
                GridViewRow row = grdEmployeeMaster.Rows[e.RowIndex];

                var objEmp = new ClsEmployeeMaster
                {
                    EMP_CODE = empCode,
                    EMP_NAME = ((TextBox)row.Cells[1].Controls[0]).Text.Trim(),
                    DESIGNATION = ((TextBox)row.Cells[2].Controls[0]).Text.Trim(),
                    DEPARTMENT = ((TextBox)row.Cells[3].Controls[0]).Text.Trim(),
                    UPDATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int rowsAffected = objEmployeeMaster.UpdateEmployee(objEmp);
                ShowResult(rowsAffected);

                grdEmployeeMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating employee: " + ex.Message, "danger");
            }
        }

        protected void grdEmployeeMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdEmployeeMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdEmployeeMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int empCode = (int)grdEmployeeMaster.DataKeys[e.RowIndex].Value;
                var objEmp = new ClsEmployeeMaster { EMP_CODE = empCode };

                int rowsAffected = objEmployeeMaster.DeleteEmployee(objEmp);
                ShowResult(rowsAffected);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting employee: " + ex.Message, "danger");
            }
        }

        protected void grdEmployeeMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                // Optional: Add client-side validation or formatting for edit mode
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeName.Text))
            {
                ShowMessage("Employee Name is required.", "warning");
                return false;
            }

            return true;
        }

        private void ClearControls()
        {
            txtEmployeeName.Text = "";
            txtDesignation.Text = "";
            txtDepartment.Text = "";
            txtMobileNo.Text = "";
            txtEmailId.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtPincode.Text = "";
            dtJoinDate.Text = "";
            txtSalary.Text = "";
            ddlStatus.SelectedValue = "ACTIVE";
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
