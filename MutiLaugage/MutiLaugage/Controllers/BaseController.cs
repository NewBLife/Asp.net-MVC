using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MutiLaugage.Helpers;

namespace MutiLaugage.Controllers
{
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            // Cookie Language setting
            string cultureName = string.Empty;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                                    null;

            //// RouteData Language setting
            ////string language = RouteData.Values["language"].ToString() ?? "nl";
            ////string culture = RouteData.Values["culture"].ToString() ?? "NL";
            //string cultureName = RouteData.Values["culture"] as string;

            //// Attempt to read the culture cookie from Request
            //if (cultureName == null)
            //    cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            //// Validate culture name
            ////cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


            //if (RouteData.Values["culture"] as string != cultureName)
            //{

            //    // Force a valid culture in the URL
            //    RouteData.Values["culture"] = cultureName.ToLowerInvariant(); // lower case too

            //    // Redirect user
            //    Response.RedirectToRoute(RouteData.Values);
            //}


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;



            return base.BeginExecuteCore(callback, state);
        }
    }
}