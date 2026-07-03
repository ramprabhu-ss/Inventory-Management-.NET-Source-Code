using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class GSTMaster : Page
    {
        readonly ClsGSTMaster objGSTMaster = new ClsGSTMaster();
        DataTable dtGSTMaster = new DataTable();

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
                dtGSTMaster = objGSTMaster.GetGSTMaster();
                grdGSTMaster.DataSource = dtGSTMaster;
                grdGSTMaster.DataBind();
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

                var objGST = new ClsGSTMaster
                {
                    GST_PERCENTAGE = decimal.Parse(txtGSTPercentage.Text),
                    DESCRIPTION = txtDescription.Text.Trim(),
                    EFFECTIVE_FROM = string.IsNullOrEmpty(dtEffectiveFrom.Text) ? (DateTime?)null : DateTime.Parse(dtEffectiveFrom.Text),
                    EFFECTIVE_TO = string.IsNullOrEmpty(dtEffectiveTo.Text) ? (DateTime?)null : DateTime.Parse(dtEffectiveTo.Text),
                    IS_ACTIVE = chkIsActive.Checked,
                    CREATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    CREATED_AT = DateTime.Now
                };

                int rowsAffected = objGSTMaster.CreateGST(objGST);
                ShowResult(rowsAffected);
                if (rowsAffected > 0)
                {
                    ClearControls();
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving GST: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void grdGSTMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdGSTMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdGSTMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int gstId = (int)grdGSTMaster.DataKeys[e.RowIndex].Value;
                GridViewRow row = grdGSTMaster.Rows[e.RowIndex];

                var objGST = new ClsGSTMaster
                {
                    GST_ID = gstId,
                    GST_PERCENTAGE = decimal.Parse(((TextBox)row.Cells[1].Controls[0]).Text),
                    UPDATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int rowsAffected = objGSTMaster.UpdateGST(objGST);
                ShowResult(rowsAffected);

                grdGSTMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating GST: " + ex.Message, "danger");
            }
        }

        protected void grdGSTMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdGSTMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdGSTMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int gstId = (int)grdGSTMaster.DataKeys[e.RowIndex].Value;
                var objGST = new ClsGSTMaster { GST_ID = gstId };

                int rowsAffected = objGSTMaster.DeleteGST(objGST);
                ShowResult(rowsAffected);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting GST: " + ex.Message, "danger");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtGSTPercentage.Text))
            {
                ShowMessage("GST Percentage is required.", "warning");
                return false;
            }

            if (!decimal.TryParse(txtGSTPercentage.Text, out _))
            {
                ShowMessage("Valid GST Percentage is required.", "warning");
                return false;
            }

            return true;
        }

        private void ClearControls()
        {
            txtGSTPercentage.Text = "";
            txtDescription.Text = "";
            dtEffectiveFrom.Text = "";
            dtEffectiveTo.Text = "";
            chkIsActive.Checked = true;
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
