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
    [SetAllowedEntityIdAttribute]
    public class PartyDebtAnalysisController : Controller
    { 
        private readonly IPartyDebtAnalysisRepository db11;
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
   
        public PartyDebtAnalysisController(IPartyDebtAnalysisRepository iDb)
        {
            db11 = iDb; 
        } 
      
        // GET: PartyDebtAnalysis  
        public ActionResult Index(string PartyCode = "")
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityId = currentUser.EntityIdScope;

            var pda = db11.GetAll()
                       .MatchCriteria(c => c.Entity_ID == currentUser.EntityIdScope)
                       .MatchCriteria(c => ((PartyCode != "") ? c.Party_Code == PartyCode : c.Party_Code != ""))
                     .OrderBy(r => r.Party_Code).ThenBy(n => n.Financials_Type);


            if (PartyCode != "")
                ViewBag.PartyCode = PartyCode;

            return View(pda.ToList()); 
        } 
         
        [FinancialsType]
        [PartyFilterIssr] 
        public ActionResult Create()
        {    
            return View();   
        }
         
        [HttpPost]
        [PartyFilterIssr]
        [FinancialsType]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Party_Code,Financials_Type,Entity_ID,Financials_Date,Investment_Properties,Other_Investments,Total_Investments,Weighted_Investments,ST_Interest_Bearing_Debt,LT_Interest_Bearing_Debt,Total_Interest_Bearing_Borrowings,Debt_Hedged_Amount,Debt_Hedged_Percent,Floating_Debt_Amount,Floating_Debt_Percent,Total_Weighted_Debt_Cost,Capital_Markets_Debt,Traditional_Bank_Debt,Last_Update_Date,Last_Update_User")] Party_Debt_Analysis partyDebtAnalysis)
        { 
            var currentUser = manager.FindById(User.Identity.GetUserId());
            partyDebtAnalysis.Entity_ID = currentUser.EntityIdScope;
 
            if (ModelState.IsValid)
            {
                Party_Debt_Analysis checkExist = db11.FindBy(r => r.Party_Code == partyDebtAnalysis.Party_Code && 
                                                                   r.Entity_ID == currentUser.EntityIdScope && 
                                                                   r.Financials_Type == partyDebtAnalysis.Financials_Type)
                                                                 .FirstOrDefault();
                if (checkExist != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create PartyDebtAnalysis type: \"" + partyDebtAnalysis.Financials_Type + "\" code:\"" + partyDebtAnalysis.Party_Code + "\". Already exists!");
                }
                else
                {
                    partyDebtAnalysis.Last_Update_Date = DateTime.Now;
                    partyDebtAnalysis.Last_Update_User = User.Identity.Name;  

                    db11.Add(partyDebtAnalysis);
                    db11.Save();
                    TempData.Add("ResultMessage", "new PartyDebtAnalysis \"" + partyDebtAnalysis.Party_Code + "\" created successfully!");
                    return RedirectToAction("Index");
                }
            }
           
            return View(partyDebtAnalysis);
        }
         
        public ActionResult Edit(decimal EntityId, string PartyCode, string FinType)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (currentUser.EntityIdScope != EntityId)
            {
                throw new Exception("Forbidden"); 
            } 
            Party_Debt_Analysis pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                r.Financials_Type == FinType).FirstOrDefault();
             
            if (pda == null)
            {
                return HttpNotFound();
            }  
       
            return View(pda);
        }

        // POST: PartyDebtAnalysis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Party_Code,Financials_Type,Entity_ID,Financials_Date,Investment_Properties,Other_Investments,Total_Investments,Weighted_Investments,ST_Interest_Bearing_Debt,LT_Interest_Bearing_Debt,Total_Interest_Bearing_Borrowings,Debt_Hedged_Amount,Debt_Hedged_Percent,Floating_Debt_Amount,Floating_Debt_Percent,Total_Weighted_Debt_Cost,Capital_Markets_Debt,Traditional_Bank_Debt,Last_Update_Date,Last_Update_User")] Party_Debt_Analysis party_Debt_Analysis)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                if (currentUser.EntityIdScope != party_Debt_Analysis.Entity_ID  )
                {
                    ModelState.AddModelError("Error", "An error occurred trying to edit. Party isnt in scope");
                }
                else
                {
                    party_Debt_Analysis.Last_Update_Date = DateTime.Now;
                    party_Debt_Analysis.Last_Update_User = User.Identity.Name;  
                    db11.Update(party_Debt_Analysis);
                    db11.Save();
                    TempData["ResultMessage"] = "PartyDebtAnalysis for party code \"" + party_Debt_Analysis.Party_Code + "\" edited successfully!";
                    //TempData.Add("ResultMessage", "PartyDebtAnalysis for party code \"" + party_Debt_Analysis.Party_Code + "\" edited successfully!");
          
                    return RedirectToAction("Index", "PartyDebtAnalysis", new {PartyCode = party_Debt_Analysis.Party_Code });

                }
            } 
            return View(party_Debt_Analysis);
        }

        // GET: PartyDebtAnalysis/Delete/5
        public ActionResult Delete(string PartyCode, string FinType, decimal EntityId)
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
                //return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!
            }

            Party_Debt_Analysis pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                    r.Financials_Type == FinType).FirstOrDefault();
              
            if (pda == null)
            {
                 return HttpNotFound();
            }

            return View(pda);
        }

        // POST: PartyDebtAnalysis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string PartyCode, string FinType, decimal EntityId)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            decimal refGenericEntity = (decimal)ViewBag.genericEntity;

            if (currentUser.EntityIdScope != EntityId )
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!

            Party_Debt_Analysis pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                    r.Financials_Type == FinType).FirstOrDefault();

            db11.Delete(pda);
            db11.Save();
            TempData.Add("ResultMessage", "Party Debt Analysis  code \"" + pda.Party_Code + "\" deleted successfully!");
              
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
