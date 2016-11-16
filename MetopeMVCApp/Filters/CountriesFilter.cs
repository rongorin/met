using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
 

namespace MetopeMVCApp.Filters
{
    public class CountriesFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services();

              IEnumerable<SelectListItem> countries ;
              if ((countries = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)) == null)
             {
                countries = svc.ListCountry();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, countries);
              }
              filterContext.Controller.ViewBag.Country_Of_Risk = countries;
              filterContext.Controller.ViewBag.Country_Of_Domicile = countries;
             
            base.OnActionExecuting(filterContext);
        }

    }
    public class SecurityTypesFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services();

            IEnumerable<SelectListItem> secTypeCodes;
            if ((secTypeCodes = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)) == null)
            {
                secTypeCodes = svc.ListSecTypeCode();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, secTypeCodes);
            }
            filterContext.Controller.ViewBag.Security_Type_Code = secTypeCodes;  

            base.OnActionExecuting(filterContext);
        }

    }
     

     public class ExchangesFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services();

            IEnumerable<SelectListItem> exchanges;
            if ((exchanges = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)) == null)
            {
                exchanges = svc.ListSecTypeCode();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, exchanges);
            }
            filterContext.Controller.ViewBag.Primary_Exch = exchanges;
            filterContext.Controller.ViewBag.Secondary_Exch = exchanges; 

            base.OnActionExecuting(filterContext);
        }

    }
}