using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using MetopeMVCApp.Data;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Filters;

namespace MetopeMVCApp.Controllers
{
    [AuthoriseGenericId]   
    public class PartyFinancialsController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly IPartyFinancialsRepository db11;
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
   
 

        public PartyFinancialsController(IPartyFinancialsRepository iDb)
        {
            db11 = iDb; 
        } 
        // GET: PartyFinancials
        public ActionResult Index()
        {  
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityId = currentUser.EntityIdScope;

            var pfin = db11.GetAll()
                       .MatchEntityID(c => c.Entity_ID == currentUser.EntityIdScope);

            return View(pfin.ToList()); 
        }

        [PartyFilterIssr]
        [SecuritiesFilter]
        [ActualForecastIndicatoreFilter]
        public ActionResult Create()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityIdScope = currentUser.EntityIdScope;  
            return View();
        }

        // POST: PartyFinancials/Create  
        [HttpPost]
        [PartyFilterIssr]
        [SecuritiesFilter]
        [ActualForecastIndicatoreFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Party_Code,Entity_ID,Actual_Forecast_Indicator,Security_ID,Financials_Date,Full_Adjusted_Share_NAV,Enterprise_Value,Dividend_Yield,Net_Present_Value,Net_Property_Income,Vacancies_Overall,Portfolio_Leases_Expiring,Assumed_Ave_Escalation_Rate,Cap_Rate_Movemt_Assump,Reversion_Exp_Rent_Reviews,Ungeared_Yield,Net_Operating_Profit,Revenue_Growth,Net_Property_Income_Growth,Net_Operating_Profit_Growth,Interest_Paid_Growth,Loan_To_Value,Interest_Cover_Ratio,Revenue,Interest_Paid,Property_Expenses,Property_Expenses_Growth,Investment_Income,Admin_Expenses,Admin_Expenses_Growth,Taxation,Other_Income,Distributable_Earnings,Distributions_Growth,Linked_Units_Issued,Borrowings,Units_Issued,Purchases_And_Developments,Sales,Total_Assets,Current_Liabilities,Interest_Bearing_Borrowings,Last_Update_Date,Last_Update_User,Forecast_Distributions_Growth,Property_Portfolio_Value")] Party_Financials partyFinancials)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            partyFinancials.Entity_ID = currentUser.EntityIdScope;

            if (ModelState.IsValid)
            {
                Party_Financials checkExist = db11.FindBy(r => r.Party_Code == partyFinancials.Party_Code && 
                                                                   r.Entity_ID == currentUser.EntityIdScope && 
                                                                   r.Actual_Forecast_Indicator == partyFinancials.Actual_Forecast_Indicator)
                                                                 .FirstOrDefault();
                if (checkExist != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Party Financials Party code: \"" + partyFinancials.Party_Code + "\"  Forecast indicator:\"" + partyFinancials.Actual_Forecast_Indicator + "\". Already exists!");
                }
                else
                {
                    partyFinancials.Last_Update_Date = DateTime.Now;
                    partyFinancials.Last_Update_User = User.Identity.Name;  

                    db11.Add(partyFinancials);
                    db11.Save();
                    TempData.Add("ResultMessage", "new Financials Party for \"" + partyFinancials.Party_Code + "\" created successfully!");
                    return RedirectToAction("Index");
                } 
            }
              
            return View(partyFinancials);
        }

        // GET: PartyFinancials/Edit/5
        [PartyFilterIssr]
        [SecuritiesFilter]
        [ActualForecastIndicatoreFilter]
        public ActionResult Edit(decimal EntityId, string PartyCode, string ActualForecastInd)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityIdScope = currentUser.EntityIdScope;  
             
            if (currentUser.EntityIdScope != EntityId)
            {
                throw new Exception("Forbidden");
            }
            Party_Financials partyFins = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                r.Actual_Forecast_Indicator == ActualForecastInd).FirstOrDefault(); 
            if (partyFins == null)
            {
                return HttpNotFound();
            } 
            return View(partyFins); 
        }

        // POST: PartyFinancials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActualForecastIndicatoreFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Party_Code,Entity_ID,Actual_Forecast_Indicator,Security_ID,Financials_Date,Full_Adjusted_Share_NAV,Enterprise_Value,Dividend_Yield,Net_Present_Value,Net_Property_Income,Vacancies_Overall,Portfolio_Leases_Expiring,Assumed_Ave_Escalation_Rate,Cap_Rate_Movemt_Assump,Reversion_Exp_Rent_Reviews,Ungeared_Yield,Net_Operating_Profit,Revenue_Growth,Net_Property_Income_Growth,Net_Operating_Profit_Growth,Interest_Paid_Growth,Loan_To_Value,Interest_Cover_Ratio,Revenue,Interest_Paid,Property_Expenses,Property_Expenses_Growth,Investment_Income,Admin_Expenses,Admin_Expenses_Growth,Taxation,Other_Income,Distributable_Earnings,Distributions_Growth,Linked_Units_Issued,Borrowings,Units_Issued,Purchases_And_Developments,Sales,Total_Assets,Current_Liabilities,Interest_Bearing_Borrowings,Last_Update_Date,Last_Update_User,Forecast_Distributions_Growth,Property_Portfolio_Value")] Party_Financials party_Financials)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                if (currentUser.EntityIdScope != party_Financials.Entity_ID)
                {
                    ModelState.AddModelError("Error", "An error occurred trying to edit. Party isnt in scope");
                }
                else
                {
                    party_Financials.Last_Update_Date = DateTime.Now;
                    party_Financials.Last_Update_User = User.Identity.Name;
                    db11.Update(party_Financials);
                    db11.Save();
                    TempData["ResultMessage"] = "Party Financials for party code \"" + party_Financials.Party_Code + "\" edited successfully!";
                    //TempData.Add("ResultMessage", "PartyDebtAnalysis for party code \"" + party_Debt_Analysis.Party_Code + "\" edited successfully!");
                    return RedirectToAction("Index");
                }
            }
            return View(party_Financials);
 
        }

        // GET: PartyFinancials/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Party_Financials party_Financials = db.Party_Financials.Find(id);
            if (party_Financials == null)
            {
                return HttpNotFound();
            }
            return View(party_Financials);
        }

        // POST: PartyFinancials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Party_Financials party_Financials = db.Party_Financials.Find(id);
            db.Party_Financials.Remove(party_Financials);
            db.SaveChanges();
            return RedirectToAction("Index");
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
