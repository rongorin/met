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
using MetopeMVCApp.Filters;
using MetopeMVCApp.Models;

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityPerformanceController : Controller
    { 
        private readonly ISecurityPerformanceRepository db11 ;

        public SecurityPerformanceController(ISecurityPerformanceRepository iDb)
        {
            db11 = iDb;
        } 
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
                                    .Select(g => new SecurityPerformanceIndexViewModel 
                                    {
                                        Entity_ID = g.Entity_ID,
                                        Security_ID = g.Security_ID,
                                         
                                        Portfolio_Code = g.Portfolio_Code, 

                                        ModDietz_Performance_Quarterly = g.ModDietz_Performance_Quarterly,
                                        ModDietz_Performance_Monthly = g.ModDietz_Performance_Monthly,
                                        ModDietz_Performance_MonthToDate = g.ModDietz_Performance_MonthToDate,
                                        Ticker = g.Security_Detail.Ticker,

                                    }).Where(x => x.Portfolio_Code == PortfolioCode && x.Entity_ID == EntityID)
                                        .OrderBy(r => r.Security_ID) 
                                        .AsQueryable();

            SecurityPerformanceIndexViewModel firsRecord = vwm.FirstOrDefault();
            ViewBag.PortfolioCode = firsRecord != null ? firsRecord.Portfolio_Code : null;

            return View(vwm.ToList());
        }  
 
        // GET: SecurityPerformance/Create
        [PortfoliosFilter]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create(string PortfolioCode)
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 
            
            ViewBag.myPortfolioCode = PortfolioCode;
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name");
            var secPerf = new Security_Performance
            {
                Entity_ID = EntityID
            };
             
            return View(secPerf);
        } 
        // POST: SecurityPerformance/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        [PortfoliosFilter]
        public ActionResult Create([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,ModDietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,Dietz_Performance_Quarterly,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Last_Update_Date,Last_Update_User")] 
                                    Security_Performance securityPerf)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            securityPerf.Entity_ID = EntityID;
     
            if (ModelState.IsValid)
            {
                if (Position_Exists(securityPerf.Entity_ID,
                                 securityPerf.Portfolio_Code,
                                 securityPerf.Security_ID))
                {
                    ModelState.AddModelError("Name", "FAILED to create performance record for portfolio" + securityPerf.Portfolio_Code + ". Already exists!");

                }
                else
                {
                    securityPerf.Last_Update_User = User.Identity.Name;
                    securityPerf.Last_Update_Date = DateTime.Now;
                    db11.Add(securityPerf);
                    db11.Save();
                    TempData["ResultMessage"] = "New Performance record for Portfolio " + securityPerf.Portfolio_Code + "\" created successfully!";
                    return RedirectToAction("Index", new { PortfolioCode = securityPerf.Portfolio_Code });
                } 
            }

            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", securityPerf.Security_ID);
            ViewBag.myPortfolioCode = securityPerf.Portfolio_Code;
            return View(securityPerf);


            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Performance.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", security_Performance.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Performance.Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Performance.Entity_ID);
             
        }

        // GET: SecurityPerformance/Edit/ 
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit(string PortfolioCode, decimal SecurityId)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Security_Performance sperf = db11.FindBy(r => r.Security_ID == SecurityId &&
                                            r.Portfolio_Code == PortfolioCode).Include(s => s.Security_Detail)
                                     .MatchCriteria(c => c.Entity_ID == EntityID).FirstOrDefault();

            if (sperf == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecuritiesAll = sperf.Security_ID;

            return View(sperf);
        }

        // POST: SecurityPerformance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllSecuritiesInclGenericFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Security_ID,Portfolio_Code,Entity_ID,ModDietz_Performance_Quarterly,ModDietz_Performance_Monthly,ModDietz_Performance_MonthToDate,Dietz_Performance_Quarterly,Dietz_Performance_Monthly,Dietz_Performance_MonthToDate,Last_Update_Date,Last_Update_User")] Security_Performance security_Performance)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            
            if (ModelState.IsValid)
            {
                db11.Update(security_Performance); //sets the modified status 
                security_Performance.Last_Update_Date = DateTime.Now;
                security_Performance.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"]= "Security Performance for \"" + security_Performance.Portfolio_Code + "\" editied successfully!";

                return RedirectToAction("Index", new { PortfolioCode = security_Performance.Portfolio_Code, SecurityId = security_Performance.Security_ID });

            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Security Performance"); 
            ViewBag.EntityIdScope = EntityID; 

            return View(security_Performance);
        } 
        // GET: SecurityPerformance/Delete/5
        public ActionResult Delete(string  PortfolioCode, decimal SecurityId)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            Security_Performance security_Performance = db11.FindBy(r => r.Security_ID == SecurityId && 
                                                r.Entity_ID == EntityID &&
                                                r.Portfolio_Code == PortfolioCode
                ).FirstOrDefault();

            if (security_Performance == null)
            {
                return HttpNotFound();
            } 
             
            return View(security_Performance);
        }

        // POST: SecurityPerformance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string  PortfolioCode, decimal SecurityId)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            Security_Performance security_Performance = db11.FindBy(r => r.Security_ID == SecurityId && 
                                                r.Entity_ID == EntityID &&
                                                r.Portfolio_Code == PortfolioCode
                                            ).FirstOrDefault();
             
            db11.Delete(security_Performance);
            db11.Save();
            TempData["ResultMessage"] = "Performance for Portfolio " + security_Performance.Portfolio_Code.ToString() + " deleted successfully!";

            return RedirectToAction("Index", null, new { PortfolioCode = security_Performance.Portfolio_Code }); 
        }  

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db11.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool Position_Exists(decimal entityID, string portfolioCode, decimal securityId )
        {
             Security_Performance checkExist= db11.FindBy (e => e.Entity_ID == entityID &&
                               e.Security_ID == securityId &&
                               e.Portfolio_Code == portfolioCode).FirstOrDefault() ;
             if (checkExist != null)
                
                  return true;
             else
                    return false; 
        }
    }
}
