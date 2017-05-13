using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace MutiLaugage.Helpers
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //string language = (string)filterContext.RouteData.Values["language"] ?? "nl";
            //string cultureName = (string)filterContext.RouteData.Values["culture"] ?? "en-US";
            string cultureName = filterContext.RouteData.Values["culture"] as string;


            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        }
    }
}