using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Data;
using System.Configuration;
using MetopeMVCApp.Filters;
namespace MetopeMVCApp.Controllers
{
    [AuthoriseGenericId]    
    public class DebtExpiryProfileController : Controller
    { 
        private readonly IDebtExpiryProfileRepository db11;
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public DebtExpiryProfileController(IDebtExpiryProfileRepository iDb)
        {
            db11 = iDb;
        }
        // GET: DebtExpiry_Profile
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityId = currentUser.EntityIdScope;

            var dep = db11.GetAll()
                       .MatchEntityID(c => c.Entity_ID == currentUser.EntityIdScope);

            return View(dep.ToList()); 
        }
 
        // GET: DebtExpiry_Profile/Create
        [PartyFilterIssr]   
        public ActionResult Create()
        { 
            return View();
        }

        // POST: DebtExpiry_Profile/Create  
        [HttpPost]
        [PartyFilterIssr]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Party_Code,Entity_ID,Record_Date,Financial_Year_End,Expiring_Debt_Amount")] Debt_Expiry_Profile debtExpProfile)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            debtExpProfile.Entity_ID = currentUser.EntityIdScope;

            if (ModelState.IsValid)
            {
                Debt_Expiry_Profile checkExist = db11.FindBy(r => r.Party_Code == debtExpProfile.Party_Code &&
                                                                   r.Entity_ID == currentUser.EntityIdScope &&
                                                                   r.Financial_Year_End == debtExpProfile.Financial_Year_End)
                                                                 .FirstOrDefault();
                if (checkExist != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Debt Expiry Profile code: \"" + debtExpProfile.Party_Code + "\" for Financial Date:\"" + debtExpProfile.Financial_Year_End + "\". Already exists!");
                }
                else
                {
                    debtExpProfile.Record_Date = DateTime.Now;

                    db11.Add(debtExpProfile);
                    db11.Save();
                    TempData.Add("ResultMessage", "new Debt Expiry Profile code \"" + debtExpProfile.Party_Code + "\" created successfully!");
                    return RedirectToAction("Index");
                }
            }

            return View(debtExpProfile); 
 
        }

        // GET: DebtExpiry_Profile/Edit/5
        [PartyFilterIssr]
        public ActionResult Edit (decimal EntityId, string PartyCode, DateTime FinYearEnd)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityIdScope = currentUser.EntityIdScope;

            if (currentUser.EntityIdScope != EntityId)
            {
                throw new Exception("Forbidden");
            }
            Debt_Expiry_Profile debt_Expiry_Profile = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                                                  r.Financial_Year_End == FinYearEnd).FirstOrDefault();
            if (debt_Expiry_Profile == null)
            {
                return HttpNotFound();
            }
            return View(debt_Expiry_Profile); 
         
        }

        // POST: DebtExpiry_Profile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [PartyFilterIssr]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Party_Code,Entity_ID,Record_Date,Financial_Year_End,Expiring_Debt_Amount")] Debt_Expiry_Profile debt_Expiry_Profile)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());


            if (ModelState.IsValid)
            {
                if (currentUser.EntityIdScope != debt_Expiry_Profile.Entity_ID)
                {
                    ModelState.AddModelError("Error", "An error occurred trying to edit. Party isnt in scope");
                }
                else
                { 
                    db11.Update(debt_Expiry_Profile);
                    db11.Save();
                    TempData["ResultMessage"] = "debt Expiry Profile for party code \"" + debt_Expiry_Profile.Party_Code + "\" edited successfully!";
                    //TempData.Add("ResultMessage", "PartyDebtAnalysis for party code \"" + party_Debt_Analysis.Party_Code + "\" edited successfully!");
                    return RedirectToAction("Index");
                }
            }
            return View(debt_Expiry_Profile);
             
        }

        // GET: DebtExpiry_Profile/Delete/5
        public ActionResult Delete(decimal EntityId, string PartyCode, DateTime FinYearEnd)
        {

            var currentUser = manager.FindById(User.Identity.GetUserId());
            decimal refGenericEntity = (decimal)ViewBag.genericEntity;

             if (EntityId == null || PartyCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             if (currentUser.EntityIdScope != EntityId && refGenericEntity != EntityId)
             {
                 throw new Exception("Not Acceptable");
                 //return new 
             }
            Debt_Expiry_Profile pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                    r.Financial_Year_End == FinYearEnd).FirstOrDefault();
            if (pda == null)
            {
                return HttpNotFound();
            }
         
            return View(pda);
        }

        // POST: DebtExpiry_Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal EntityId, string PartyCode, DateTime FinYearEnd)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            decimal refGenericEntity = (decimal)ViewBag.genericEntity;

            if (currentUser.EntityIdScope != EntityId)
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!

            Debt_Expiry_Profile debt_Expiry_Profile = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                   r.Financial_Year_End == FinYearEnd).FirstOrDefault();

            db11.Delete(debt_Expiry_Profile);
            db11.Save();
            TempData.Add("ResultMessage", "Debt Expiry Profile code \"" + debt_Expiry_Profile.Party_Code + "\" deleted successfully!");

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
    }
}
