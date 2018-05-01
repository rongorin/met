using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Filters;
using Metope.DAL;
namespace MetopeMVCApp.Controllers
{

    [SetAllowedEntityIdAttribute]
    public class SecurityPriceController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly ISecurityPriceRepository db11; 
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            };
        public SecurityPriceController(ISecurityPriceRepository iDb   )
        {
            db11 = iDb; 
        } 

        //this is an example of logging database functionaily:
        private void LogInfo(string logmessage)
        {
            string FilePath = HttpContext.Server.MapPath("~/Data/Repositories/LoggerRepository/LoggerFile.txt");
            System.IO.File.AppendAllText(FilePath, logmessage);
        }

        // GET: SecurityPrice  
        public ActionResult Index(int? numberOfRows, int? SecurityId, int? EntityId, string iPriceCurr = "")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            decimal genericId = (decimal)ViewBag.genericEntity;
             
            var viewModel = new SecurityPriceIndexViewModel();

            if (numberOfRows == null)
                numberOfRows = 20; 
            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows); 

            if (SecurityId != null)
            {
                ViewBag.SecurityID = SecurityId.Value;
                viewModel.SecurityDetails = db.Security_Detail 
                    .Where(c => c.Security_ID == SecurityId).FirstOrDefault<Security_Detail>() ; 
            }  

            viewModel.SecurityPrices = db11.GetAll()
                                      .SearchPrices(SecurityId, EntityID, genericId)
                                      .Include(s => s.Currency)
                                      .Include(s => s.Security_Detail)
                                      .OrderBy(s => s.Security_Detail.Ticker )
                                      .ToList();

            //force auto-load the History if only one Price
            if (SecurityId != null && iPriceCurr == "" && viewModel.SecurityPrices.Count() == 1) 
                iPriceCurr = viewModel.SecurityPrices.First().Price_Curr;
  
            ViewBag.PriceCurr = iPriceCurr;  

            //Show history:
            //-----------------
            if (SecurityId != null && iPriceCurr != "")
            { 
                viewModel.SecurityPriceHistory = db.Security_Price_History
                    .Where(c => c.Security_ID == SecurityId && c.Price_Curr == iPriceCurr && EntityID == c.Entity_ID)
                    .Include(c => c.Currency)
                    .ToList(); 

            }
            return View(viewModel);
        }
 
        // GET: SecurityPrice/Create
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create(int? SecurityId)
        { 
            var EntityID = (decimal)ViewBag.EntityId;
            ViewBag.SecuritiesAll = SecurityId;

            ViewBag.Price_Curr = new SelectList(db.Currencies.OrderBy(r => r.Currency_Name) ,     "Currency_Code", "Currency_Name"); 

            //ViewBag.SecurityID = SecurityId;
            //ViewBag.Security_ID = new SelectList(db.Security_Detail
            //                                    .Where(c => c.Entity_ID == EntityID)
            //                                    .OrderBy(r => r.Security_Name),    "Security_ID", "Security_Name", SecurityId); 
            return View();
        }

        // POST: SecurityPrice/Create  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllSecuritiesInclGenericFilter]
        public ActionResult Create([Bind(Include = "Security_ID,Price_Curr,All_In_Price,Clean_Price,Accrued_Income_Price,Price_Source,Yield_To_Maturity,Discount_Rate,Last_Update_User,Last_Update_Date,Issued_Amount,Free_Float_Issued_Amount,Record_Date")] Security_Price security_Price)
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 

            security_Price.Entity_ID = EntityID; 
            /*------------------------------------------ 
            first check if this party is already used ! 
            ----------------------------------------*/ 
            if (ModelState.IsValid)
            {
                Security_Price check = db11.FindBy(r => r.Security_ID == security_Price.Security_ID && r.Price_Curr == security_Price.Price_Curr && r.Entity_ID == security_Price.Entity_ID)
                           .FirstOrDefault();
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Security-Price for Security \"" + security_Price.Security_ID + "\" Currency:\"" + security_Price.Price_Curr + "\". Already exists!");
                }
            }

            if (ModelState.IsValid)
            { 
                security_Price.Last_Update_Date = DateTime.Now;
                security_Price.Last_Update_User = User.Identity.Name; 
                db11.Add(security_Price);
                db11.Save();
                TempData.Add("ResultMessage", "new Security-Price for \"" + security_Price.Security_ID + "\" created successfully!");
  
                return RedirectToAction("Index");
            }

            ViewBag.Price_Curr = new SelectList(db.Currencies.OrderBy(r => r.Currency_Name), "Currency_Code", "Currency_Name", security_Price.Price_Curr);
            ViewBag.Security_ID = new SelectList(db.Security_Detail
                                                .Where(c => c.Entity_ID == EntityID)
                                                .OrderBy(r => r.Security_Name),    "Security_ID", "Security_Name", security_Price.Security_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Price.Entity_ID);
            return View(security_Price);
        }

        // GET: SecurityPrice/Edit/5
        [HttpGet] 
        [CustomEntityAuthoriseFilter]
        public ActionResult Edit(decimal EntityId, decimal SecurityId, string PriceCurr)
        {  
            Security_Price secPrice = db11.FindBy(r => r.Entity_ID == EntityId && r.Security_ID == SecurityId &&
                                            r.Price_Curr == PriceCurr).Include(r => r.Security_Detail).FirstOrDefault();

            SecurityPriceEditModel model; 
             model = new SecurityPriceEditModel
            {
                Entity_ID = secPrice.Entity_ID,
                Security_ID = secPrice.Security_ID,
                Price_Curr = secPrice.Price_Curr,
                All_In_Price = secPrice.All_In_Price,
                Clean_Price = secPrice.Clean_Price,
                Accrued_Income_Price = secPrice.Accrued_Income_Price,
                Price_Source = secPrice.Price_Source,
                Yield_To_Maturity = secPrice.Yield_To_Maturity,
                Discount_Rate = secPrice.Discount_Rate, 
                Issued_Amount = secPrice.Issued_Amount,
                Free_Float_Issued_Amount = secPrice.Free_Float_Issued_Amount,
                Record_Date = secPrice.Record_Date,
                Security_Name = secPrice.Security_Detail.Short_Name
             }; 
         
            //var secPrice = db11.FindBy(r => r.Entity_ID == EntityId && r.Security_ID == SecurityId &&
            //                        r.Price_Curr == PriceCurr).Include(r => r.Security_Detail)
            //                        .Select (g => new SecurityPriceEditModel
            //                        { 
            //                            Security_Name = g.Security_Detail.Security_Name,

            //                            SecurityPrice = g.SecurityP

            //                        })
            //                        .FirstOrDefault (); 
            if (secPrice == null)
            {
                return HttpNotFound();
            }
            return View(model); 
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public ActionResult Edit([Bind( Include = "Entity_ID,Security_ID,Price_Curr,All_In_Price,Clean_Price,Accrued_Income_Price,Price_Source,Yield_To_Maturity,Discount_Rate,Last_Update_User,Last_Update_Date,Issued_Amount,Free_Float_Issued_Amount,Record_Date")] 
                                        SecurityPriceEditModel secPrice)
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 

            Security_Price security_Price = db11.FindBy(r => r.Entity_ID == EntityID && 
                                                             r.Security_ID == secPrice.Security_ID &&
                                                             r.Price_Curr == secPrice.Price_Curr).Include(r => r.Security_Detail).FirstOrDefault();

            if (ModelState.IsValid)
            {
                security_Price.Last_Update_Date = DateTime.Now;
                security_Price.Last_Update_User = User.Identity.Name;
                security_Price.Price_Curr = secPrice.Price_Curr;
                security_Price.All_In_Price = secPrice.All_In_Price;
                security_Price.Clean_Price = secPrice.Clean_Price;
                security_Price.Accrued_Income_Price = secPrice.Accrued_Income_Price;
                security_Price.Price_Source = secPrice.Price_Source;
                security_Price.Yield_To_Maturity = secPrice.Yield_To_Maturity;
                security_Price.Discount_Rate = secPrice.Discount_Rate;
                 
                security_Price.Issued_Amount = secPrice.Issued_Amount;
                security_Price.Free_Float_Issued_Amount = secPrice.Free_Float_Issued_Amount;
                security_Price.Record_Date = secPrice.Record_Date; 
                //db11.Database.Log = l => LogInfo(l);

                db11.Update(security_Price);
                db11.Save();
                TempData.Add("ResultMessage", "Security \"" + security_Price.Security_ID + "\" for Price Currency " + security_Price.Price_Curr + "\" edited successfully!");
  
                return RedirectToAction("Index", "SecurityPrice", new {/* routeValues */ SecurityId = security_Price.Security_ID, iPriceCurr = security_Price.Price_Curr });

            } 
            //re-populate model:
            secPrice.Security_Name = security_Price.Security_Detail.Security_Name; 
 
            return View(secPrice);
        }

        // GET: SecurityPrice/Delete/5 
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(decimal EntityId, decimal SecurityId, string PriceCurr)
        {  
            Security_Price security_Price = db.Security_Price.Find(EntityId, SecurityId, PriceCurr);
            if (security_Price == null)
            {
                return HttpNotFound();
            }
            return View(security_Price);
        }

        // POST: SecurityPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(decimal EntityId, decimal SecurityId, string PriceCurr)
        { 
            Security_Price security_Price = db.Security_Price.Find(EntityId, SecurityId, PriceCurr);
            db.Security_Price.Remove(security_Price);
            db.SaveChanges();
            TempData.Add("ResultMessage", "Security Price record for security \"" + security_Price.Security_ID + "\" Deleted successfully!");

            return RedirectToAction("Index");
        }

        // GET: Security_Price_History/Edit/5 
        [CustomEntityAuthoriseFilter]
        public ActionResult EditHistory(decimal EntityId, decimal SecurityId, string PriceCurr, DateTime PriceDateTime)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Security_Price_History security_Price_History = db.Security_Price_History.
                                                             Where(r => r.Entity_ID == EntityId && 
                                                                 r.Security_ID == SecurityId &&
                                                                 r.Price_Curr == PriceCurr && 
                                                                 r.Price_DateTime == PriceDateTime)
                                                            .Include(r => r.Security_Detail)
                                                            .FirstOrDefault();  

            if (security_Price_History == null)
            {
                return HttpNotFound();
            }
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "Currency_Name", security_Price_History.Price_Curr);
            ViewBag.Security_ID = new SelectList(db.Security_Detail
                                                    .Where(c => c.Entity_ID == EntityID)
                                                    .OrderBy(r => r.Security_Name),
                                                  "Security_ID", "Security_Name", security_Price_History.Security_ID);
             
            return View(security_Price_History);
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHistory([Bind(Include = "Entity_ID,Security_ID,Price_Curr,All_In_Price,Clean_Price,Accrued_Income_Price,Price_Source,Yield_To_Maturity,Discount_Rate,Last_Update_User,Last_Update_Date,Price_DateTime,Record_Date,Session_ID,Hist_Last_Update_Date,Hist_Last_Update_User,Issued_Amount,Free_Float_Issued_Amount")] Security_Price_History security_Price_History)
        {

            if (ModelState.IsValid)
            {
                //db.Entry(security_Price_History).State = EntityState.Modified;
                db.Security_Price_History.Attach(security_Price_History);
                db.Entry(security_Price_History).Property(r => r.All_In_Price).IsModified = true;
                db.Entry(security_Price_History).Property(r => r.Clean_Price).IsModified = true;
                db.Entry(security_Price_History).Property(r => r.Discount_Rate).IsModified = true;
                db.Entry(security_Price_History).Property(r => r.Accrued_Income_Price).IsModified = true;
                db.Entry(security_Price_History).Property(r => r.Price_Source).IsModified = true;
                db.Entry(security_Price_History).Property(r => r.Yield_To_Maturity).IsModified = true;
                db.Entry(security_Price_History).Property(r => r.Issued_Amount).IsModified = true;
                db.Entry(security_Price_History).Property(r => r.Free_Float_Issued_Amount).IsModified = true;   
                db.SaveChanges();
                TempData.Add("ResultMessage", "Security Price History record for security \"" + security_Price_History.Security_ID + "\" Edited successfully!");

                return RedirectToAction("Index", "SecurityPrice", new { SecurityId = security_Price_History.Security_ID, iPriceCurr = security_Price_History.Price_Curr});
            }
            var EntityID = (decimal)ViewBag.EntityId;

            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_Price_History.Price_Curr);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Price_History.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail
                                                    .Where(c => c.Entity_ID == EntityID)
                                                    .OrderBy(r => r.Security_Name), 
                                                   "Security_ID", "Security_Name", security_Price_History.Security_ID);
       
            return View( security_Price_History);
        }

        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteHistory(decimal EntityId, decimal SecurityId, string PriceCurr, DateTime PriceDateTime)
        { 
            Security_Price_History security_Price_History = db.Security_Price_History.Find(EntityId, SecurityId, PriceCurr, PriceDateTime);
            if (security_Price_History == null)
            {
                return HttpNotFound();
            }
            return View(security_Price_History);
        }

        // POST: SecurityPrice/Delete/5
        [HttpPost, ActionName("DeleteHistory")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteHistoryConfirmed(decimal EntityId, decimal SecurityId, string PriceCurr, DateTime PriceDateTime)
        {
            Security_Price_History security_Price_History = db.Security_Price_History.Find(EntityId, SecurityId, PriceCurr, PriceDateTime);
            db.Security_Price_History.Remove(security_Price_History);
            db.SaveChanges();
            TempData.Add("ResultMessage", "Security-Price History record  \"" + security_Price_History.Security_ID + " Currency" + PriceCurr + "\" Deleted successfully!");

            return RedirectToAction("Index", "SecurityPrice", new { SecurityId = security_Price_History.Security_ID, iPriceCurr = security_Price_History.Price_Curr });
             
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                db11.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
