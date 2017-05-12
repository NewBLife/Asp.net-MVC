using System.Web.Mvc;
using MutiLaugage.Helpers;

namespace MutiLaugage
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InternationalizationAttribute());
        }
    }
}
