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
using MetopeMVCApp.Models;

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class PositionSODController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            };
        // GET: PositionSOD

         


        [CustomEntityAuthoriseFilter]
        public ActionResult Index(string PortfolioCode, DateTime? inputDate, int? numberOfRows, int page = 1, string searchTerm = null, string Nav = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            if (numberOfRows == null)
                numberOfRows = 20;

            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);

            ViewBag.Portfolio = PortfolioCode;
            var vm = db.Position_SOD.
                                    Include(s => s.Security_Detail).
                                    Select(r => new PositionsSODIndexViewModel
                                    {
                                        Entity_ID = r.Entity_ID,
                                        Security_ID = r.Security_ID,
                                        Long_Short_Indicator = r.Long_Short_Indicator,
                                        Portfolio_Code = r.Portfolio_Code,
                                        Position_Date = r.Position_Date,
                                        Dealt_Quantity = r.Dealt_Quantity,
                                        Dealt_AllIn_Mkt_Value_BaseCur = r.Dealt_AllIn_Mkt_Value_BaseCur,
                                        Ticker = r.Security_Detail.Ticker
                                    }).Where(x => x.Portfolio_Code == PortfolioCode && x.Entity_ID == EntityID)
                                        .OrderByDescending(r => r.Position_Date).ThenBy(r => r.Ticker)
                                        .AsQueryable();
            //ToList(); 

            if (inputDate != null)
            {
                DateTime dtEqualTo = Convert.ToDateTime(inputDate);
                vm = vm.Where(x => x.Position_Date == dtEqualTo);
                ViewBag.UserInputDate = dtEqualTo.ToString("dd/MM/yyyy");
            }


            return View(vm.ToList());
        }
         

        // GET: PositionSOD/Create
        [LongShortIndicatorFilter]
        [PortfoliosFilter]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create(string PortfolioCode)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            ViewBag.myPortfolioCode = PortfolioCode;
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name");
            return View();
        }

        // POST: PositionSOD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        [PortfoliosFilter]
        [LongShortIndicatorFilter]
        public ActionResult Create([Bind(Include = "Entity_ID,Portfolio_Code,Security_ID,Long_Short_Indicator,Position_Date,Dealt_Quantity,Unsettled_Quantity,Settled_Quantity,Dealt_AllIn_Mkt_Value_PriceCur,Unsettled_AllIn_Mkt_Value_PriceCur,Settled_AllIn_Mkt_Value_PriceCur,Dealt_AllIn_Mkt_Value_BaseCur,Unsettled_AllIn_Mkt_Value_BaseCur,Settled_AllIn_Mkt_Value_BaseCur,Dealt_Clean_Mkt_Value_PriceCur,Unsettled_Clean_Mkt_Value_PriceCur,Settled_Clean_Mkt_Value_PriceCur,Dealt_Clean_Mkt_Value_BaseCur,Unsettled_Clean_Mkt_Value_BaseCur,Settled_Clean_Mkt_Value_BaseCur,Dealt_Income_Mkt_Value_PriceCur,Unsettled_Income_Mkt_Value_PriceCur,Settled_Income_Mkt_Value_PriceCur,Dealt_Income_Mkt_Value_BaseCur,Unsettled_Income_Mkt_Value_BaseCur,Settled_Income_Mkt_Value_BaseCur,Exposure_BaseCur,Exposure_PriceCur,Average_Unit_Cost_BaseCur,Average_Unit_Cost_PriceCur,Pledged_Quantity,Segregated_Quantity,Model_Percent,Last_Update_Date,Last_Update_User,Unsettled_Expenses_BaseCur,Unsettled_Expenses_PriceCur")] Position_SOD position_SOD)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            position_SOD.Entity_ID = EntityID;
            if (ModelState.IsValid)
            {
                if (Position_Exists(position_SOD.Entity_ID,
                                    position_SOD.Portfolio_Code,
                                    position_SOD.Security_ID,
                                    position_SOD.Position_Date,
                                    position_SOD.Long_Short_Indicator))
                {
                    ModelState.AddModelError("Name", "FAILED to create " + position_SOD.Long_Short_Indicator + "\" Position for Security: \"" + position_SOD.Security_ID + "\" Position date:\"" + position_SOD.Position_Date.ToString("yyyy/MM/dd") + "\". Already exists!");

                }
                else
                {
                    position_SOD.Last_Update_User = User.Identity.Name;
                    position_SOD.Last_Update_Date = DateTime.Now;
                    db.Position_SOD.Add(position_SOD);
                    db.SaveChanges();
                    TempData["ResultMessage"] = "New Position for Portfolio " + position_SOD.Portfolio_Code + "\" created successfully!";
                    return RedirectToAction("Index", new { PortfolioCode = position_SOD.Portfolio_Code });
                }
            }
             
        //    ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", position_SOD.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", position_SOD.Security_ID);
            ViewBag.myPortfolioCode = position_SOD.Portfolio_Code;
            return View(position_SOD);
        }

        // GET: PositionSOD/Edit/5
        [CustomEntityAuthoriseFilter]
        [AllSecuritiesInclGenericFilter]
        [LongShortIndicatorFilter]
        public ActionResult Edit(string PortfolioCode, decimal EntityId, decimal SecurityId, DateTime PosDate, string LongShortInd, string Nav = "")

        {
            if (SecurityId == null || EntityId == null || PortfolioCode == null || LongShortInd == null || PosDate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position_SOD position_SOD = db.Position_SOD.Include(i => i.Security_Detail)
                                                            .Single(p => p.Entity_ID == EntityId && p.Portfolio_Code == PortfolioCode
                                                                  && p.Security_ID == SecurityId
                                                                  && p.Position_Date == PosDate && p.Long_Short_Indicator == LongShortInd); 
            if (position_SOD == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecuritiesAll = position_SOD.Security_ID;
            ViewBag.LongShortInd = position_SOD.Long_Short_Indicator;
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", position_SOD.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", position_SOD.Security_ID);
            return View(position_SOD);
        }

        // POST: PositionSOD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Edit([Bind(Include = "Entity_ID,Portfolio_Code,Security_ID,Long_Short_Indicator,Position_Date,Dealt_Quantity,Unsettled_Quantity,Settled_Quantity,Dealt_AllIn_Mkt_Value_PriceCur,Unsettled_AllIn_Mkt_Value_PriceCur,Settled_AllIn_Mkt_Value_PriceCur,Dealt_AllIn_Mkt_Value_BaseCur,Unsettled_AllIn_Mkt_Value_BaseCur,Settled_AllIn_Mkt_Value_BaseCur,Dealt_Clean_Mkt_Value_PriceCur,Unsettled_Clean_Mkt_Value_PriceCur,Settled_Clean_Mkt_Value_PriceCur,Dealt_Clean_Mkt_Value_BaseCur,Unsettled_Clean_Mkt_Value_BaseCur,Settled_Clean_Mkt_Value_BaseCur,Dealt_Income_Mkt_Value_PriceCur,Unsettled_Income_Mkt_Value_PriceCur,Settled_Income_Mkt_Value_PriceCur,Dealt_Income_Mkt_Value_BaseCur,Unsettled_Income_Mkt_Value_BaseCur,Settled_Income_Mkt_Value_BaseCur,Exposure_BaseCur,Exposure_PriceCur,Average_Unit_Cost_BaseCur,Average_Unit_Cost_PriceCur,Pledged_Quantity,Segregated_Quantity,Model_Percent,Last_Update_Date,Last_Update_User,Unsettled_Expenses_BaseCur,Unsettled_Expenses_PriceCur")] Position_SOD position_SOD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(position_SOD).State = EntityState.Modified;
                position_SOD.Last_Update_User = User.Identity.Name;
                position_SOD.Last_Update_Date = DateTime.Now; 
                db.SaveChanges();
                TempData["ResultMessage"] = "Position for portfolio \"" + position_SOD.Portfolio_Code + "\" edited successfully!";
                return RedirectToAction("Index", new { PortfolioCode = position_SOD.Portfolio_Code });
             
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", position_SOD.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", position_SOD.Security_ID);
            return View(position_SOD);
        }

        [CustomEntityAuthoriseFilter]
        // GET: PositionSOD/Delete/5
        public ActionResult Delete(decimal SecurityId, decimal EntityId, string PortfolioCode, DateTime PosDate, string LongShortInd)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
          
            if (SecurityId == null || EntityId == null || PortfolioCode == null || LongShortInd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position_SOD position_SOD = db.Position_SOD.Include(i => i.Security_Detail).Include(x => x.Security_Detail)
                                                                    .SingleOrDefault(p => p.Entity_ID == EntityId && p.Portfolio_Code == PortfolioCode
                                                                          && p.Security_ID == SecurityId
                                                                          && p.Position_Date == PosDate && p.Long_Short_Indicator == LongShortInd);
            if (position_SOD == null)
            {
                return HttpNotFound();
            }
        
            return View(position_SOD);
        }

        // POST: PositionSOD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal SecurityId, decimal EntityId, string PortfolioCode, DateTime PosDate, string LongShortInd)

        {
            Position_SOD position_SOD = db.Position_SOD.Include(i => i.Security_Detail)
                                                                         .SingleOrDefault(p => p.Entity_ID == EntityId && p.Portfolio_Code == PortfolioCode
                                                                               && p.Security_ID == SecurityId
                                                                               && p.Position_Date == PosDate && p.Long_Short_Indicator == LongShortInd);
  
            db.Position_SOD.Remove(position_SOD);
            db.SaveChanges();
            TempData.Add("ResultMessage", "Position for portfolio \"" + position_SOD.Portfolio_Code + "\" deleted successfully!");
            return RedirectToAction("Index", new { PortfolioCode = position_SOD.Portfolio_Code });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Position_Exists(decimal entityID, string portfolioCode, decimal securityID, DateTime posDate, string longShortInd)
        {
            return db.Position_SOD.Count(e => e.Entity_ID == entityID &&
                               e.Security_ID == securityID &&
                               e.Portfolio_Code == portfolioCode &&
                               e.Position_Date == posDate &&
                               e.Long_Short_Indicator == longShortInd) > 0;
        }
    }
}
