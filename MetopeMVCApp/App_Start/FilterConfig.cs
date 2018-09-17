using MetopeMVCApp.Filters;
using System.Web.Mvc;

namespace ASP.MetopeNspace
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SetGenericEntityAttribute()); 
        }
    }
}