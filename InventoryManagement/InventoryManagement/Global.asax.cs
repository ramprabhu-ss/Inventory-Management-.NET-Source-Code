using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace InventoryManagement
{
    public class Global : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //if (Request.AppRelativeCurrentExecutionFilePath == "~/")
                //Response.Redirect("~/AreaMaster.aspx");
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Response.Redirect("~/About.aspx");
        }
    }
}