using SASSADirectCapture.Services;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace SASSADirectCapture
{
    public class Global : HttpApplication
    {
        #region Private Methods

        private void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
        }
        private void Session_Start(object sender, EventArgs e)
        {
            //HttpContext.Current.Session["us"] = new UserSession();
        }
        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        #endregion Private Methods
    }
}
