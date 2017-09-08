using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Data;

using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Data.GenericRepository;
namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityAnalyticsController : Controller
    {
        private readonly ISecurityAnalyticsRepository db11;
        //private readonly ISecurityDetailRepository db2;
        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            };

        private MetopeDbEntities db = new MetopeDbEntities();

        public SecurityAnalyticsController(ISecurityAnalyticsRepository iDb)
        {
            db11 = iDb;
        } 

        // GET: SecurityAnalytics
        public ActionResult Index(int? numberOfRows, int page = 1, string searchTerm = null, string Nav = "")

        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            if (numberOfRows == null)
                numberOfRows = 20;

            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);

            var vm = db.Security_Analytics.
                                        Include(s => s.Security_Detail). 
                                        Select( r => new SecurityAnalyticsIndexViewModel{
                                                Entity_ID = r.Entity_ID,
                                                Security_ID = r.Security_ID, 
                                                Issued_Amount = r.Issued_Amount,

                                                Earnings_Forecast_Yr1 = r.Earnings_Forecast_Yr1,
                                                Earnings_Forecast_Yr2= r.Earnings_Forecast_Yr2,
                                                Earnings_Forecast_Yr3 = r.Earnings_Forecast_Yr3, 
                                                Total_Return_ME_1YR = r.Total_Return_ME_1YR,
                                                Total_Return_ME_2YR = r.Total_Return_ME_2YR,
                                                Total_Return_ME_3YR = r.Total_Return_ME_3YR,  
                                                Short_Name = r.Security_Detail.Short_Name,
                                                Ticker = r.Security_Detail.Ticker 
                                        }).ToList(); 
            ViewBag.Nav = "";
            
            return View(vm.ToList());
        }
         
        // GET: SecurityAnalytics/Create
        [AllSecuritiesFilter]
        public ActionResult Create(int? SecurityId, string Nav)
        {   
            ViewBag.EntityIdScope = ViewBag.EntityId; 
            ViewBag.SecuritiesAll = SecurityId;

            ViewBag.Nav = Nav;
            return View(); 
        }

        // POST: SecurityAnalytics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesFilter]
        public ActionResult Create([Bind(Include = "Entity_ID,Security_ID,Issued_Amount,Market_Capitalisation,Risk_Premium,Exit_Price,Total_Return_ME_1YR,Total_Return_ME_2YR,Total_Return_ME_3YR,Historic_Distribution,Distribution_Growth_Forecast_Yr1,Distribution_Growth_Forecast_Yr2,Distribution_Growth_Forecast_Yr3,Forecast_Distribution_Yr1,Forecast_Distribution_Yr2,Forecast_Distribution_Yr3,Earnings_Growth_Compounded,Earnings_Forecast_Yr3,Historic_Dividend_Yield_FYE,Forward_Dividend_Yield_FYE,Yr3_Forward_Dividend_Yield,Rolling_12M_Forward_Yield,Historical_Earnings_FYE,Earnings_Forecast_Yr1,Earnings_Forecast_Yr2,Historic_Dividend_Yield_Relative,Rolling_12M_Historic_Distribution_PriceCurr,Short_Term_Market_Rerate,Forecast_12M_Total_Return,Clean_Price,Rolling_Dividend_Yield_Historic,Last_Update_Date,Last_Update_User,Discount_Security_ID,Net_Present_Value,Forecast_Rolling_Custom_Income,Custom_Period,Rolling_12M_Historic_Distribution,Rolling_12M_Forecast_Distribution,Yr2_Forward_Dividend_Yield,Exit_Yield,Discount_Rate,Distribution_Growth_Compounded,Forecast_12MRolling_Income_Accrual,Rolling_12_to_24M_Forecast_Distribution,Rolling_24_to_36M_Forecast_Distribution,Forecast_Yr1_Dividend_Yield_Relative,Market_Historic_Yield,Market_Forward_Yield,Historic_Distribution_PriceCurr,Forecast_Distribution_Yr1_PriceCurr,Forecast_Distribution_Yr2_PriceCurr,Forecast_Distribution_Yr3_PriceCurr,Rolling_12M_Forecast_Distribution_PriceCurr,Rolling_12_24M_Forecast_Distribution_PriceCurr,Rolling_24_36M_Forecast_Distribution_PriceCurr,Distribution_Growth_Forecast_Yr4,Earnings_Forecast_Yr4,Forecast_Distribution_Yr4,Forecast_Distribution_Yr4_PriceCurr")] Security_Analytics security_Analytics
                                          ,string navIndicator="")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            security_Analytics.Entity_ID = EntityID;
            /*------------------------------------------ 
             first check if this party code is already used ! 
             ----------------------------------------*/
            Security_Analytics check = db11.FindBy(r => r.Security_ID == security_Analytics.Security_ID &&
                                                        r.Entity_ID == security_Analytics.Entity_ID).FirstOrDefault<Security_Analytics>();             
            if (ModelState.IsValid)
            { 
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Analytics for security " + security_Analytics.Security_ID.ToString() + " as it Already exists!");
                }
                else
                { 
                     security_Analytics.Last_Update_Date = DateTime.Now;
                     security_Analytics.Last_Update_User = User.Identity.Name; 

                     db.Security_Analytics.Add(security_Analytics);
                     db.SaveChanges();
                     TempData["ResultMessage"] = "Analytics for Security " + security_Analytics.Security_ID.ToString() + " created successfully!";

                     if (navIndicator == "")
                         return RedirectToAction("Index");
                     else
                         return RedirectToAction("Index", "SecurityDetail");
                }
            }
             
             return View(security_Analytics);
        }
        public ActionResult SecurityAnalyticsHistory(decimal EntityId, decimal SecurityId)
        {
            return View();
        }
        // GET: SecurityAnalytics/Edit/5
        [CustomEntityAuthoriseFilter]
        [AllSecuritiesFilter]
        public ActionResult Edit(decimal EntityId, decimal SecurityId, string Nav)
        {
            var EntityID = (decimal)ViewBag.EntityId;
 
            var security_Analytics = db11.FindBy(r => r.Security_ID == SecurityId  )
                                           .MatchCriteria(c => c.Entity_ID == EntityID).FirstOrDefault();
             
            if (security_Analytics == null)
            {
                return HttpNotFound();
            }

            ViewBag.Nav = Nav;

            return View(security_Analytics);
        }

        // POST: SecurityAnalytics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesFilter]
        public ActionResult Edit([Bind(Include = "Entity_ID,Security_ID,Issued_Amount,Market_Capitalisation,Risk_Premium,Exit_Price,Total_Return_ME_1YR,Total_Return_ME_2YR,Total_Return_ME_3YR,Historic_Distribution,Distribution_Growth_Forecast_Yr1,Distribution_Growth_Forecast_Yr2,Distribution_Growth_Forecast_Yr3,Forecast_Distribution_Yr1,Forecast_Distribution_Yr2,Forecast_Distribution_Yr3,Earnings_Growth_Compounded,Earnings_Forecast_Yr3,Historic_Dividend_Yield_FYE,Forward_Dividend_Yield_FYE,Yr3_Forward_Dividend_Yield,Rolling_12M_Forward_Yield,Historical_Earnings_FYE,Earnings_Forecast_Yr1,Earnings_Forecast_Yr2,Historic_Dividend_Yield_Relative,Rolling_12M_Historic_Distribution_PriceCurr,Short_Term_Market_Rerate,Forecast_12M_Total_Return,Clean_Price,Rolling_Dividend_Yield_Historic,Last_Update_Date,Last_Update_User,Discount_Security_ID,Net_Present_Value,Forecast_Rolling_Custom_Income,Custom_Period,Rolling_12M_Historic_Distribution,Rolling_12M_Forecast_Distribution,Yr2_Forward_Dividend_Yield,Exit_Yield,Discount_Rate,Distribution_Growth_Compounded,Forecast_12MRolling_Income_Accrual,Rolling_12_to_24M_Forecast_Distribution,Rolling_24_to_36M_Forecast_Distribution,Forecast_Yr1_Dividend_Yield_Relative,Market_Historic_Yield,Market_Forward_Yield,Historic_Distribution_PriceCurr,Forecast_Distribution_Yr1_PriceCurr,Forecast_Distribution_Yr2_PriceCurr,Forecast_Distribution_Yr3_PriceCurr,Rolling_12M_Forecast_Distribution_PriceCurr,Rolling_12_24M_Forecast_Distribution_PriceCurr,Rolling_24_36M_Forecast_Distribution_PriceCurr,Distribution_Growth_Forecast_Yr4,Earnings_Forecast_Yr4,Forecast_Distribution_Yr4,Forecast_Distribution_Yr4_PriceCurr")] Security_Analytics security_Analytics
                                    , string navIndicator = "")
        {
            var EntityID = (decimal)ViewBag.EntityId;
             
            if (ModelState.IsValid)
            {
                db11.Update(security_Analytics); //sets the modified status 
                security_Analytics.Last_Update_Date = DateTime.Now;
                security_Analytics.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Analytics for Security " + security_Analytics.Security_ID.ToString() + " edited successfully!";
                 
                if (navIndicator == "")
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", "SecurityDetail" );
                 
            } 
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Analytics.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Analytics.Security_ID);
            //ViewBag.Discount_Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Analytics.Discount_Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Analytics.Entity_ID);
            return View(security_Analytics);
        }

        // GET: SecurityAnalytics/Delete/5
        public ActionResult Delete(decimal SecurityId )
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Security_Analytics saa = db11.FindBy(r => r.Security_ID == SecurityId && r.Entity_ID == EntityID
                   ).FirstOrDefault();

            if (saa == null)
            {
                return HttpNotFound();
            }  
            return View(saa);
        }

        // POST: SecurityAnalytics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(decimal SecurityId)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Security_Analytics security_Analytics = db11.FindBy(r => r.Security_ID == SecurityId && r.Entity_ID == EntityID
                   ).FirstOrDefault();
             
            db11.Delete(security_Analytics);
            db11.Save();
            TempData["ResultMessage"] = "Analytics for Security " + security_Analytics.Security_ID.ToString() + " deleted successfully!";

             return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db11.Dispose();
                db .Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
