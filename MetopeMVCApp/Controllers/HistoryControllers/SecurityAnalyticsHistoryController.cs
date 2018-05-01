using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Metope.DAL;
using MetopeMVCApp.Data;
using MetopeMVCApp.Models;
using MetopeMVCApp.Filters;

namespace MetopeMVCApp.Controllers.HistoryControllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityAnalyticsHistoryController : Controller
    { 
        private readonly ISecurityAnalyticsHistoryRepository db11;

        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            }; 
        public SecurityAnalyticsHistoryController(ISecurityAnalyticsHistoryRepository iDb)
        {
            db11 = iDb;
        }
        // GET: SecurityAnalyticsHistory
        public ActionResult Index(decimal SecurityId, DateTime? inputDate, int? numberOfRows, int page = 1)
        {

            decimal EntityID = (decimal)ViewBag.EntityId;
            ViewBag.SecurityID = SecurityId;

            if (numberOfRows == null)
                numberOfRows = 20;
            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);

            if (inputDate == null)
            {
                inputDate = DateTime.Now;
            }
            
            DateTime dtEqualTo = Convert.ToDateTime(inputDate); 

            //returns iEnumerable.
            var vwm = db11.GetRecsTop100(c => c.Security_ID == SecurityId
                                            && c.Record_Date <= inputDate).
                                        Select(r => new SecurityAnalyticsIndexViewModel
                                        {
                                            Entity_ID = r.Entity_ID,
                                            Security_ID = r.Security_ID,
                                            Issued_Amount = r.Issued_Amount,

                                            Earnings_Forecast_Yr1 = r.Earnings_Forecast_Yr1,
                                            Earnings_Forecast_Yr2 = r.Earnings_Forecast_Yr2,
                                            Earnings_Forecast_Yr3 = r.Earnings_Forecast_Yr3,
                                            Total_Return_ME_1YR = r.Total_Return_ME_1YR,
                                            Total_Return_ME_2YR = r.Total_Return_ME_2YR,
                                            Total_Return_ME_3YR = r.Total_Return_ME_3YR,
                                            Short_Name = r.Security_Detail.Short_Name,
                                            Ticker = r.Security_Detail.Ticker, 
                                            RecordDate = r.Record_Date
                                        }); 
             
            if (vwm.Any()) // if records found then populate the to-date
            {
                ViewBag.LastRecordDate = vwm.Min(p => p.RecordDate).ToString("dd/MM/yyyy");
            }

            ViewBag.UserInputDate = dtEqualTo.ToString("dd/MM/yyyy");

            SecurityAnalyticsIndexViewModel firsRecord = vwm.FirstOrDefault();
            ViewBag.Ticker = firsRecord != null ? firsRecord.Ticker : null;

            //var cash_Transactions = db.Cash_Transactions.Include(c => c.Currency).Include(c => c.Entity).Include(c => c.Order_Allocation).Include(c => c.Order_Detail).Include(c => c.Portfolio).Include(c => c.Security_Detail).Include(c => c.Security_Detail1).Include(c => c.User).Include(c => c.User1);
            return View(vwm );
        }   

        // GET: SecurityAnalyticsHistory/Create
        [AllSecuritiesInclGenericFilter] 
        public ActionResult Create(int? SecurityId, string Nav = "")
        {
            ViewBag.EntityIdScope = ViewBag.EntityId; 
            decimal EntityID = (decimal)ViewBag.EntityId; 
             
            var secAnalHist = new Security_Analytics_History
            {
                Entity_ID = EntityID ,
                Security_ID =  (decimal)SecurityId ,
                Record_Date = DateTime.Now.AddDays(-1)
            }; 

            //ViewBag.SecuritiesAll2 = security_Analytics_History.Discount_Security_ID; 

            ViewBag.Nav = Nav;
            return View(secAnalHist);
        }

        // POST: SecurityAnalyticsHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create([Bind(Include = "Entity_ID,Security_ID,Issued_Amount,Market_Capitalisation,Risk_Premium,Exit_Price,Total_Return_ME_1YR,Total_Return_ME_2YR,Total_Return_ME_3YR,Historic_Distribution,Distribution_Growth_Forecast_Yr1,Distribution_Growth_Forecast_Yr2,Distribution_Growth_Forecast_Yr3,Forecast_Distribution_Yr1,Forecast_Distribution_Yr2,Forecast_Distribution_Yr3,Earnings_Growth_Compounded,Earnings_Forecast_Yr3,Historic_Dividend_Yield_FYE,Forward_Dividend_Yield_FYE,Yr3_Forward_Dividend_Yield,Rolling_12M_Forward_Yield,Historical_Earnings_FYE,Earnings_Forecast_Yr1,Earnings_Forecast_Yr2,Historic_Dividend_Yield_Relative,Rolling_12M_Historic_Distribution_PriceCurr,Short_Term_Market_Rerate,Forecast_12M_Total_Return,Clean_Price,Rolling_Dividend_Yield_Historic,Last_Update_Date,Last_Update_User,Discount_Security_ID,Net_Present_Value,Forecast_Rolling_Custom_Income,Custom_Period,Rolling_12M_Historic_Distribution,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User,Rolling_12M_Forecast_Distribution,Yr2_Forward_Dividend_Yield,Exit_Yield,Discount_Rate,Distribution_Growth_Compounded,Forecast_12MRolling_Income_Accrual,Rolling_12_to_24M_Forecast_Distribution,Rolling_24_to_36M_Forecast_Distribution,Forecast_Yr1_Dividend_Yield_Relative,Market_Historic_Yield,Market_Forward_Yield,Historic_Distribution_PriceCurr,Forecast_Distribution_Yr1_PriceCurr,Forecast_Distribution_Yr2_PriceCurr,Forecast_Distribution_Yr3_PriceCurr,Rolling_12M_Forecast_Distribution_PriceCurr,Rolling_12_24M_Forecast_Distribution_PriceCurr,Rolling_24_36M_Forecast_Distribution_PriceCurr,Distribution_Growth_Forecast_Yr4,Earnings_Forecast_Yr4,Forecast_Distribution_Yr4,Forecast_Distribution_Yr4_PriceCurr")] Security_Analytics_History security_Analytics_History)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
             /*------------------------------------------ 
             first check if record is already exist ! 
             ----------------------------------------*/ 
            bool exists = db11.AnyExists(r => r.Security_ID == security_Analytics_History.Security_ID
                                                      && r.Record_Date == security_Analytics_History.Record_Date
                                                      && r.Entity_ID == EntityID);
            security_Analytics_History.Entity_ID = EntityID;
            security_Analytics_History.Security_ID = EntityID; 

            if (ModelState.IsValid)
            {
                if (exists != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Analytics History for Security " + security_Analytics_History.Security_ID.ToString() + " as it Already exists!");
                }
                else
                { 
                    security_Analytics_History.Last_Update_Date = DateTime.Now;
                    security_Analytics_History.Last_Update_User = User.Identity.Name;
                    security_Analytics_History.Hist_Last_Update_Date = DateTime.Now;
                    security_Analytics_History.Hist_Last_Update_User = User.Identity.Name;

                    db11.Add(security_Analytics_History);
                    db11.Save();
                    TempData["ResultMessage"] = "Analytics History for Security " + security_Analytics_History.Security_ID.ToString() + " created successfully!";

                    return RedirectToAction("Index", new { SecurityId = security_Analytics_History.Security_ID });
     
                }
            }

            return View(security_Analytics_History);
             

            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Analytics_History.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Analytics_History.Security_ID);
            //ViewBag.Discount_Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Analytics_History.Discount_Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Analytics_History.Entity_ID);
            //return View(security_Analytics_History);
        }

        // GET: SecurityAnalyticsHistory/Edit/5
        [CustomEntityAuthoriseFilter]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit(decimal SecurityId, decimal EntityId, DateTime RecordDate )
        {  
            var security_Analytics_History = db11.FindBy2(r => r.Security_ID == SecurityId && r.Record_Date == RecordDate
                                                        && r.Entity_ID == EntityId)
                                                    .FirstOrDefault();  

            if (security_Analytics_History == null)
            {
                return HttpNotFound();
            }

            ViewBag.SecuritiesAll = security_Analytics_History.Security_ID;
            ViewBag.SecuritiesAll2 = security_Analytics_History.Discount_Security_ID; 

            return View(security_Analytics_History); 
        }

        // POST: SecurityAnalyticsHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit([Bind(Include = "Entity_ID,Security_ID,Issued_Amount,Market_Capitalisation,Risk_Premium,Exit_Price,Total_Return_ME_1YR,Total_Return_ME_2YR,Total_Return_ME_3YR,Historic_Distribution,Distribution_Growth_Forecast_Yr1,Distribution_Growth_Forecast_Yr2,Distribution_Growth_Forecast_Yr3,Forecast_Distribution_Yr1,Forecast_Distribution_Yr2,Forecast_Distribution_Yr3,Earnings_Growth_Compounded,Earnings_Forecast_Yr3,Historic_Dividend_Yield_FYE,Forward_Dividend_Yield_FYE,Yr3_Forward_Dividend_Yield,Rolling_12M_Forward_Yield,Historical_Earnings_FYE,Earnings_Forecast_Yr1,Earnings_Forecast_Yr2,Historic_Dividend_Yield_Relative,Rolling_12M_Historic_Distribution_PriceCurr,Short_Term_Market_Rerate,Forecast_12M_Total_Return,Clean_Price,Rolling_Dividend_Yield_Historic,Last_Update_Date,Last_Update_User,Discount_Security_ID,Net_Present_Value,Forecast_Rolling_Custom_Income,Custom_Period,Rolling_12M_Historic_Distribution,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User,Rolling_12M_Forecast_Distribution,Yr2_Forward_Dividend_Yield,Exit_Yield,Discount_Rate,Distribution_Growth_Compounded,Forecast_12MRolling_Income_Accrual,Rolling_12_to_24M_Forecast_Distribution,Rolling_24_to_36M_Forecast_Distribution,Forecast_Yr1_Dividend_Yield_Relative,Market_Historic_Yield,Market_Forward_Yield,Historic_Distribution_PriceCurr,Forecast_Distribution_Yr1_PriceCurr,Forecast_Distribution_Yr2_PriceCurr,Forecast_Distribution_Yr3_PriceCurr,Rolling_12M_Forecast_Distribution_PriceCurr,Rolling_12_24M_Forecast_Distribution_PriceCurr,Rolling_24_36M_Forecast_Distribution_PriceCurr,Distribution_Growth_Forecast_Yr4,Earnings_Forecast_Yr4,Forecast_Distribution_Yr4,Forecast_Distribution_Yr4_PriceCurr")] Security_Analytics_History security_Analytics_History)
        {

            if (ModelState.IsValid)
            {
                db11.Update(security_Analytics_History); //sets the modified status 
                security_Analytics_History.Hist_Last_Update_Date = DateTime.Now;
                security_Analytics_History.Hist_Last_Update_User = User.Identity.Name; 
                db11.Save();
                TempData["ResultMessage"] = "Analytics History for Security " + security_Analytics_History.Security_ID.ToString() + " edited successfully!";
                return RedirectToAction("Index", new { SecurityId = security_Analytics_History.Security_ID });     
             } 
              
            return View(security_Analytics_History);
        }

        // GET: SecurityAnalyticsHistory/Delete/5
      
        public ActionResult Delete( decimal SecurityId, DateTime RecordDate)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            var security_Analytics_History = db11.FindBy2(r => r.Security_ID == SecurityId  && r.Record_Date == RecordDate
                                                             && r.Entity_ID == EntityID)
                                                .FirstOrDefault();

            if (security_Analytics_History == null)
            {
                return HttpNotFound();
            }

            return View(security_Analytics_History);
        }
        // POST: SecurityAnalyticsHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public ActionResult DeleteConfirmed(  decimal SecurityId, DateTime RecordDate)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            var security_Analytics_History = db11.FindBy2(r => r.Security_ID == SecurityId &&  r.Record_Date == RecordDate
                                                         && r.Entity_ID == EntityID)
                                             .FirstOrDefault();

            db11.Delete(security_Analytics_History);
            db11.Save();
            TempData["ResultMessage"] = "A Security Analytics History record for Security " + security_Analytics_History.Security_ID.ToString() + " deleted successfully!";
            return RedirectToAction("Index", new {  SecurityId = security_Analytics_History.Security_ID });

        }  
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db11.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
