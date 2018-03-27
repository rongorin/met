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

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityAttributionController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly ISecurityAttributionRepository db11;


        public SecurityAttributionController(ISecurityAttributionRepository iDb)
        {
            db11 = iDb;
        } 


        // GET: SecurityAttribution
        [CustomEntityAuthoriseFilter]
        public ActionResult Index(string PortfolioCode, DateTime? inputDate, int? numberOfRows, int page = 1, string searchTerm = null, string Nav = "")
      
        { 
            decimal EntityID = (decimal)ViewBag.EntityId;
             
            if (numberOfRows == null)
                numberOfRows = 20;

            //ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);

            ViewBag.Portfolio = PortfolioCode;
            var vwm = db11.GetAll().Include(a => a.Security_Detail)
                                    .Include(a => a.Portfolio)
                                    .Where(a => a.Entity_ID == EntityID)
                                    .Select(g => new SecurityAttributionIndexViewModel
                                    {
                                        Entity_ID = g.Entity_ID,
                                        Security_ID = g.Security_ID, 
                                        Portfolio_Code = g.Portfolio_Code, 
                                        Excess_Weight_Quarterly = g.Excess_Weight_Quarterly,
                                        Relative_Contribution_Quarterly = g.Relative_Contribution_Quarterly,
                                        Security_Weight_Portfolio_Monthly = g.Security_Weight_Portfolio_Monthly,

                                        Ticker = g.Security_Detail.Ticker,

                                    }).Where(x => x.Portfolio_Code == PortfolioCode && x.Entity_ID == EntityID)
                                        .OrderBy(r => r.Security_ID)
                                        .AsQueryable();

            SecurityAttributionIndexViewModel firsRecord = vwm.FirstOrDefault();
            ViewBag.PortfolioCode = firsRecord != null ? firsRecord.Portfolio_Code : null;

            return View(vwm.ToList());  
        }
         

        // GET: 
        [PortfoliosFilter]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create(string PortfolioCode)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            ViewBag.myPortfolioCode = PortfolioCode;
            var secPerf = new Security_Attribution
            {
                Entity_ID = EntityID
            };

            return View(secPerf);
             
        }

        // POST: SecurityAttribution/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        [PortfoliosFilter] 
        public ActionResult Create([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,Excess_Weight_Quarterly,Relative_Contribution_Quarterly,Allocation_Selection_Interaction_Quarterly,StockVs_TotBenchmarkReturn_Quarterly,Selection_Interaction_Quarterly,Asset_Allocation_Quarterly,Excess_Weight_Monthly,Relative_Contribution_Monthly,Allocation_Selection_Interaction_Monthly,StockVs_TotBenchmarkReturn_Monthly,Selection_Interaction_Monthly,Asset_Allocation_Monthly,Excess_Weight_MonthToDate,Relative_Contribution_MonthToDate,Allocation_Selection_Interaction_MonthToDate,StockVs_TotBenchmarkReturn_MonthToDate,Selection_Interaction_MonthToDate,Asset_Allocation_MonthToDate,Last_Update_Date,Last_Update_User,Security_Weight_Portfolio_Monthly,Security_Weight_Portfolio_MonthToDate,Security_Weight_Portfolio_Quarterly,Security_Weight_Benchmark_Monthly,Security_Weight_Benchmark_MonthToDate,Security_Weight_Benchmark_Quarterly,Total_Return_Security_Price_Monthly,Total_Return_Security_Price_MonthToDate,Total_Return_Security_Price_Quarterly,TW_Return_Portfolio_Monthly,TW_Return_Portfolio_MonthToDate,TW_Return_Portfolio_Quarterly,Benchmark_Price_Performance_Monthly,Benchmark_Price_Performance_MonthToDate,Benchmark_Price_Performance_Quarterly")] Security_Attribution security_Attribution)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            security_Attribution.Entity_ID = EntityID;
            if (ModelState.IsValid)
            {   
                if (Attribution_Exists(security_Attribution.Entity_ID,
                                 security_Attribution.Portfolio_Code,
                                 security_Attribution.Security_ID))
                {
                    ModelState.AddModelError("Name", "FAILED to create Attribution record for Portfolio" + security_Attribution.Portfolio_Code + ". Record already exist for this Portfolio/ Security.");

                }
                else
                {
                    security_Attribution.Last_Update_User = User.Identity.Name;
                    security_Attribution.Last_Update_Date = DateTime.Now;
                    db11.Add(security_Attribution);
                    db11.Save();
                    TempData["ResultMessage"] = "New Attribution record for Portfolio " + security_Attribution.Portfolio_Code + "\" created successfully!";
                    return RedirectToAction("Index", new { PortfolioCode = security_Attribution.Portfolio_Code });
                }
            }
            ViewBag.myPortfolioCode = security_Attribution.Portfolio_Code;
            return View(security_Attribution);

            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Attribution.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", security_Attribution.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Attribution.Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Attribution.Entity_ID);
            //return View(security_Attribution);
        }

        // GET: SecurityAttribution/Edit/5
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit(string PortfolioCode, decimal SecurityId) 
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Security_Attribution sperf = db11.FindBy(r => r.Security_ID == SecurityId &&
                                            r.Portfolio_Code == PortfolioCode).Include(s => s.Security_Detail)
                                     .MatchCriteria(c => c.Entity_ID == EntityID).FirstOrDefault();

            if (sperf == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecuritiesAll = sperf.Security_ID;
            return View(sperf);
              
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Attribution.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", security_Attribution.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Attribution.Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Attribution.Entity_ID);
          ;
        }

        // POST: SecurityAttribution/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllSecuritiesInclGenericFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,Excess_Weight_Quarterly,Relative_Contribution_Quarterly,Allocation_Selection_Interaction_Quarterly,StockVs_TotBenchmarkReturn_Quarterly,Selection_Interaction_Quarterly,Asset_Allocation_Quarterly,Excess_Weight_Monthly,Relative_Contribution_Monthly,Allocation_Selection_Interaction_Monthly,StockVs_TotBenchmarkReturn_Monthly,Selection_Interaction_Monthly,Asset_Allocation_Monthly,Excess_Weight_MonthToDate,Relative_Contribution_MonthToDate,Allocation_Selection_Interaction_MonthToDate,StockVs_TotBenchmarkReturn_MonthToDate,Selection_Interaction_MonthToDate,Asset_Allocation_MonthToDate,Last_Update_Date,Last_Update_User,Security_Weight_Portfolio_Monthly,Security_Weight_Portfolio_MonthToDate,Security_Weight_Portfolio_Quarterly,Security_Weight_Benchmark_Monthly,Security_Weight_Benchmark_MonthToDate,Security_Weight_Benchmark_Quarterly,Total_Return_Security_Price_Monthly,Total_Return_Security_Price_MonthToDate,Total_Return_Security_Price_Quarterly,TW_Return_Portfolio_Monthly,TW_Return_Portfolio_MonthToDate,TW_Return_Portfolio_Quarterly,Benchmark_Price_Performance_Monthly,Benchmark_Price_Performance_MonthToDate,Benchmark_Price_Performance_Quarterly")] Security_Attribution security_Attribution)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(security_Attribution); //sets the modified status 
                security_Attribution.Last_Update_Date = DateTime.Now;
                security_Attribution.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Security Attribution for \"" + security_Attribution.Portfolio_Code + "\" editied successfully!";

                return RedirectToAction("Index", new { PortfolioCode = security_Attribution.Portfolio_Code });
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Security Attribution");
            ViewBag.EntityIdScope = EntityID; 
             
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Attribution.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", security_Attribution.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Attribution.Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Attribution.Entity_ID);
            return View(security_Attribution);
        }
 
        public ActionResult Delete(string PortfolioCode, decimal SecurityId) 
        {
            var EntityID = (decimal)ViewBag.EntityId;
            Security_Attribution security_Attribution = db11.FindBy(r => r.Security_ID == SecurityId &&
                                                r.Entity_ID == EntityID &&
                                                r.Portfolio_Code == PortfolioCode
                ).FirstOrDefault();

            if (security_Attribution == null)
            {
                return HttpNotFound();
            }

            return View(security_Attribution); 
             
        }

        // POST: SecurityAttribution/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string PortfolioCode, decimal SecurityId) 
        {
            var EntityID = (decimal)ViewBag.EntityId;
            Security_Attribution security_Attribution = db11.FindBy(r => r.Security_ID == SecurityId &&
                                                r.Entity_ID == EntityID &&
                                                r.Portfolio_Code == PortfolioCode
                                            ).FirstOrDefault();

            db11.Delete(security_Attribution);
            db11.Save();
            TempData["ResultMessage"] = "Attribution for Portfolio " + security_Attribution.Portfolio_Code.ToString() + " deleted successfully!";

            return RedirectToAction("Index", null, new { PortfolioCode = security_Attribution.Portfolio_Code });  
        } 

        private bool Attribution_Exists(decimal entityID, string portfolioCode, decimal securityId)
        {
            Security_Attribution checkExist = db11.FindBy(e => e.Entity_ID == entityID &&
                              e.Security_ID == securityId &&
                              e.Portfolio_Code == portfolioCode).FirstOrDefault();
            if (checkExist != null)

                return true;
            else
                return false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
