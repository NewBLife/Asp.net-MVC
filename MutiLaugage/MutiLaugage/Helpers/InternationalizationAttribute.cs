using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace MutiLaugage.Helpers
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string language = (string)filterContext.RouteData.Values["language"] ?? "nl";
            string culture = (string)filterContext.RouteData.Values["culture"] ?? "NL";


            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", language, culture));

            if (language == "ja")
            {
                Thread.Sleep(10000);
            }
        }
    }
}