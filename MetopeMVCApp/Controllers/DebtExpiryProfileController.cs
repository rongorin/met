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
using PagedList;
namespace MetopeMVCApp.Controllers

{ 
    [SetAllowedEntityIdAttribute]
    public class DebtExpiryProfileController : Controller
    { 
        private readonly IDebtExpiryProfileRepository db11;
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public DebtExpiryProfileController(IDebtExpiryProfileRepository iDb)
        {
            db11 = iDb;
        }

        public ActionResult Index(string PartyCode = "", int? iEntityId =null, string Nav = "" )
        {
            var EntityID = (decimal)ViewBag.EntityId;
            decimal refGenericEntity = (decimal)ViewBag.genericEntity;
             

            //var dep = db11.GetAllDebtExpiryValues( currentUser.EntityIdScope, (decimal)ViewBag.genericEntity).

            var dep = db11.GetAll() 
                    .MatchCriteria(c => ( (PartyCode != "") ? c.Party_Code == PartyCode : c.Party_Code != "")
                                  )
                    .MatchCriteria(c => ((iEntityId != null) ? c.Entity_ID == iEntityId : (c.Entity_ID == EntityID || c.Entity_ID == refGenericEntity))
                                  ) 
                     .OrderBy(r => r.Party_Code).ThenByDescending(n => n.Financial_Year_End) ;
                    //.SearchPartyCodes(PartyCode);

            if (PartyCode != "")
                ViewBag.PartyCode = PartyCode;

            ViewBag.Nav = Nav;

            return View(dep.ToList()); 
        } 
     
        [PartyFilterIssr]   
        public ActionResult Create( string Nav)
        {
            ViewBag.Nav = Nav;
            return View();
        }

        // POST:  
        [HttpPost]
        [PartyFilterIssr]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Party_Code,Entity_ID,Record_Date,Financial_Year_End, Expiring_Debt_Amount")] Debt_Expiry_Profile debtExpProfile, string navIndicator="")
        {
            var EntityID = (decimal)ViewBag.EntityId; 

            if (ModelState.IsValid)
            {
                Debt_Expiry_Profile checkExist = db11.FindBy(r => r.Party_Code == debtExpProfile.Party_Code &&
                                                                   r.Entity_ID == EntityID &&
                                                                   r.Financial_Year_End == debtExpProfile.Financial_Year_End)
                                                                 .FirstOrDefault();
            
                if (checkExist != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Debt Expiry Profile code: \"" + debtExpProfile.Party_Code + "\" for Financial Date:\"" + debtExpProfile.Financial_Year_End + "\". Already exists!");
                }
                else
                {
                    debtExpProfile.Record_Date = DateTime.Now;
                    debtExpProfile.Entity_ID = EntityID;

                    db11.Add(debtExpProfile);
                    db11.Save();
                    TempData.Add("ResultMessage", "new Debt Expiry Profile code \"" + debtExpProfile.Party_Code + "\" created successfully!");
                    if (navIndicator == "")
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", null, new { PartyCode = debtExpProfile.Party_Code, Nav = navIndicator });

                }
            }
                
            return View(debtExpProfile); 
 
        }

        // GET: DebtExpiry_Profile/Edit/5
        [CustomEntityAuthoriseFilter]
        [PartyFilterIssr]
        public ActionResult Edit(decimal EntityId, string PartyCode, DateTime FinYearEnd, string Nav)
        {   
            Debt_Expiry_Profile debt_Expiry_Profile = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                                                  r.Financial_Year_End == FinYearEnd).FirstOrDefault();
            if (debt_Expiry_Profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.PartyCode = debt_Expiry_Profile.Party_Code;

            ViewBag.Nav = Nav;
            return View(debt_Expiry_Profile);  
        }

        // POST: DebtExpiry_Profile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [PartyFilterIssr]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Party_Code,Entity_ID,Record_Date,Financial_Year_End,Expiring_Debt_Amount ")] Debt_Expiry_Profile debt_Expiry_Profile, string navIndicator = "")
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
                    TempData["ResultMessage"] = "Debt Expiry Profile for party code \"" + debt_Expiry_Profile.Party_Code + "\" edited successfully!";
                    //TempData.Add("ResultMessage", "PartyDebtAnalysis for party code \"" + party_Debt_Analysis.Party_Code + "\" edited successfully!");

                    if (navIndicator == "")  
                        return RedirectToAction("Index"  );
                    else 
                       return RedirectToAction("Index", null, new { PartyCode = debt_Expiry_Profile.Party_Code ,Nav = navIndicator });

                }
            } 
             
            return View(debt_Expiry_Profile);
        }

        // GET: DebtExpiry_Profile/Delete/5
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(decimal EntityId, string PartyCode, DateTime FinYearEnd, string Nav)
        { 
             
             Debt_Expiry_Profile pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                    r.Financial_Year_End == FinYearEnd).FirstOrDefault();
            if (pda == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nav = Nav; 
            ViewBag.PartyCode = PartyCode;
         
            return View(pda);
        }

        // POST: DebtExpiry_Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal EntityId, string PartyCode, DateTime FinYearEnd, string navIndicator = "")
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

            if (navIndicator == "")
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", null, new { PartyCode = debt_Expiry_Profile.Party_Code, Nav = navIndicator });
             
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
