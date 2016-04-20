using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity;


namespace MetopeMVCApp.Controllers
{
    public class PortfolioController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();

        // GET: /Portfolio/
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
           //var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id;
            var checkingAccountId = db.AspNetUsers.Where(c => c.Id == userId).First().Id;


            ViewBag.CheckingAccountId = checkingAccountId;
            var portfolios = db.Portfolios.Include(p => p.Entity).Include(p => p.User);
            return View(portfolios.ToList());
        }

        // GET: /Portfolio/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = db.Portfolios.Find(id);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        // GET: /Portfolio/Create
        public ActionResult Create()
        {

            ViewBag.Entity_ID  = new SelectList(db.Entities, "Entity_ID", "Entity_Code"); 
            ViewBag.managers = new SelectList(LoadManagers(), "User_Code", "User_Name");
                
            return View();
        }

        // POST: /Portfolio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Entity_ID,Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End,Custodian_Code,Active_Flag,System_Locked")] Portfolio portfolio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Portfolios.Add(portfolio);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } 

                ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
                ViewBag.managers = new SelectList(LoadManagers(), "User_Code", "User_Name");

            }


            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            return View(portfolio);
        }

        // GET: /Portfolio/Edit/5
        public ActionResult Edit(decimal EntityId, string PortfolioCode)
        {
            if (EntityId == null || PortfolioCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = db.Portfolios.Find(EntityId, PortfolioCode);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.managers = new SelectList(LoadManagers(), "User_Code", "User_Name",portfolio.Manager);

            return View(portfolio);
        }

        // POST: /Portfolio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Entity_ID,Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End,Custodian_Code,Active_Flag,System_Locked")] Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portfolio).State = EntityState.Modified;
                db.SaveChanges();
                TempData.Add("SuccessMessage", "Portfolio \"" + portfolio.Portfolio_Name + "\" editied successfully!");
                return RedirectToAction("Index");
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID); 
            ViewBag.managers = new SelectList(LoadManagers(), "User_Code", "User_Name", portfolio.Manager);
            return View(portfolio);
        }

        // GET: /Portfolio/Delete/5
        public ActionResult Delete(decimal EntityId, string PortfolioCode)
        {
            if (EntityId == null || PortfolioCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = db.Portfolios.Find(EntityId, PortfolioCode);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        // POST: /Portfolio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal EntityId, string PortfolioCode)
        {
            Portfolio portfolio = db.Portfolios.Find(EntityId, PortfolioCode);
            db.Portfolios.Remove(portfolio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IQueryable<User> LoadManagers()
        {

            return db.Users.Where(r => r.Entity_ID == 1);

            //return from t in db.Users
            //       where t.Entity_ID == 1
            //       select t;
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
