using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string user = txtUsername.Text.Trim();
                string pass = txtPassword.Text.Trim();

                // Simple static validation (Replace this with a Database check!)
                if (user == "admin" && pass == "admin")
                {
                    // Create a session to keep the user logged in
                    Session["UserRole"] = "Guest";
                    Session["UserId"] = user;
                    Session["SessionStartTime"] = DateTime.Now;
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid Username or Password.";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}