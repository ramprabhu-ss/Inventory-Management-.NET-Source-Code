using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InventoryManagement.IL;

namespace InventoryManagement
{
    public partial class Area : System.Web.UI.Page
    {
        ClsAreaMaster objAreaMaster = new ClsAreaMaster();
        DataTable dtAreaMaster = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*protected void gvAreaMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvAreaMaster.Rows[RowIndex];

                if (e.CommandName == "Edit")
                {
                    txtAreaId.Text = row.Cells[1].Text;
                    txtAreaName.Text = row.Cells[2].Text;
                    txtZipCode.Text = row.Cells[3].Text;

                }
                else if (e.CommandName == "Delete")
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                objAreaMaster.AREA_ID = txtAreaId.Text;
                objAreaMaster.AREA_NAME = txtAreaName.Text;
                objAreaMaster.ZIP_CODE = txtZipCode.Text;
                objAreaMaster.CREATED_BY = "user";
                objAreaMaster.CREATED_AT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                objAreaMaster.CreateArea(objAreaMaster);
                ViewState["dtAreaMaster"] = null;
                ClearControls();
                BindGridView();
                //Response.Redirect("~/AreaMaster.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["dtAreaMaster"] = null;
                Response.Redirect("~/Area.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdAreaMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdAreaMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdAreaMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find the control in the EditItemTemplate
            GridViewRow row = grdAreaMaster.Rows[e.RowIndex];
            //string AreaId = ((TextBox)row.FindControl("grdTxtAreaId")).Text;
            //string AreaName = ((TextBox)row.FindControl("grdTxtAreaName")).Text;
            //string ZipCode = ((TextBox)row.FindControl("grdTxtZipCode")).Text;

            // Code to update data source using e.RowIndex or DataKeys[e.RowIndex]
            // 2. Perform the actual update in your database
            objAreaMaster.AREA_ID = ((TextBox)row.FindControl("grdTxtAreaId")).Text;
            objAreaMaster.AREA_NAME = ((TextBox)row.FindControl("grdTxtAreaName")).Text;
            objAreaMaster.ZIP_CODE = ((TextBox)row.FindControl("grdTxtZipCode")).Text;
            objAreaMaster.UPDATED_BY = "user";
            objAreaMaster.UPDATED_AT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            objAreaMaster.UpdateArea(objAreaMaster);

            grdAreaMaster.EditIndex = -1; // Exit edit mode
            ViewState["dtAreaMaster"] = null;
            BindGridView();
        }

        protected void grdAreaMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdAreaMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdAreaMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 1. Get the ID of the record to delete from DataKeys
            GridViewRow row = grdAreaMaster.Rows[e.RowIndex];
            string AreaId = grdAreaMaster.DataKeys[e.RowIndex].Value.ToString();

            // 2. Perform the actual deletion in your database
            objAreaMaster.AREA_ID = AreaId; // ((TextBox)row.FindControl("grdTxtAreaId")).Text;
            objAreaMaster.DeleteArea(objAreaMaster);

            // 3. Refresh the GridView to show updated data
            ViewState["dtAreaMaster"] = null;
            BindGridView();
        }

        protected void grdAreaMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find the Delete button (it is usually the first control in the CommandField cell)
                foreach (Control control in e.Row.Cells[e.Row.Cells.Count - 1].Controls)
                {
                    if (control is LinkButton && ((LinkButton)control).CommandName == "Delete")
                    {
                        ((LinkButton)control).OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                    }
                }
            }
        }

        private void BindGridView()
        {
            if (ViewState["dtAreaMaster"] == null)
            {
                dtAreaMaster = objAreaMaster.GetAreaMaster();
                ViewState["dtAreaMaster"] = dtAreaMaster;
            }
            else
            {
                dtAreaMaster = (DataTable)ViewState["dtAreaMaster"];
            }

            if (dtAreaMaster != null && dtAreaMaster.Rows.Count > 0)
            {
                grdAreaMaster.DataSource = dtAreaMaster;
                ViewState["dtAreaMaster"] = dtAreaMaster;
            }
            else
            {
                grdAreaMaster.DataSource = null;
                ViewState["dtAreaMaster"] = null;
            }
            grdAreaMaster.DataBind();
        }

        protected void grdAreaMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Set the GridView to the new page index
                grdAreaMaster.PageIndex = e.NewPageIndex;

                // Re-bind the data source to refresh the grid view
                BindGridView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdAreaMaster_Sorting(object sender, GridViewSortEventArgs e)
        {
            string SortDir = string.Empty;
            /*if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                SortDir = "Desc";
            }
            else
            {
                dir = SortDirection.Ascending;
                SortDir = "Asc";
            }

            Da
            ViewState["dtAreaMaster"]
            DataView sortedView = new DataView(dt);
            sortedView.Sort = e.SortExpression + " " + SortDir;
            Datagrid1.DataSource = sortedView;
            Datagrid1.DataBind();*/
        }

        private void ClearControls()
        {
            txtAreaId.Text = "";
            txtAreaName.Text = "";
            txtZipCode.Text = "";
        }
    }
}