using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.SessionState;

namespace Backload.HowTo.ASPNET
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Register files bundles
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BackloadConfig.Initialize();
        }
    }
}