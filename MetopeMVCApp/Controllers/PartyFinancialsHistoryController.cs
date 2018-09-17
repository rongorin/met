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
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Filters;
using System.Data.Entity.Core.Objects;

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class PartyFinancialsHistoryController : Controller
    { 
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly IPartyFinancialsHistoryRepository  db11;
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public PartyFinancialsHistoryController(IPartyFinancialsHistoryRepository db)
        {   
            db11 = db;
        }
         
        [CustomEntityAuthoriseFilter]
        public ActionResult Index(decimal EntityId, string PartyCode, string ActualForecastInd)
        {  
            var finHist = db11.GetAll()
                    .MatchCriteria(c => c.Entity_ID == EntityId)
                    .MatchCriteria(c => c.Party_Code == PartyCode)
                    .MatchCriteria(c => c.Actual_Forecast_Indicator == ActualForecastInd)
                 .OrderByDescending(n => n.Record_Date) ;
                  
            ViewBag.PartyCode = PartyCode;
            ViewBag.ActualForecastInd = ActualForecastInd;

            return View(finHist.ToList());  
        } 

        // GET: 
        [CustomEntityAuthoriseFilter]
        [PartyFilterIssr]
        [SecuritiesFilter]
        [ActualForecastIndicatoreFilter]
        [Route("PartyFinancialsHistory/EntityId/PartyCode/ActualForecastInd/RecordDate:datetime")]
        public ActionResult Edit(decimal EntityId, string PartyCode, string ActualForecastInd, DateTime RecordDate)
        {    
            Party_Financials_History partyFins = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                r.Actual_Forecast_Indicator == ActualForecastInd &&  r.Record_Date == RecordDate).FirstOrDefault();
                                //r.Actual_Forecast_Indicator == ActualForecastInd && DbFunctions.AddMilliseconds(r.Record_Date, -1*(r.Record_Date.Millisecond)) == RecordDate).FirstOrDefault();
            if (partyFins == null)
            {
                return HttpNotFound();
            }
            return View(partyFins);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Party_Code,Entity_ID,Actual_Forecast_Indicator,Security_ID,Financials_Date,Full_Adjusted_Share_NAV,Enterprise_Value,Dividend_Yield,Net_Present_Value,Net_Property_Income,Vacancies_Overall,Portfolio_Leases_Expiring,Assumed_Ave_Escalation_Rate,Cap_Rate_Movemt_Assump,Reversion_Exp_Rent_Reviews,Ungeared_Yield,Net_Operating_Profit,Revenue_Growth,Net_Property_Income_Growth,Net_Operating_Profit_Growth,Interest_Paid_Growth,Loan_To_Value,Interest_Cover_Ratio,Revenue,Interest_Paid,Property_Expenses,Property_Expenses_Growth,Investment_Income,Admin_Expenses,Admin_Expenses_Growth,Taxation,Other_Income,Distributable_Earnings,Distributions_Growth,Linked_Units_Issued,Borrowings,Units_Issued,Purchases_And_Developments,Sales,Total_Assets,Current_Liabilities,Interest_Bearing_Borrowings,Last_Update_Date,Last_Update_User,Forecast_Distributions_Growth,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User,Property_Portfolio_Value")] Party_Financials_History party_Financials_History)
        {
            //ViewBag.EntityIdScope = party_Financials_History.Entity_ID;
            if (ModelState.IsValid)
            { 
                party_Financials_History.Last_Update_Date = DateTime.Now;
                party_Financials_History.Last_Update_User = User.Identity.Name;
                db11.Update(party_Financials_History);
                db11.Save();
                TempData["ResultMessage"] = "Party Financial Historical record for party code \"" + party_Financials_History.Party_Code + "\" edited successfully!";
                //TempData.Add("ResultMessage", "PartyDebtAnalysis for party code \"" + party_Debt_Analysis.Party_Code + "\" edited successfully!"); 
                return RedirectToAction("Index", "PartyFinancialsHistory", new { EntityId = party_Financials_History.Entity_ID, PartyCode = party_Financials_History.Party_Code  , ActualForecastInd = party_Financials_History.Actual_Forecast_Indicator });
            }
            return View(party_Financials_History); 
        }

        // Get  
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(decimal EntityId, string PartyCode, string ActualForecastInd, DateTime RecordDate)
        {  
            Party_Financials_History partyFins = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                r.Actual_Forecast_Indicator == ActualForecastInd &&  r.Record_Date == RecordDate).FirstOrDefault();
             
            if (partyFins == null)
            {
                return HttpNotFound();
            }

            return View(partyFins);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(decimal EntityId, string PartyCode, string ActualForecastInd, DateTime RecordDate)
        {  

            Party_Financials_History partyFins = db11.FindBy(r => r.Entity_ID == EntityId && r.Party_Code == PartyCode &&
                                r.Actual_Forecast_Indicator == ActualForecastInd && r.Record_Date == RecordDate).FirstOrDefault();

            db11.Delete(partyFins);
            db11.Save();
            TempData.Add("ResultMessage", "Party Financial Historical record for party code \"" + partyFins.Party_Code + "\" deleted successfully!");

            return RedirectToAction("Index", "PartyFinancialsHistory", new { EntityId = partyFins.Entity_ID, PartyCode = partyFins.Party_Code, ActualForecastInd = partyFins.Actual_Forecast_Indicator });
             
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
