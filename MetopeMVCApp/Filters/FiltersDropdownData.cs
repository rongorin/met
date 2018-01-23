﻿
using ASP.MetopeNspace.Models;
using Metope.DAL;
using MetopeMVCApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing; 
 

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
     
    //Removed . Attempted this: http://benfoster.io/blog/automatic-modelstate-validation-in-aspnet-mvc
    //   Also see the Filters ModelStateTempDataTransfer and ImportModelStateFromTempDataAttribute
    //public class ValidateModelStateAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        var viewData = filterContext.Controller.ViewData;

    //        if (!viewData.ModelState.IsValid)
    //        {
    //            filterContext.Result = new ViewResult
    //            {
    //                ViewData = viewData,
    //                TempData = filterContext.Controller.TempData
    //            };
    //        }

    //        base.OnActionExecuting(filterContext);
    //    }
    //}
    public class SetGenericEntityAttribute : ActionFilterAttribute
    {  
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string genEntityID;

            if ((genEntityID = (filterContext.HttpContext.Cache.Get(GetType().FullName) as string)) == null)
            {
                 genEntityID  =  ConfigurationManager.AppSettings["GenericEntityId"]; 
                filterContext.HttpContext.Cache.Insert(GetType().FullName, genEntityID);
  
            }
            filterContext.Controller.ViewBag.genericEntity = Convert.ToDecimal(genEntityID);  
        }
    }
    public class CustomEntityAuthoriseFilter : ActionFilterAttribute
    {
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        string userId = HttpContext.Current.User.Identity.GetUserId(); 

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //get the Entity passed as parameter from the Uri
            decimal ParmEntity = Convert.ToDecimal(filterContext.ActionParameters.SingleOrDefault(ap => ap.Key == "EntityId").Value);
            //get generic Entity
            decimal GenericEntity = (decimal)filterContext.Controller.ViewBag.genericEntity;
            //get the Entity of user
            decimal usersEntity = Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute"));

            if (ParmEntity > 0)  
            { 
                if (GenericEntity != ParmEntity &&
                    usersEntity   != ParmEntity)
                {
                    filterContext.Result = new RedirectToRouteResult(
                          new RouteValueDictionary{{ "controller", "Account" },
                                                    { "action", "Login" } 
                                                });
                    //filterContext.Result = new RedirectToRouteResult(
                    //                new RouteValueDictionary { 
                    //                            { "action", "Index" }, 
                    //                            { "controller", "Unauthorised" } }); 
                }
            }
            //decimal refEntityIdScope = (decimal)filterContext.Controller.ViewBag.EntityIdScope;  
            base.OnActionExecuting(filterContext);
        } 
    }

    public class SetAllowedEntityIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string myEntityID;
            string myUserName="";  
            if ((myEntityID = (filterContext.HttpContext.Cache.Get(GetType().FullName) as string)) == null)
             {
               UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId());

                myEntityID = currentUser.EntityIdScope.ToString();
                myUserName = currentUser.UserName.ToString();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, myEntityID);
                  
            }
            filterContext.Controller.ViewBag.EntityId = Convert.ToDecimal(myEntityID);
            filterContext.Controller.ViewBag.Username = myUserName;

            base.OnActionExecuting(filterContext);
        } 
        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    string myEntityID;
        //    var currentUser = manager.FindById(HttpContext.Current.User.Identity.GetUserId());

        //    if ((myEntityID = (filterContext.HttpContext.Cache.Get(GetType().FullName) as string)) == null)
        //    {
        //        myEntityID = currentUser.EntityIdScope.ToString();
        //        filterContext.HttpContext.Cache.Insert(GetType().FullName, myEntityID);

        //    }
        //    filterContext.Controller.ViewBag.mainEntity = Convert.ToDecimal(myEntityID);

        //    base.OnActionExecuted(filterContext);
        //}

    }
     
    public class CountryFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        { 
            IQueryable<Country> countries;
            if ((countries = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Country>)) == null)
            { 
                MetopeMVCApp.Services.IServices svc = new MetopeMVCApp.Services.Services(false);
                countries = svc.ListCountry(); 
                filterContext.HttpContext.Cache.Insert(GetType().FullName, countries);
            }

            filterContext.Controller.ViewBag.Country_Of_Domicile = new SelectList(countries, "Country_Code", "Country_Name", filterContext.Controller.ViewBag.RecordCountryOfDomicile);//  ;;
            filterContext.Controller.ViewBag.Country_Of_Risk = new SelectList(countries, "Country_Code", "Country_Name", filterContext.Controller.ViewBag.RecordCountryOfRisk);//  ;;
            filterContext.Controller.ViewBag.Country_Code = new SelectList(countries, "Country_Code", "Country_Name", filterContext.Controller.ViewBag.RecordCountryOfDomicile);//  ;;
               
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
                MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);
                    
                currencies = svc.ListCurrencies();
                filterContext.HttpContext.Cache.Insert(GetType().FullName, currencies);
            }

            filterContext.Controller.ViewBag.Price_Curr = new SelectList(currencies, "Currency_Code", "Currency_Name", filterContext.Controller.ViewBag.PriceCurr); // 
            filterContext.Controller.ViewBag.Asset_Currency = new SelectList(currencies, "Currency_Code", "Currency_Name", filterContext.Controller.ViewBag.AssetCurrency); // 
            filterContext.Controller.ViewBag.Trade_Currency = new SelectList(currencies, "Currency_Code", "Currency_Name", filterContext.Controller.ViewBag.TradeCurrency); // 
            filterContext.Controller.ViewBag.Dividend_Currency_Code = new SelectList(currencies, "Currency_Code", "Currency_Name", filterContext.Controller.ViewBag.DividendCurrencyCode); // 


            //List<SelectListItem> IntsForDivSplit = new List<SelectListItem> {
            //            new SelectListItem { Text = "ACTIVE", Value = "ACTIVE" },
            //            new SelectListItem { Text = "SUSPENDED", Value = "SUSPENDED" },
            //            new SelectListItem { Text = "INACTIVE", Value = "INACTIVE" }	 };

            
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
                MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);
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
            filterContext.Controller.ViewBag.Primary_Exch = new SelectList(exchanges, "Exchange_Code", "Exchange_Name", filterContext.Controller.ViewBag.PrimaryExch); 
            filterContext.Controller.ViewBag.Secondary_Exch = new SelectList(exchanges, "Exchange_Code", "Exchange_Name", filterContext.Controller.ViewBag.SecondaryExch); 
             
            base.OnActionExecuted(filterContext);
        }

     }
     public class CodeMiscellaneousFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {

             IQueryable<Code_Miscellaneous> miscCodes;
             IQueryable<Code_Miscellaneous> miscCodesCP;
             IQueryable<Code_Miscellaneous> miscCodesBD;
             IQueryable<Code_Miscellaneous> miscCodesEX;
             IQueryable<Code_Miscellaneous> miscCodesSH; 
             if (
                 (miscCodes = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Code_Miscellaneous>)
                 ) == null  ||
                  (miscCodesCP = (filterContext.HttpContext.Cache.Get(GetType().FullName + "CP") as IQueryable<Code_Miscellaneous>)
                 ) == null ||
                  (miscCodesBD = (filterContext.HttpContext.Cache.Get(GetType().FullName + "BD") as IQueryable<Code_Miscellaneous>)
                 ) == null ||
                  (miscCodesEX = (filterContext.HttpContext.Cache.Get(GetType().FullName + "EX") as IQueryable<Code_Miscellaneous>)
                 ) == null ||
                  (miscCodesSH = (filterContext.HttpContext.Cache.Get(GetType().FullName + "SH") as IQueryable<Code_Miscellaneous>)
                 ) == null   
                 )  
             {
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);
                 miscCodes = svc.ListMiscellanousTypes("IPFORM");  
                 miscCodesCP = svc.ListMiscellanousTypes("CPFORM"); 
                 miscCodesBD = svc.ListMiscellanousTypes("BDAYADJ"); 
                 miscCodesEX = svc.ListMiscellanousTypes("EXDPERIOD") ;
                 miscCodesSH = svc.ListMiscellanousTypes("SHRCLASS");
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, miscCodes);
                 filterContext.HttpContext.Cache.Insert(GetType().FullName + "CP", miscCodesCP);
                 filterContext.HttpContext.Cache.Insert(GetType().FullName + "BD", miscCodesBD);
                 filterContext.HttpContext.Cache.Insert(GetType().FullName + "EX", miscCodesEX);
                 filterContext.HttpContext.Cache.Insert(GetType().FullName + "SH", miscCodesSH);
             }
             filterContext.Controller.ViewBag.Ex_Div_Period = new SelectList(miscCodesEX, "MisCode", "MisCode_Description", filterContext.Controller.ViewBag.ExDivPeriod);

             filterContext.Controller.ViewBag.Accrued_Income_Price_Formula = new SelectList(miscCodes, "MisCode", "MisCode_Description", filterContext.Controller.ViewBag.AccruedIncomePriceFormula);
             filterContext.Controller.ViewBag.Share_Class = new SelectList(miscCodesSH, "MisCode", "MisCode_Description", filterContext.Controller.ViewBag.ShareClass); 
             filterContext.Controller.ViewBag.Coupon_BusDay_Adjustment = new SelectList(miscCodesBD, "MisCode", "MisCode_Description", filterContext.Controller.ViewBag.CouponBusDayAdjustment);
             filterContext.Controller.ViewBag.Clean_Price_Formula = new SelectList(miscCodesCP, "MisCode", "MisCode_Description", filterContext.Controller.ViewBag.CleanPriceFormula);
             
             //filterContext.Controller.ViewBag.Accrued_Income_Price_Formula = miscCodes;
             //filterContext.Controller.ViewBag.Clean_Price_Formula = miscCodesCP;
             //filterContext.Controller.ViewBag.Coupon_BusDay_Adjustment = miscCodesBD;
             //filterContext.Controller.ViewBag.Ex_Div_Period = miscCodesEX;
             //filterContext.Controller.ViewBag.Share_Class = miscCodesSH;

             base.OnActionExecuted(filterContext);
         } 
     }

     public class  PortfoliosFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             IQueryable<Portfolio> portfolios;
             if ((portfolios = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Portfolio>)) == null)
             {
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 portfolios = svc.ListPortfolios(Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute")), "ACTIVE");
                 //(Convert.ToDecimal(filterContext.Controller.ViewBag.EntityIdScope));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, portfolios);
             }
             filterContext.Controller.ViewBag.Portfolio_Code = new SelectList(portfolios, "Portfolio_Code", "Portfolio_Name", filterContext.Controller.ViewBag.myPortfolioCode);

             base.OnActionExecuted(filterContext);
         }

     } 
     public class BenchmarkPortfolioFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             IQueryable<Portfolio> portfolios;
             if ((portfolios = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Portfolio>)) == null)
             {  
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 portfolios = svc.ListPortfolios(Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute")), "ACTIVE");
                                                 //(Convert.ToDecimal(filterContext.Controller.ViewBag.EntityIdScope));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, portfolios);
             }
             filterContext.Controller.ViewBag.Benchmark_Portfolio = new SelectList(portfolios, "Portfolio_Code", "Portfolio_Name", filterContext.Controller.ViewBag.BenchmarkPortfolio); 

             base.OnActionExecuted(filterContext);
         }

     } 
     public class SecuritiesFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             IQueryable<Security_Detail> secs;
             if ((secs = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Security_Detail>)) == null)
             {
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 secs = svc.ListSecurities(Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute")), 
                                           Convert.ToDecimal(filterContext.Controller.ViewBag.genericEntity),
                                           "FXRATE");
                 //(Convert.ToDecimal(filterContext.Controller.ViewBag.EntityIdScope));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, secs);
             }
             filterContext.Controller.ViewBag.Dividend_FX_Security_ID = new SelectList(secs, "Security_ID", "Security_Name", filterContext.Controller.ViewBag.DividendFXSecurityID);
             filterContext.Controller.ViewBag.Securities_Select = new SelectList(secs, "Security_ID", "Security_Name", filterContext.Controller.ViewBag.SecuritiesSelect);

             base.OnActionExecuted(filterContext);
         }

     }
     public class AllSecuritiesFilter : ActionFilterAttribute
     {
         //This Action filter obtains all the securities for the inscope Entity only. 
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             IQueryable<Security_Detail> secs;
             if ((secs = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Security_Detail>)) == null)
             {
                 bool getInScopeOnly = true;
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 secs = svc.ListSecurities(Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute")),
                                           Convert.ToDecimal(filterContext.Controller.ViewBag.genericEntity),
                                           "", getInScopeOnly);
                 //(Convert.ToDecimal(filterContext.Controller.ViewBag.EntityIdScope));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, secs);
             } 
             filterContext.Controller.ViewBag.Securities_All = new SelectList(secs, "Security_ID", "Security_Name", filterContext.Controller.ViewBag.SecuritiesAll);
                                      //ViewBag.ActionStatusId = new SelectList(repository.GetAll<ActionStatus>(), "ActionStatusId", "Name", myAction.ActionStatusId);
             base.OnActionExecuted(filterContext);
         }

     }

     public class   AllSecuritiesInclGenericFilter : ActionFilterAttribute
     {
         //This Action filter obtains all the securities for both inScope and Generic Entitys . 
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             IQueryable<Security_Detail> secs;
             if ((secs = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Security_Detail>)) == null)
             {
                 bool getInScopeOnly = false;
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 secs = svc.ListSecurities(Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute")),
                                           Convert.ToDecimal(filterContext.Controller.ViewBag.genericEntity),
                                           "", getInScopeOnly);
                 //(Convert.ToDecimal(filterContext.Controller.ViewBag.EntityIdScope));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, secs);
             }
             filterContext.Controller.ViewBag.Securities_All = new SelectList(secs, "Security_ID", "Security_Name", filterContext.Controller.ViewBag.SecuritiesAll);
             //ViewBag.ActionStatusId = new SelectList(repository.GetAll<ActionStatus>(), "ActionStatusId", "Name", myAction.ActionStatusId);
             base.OnActionExecuted(filterContext);
         }

     }
     public class PartyFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {   
             IQueryable<Party> parties;
             if ((parties = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Party>)) == null)
             {
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 parties = svc.ListPartyValues("CORPORATE", Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute")),
                                               Convert.ToDecimal(filterContext.Controller.ViewBag.genericEntity));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, parties);
             } 

             filterContext.Controller.ViewBag.Ultimate_Issuer_Code =
                            new SelectList(parties, "Party_Code", "Party_Name", filterContext.Controller.ViewBag.UltimateIssuerCode); 
             filterContext.Controller.ViewBag.Issuer_Code = 
                            new SelectList(parties, "Party_Code", "Party_Name", filterContext.Controller.ViewBag.IssuerCode); 

             base.OnActionExecuted(filterContext);
         }

     }

     public class PartyFilterIssr : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             IQueryable<Party> parties;
             if ((parties = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Party>)) == null)
             {
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 parties = svc.ListPartyAllIssuers( Convert.ToDecimal(filterContext.HttpContext.Cache.Get("MetopeMVCApp.Filters.SetAllowedEntityIdAttribute")),
                                               Convert.ToDecimal(filterContext.Controller.ViewBag.genericEntity));
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, parties);
             }

             filterContext.Controller.ViewBag.Party_Code =
                            new SelectList(parties, "Party_Code", "Party_Name", filterContext.Controller.ViewBag.PartyCode);
 

             base.OnActionExecuted(filterContext);
         }

     }
     public class CurrencyPairFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             //IEnumerable<SelectListItem> currencyPairs;
             IQueryable<Currency_Pair> currencyPairs;
             if ((currencyPairs = (filterContext.HttpContext.Cache.Get(GetType().FullName) as IQueryable<Currency_Pair>)) == null)
             {  
                 MetopeMVCApp.Services.Services svc = new MetopeMVCApp.Services.Services(false);

                 currencyPairs = svc.ListCurrencyPairs();
                 filterContext.HttpContext.Cache.Insert(GetType().FullName, currencyPairs);
             } 
             filterContext.Controller.ViewBag.Currency_Pair_Code = new SelectList(currencyPairs, "Currency_Pair_Code", "Currency_Pair_Code", filterContext.Controller.ViewBag.CurrencyPairCode); 

             base.OnActionExecuted(filterContext);
         }
     }
     public class TrueFalseFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             List<SelectListItem> securityStati = new List<SelectListItem> {
						new SelectListItem { Text = "ACTIVE", Value = "ACTIVE" },
						new SelectListItem { Text = "SUSPENDED", Value = "SUSPENDED" },
						new SelectListItem { Text = "INACTIVE", Value = "INACTIVE" }	 };

             var trueFalse = new List<SelectListItem>();
             trueFalse.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
             trueFalse.Add(new SelectListItem { Text = "False", Value = bool.FalseString });

             filterContext.Controller.ViewBag.Track_EOM_Flag = new SelectList(trueFalse, "Value", "Text", filterContext.Controller.ViewBag.MyTrackEOMFlagList);
             filterContext.Controller.ViewBag.Call_Account_Flag = new SelectList(trueFalse, "Value", "Text", filterContext.Controller.ViewBag.MyCallAccountFgList);
             filterContext.Controller.ViewBag.System_Locked = new SelectList(trueFalse, "Value", "Text", filterContext.Controller.ViewBag.MySysLockedList);
             filterContext.Controller.ViewBag.Security_Status = new SelectList(securityStati, "Value", "Text", filterContext.Controller.ViewBag.MySecurityStatus);

             base.OnActionExecuted(filterContext);
         }
     } 
     public class FinancialsType : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             List<SelectListItem> fType = new List<SelectListItem> {
						new SelectListItem { Text = "Interim", Value = "I" },
						new SelectListItem { Text = "Final", Value = "F" } ,
						new SelectListItem { Text = "Special", Value = "S" },
						new SelectListItem { Text = "Exceptional", Value = "E" } };


             filterContext.Controller.ViewBag.Financials_Type = new SelectList(fType, "Value", "Text", filterContext.Controller.ViewBag.FinancialsType);
             filterContext.Controller.ViewBag.Dividend_Type = new SelectList(fType, "Value", "Text", filterContext.Controller.ViewBag.DividendType); 

             base.OnActionExecuted(filterContext);
         }
          
     }
     public class ActualForecastIndicatoreFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             List<SelectListItem> fAIndicator = new List<SelectListItem> {
						new SelectListItem { Text = "Actual", Value = "A" },
						new SelectListItem { Text = "Forecast", Value = "F" } };

             filterContext.Controller.ViewBag.Actual_Forecast_Indicator = new SelectList(fAIndicator, "Value", "Text", filterContext.Controller.ViewBag.ActualForecastIndicator);

             base.OnActionExecuted(filterContext);
         }

     }
     public class LongShortIndicatorFilter : ActionFilterAttribute
     {
         public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
             List<SelectListItem> lSInd = new List<SelectListItem> {
						new SelectListItem { Text = "Long", Value = "L" },
						new SelectListItem { Text = "Short", Value = "S" } };

             filterContext.Controller.ViewBag.Long_Short_Indicator = new SelectList(lSInd, "Value", "Text", filterContext.Controller.ViewBag.LongShortInd); 

             base.OnActionExecuted(filterContext);
         }
          
     }
     // get clever with loading the model in tempData http://benfoster.io/blog/automatic-modelstate-validation-in-aspnet-mvc
     //public class ValidateModelStateAttribute : ActionFilterAttribute
     //{
     //    public override void OnActionExecuting(ActionExecutingContext filterContext)
     //    {
     //        var viewData = filterContext.Controller.ViewData;

     //        if (!viewData.ModelState.IsValid)
     //        {
     //            filterContext.Result = new ViewResult
     //            {
     //                ViewData = viewData,
     //                TempData = filterContext.Controller.TempData
     //            };
     //        }

     //        base.OnActionExecuting(filterContext);
     //    }
     //}            
     
}