using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc; 
using MetopeMVCApp.Data;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Filters;
using Metope.DAL;

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class PartyFinancialsController : Controller
    { 
        private readonly IPartyFinancialsRepository db11;
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public PartyFinancialsController(IPartyFinancialsRepository iDb)
        {
            db11 = iDb; 
        } 
        // GET: PartyFinancials   
        public ActionResult Index(string PartyCode = "" , string Nav = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;    

            var pfin = db11.GetAll()   
                    .MatchCriteria(c => ((PartyCode != "") ? c.Party_Code == PartyCode : c.Party_Code != ""))
                    .MatchCriteria(c => ( c.Entity_ID == EntityID )
                                   ) 
                    .OrderBy(r => r.Party_Code).ThenBy(n => n.Actual_Forecast_Indicator);

            if (PartyCode != "")
                ViewBag.PartyCode = PartyCode;

            ViewBag.Nav = Nav;

            return View(pfin.ToList()); 
        } 
          
        [PartyFilterIssr]
        [SecuritiesFilter]
        [ActualForecastIndicatoreFilter]
        public ActionResult Create(string Nav ="")
        { 
            var currentUser = manager.FindById(User.Identity.GetUserId());
            ViewBag.EntityIdScope = currentUser.EntityIdScope;

            ViewBag.Nav = Nav;

            return View();
        }

        // POST: PartyFinancials/Create  
        [HttpPost]
        [PartyFilterIssr]
        [SecuritiesFilter]
        [ActualForecastIndicatoreFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Party_Code,Entity_ID,Actual_Forecast_Indicator,Security_ID,Financials_Date,Full_Adjusted_Share_NAV,Enterprise_Value,Dividend_Yield,Net_Present_Value,Net_Property_Income,Vacancies_Overall,Portfolio_Leases_Expiring,Assumed_Ave_Escalation_Rate,Cap_Rate_Movemt_Assump,Reversion_Exp_Rent_Reviews,Ungeared_Yield,Net_Operating_Profit,Revenue_Growth,Net_Property_Income_Growth,Net_Operating_Profit_Growth,Interest_Paid_Growth,Loan_To_Value,Interest_Cover_Ratio,Revenue,Interest_Paid,Property_Expenses,Property_Expenses_Growth,Investment_Income,Admin_Expenses,Admin_Expenses_Growth,Taxation,Other_Income,Distributable_Earnings,Distributions_Growth,Linked_Units_Issued,Borrowings,Units_Issued,Purchases_And_Developments,Sales,Total_Assets,Current_Liabilities,Interest_Bearing_Borrowings,Last_Update_Date,Last_Update_User,Forecast_Distributions_Growth,Property_Portfolio_Value,OffshoreExposure_WesternEurope,OffshoreExposure_CEE,OffshoreExposure_US,OffshoreExposure_UK,OffshoreExposure_Australia,OffshoreExposure_Africa,OffshoreExposure_Other,Sector_Retail,Sector_Office,Sector_Industrial,Sector_Residential,Sector_Other,Sector_Offshore")] Party_Financials partyFinancials
                                            , string navIndicator = "")
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
                    if (navIndicator == "")
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", null, new { PartyCode = partyFinancials.Party_Code, Nav = navIndicator });

                } 
            }
              
            return View(partyFinancials);
        }
        public ActionResult HistoryUnderConstruction()
        {
            return View();
        }

        // GET: PartyFinancials/Edit/5
        [CustomEntityAuthoriseFilter]
        [PartyFilterIssr]
        [SecuritiesFilter]
        [ActualForecastIndicatoreFilter]
        public ActionResult Edit(decimal EntityId, string PartyCode, string ActualForecastInd, string Nav)
        {
            Party_Financials partyFins = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                r.Actual_Forecast_Indicator == ActualForecastInd).FirstOrDefault(); 
            if (partyFins == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nav = Nav;

            return View(partyFins); 
        }

        // POST: PartyFinancials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActualForecastIndicatoreFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Party_Code,Entity_ID,Actual_Forecast_Indicator,Security_ID,Financials_Date,Full_Adjusted_Share_NAV,Enterprise_Value,Dividend_Yield,Net_Present_Value,Net_Property_Income,Vacancies_Overall,Portfolio_Leases_Expiring,Assumed_Ave_Escalation_Rate,Cap_Rate_Movemt_Assump,Reversion_Exp_Rent_Reviews,Ungeared_Yield,Net_Operating_Profit,Revenue_Growth,Net_Property_Income_Growth,Net_Operating_Profit_Growth,Interest_Paid_Growth,Loan_To_Value,Interest_Cover_Ratio,Revenue,Interest_Paid,Property_Expenses,Property_Expenses_Growth,Investment_Income,Admin_Expenses,Admin_Expenses_Growth,Taxation,Other_Income,Distributable_Earnings,Distributions_Growth,Linked_Units_Issued,Borrowings,Units_Issued,Purchases_And_Developments,Sales,Total_Assets,Current_Liabilities,Interest_Bearing_Borrowings,Last_Update_Date,Last_Update_User,Forecast_Distributions_Growth,Property_Portfolio_Value,OffshoreExposure_WesternEurope,OffshoreExposure_CEE,OffshoreExposure_US,OffshoreExposure_UK,OffshoreExposure_Australia,OffshoreExposure_Africa,OffshoreExposure_Other,Sector_Retail,Sector_Office,Sector_Industrial,Sector_Residential,Sector_Other,Sector_Offshore")] Party_Financials party_Financials
                                    , string navIndicator = "")
        { 
            if (ModelState.IsValid)
            { 
                if (ViewBag.EntityId != party_Financials.Entity_ID)
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
                    if (navIndicator == "")
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", null, new { PartyCode = party_Financials.Party_Code, Nav = navIndicator });

                }
            } 
            return View(party_Financials); 
        }

        // GET: PartyFinancials/Delete/5
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(decimal EntityId, string PartyCode, string ActualForecastInd, string Nav)
        { 
            Party_Financials pf = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                   r.Actual_Forecast_Indicator == ActualForecastInd).FirstOrDefault();
            if (pf == null)
            {
                return HttpNotFound();
            }

            ViewBag.Nav = Nav;

            return View(pf); 
        } 
        // POST: PartyFinancials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(decimal EntityId, string PartyCode, string ActualForecastInd, string navIndicator = "")
        {   
            Party_Financials party_Financials = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                   r.Actual_Forecast_Indicator == ActualForecastInd).FirstOrDefault();
   
            db11.Delete(party_Financials);
            db11.Save();
            TempData.Add("ResultMessage", "Party Financial record for party code \"" + party_Financials.Party_Code + "\" deleted successfully!");

            if (navIndicator == "")
                return RedirectToAction("Index");
            else
                return RedirectToAction("Index", null, new { PartyCode = party_Financials.Party_Code, Nav = navIndicator });
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
