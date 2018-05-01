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
    public class SecurityAttributionHistoryController : Controller
    { 
        private readonly ISecurityAttributionHistoryRepository db11;

        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            }; 
        public SecurityAttributionHistoryController(ISecurityAttributionHistoryRepository iDb)
        {
            db11 = iDb;
        }

        // GET: SecurityAnalyticsHistory
        public ActionResult Index(string PortfolioCode, decimal SecurityId, DateTime? inputDate, int? numberOfRows, int page = 1)
        { 
            decimal EntityID = (decimal)ViewBag.EntityId;
            ViewBag.SecurityID = SecurityId;
            ViewBag.Portfolio = PortfolioCode;
             
            if (numberOfRows == null)
                numberOfRows = 20;
            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);

            if (inputDate == null)
            {
                inputDate = DateTime.Now;
            }
            DateTime dtEqualTo = Convert.ToDateTime(inputDate); 
            //returns iEnumerable.
            var vwm = db11.GetRecsTop100(c => c.Security_ID == SecurityId && c.Portfolio_Code == PortfolioCode &&
                                           c.Record_Date <= inputDate) 
                                        .Select(g => new SecurityAttributionIndexViewModel
                                        {
                                            Entity_ID = g.Entity_ID,
                                            Security_ID = g.Security_ID,
                                            Portfolio_Code = g.Portfolio_Code,
                                            Excess_Weight_Quarterly = g.Excess_Weight_Quarterly,
                                            Relative_Contribution_Quarterly = g.Relative_Contribution_Quarterly,
                                            Security_Weight_Portfolio_Monthly = g.Security_Weight_Portfolio_Monthly, 
                                            Ticker = g.Security_Detail.Ticker,
                                            RecordDate = g.Record_Date
                                        });

            if (vwm.Any()) // if records found then populate the to-date
            {
                ViewBag.LastRecordDate = vwm.Min(p => p.RecordDate).ToString("dd/MM/yyyy");
            }

            ViewBag.UserInputDate = dtEqualTo.ToString("dd/MM/yyyy");

            SecurityAttributionIndexViewModel firsRecord = vwm.FirstOrDefault();
            ViewBag.Ticker = firsRecord != null ? firsRecord.Ticker : String.Format(" Id: {0}",SecurityId.ToString() );
    
             return View(vwm);
        }   
           
        [AllSecuritiesInclGenericFilter] 
        public ActionResult Create(string PortfolioCode, int? SecurityId, string Nav = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            ViewBag.SecuritiesAll = SecurityId;
            ViewBag.Portfolio = PortfolioCode; 
            var secAttHist = new Security_Attribution_History
            {
                Entity_ID = EntityID,
                Portfolio_Code = PortfolioCode,
                Security_ID = (decimal)SecurityId,
                Record_Date = DateTime.Now.AddDays(-1)
            }; 

            //ViewBag.SecuritiesAll2 = security_Attribution_History.Discount_Security_ID; 

            ViewBag.Nav = Nav;
            return View(secAttHist);
        }

        // POST: SecurityAttributionHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,Excess_Weight_Quarterly,Relative_Contribution_Quarterly,Allocation_Selection_Interaction_Quarterly,StockVs_TotBenchmarkReturn_Quarterly,Selection_Interaction_Quarterly,Asset_Allocation_Quarterly,Excess_Weight_Monthly,Relative_Contribution_Monthly,Allocation_Selection_Interaction_Monthly,StockVs_TotBenchmarkReturn_Monthly,Selection_Interaction_Monthly,Asset_Allocation_Monthly,Excess_Weight_MonthToDate,Relative_Contribution_MonthToDate,Allocation_Selection_Interaction_MonthToDate,StockVs_TotBenchmarkReturn_MonthToDate,Selection_Interaction_MonthToDate,Asset_Allocation_MonthToDate,Last_Update_Date,Last_Update_User,Security_Weight_Portfolio_Monthly,Security_Weight_Portfolio_MonthToDate,Security_Weight_Portfolio_Quarterly,Security_Weight_Benchmark_Monthly,Security_Weight_Benchmark_MonthToDate,Security_Weight_Benchmark_Quarterly,Total_Return_Security_Price_Monthly,Total_Return_Security_Price_MonthToDate,Total_Return_Security_Price_Quarterly,TW_Return_Portfolio_Monthly,TW_Return_Portfolio_MonthToDate,TW_Return_Portfolio_Quarterly,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User,Benchmark_Price_Performance_Monthly,Benchmark_Price_Performance_MonthToDate,Benchmark_Price_Performance_Quarterly")] Security_Attribution_History security_Attribution_History)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            /*------------------------------------------ 
            first check if record is already exist ! 
            ----------------------------------------*/
            bool exists = db11.AnyExists(r => r.Security_ID == security_Attribution_History.Security_ID
                                                      && r.Portfolio_Code == security_Attribution_History.Portfolio_Code
                                                      && r.Record_Date == security_Attribution_History.Record_Date
                                                      && r.Entity_ID == EntityID);
            security_Attribution_History.Entity_ID = EntityID;

            if (ModelState.IsValid)
            {
                if (exists )
                {
                    ModelState.AddModelError("Name", "FAILED to create Attribution History for Portfolio " + security_Attribution_History.Portfolio_Code + " as it Already exists!");
                }
                else
                {
                    security_Attribution_History.Last_Update_Date = DateTime.Now;
                    security_Attribution_History.Last_Update_User = User.Identity.Name;
                    security_Attribution_History.Hist_Last_Update_Date = DateTime.Now;
                    security_Attribution_History.Hist_Last_Update_User = User.Identity.Name;

                    db11.Add(security_Attribution_History);
                    db11.Save();
                    TempData["ResultMessage"] = "Attribution History for Security " + security_Attribution_History.Security_ID.ToString()  
                                                 + " Portfolio " + security_Attribution_History.Portfolio_Code +
                                                    " created successfully!";

                    return RedirectToAction("Index", new { SecurityId = security_Attribution_History.Security_ID, PortfolioCode = security_Attribution_History.Portfolio_Code });

                }
            } 
            return View(security_Attribution_History); 
        }

        // GET  
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit(string PortfolioCode, decimal SecurityId, DateTime RecordDate)  
        {
     
            var EntityID = (decimal)ViewBag.EntityId; 

            var security_Attribution_History = db11.FindBy2(r => r.Security_ID == SecurityId && r.Portfolio_Code == PortfolioCode
                                                            && r.Record_Date == RecordDate
                                                            && r.Entity_ID == EntityID)
                                                .FirstOrDefault();  

            if (security_Attribution_History == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecuritiesAll = security_Attribution_History.Security_ID;
            return View(security_Attribution_History);

        }

        // POST: SecurityAttributionHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,Excess_Weight_Quarterly,Relative_Contribution_Quarterly,Allocation_Selection_Interaction_Quarterly,StockVs_TotBenchmarkReturn_Quarterly,Selection_Interaction_Quarterly,Asset_Allocation_Quarterly,Excess_Weight_Monthly,Relative_Contribution_Monthly,Allocation_Selection_Interaction_Monthly,StockVs_TotBenchmarkReturn_Monthly,Selection_Interaction_Monthly,Asset_Allocation_Monthly,Excess_Weight_MonthToDate,Relative_Contribution_MonthToDate,Allocation_Selection_Interaction_MonthToDate,StockVs_TotBenchmarkReturn_MonthToDate,Selection_Interaction_MonthToDate,Asset_Allocation_MonthToDate,Last_Update_Date,Last_Update_User,Security_Weight_Portfolio_Monthly,Security_Weight_Portfolio_MonthToDate,Security_Weight_Portfolio_Quarterly,Security_Weight_Benchmark_Monthly,Security_Weight_Benchmark_MonthToDate,Security_Weight_Benchmark_Quarterly,Total_Return_Security_Price_Monthly,Total_Return_Security_Price_MonthToDate,Total_Return_Security_Price_Quarterly,TW_Return_Portfolio_Monthly,TW_Return_Portfolio_MonthToDate,TW_Return_Portfolio_Quarterly,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User,Benchmark_Price_Performance_Monthly,Benchmark_Price_Performance_MonthToDate,Benchmark_Price_Performance_Quarterly")] Security_Attribution_History security_Attribution_History)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(security_Attribution_History); //sets the modified status 
                security_Attribution_History.Hist_Last_Update_Date = DateTime.Now;
                security_Attribution_History.Hist_Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Security Attribution Hiatory for \"" + security_Attribution_History.Portfolio_Code + "\" editied successfully!";

                return RedirectToAction("Index", new { PortfolioCode = security_Attribution_History.Portfolio_Code, SecurityId = security_Attribution_History.Security_ID });
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Security Attribution");
            ViewBag.EntityIdScope = EntityID; 
            
            return View(security_Attribution_History);
        }
        // GET:  
        public ActionResult Delete(string PortfolioCode, decimal SecurityId, DateTime RecordDate)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            var security_Attribution_History = db11.FindBy2(r => r.Security_ID == SecurityId && r.Portfolio_Code == PortfolioCode
                                                            && r.Record_Date == RecordDate
                                                            && r.Entity_ID == EntityID)
                                                .FirstOrDefault();

            if (security_Attribution_History == null)
            {
                return HttpNotFound();
            }

            return View(security_Attribution_History);
        }
        

        // POST: SecurityAttributionHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string PortfolioCode, decimal SecurityId, DateTime RecordDate)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            var security_Attrib_History = db11.FindBy2(r => r.Security_ID == SecurityId && r.Portfolio_Code == PortfolioCode
                                                         && r.Record_Date == RecordDate
                                                         && r.Entity_ID == EntityID)
                                             .FirstOrDefault();

            db11.Delete(security_Attrib_History);
            db11.Save();
            TempData["ResultMessage"] = "A Security Attribution History record for Portfolio " + security_Attrib_History.Portfolio_Code.ToString() + " deleted successfully!";
            return RedirectToAction("Index", new { PortfolioCode = security_Attrib_History.Portfolio_Code, SecurityId = security_Attrib_History.Security_ID });

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
