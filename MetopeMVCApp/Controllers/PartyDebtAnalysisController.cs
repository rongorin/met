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
        public ActionResult Index(string PartyCode = "", string Nav = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;     

            var pda = db11.GetAll()
                       .MatchCriteria(c => c.Entity_ID == EntityID)
                       .MatchCriteria(c => ((PartyCode != "") ? c.Party_Code == PartyCode : c.Party_Code != ""))
                     .OrderBy(r => r.Party_Code).ThenBy(n => n.Financials_Type);
             
            if (PartyCode != "")
                ViewBag.PartyCode = PartyCode;

            ViewBag.Nav = Nav;

            return View(pda.ToList()); 
        } 
         
        [FinancialsType]
        [PartyFilterIssr]
        public ActionResult Create(string Nav)
        {
            ViewBag.Nav = Nav; 
            return View();   
        }
         
        [HttpPost]
        [PartyFilterIssr]
        [FinancialsType]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Party_Code,Financials_Type,Entity_ID,Financials_Date,Investment_Properties,Other_Investments,Total_Investments,Weighted_Investments,ST_Interest_Bearing_Debt,LT_Interest_Bearing_Debt,Total_Interest_Bearing_Borrowings,Debt_Hedged_Amount,Debt_Hedged_Percent,Floating_Debt_Amount,Floating_Debt_Percent,Total_Weighted_Debt_Cost,Capital_Markets_Debt,Traditional_Bank_Debt,Last_Update_Date,Last_Update_User")] Party_Debt_Analysis partyDebtAnalysis, string navIndicator="")
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
                    if (navIndicator == "")
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", null, new { PartyCode = partyDebtAnalysis.Party_Code, Nav = navIndicator });

                }
            }
           
            return View(partyDebtAnalysis);
        }
        [CustomEntityAuthoriseFilter]
        public ActionResult Edit(decimal EntityId, string PartyCode, string FinType, string Nav)
        { 
            Party_Debt_Analysis pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                r.Financials_Type == FinType).FirstOrDefault();
             
            if (pda == null)
            {
                return HttpNotFound();
            }

            ViewBag.Nav = Nav;

            return View(pda);
        }

        // POST: PartyDebtAnalysis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Party_Code,Financials_Type,Entity_ID,Financials_Date,Investment_Properties,Other_Investments,Total_Investments,Weighted_Investments,ST_Interest_Bearing_Debt,LT_Interest_Bearing_Debt,Total_Interest_Bearing_Borrowings,Debt_Hedged_Amount,Debt_Hedged_Percent,Floating_Debt_Amount,Floating_Debt_Percent,Total_Weighted_Debt_Cost,Capital_Markets_Debt,Traditional_Bank_Debt,Last_Update_Date,Last_Update_User")] Party_Debt_Analysis party_Debt_Analysis
                                             , string navIndicator = "")
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
          
                    //return RedirectToAction("Index", "PartyDebtAnalysis", new {PartyCode = party_Debt_Analysis.Party_Code });

                    if (navIndicator == "")
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", null, new { PartyCode = party_Debt_Analysis.Party_Code, Nav = navIndicator });
                }
            } 
            return View(party_Debt_Analysis);
        }

        // GET: PartyDebtAnalysis/Delete/5
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(string PartyCode, string FinType, decimal EntityId, string Nav)
        { 
            Party_Debt_Analysis pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                    r.Financials_Type == FinType).FirstOrDefault();
              
            if (pda == null)
            {
                 return HttpNotFound();
            }
            ViewBag.Nav = Nav; 

            return View(pda);
        }

        // POST: PartyDebtAnalysis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(string PartyCode, string FinType, decimal EntityId, string navIndicator = "")
        {  
            Party_Debt_Analysis pda = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                    r.Financials_Type == FinType).FirstOrDefault();

            db11.Delete(pda);
            db11.Save();
            TempData.Add("ResultMessage", "Party Debt Analysis  code \"" + pda.Party_Code + "\" deleted successfully!");
             
            if (navIndicator == "")
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", null, new { PartyCode = pda.Party_Code, Nav = navIndicator });
             
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
