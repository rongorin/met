using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using MetopeMVCApp.Data;
using System.Configuration;
using MetopeMVCApp.Data.Repositories;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Data.GenericRepository;
namespace MetopeMVCApp.Controllers
{
    [AuthoriseGenericId] 
    public class SecurityDetailController : Controller
    {
        //private IMetopeDbEntities db11;  

        private readonly ISecurityDetailRepository db11 ;
        //private MetopeDbEntities db;
        //private MetopeMVCApp.Services.IServices svc;
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private IEnumerable<Code_Miscellaneous> AllCodeMisc;

        //public SecurityDetailController()
        //{
        //    db = new MetopeDbEntities();
        //}
        public SecurityDetailController(ISecurityDetailRepository iDb)
        {
            db11 = iDb; 
        } 
        //public SecurityDetailController() 
        //{
        //    this._repo = new SecurityDetailController(new MetopeDbEntities());
        //} 
        //public SecurityDetailController(IPortfolioRepository repo)
        //{
        //    _repo = repo;
        //}
        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            };
        //this is an example of logging database functionaily:
        private void LogInfo(string logmessage)
        {
            string FilePath = HttpContext.Server.MapPath("~/Data/Repositories/LoggerRepository/LoggerFile.txt");
            System.IO.File.AppendAllText(FilePath, logmessage);
        } 
         
        [LogAttribuite]
        public ActionResult Index(int? numberOfRows , int page = 1, string searchTerm = null)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (numberOfRows == null) 
                numberOfRows = 20;

            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);
             
            //var security_detailx = db11.GetAll(r => r.Call_Account_Flag == true) ;

            var security_detail = db11.GetAll()
                     .MatchEntityID(c => c.Entity_ID == currentUser.EntityIdScope) 
                     //SearchSecName(searchTerm)  
                     .OrderBy(s => s.Security_Name) 
                     .Select(g => new SecurityDetailIndexModel
                     {
                         Security_ID = g.Security_ID,
                         Entity_ID = g.Entity_ID,
                         Security_Type_Code = g.Security_Type_Code,
                         Security_Name = g.Security_Name,
                         Current_Market_Price = g.Current_Market_Price,
                         Ticker = g.Ticker,
                         Primary_Exch = g.Primary_Exch,
                         Maturity_Date = g.Maturity_Date,
                         Security_Status = g.Security_Status,
                         NumberOfRows = numberOfRows
                     }).
                     ToList();

            if (Request.IsAjaxRequest())
            {
                return View(security_detail);
                //return PartialView("_Securities", security_detail);

            }
            return View(security_detail);
        } 

        
        public ActionResult Details(decimal? id)
        { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Security_Detail security_detail = db.Security_Detail.Find(id);

            Security_Detail security_detail = db11.GetAll().
                                                Include(s => s.Currency).
                                                Include(s => s.Currency1).Include(s => s.Currency2).
                                                Include(s => s.Currency3).
                                                Include(s => s.Currency_Pair).Include(s => s.Country).
                                                Include(s => s.Country1)
                     .Where(s => s.Security_ID == id)
                     .FirstOrDefault<Security_Detail>(); 

         //   db.Entry(security_detail).Reference(p => p.Country).Load();
          //  db11.Entry(security_detail).Reference(p => p.Country).Load();
             
            if (security_detail == null)
            {
                return HttpNotFound();
            }
            return View(security_detail);
        } 
         
        [CountryFilter]     
        [SecuritiesFilter]
        [SecurityTypesFilter]
        [CurrencyFilter] 
        [ExchangesFilter]
        [CodeMiscellaneousFilter]
        [BenchmarkPortfolioFilter]
        [PartyFilter]
        [CurrencyPairFilter]
        [TrueFalseFilter]
        public ActionResult Create()
        {   
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityIdScope = currentUser.EntityIdScope;  
            return View(); 
        }

        // POST: /SecurityDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SecuritiesFilter]
        [CountryFilter]
        [SecurityTypesFilter]
        [CurrencyFilter] 
        [ExchangesFilter]
        [CodeMiscellaneousFilter]
        [BenchmarkPortfolioFilter]
         [PartyFilter]
        [CurrencyPairFilter] 
        [TrueFalseFilter]
        public ActionResult Create([Bind(Include = "Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,Security_Status,System_Locked, Benchmark_Portfolio")] Security_Detail security_detail)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId()); 
            if (ModelState.IsValid)
            { 
                security_detail.Entity_ID = currentUser.EntityIdScope;
                security_detail.Last_Update_Date = DateTime.Now;
                security_detail.Last_Update_User = User.Identity.Name; 

                db11.Add(security_detail);
                db11.Save();
                TempData.Add("ResultMessage", "new Security \"" + security_detail.Security_Name + "\" created successfully!");
 
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "An error occurred trying to add a Security"); 
            ViewBag.EntityIdScope = currentUser.EntityIdScope;  
            return View(security_detail);
        }  
        [SecuritiesFilter]
        [CountryFilter] 
        [TrueFalseFilter]
        [SecurityTypesFilter]
        [ExchangesFilter]
        [CurrencyFilter]  
        [CodeMiscellaneousFilter]
        [BenchmarkPortfolioFilter]
        [PartyFilter]
        [CurrencyPairFilter]  


        public ActionResult Edit(decimal id)
        {  
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var currentUser = manager.FindById(User.Identity.GetUserId());
                ViewBag.EntityIdScope = currentUser.EntityIdScope;

                Security_Detail security_detail = db11.FindBy(r => r.Security_ID == id)
                                    .MatchEntityID(c => c.Entity_ID == currentUser.EntityIdScope).FirstOrDefault(); 
                if (security_detail == null)
                {
                    return HttpNotFound();
                } 
                // [CountriesFilter] :
                ViewBag.DividendFXSecurityID = security_detail.Dividend_FX_Security_ID;
                ViewBag.RecordCountryOfDomicile = security_detail.Country_Of_Domicile;

                ViewBag.RecordCountryOfRisk = security_detail.Country_Of_Risk;
                // [CurrencyFilter]:
                ViewBag.PriceCurr = security_detail.Price_Curr;
                ViewBag.AssetCurrency = security_detail.Asset_Currency;
                ViewBag.TradeCurrency = security_detail.Trade_Currency;
                // SecurityTypesFilter:
                ViewBag.SecurityTypeCode = security_detail.Security_Type_Code; 
                 // Exchange
                ViewBag.PrimaryExch  = security_detail.Primary_Exch;
                ViewBag.SecondaryExch = security_detail.Secondary_Exch; 
                // Party
                ViewBag.IssuerCode = security_detail.Issuer_Code;
                ViewBag.UltimateIssuerCode = security_detail.Ultimate_Issuer_Code;

                ViewBag.BenchmarkPortfolio = security_detail.Benchmark_Portfolio;

                ViewBag.CurrencyPairCode = security_detail.Currency_Pair_Code;

                ViewBag.MyTrackEOMFlagList =  security_detail.Track_EOM_Flag;
                ViewBag.MyCallAccountFgList = security_detail.Call_Account_Flag;
                ViewBag.MySysLockedList = security_detail.System_Locked;
                ViewBag.MySecurityStatus = security_detail.Security_Status;

                ViewBag.ExDivPeriod = security_detail.Ex_Div_Period;
                ViewBag.AccruedIncomePriceFormula = security_detail.Accrued_Income_Price_Formula;
                ViewBag.ShareClass = security_detail.Share_Class;
                ViewBag.CouponBusDayAdjustment = security_detail.Coupon_BusDay_Adjustment;
                ViewBag.CleanPriceFormula = security_detail.Clean_Price_Formula;
   
                return View(security_detail); 
        } 
         [HttpPost]
         [SecuritiesFilter]
         [ValidateAntiForgeryToken]
         [CountryFilter]
         [SecurityTypesFilter]
         [PartyFilter]
         [CodeMiscellaneousFilter]
         [BenchmarkPortfolioFilter]
         [CurrencyFilter]               
         [CurrencyPairFilter]  
         [ExchangesFilter] 
        [TrueFalseFilter]
        public ActionResult Edit([Bind(Include = "Security_ID,Entity_ID,Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,System_Locked,Security_Status,Last_Update_User,Last_Update_Date,Benchmark_Portfolio,Dividend_FX_Security_ID")] 
                                    Security_Detail security_detail)
         {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            
            if (ModelState.IsValid) 
            {
                db11.Update(security_detail); //sets the modified status
                security_detail.Entity_ID = currentUser.EntityIdScope;
                security_detail.Last_Update_Date = DateTime.Now;
                security_detail.Last_Update_User = User.Identity.Name; 
                db11.Save();
                TempData.Add("ResultMessage", "Security \"" + security_detail.Security_Name + "\" editied successfully!");

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Security"); 
            ViewBag.EntityIdScope = currentUser.EntityIdScope; 
            return View(security_detail);
         
       
        }
        // GET: /SecurityDetail/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_Detail security_detail = db11.Get(id);
            if (security_detail == null)
            {
                return HttpNotFound();
            }
            return View(security_detail);
        }

        // POST: /SecurityDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        { 
            Security_Detail security_detail = db11.Get(id);

            db11.Delete (security_detail);
            db11.Save ();
            TempData.Add("ResultMessage", "Security \"" + security_detail.Security_Name + "\" Deleted successfully!");
            return RedirectToAction("Index");
        } 
         
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db11.Dispose();
            }
            base.Dispose(disposing);
        }

        //// GET: /SecurityDetail/Edit/5
        //public ActionResult Edit(decimal id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Security_Detail security_detail = db.Security_Detail.Find(id);
        //    if (security_detail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
        //    ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
        //    //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "Currency_Name", security_detail.Asset_Currency);
        //    ViewBag.Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Issuer_Code);
        //    ViewBag.Ultimate_Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Ultimate_Issuer_Code);

        //    ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "Currency_Name", security_detail.Price_Curr);
        //    ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "Currency_Name", security_detail.Asset_Currency);
        //    ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "Currency_Name", security_detail.Trade_Currency);
        //    ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Currency_Pair_Code", security_detail.Currency_Pair_Code);
        //    ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);

        //    var selectListItems = new List<SelectListItem>();
        //    selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
        //    selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
        //    ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text", security_detail.Track_EOM_Flag);
        //    ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text", security_detail.Call_Account_Flag);
        //    ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", security_detail.System_Locked);

        //    return View(security_detail);
        //} 
    }
}
