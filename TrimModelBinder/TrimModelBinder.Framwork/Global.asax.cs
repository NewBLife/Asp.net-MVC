using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TrimModelBinder.Framwork
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new ModelBinders.TrimModelBinder();
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(string), new ModelBinders.TrimModelBinder());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
