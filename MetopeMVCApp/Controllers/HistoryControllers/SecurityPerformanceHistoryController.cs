using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Metope.DAL;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Data;
using MetopeMVCApp.Models;

namespace MetopeMVCApp.Controllers.HistoryControllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityPerformanceHistoryController : Controller
    {
         
        private readonly ISecurityPerformanceHistoryRepository db11;

        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            };
        public SecurityPerformanceHistoryController(ISecurityPerformanceHistoryRepository iDb)
        {
            db11 = iDb;
        }
          
        // GET: SecurityPerformanceHistory
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
                                        .Select(g => new SecurityPerformanceIndexViewModel
                                        {
                                            Entity_ID = g.Entity_ID,
                                            Security_ID = g.Security_ID,

                                            Portfolio_Code = g.Portfolio_Code,

                                            ModDietz_Performance_Quarterly = g.ModDietz_Performance_Quarterly,
                                            ModDietz_Performance_Monthly = g.ModDietz_Performance_Monthly,
                                            ModDietz_Performance_MonthToDate = g.ModDietz_Performance_MonthToDate,
                                            Ticker = g.Security_Detail.Ticker,
                                            RecordDate = g.Record_Date
                                        });

            if (vwm.Any()) // if records found then populate the to-date
            {
                ViewBag.LastRecordDate = vwm.Min(p => p.RecordDate).ToString("dd/MM/yyyy");
            }

            ViewBag.UserInputDate = dtEqualTo.ToString("dd/MM/yyyy");

            SecurityPerformanceIndexViewModel firsRecord = vwm.FirstOrDefault();
            ViewBag.Ticker = firsRecord != null ? firsRecord.Ticker : String.Format(" Id: {0}", SecurityId.ToString());
            ViewBag.PortfolioCode = PortfolioCode;

             return View(vwm);
        }
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create(string PortfolioCode, int? SecurityId, string Nav = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            ViewBag.SecuritiesAll = SecurityId;
            ViewBag.Portfolio = PortfolioCode;
            var secPerfHist = new Security_Performance_History
            {
                Entity_ID = EntityID,
                Portfolio_Code = PortfolioCode,
                Security_ID = (decimal)SecurityId,
                Record_Date = DateTime.Now.AddDays(-1)
            };
              
            ViewBag.Nav = Nav;
            return View(secPerfHist);
        } 

        // POST: SecurityPerformanceHistory/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,ModDietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,Dietz_Performance_Quarterly,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Last_Update_Date,Last_Update_User,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User")] Security_Performance_History security_Performance_History)
        {
            
            decimal EntityID = (decimal)ViewBag.EntityId;
            /*------------------------------------------ 
            first check if record is already exist ! 
            ----------------------------------------*/
            bool exists = db11.AnyExists(r => r.Security_ID == security_Performance_History.Security_ID
                                                      && r.Portfolio_Code == security_Performance_History.Portfolio_Code
                                                      && r.Record_Date == security_Performance_History.Record_Date
                                                      && r.Entity_ID == EntityID);
            security_Performance_History.Entity_ID = EntityID;

            if (ModelState.IsValid)
            {
                if (exists)
                {
                    ModelState.AddModelError("Name", "FAILED to create Security Performance History for Portfolio " + security_Performance_History.Portfolio_Code + " as it Already exists!");
                }
                else
                {
                    security_Performance_History.Last_Update_Date = DateTime.Now;
                    security_Performance_History.Last_Update_User = User.Identity.Name;
                    security_Performance_History.Hist_Last_Update_Date = DateTime.Now;
                    security_Performance_History.Hist_Last_Update_User = User.Identity.Name;

                    db11.Add(security_Performance_History);
                    db11.Save();
                    TempData["ResultMessage"] = "Security Performance  History for Security " + security_Performance_History.Security_ID.ToString()
                                                 + " Portfolio " + security_Performance_History.Portfolio_Code +
                                                    " created successfully!";

                    return RedirectToAction("Index", new { SecurityId = security_Performance_History.Security_ID, PortfolioCode = security_Performance_History.Portfolio_Code });

                }
            }
            return View(security_Performance_History); 

        }

        // GET: 
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit(string PortfolioCode, decimal SecurityId, DateTime RecordDate)
        {

            var EntityID = (decimal)ViewBag.EntityId;

            var security_Performance_History = db11.FindBy2(r => r.Security_ID == SecurityId && r.Portfolio_Code == PortfolioCode
                                                            && r.Record_Date == RecordDate
                                                            && r.Entity_ID == EntityID)
                                                .FirstOrDefault();

            if (security_Performance_History == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecuritiesAll = security_Performance_History.Security_ID;
            return View(security_Performance_History);

        }

        // POST: SecurityPerformanceHistory/ 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,ModDietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,Dietz_Performance_Quarterly,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Last_Update_Date,Last_Update_User,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User")] Security_Performance_History security_Performance_History)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(security_Performance_History); //sets the modified status 
                security_Performance_History.Hist_Last_Update_Date = DateTime.Now;
                security_Performance_History.Hist_Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Security Performance History for \"" + security_Performance_History.Portfolio_Code + "\" editied successfully!";

                return RedirectToAction("Index", new { PortfolioCode = security_Performance_History.Portfolio_Code, SecurityId = security_Performance_History.Security_ID });
       
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Security Performance history");
            ViewBag.EntityIdScope = EntityID;

            return View(security_Performance_History);
 
        }

        // GET:  
        public ActionResult Delete(string PortfolioCode, decimal SecurityId, DateTime RecordDate)
        {  
            var EntityID = (decimal)ViewBag.EntityId; 

            var security_Performance_History = db11.FindBy2(r => r.Security_ID == SecurityId && r.Portfolio_Code == PortfolioCode
                                                            && r.Record_Date == RecordDate
                                                            && r.Entity_ID == EntityID)
                                                .FirstOrDefault();
             
            if (security_Performance_History == null)
            {
                return HttpNotFound();
            }

            return View(security_Performance_History);
        }

        // POST: SecurityPerformanceHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string PortfolioCode, decimal SecurityId, DateTime RecordDate)

        {
            var EntityID = (decimal)ViewBag.EntityId;
            var security_Performance_History = db11.FindBy2(r => r.Security_ID == SecurityId && r.Portfolio_Code == PortfolioCode
                                                         && r.Record_Date == RecordDate
                                                         && r.Entity_ID == EntityID)
                                             .FirstOrDefault();

            db11.Delete(security_Performance_History);
            db11.Save();
            TempData["ResultMessage"] = "A Security Performance History record for Portfolio " + security_Performance_History.Portfolio_Code.ToString() + " deleted successfully!";
            return RedirectToAction("Index", new { PortfolioCode = security_Performance_History.Portfolio_Code, SecurityId = security_Performance_History.Security_ID });
 
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
