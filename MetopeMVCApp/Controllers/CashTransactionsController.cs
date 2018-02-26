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

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class CashTransactionsController : Controller
    {
        private readonly ICashTransactionsRepository db11;
        private MetopeDbEntities db = new MetopeDbEntities();
        
        public CashTransactionsController(ICashTransactionsRepository iDb)
        {
            db11 = iDb;
        } 
          
        // Retrieves the latest 50 records
        [CustomEntityAuthoriseFilter]

        public ActionResult Index(string PortfolioCode, DateTime? inputDate)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            ViewBag.Portfolio = PortfolioCode; 
                                    
            var vwm = db11.GetAll() 
                       .MatchCriteria(c => c.Entity_ID == EntityID)
                       .MatchCriteria(c => c.Portfolio_Code == PortfolioCode)
                     .OrderByDescending(r => r.Transaction_Date) 
                     .Include(d => d.Security_Detail)
                     .Include(d => d.Order_Detail).Take(50);

            if (inputDate != null)
            {
                DateTime dtEqualTo = Convert.ToDateTime(inputDate);
                vwm = vwm.Where(x => x.Transaction_Date <= dtEqualTo);

                if (vwm.Any()) // if records found then populate the to-date
                { 
                    ViewBag.LastRecordDate = vwm.Min(p => p.Transaction_Date).ToString("dd/MM/yyyy");
                }
                ViewBag.UserInputDate = dtEqualTo.ToString("dd/MM/yyyy");
            }
             
            //var cash_Transactions = db.Cash_Transactions.Include(c => c.Currency).Include(c => c.Entity).Include(c => c.Order_Allocation).Include(c => c.Order_Detail).Include(c => c.Portfolio).Include(c => c.Security_Detail).Include(c => c.Security_Detail1).Include(c => c.User).Include(c => c.User1);
            return View(vwm.ToList());
        }
 
        // GET: CashTransactions/Create
        [AllSecuritiesInclGenericFilter]
        [CurrencyFilter]
        [OrderAllocationFilter]
        [OrderDetailFilter]
        [UsersFilter]
        [PortfoliosFilter]
        public ActionResult Create(string PortfolioCode)
        {

            var EntityID = (decimal)ViewBag.EntityId;

            ViewBag.myPortfolioCode = PortfolioCode;
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name");
            var s = new Cash_Transactions
            {
                Entity_ID = EntityID,
                Value_Date = DateTime.Now,
                Transaction_Date = DateTime.Now
            }; 

            return View(s);
              
        }

        // POST: CashTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllSecuritiesInclGenericFilter]
        [CurrencyFilter]
        [OrderAllocationFilter]
        [OrderDetailFilter]
        [PortfoliosFilter]
        [UsersFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cash_Transaction_ID,Portfolio_Code,Entity_ID,Transaction_Security_ID,Cash_Security_ID,Transaction_Source_Code,Order_ID,Allocation_ID,Cashflow_ID,Dividend_Seq_Number,External_ID,Transaction_Date,Value_Date,Cash_Transaction_Type,Transaction_Amount,Transaction_Currency_Code,BaseCur_Amount,Create_User_Code,Create_Date,Last_Update_User,Last_Update_Date")] Cash_Transactions cash_Transactions)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            if (ModelState.IsValid)
            {  
                    cash_Transactions.Last_Update_User = User.Identity.Name;
                    cash_Transactions.Last_Update_Date = DateTime.Now;
                    db11.Add(cash_Transactions);
                    db11.Save();
                    TempData["ResultMessage"] = "New Cash Trans record for Portfolio " + cash_Transactions.Portfolio_Code + "\" created successfully!";
                    return RedirectToAction("Index", new { PortfolioCode = cash_Transactions.Portfolio_Code }); 
            }
            ViewBag.myPortfolioCode = cash_Transactions.Portfolio_Code;
            return View(cash_Transactions);
             
            //ViewBag.Transaction_Currency_Code = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", cash_Transactions.Transaction_Currency_Code);
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", cash_Transactions.Entity_ID);
            //ViewBag.Allocation_ID = new SelectList(db.Order_Allocation, "Allocation_ID", "Portfolio_Code", cash_Transactions.Allocation_ID);
            //ViewBag.Order_ID = new SelectList(db.Order_Detail, "Order_ID", "Transaction_Type", cash_Transactions.Order_ID);
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", cash_Transactions.Entity_ID);
            //ViewBag.Transaction_Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", cash_Transactions.Transaction_Security_ID);
            //ViewBag.Cash_Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", cash_Transactions.Cash_Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", cash_Transactions.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", cash_Transactions.Entity_ID);
            //return View(cash_Transactions);
        }

        // GET: CashTransactions/Edit/5
        [AllSecuritiesInclGenericFilter]
        [CurrencyFilter]
        [OrderAllocationFilter]
        [OrderDetailFilter]
        [UsersFilter]
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var EntityID = (decimal)ViewBag.EntityId;

            Cash_Transactions cashTran = db11.Get(id);     //.Include(s => s.Security_Detail)

            if (cashTran == null)
            {
                return HttpNotFound();
            }
                
            ViewBag.SecuritiesAll = cashTran.Cash_Security_ID;
            ViewBag.SecuritiesAll2 = cashTran.Transaction_Security_ID;
            ViewBag.TransactionCurrencyCode = cashTran.Transaction_Currency_Code;
            ViewBag.OrderID = cashTran.Order_ID;
            ViewBag.AllocationID = cashTran.Allocation_ID;
            ViewBag.CreateUserCode = cashTran.Create_User_Code;
      
            return View(cashTran);
        }

        // POST: CashTransactions/Edit/5 
        [HttpPost]
        [AllSecuritiesInclGenericFilter]
        [CurrencyFilter]
        [OrderAllocationFilter]
        [OrderDetailFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cash_Transaction_ID,Portfolio_Code,Entity_ID,Transaction_Security_ID,Cash_Security_ID,Transaction_Source_Code,Order_ID,Allocation_ID,Cashflow_ID,Dividend_Seq_Number,External_ID,Transaction_Date,Value_Date,Cash_Transaction_Type,Transaction_Amount,Transaction_Currency_Code,BaseCur_Amount,Create_User_Code,Create_Date,Last_Update_User,Last_Update_Date")] Cash_Transactions cash_Transactions)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            if (ModelState.IsValid)
            {
                db11.Update(cash_Transactions); //sets the modified status 
                cash_Transactions.Last_Update_Date = DateTime.Now;
                cash_Transactions.Last_Update_User = User.Identity.Name; 
                db11.Save();
                TempData["ResultMessage"] = "Security Performance for \"" + cash_Transactions.Portfolio_Code + "\" editied successfully!"; 

                return RedirectToAction("Index", new { PortfolioCode = cash_Transactions.Portfolio_Code });  
            }  

            return View(cash_Transactions);
        }

        // GET: CashTransactions/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cash_Transactions cash_Transactions  = db11.Get(id);
            if (cash_Transactions == null)
            {
                return HttpNotFound();
            }
            return View(cash_Transactions);
        }

        // POST: CashTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Cash_Transactions  cash_tran = db11.Get(id);

            var portfolioCd = cash_tran.Portfolio_Code;
            db11.Delete(cash_tran);
            db11.Save();
            TempData["ResultMessage"] = "Cash Transaction for portfolio \"" + portfolioCd + "\" Deleted successfully!";
            return RedirectToAction("Index", null, new { PortfolioCode = portfolioCd });  
        
        }

        public ActionResult CashTransactionsHistory(string PortfolioCode)
        {
            return View();
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
