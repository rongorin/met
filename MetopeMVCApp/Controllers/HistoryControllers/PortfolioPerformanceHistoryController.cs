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
    public class PortfolioPerformanceHistoryController : Controller
    {
        private readonly IPortfolioPerformanceHistoryRepository db11;

        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            };
        public PortfolioPerformanceHistoryController(IPortfolioPerformanceHistoryRepository iDb)
        {
            db11 = iDb;
        }

        // GET: portfolioAnalyticsHistory
        public ActionResult Index(string PortfolioCode, DateTime? inputDate, int? numberOfRows, int page = 1)
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 
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
            var  vwm = db11.GetRecsTop100(c => c.Portfolio_Code == PortfolioCode &&
                                           c.Record_Date <= inputDate) ;

            if (vwm.Any()) // if records found then populate the to-date
            {
                ViewBag.LastRecordDate = vwm.Min(p => p.Record_Date).ToString("dd/MM/yyyy");
            };
            ViewBag.UserInputDate = dtEqualTo.ToString("dd/MM/yyyy");
             
            ViewBag.PortfolioCode =  PortfolioCode;

            return View(vwm);
        }

        [AllSecuritiesInclGenericFilter]
        public ActionResult Create(string PortfolioCode,  string Nav = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 
            ViewBag.Portfolio = PortfolioCode;
            var pHist = new Portfolio_Performance_History
            {
                Entity_ID = EntityID,
                Portfolio_Code = PortfolioCode, 
                Record_Date = DateTime.Now.AddDays(-1)
            };
             
                
            ViewBag.Nav = Nav;
            return View(pHist);
        }

        // POST: PortfolioPerformanceHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create([Bind(Include = "Entity_ID,Portfolio_Code,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Dietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,ModDietz_Performance_Quarterly,Last_Update_Date,Last_Update_User,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User")] Portfolio_Performance_History portfolio_Performance_History)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            /*------------------------------------------ 
            first check if record is already exist ! 
            ----------------------------------------*/
            bool exists = db11.AnyExists(r =>  r.Portfolio_Code == portfolio_Performance_History.Portfolio_Code
                                                      && r.Record_Date == portfolio_Performance_History.Record_Date
                                                      && r.Entity_ID == EntityID);
            portfolio_Performance_History.Entity_ID = EntityID;

            if (ModelState.IsValid)
            {
                if (exists)
                {
                    ModelState.AddModelError("Name", "FAILED to create Portfolio Performance History for Portfolio " + portfolio_Performance_History.Portfolio_Code + " as it Already exists!");
                }
                else
                {
                    portfolio_Performance_History.Last_Update_Date = DateTime.Now;
                    portfolio_Performance_History.Last_Update_User = User.Identity.Name;
                    portfolio_Performance_History.Hist_Last_Update_Date = DateTime.Now;
                    portfolio_Performance_History.Hist_Last_Update_User = User.Identity.Name;

                    db11.Add(portfolio_Performance_History);
                    db11.Save();
                    TempData["ResultMessage"] = "Portfolio Performance History for " + portfolio_Performance_History.Portfolio_Code +
                                                    " created successfully!";

                    return RedirectToAction("Index", new {   PortfolioCode = portfolio_Performance_History.Portfolio_Code });

                }
            }
            return View(portfolio_Performance_History);
        }

        // GET  
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit(string PortfolioCode,  DateTime RecordDate)
        {

            var EntityID = (decimal)ViewBag.EntityId;

            var portfolio_Performance_History = db11.FindBy2(r =>   r.Portfolio_Code == PortfolioCode
                                                            && r.Record_Date == RecordDate
                                                            && r.Entity_ID == EntityID)
                                                .FirstOrDefault();

            if (portfolio_Performance_History == null)
            {
                return HttpNotFound();
            } 
            return View(portfolio_Performance_History);

        }

        // POST: PortfolioPerformanceHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Entity_ID,Portfolio_Code,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Dietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,ModDietz_Performance_Quarterly,Last_Update_Date,Last_Update_User,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User")] Portfolio_Performance_History portfolio_Performance_History)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(portfolio_Performance_History); //sets the modified status 
                portfolio_Performance_History.Hist_Last_Update_Date = DateTime.Now;
                portfolio_Performance_History.Hist_Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Portfolio Performance History for \"" + portfolio_Performance_History.Portfolio_Code + "\" editied successfully!";

                return RedirectToAction("Index", new { PortfolioCode = portfolio_Performance_History.Portfolio_Code });
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Portfolio Performance History");
            ViewBag.EntityIdScope = EntityID;

            return View(portfolio_Performance_History);
        }
        // GET:  
        public ActionResult Delete(string PortfolioCode, DateTime RecordDate)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            var portfolio_Performance_History = db11.FindBy2(r =>   r.Portfolio_Code == PortfolioCode
                                                            && r.Record_Date == RecordDate
                                                            && r.Entity_ID == EntityID)
                                                .FirstOrDefault();

            if (portfolio_Performance_History == null)
            {
                return HttpNotFound();
            } 
            return View(portfolio_Performance_History);
        }


        // POST: PortfolioPerformanceHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string PortfolioCode, DateTime RecordDate)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            var portfolio_perf_History = db11.FindBy2(r =>  r.Portfolio_Code == PortfolioCode
                                                         && r.Record_Date == RecordDate
                                                         && r.Entity_ID == EntityID)
                                             .FirstOrDefault();

            db11.Delete(portfolio_perf_History);
            db11.Save();
            TempData["ResultMessage"] = "A Portfolio Performance History record for Portfolio " + portfolio_perf_History.Portfolio_Code.ToString() + " deleted successfully!";
            return RedirectToAction("Index", new { PortfolioCode = portfolio_perf_History.Portfolio_Code });

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
