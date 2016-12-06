 
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
 

namespace MetopeMVCApp.Filters
{

    //public class SecurityDetailsControlFilter : ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    { 
           
    //        filterContext.Controller.ViewBag.MyPort = 33.123; 

    //        base.OnActionExecuted(filterContext);
    //    }

    //}
             
     
    public class CountriesFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            IQueryable<Country> countries;
            if ((countries = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Country>)) == null)
            {
                MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(true);
                countries = svc.ListCountry();

                filterContext.HttpContext.Cache.Insert(GetType().FullName, countries);
            }

            filterContext.Controller.ViewBag.Country_Of_Domicile = new SelectList(countries, "Country_Code", "Country_Name", filterContext.Controller.ViewBag.RecordCountryOfDomicile);//  ;;
            filterContext.Controller.ViewBag.Country_Of_Risk = new SelectList(countries, "Country_Code", "Country_Name", filterContext.Controller.ViewBag.RecordCountryOfRisk);//  ;;


            //filterContext.Controller.ViewBag.CountryOfRisk = countries;
            //filterContext.Controller.ViewBag.CountryOfDomicile = countries;
            base.OnActionExecuted(filterContext);
        }

    }

    public class CurrencyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            IQueryable<Currency> currencies;
            if ((currencies = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Currency>)) == null)
            {
                MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(true);

                currencies = svc.ListCurrencies();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, currencies);
            }

            filterContext.Controller.ViewBag.Price_Curr = new SelectList(currencies, "Currency_Code", "Currency_Name", filterContext.Controller.ViewBag.PriceCurr); // 
            filterContext.Controller.ViewBag.Asset_Currency = new SelectList(currencies, "Currency_Code", "Currency_Name", filterContext.Controller.ViewBag.AssetCurrency); // 
            filterContext.Controller.ViewBag.Trade_Currency = new SelectList(currencies, "Currency_Code", "Currency_Name", filterContext.Controller.ViewBag.TradeCurrency); // 

            base.OnActionExecuted(filterContext);
        }
    }
    public class SecurityTypesFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        { 
            IQueryable<Security_Type> secTypeCodes;
            if ((secTypeCodes = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Security_Type>)) == null)
            {
                MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(true);
                secTypeCodes = svc.ListSecTypeCode();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, secTypeCodes);
            }
            filterContext.Controller.ViewBag.Security_Type_Code = new SelectList(secTypeCodes, "Security_Type_Code", "Name", filterContext.Controller.ViewBag.SecurityTypeCode); // 
             
            base.OnActionExecuted(filterContext);
        } 
     } 
     public class ExchangesFilter : ActionFilterAttribute
     {
        public override void OnActionExecuted(ActionExecutedContext  filterContext)
        {
            IQueryable<Exchange> exchanges;
            if ((exchanges = (filterContext.HttpContext.Cache.Get(GetType().FullName) as  IQueryable<Exchange>)) == null)
            {
                MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);
                exchanges = svc.ListExchanges();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, exchanges);
            }
            filterContext.Controller.ViewBag.Primary_Exch = new SelectList(exchanges, "Exchange_Code", "Exchange_Name", filterContext.Controller.ViewBag.PrimaryExch);//  ;;
            filterContext.Controller.ViewBag.Secondary_Exch = new SelectList(exchanges, "Exchange_Code", "Exchange_Name", filterContext.Controller.ViewBag.SecondaryExch);//  ;;
             
            base.OnActionExecuted(filterContext);
        }

     }
     public class CodeMiscellaneousFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {

             IEnumerable<SelectListItem> miscCodes;
             IEnumerable<SelectListItem> miscCodesCP ;
             IEnumerable<SelectListItem> miscCodesBD;
             IEnumerable<SelectListItem> miscCodesEX ;
             IEnumerable<SelectListItem> miscCodesSH ; 
             if (
                 (miscCodes = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)
                 ) == null  ||
                  (miscCodesCP = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)
                 ) == null ||
                  (miscCodesBD = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)
                 ) == null ||
                  (miscCodesEX = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)
                 ) == null ||
                  (miscCodesSH = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)
                 ) == null   
                 )  
             {
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(true);
                 miscCodes = svc.ListMiscellanousTypes("IPFORM");
                 miscCodesCP = svc.ListMiscellanousTypes("CPFORM");
                 miscCodesBD = svc.ListMiscellanousTypes("BDAYADJ");
                 miscCodesEX = svc.ListMiscellanousTypes("EXDPERIOD");
                 miscCodesSH = svc.ListMiscellanousTypes("SHRCLASS");
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, miscCodes);
             }
             
             filterContext.Controller.ViewBag.Accrued_Income_Price_Formula = miscCodes;
             filterContext.Controller.ViewBag.Clean_Price_Formula = miscCodesCP;
             filterContext.Controller.ViewBag.Coupon_BusDay_Adjustment = miscCodesBD;
             filterContext.Controller.ViewBag.Ex_Div_Period = miscCodesEX;
             filterContext.Controller.ViewBag.Share_Class = miscCodesSH;

             base.OnActionExecuted(filterContext);
         } 
     }
     public class BenchmarkPortfolioFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         { 
             IEnumerable<SelectListItem> portfolios;
             if ((portfolios = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)) == null)
             {  
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(true);

                 portfolios = svc.ListPortfolios(filterContext.Controller.ViewBag.EntityIdScope );
                                                        //(Convert.ToDecimal(filterContext.Controller.ViewBag.EntityIdScope));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, portfolios);
             }
             filterContext.Controller.ViewBag.Benchmark_Portfolio = portfolios; 

             base.OnActionExecuted(filterContext);
         }

     }
     public class PartyFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         { 
             IEnumerable<SelectListItem> parties;
             if ((parties = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)) == null)
             {  
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(true);

                 parties = svc.ListPartyValues("CORPORATE", filterContext.Controller.ViewBag.EntityIdScope ,
                                               Convert.ToDecimal(filterContext.Controller.ViewBag.GenericEntityId));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, parties);
             }
             filterContext.Controller.ViewBag.Issuer_Code = parties; 
             filterContext.Controller.ViewBag.Ultimate_Issuer_Code = parties; 

             base.OnActionExecuted(filterContext);
         }

     }
     public class CurrencyPairFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         { 
             IEnumerable<SelectListItem> currencyPairs;
             if ((currencyPairs = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)) == null)
             {  
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(true);

                 currencyPairs = svc.ListCurrencyPairs();
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, currencyPairs);
             }
             filterContext.Controller.ViewBag.Currency_Pair_Code = currencyPairs; 

             base.OnActionExecuted(filterContext);
         }
     }
     public class TrueFalseFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         { 
             IEnumerable<SelectListItem> trueFalse;
             if ((trueFalse = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IEnumerable<SelectListItem>)) == null)
             {  
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 trueFalse = svc.ListTrueFalse();
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, trueFalse);
             }
             filterContext.Controller.ViewBag.MyTrackEOMFlagList = new SelectList(trueFalse, "Value", "Text"); 
             filterContext.Controller.ViewBag.MyCallAccountFgList = new SelectList(trueFalse, "Value", "Text");

             filterContext.Controller.ViewBag.MySysLockedList = new SelectList(trueFalse, "Value", "Text"); 

             base.OnActionExecuted(filterContext);
         }
          
     }
    
            
     
}