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

namespace MetopeMVCApp.Controllers
{
    public class SecurityPriceController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly ISecurityPriceRepository db11;
        private readonly ISecurityDetailRepository db22; 
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        

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
        public ActionResult Index(int? SecurityId, int? iEntityId, string iPriceCurr ="")
        {

            //var security_Price = db.Security_Price.Include(s => s.Currency)
            //                        .Include(s => s.Entity).Include(s => s.Security_Detail).Include(s => s.User);
            if (SecurityId != null) 
                 ViewBag.SecurityID = SecurityId.Value;

            var viewModel = new SecurityPriceIndexViewModel(); 
            viewModel.SecurityDetails = db.Security_Detail
                .Where(c => (SecurityId != null) ? c.Security_ID >= SecurityId : c.Security_ID > 0) 
                     .ToList().Take(3);    
            //viewModel.SecurityDetails = db22.GetAll().Take(10); 

             viewModel.SecurityPrices = db11.GetAll()
                                        .Include(s => s.Security_Detail)
                                        //.Include(s => s.Security_Detail.Security_Price_History) 
                                        .OrderBy(s => s.Security_ID);

             if (SecurityId != null && iPriceCurr != "")
             {
           
                ViewBag.PriceCurr = iPriceCurr ;
                   
                viewModel.SecurityPriceHistory = db.Security_Price_History
                    .Where(c => c.Security_ID == SecurityId && c.Price_Curr == iPriceCurr)
                    .Include(c => c.Currency)
                    .ToList();  
               
                //var security_Price = db.Security_Price.Include(s => s.Currency)
                //                        .Include(s => s.Entity).Include(s => s.Security_Detail).Include(s => s.User);


                //viewModel.Courses = viewModel.Instructors.Where(
                //    i => i.ID == id.Value).Single().Courses;

            }
            return View(viewModel);
        }

        // GET: SecurityPrice/Details/5
        public ActionResult Details(decimal SecurityId)
        {
            if (SecurityId == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
 
            Security_Price security_Price = db11.GetAll()
                     .Where(s => s.Security_ID == SecurityId)
                     .FirstOrDefault<Security_Price>();   

            //Security_Price security_Price = db.Security_Price.Find(SecurityId, SecurityId);
            if (security_Price == null)
            {
                return HttpNotFound();
            }
            return View(security_Price);
        }

        // GET: SecurityPrice/Create
        public ActionResult Create()
        {
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name");
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name");
            return View();
        }

        // POST: SecurityPrice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Entity_ID,Security_ID,Price_Curr,All_In_Price,Clean_Price,Accrued_Income_Price,Price_Source,Yield_To_Maturity,Discount_Rate,Last_Update_User,Last_Update_Date,Issued_Amount,Free_Float_Issued_Amount,Record_Date")] Security_Price security_Price)
        {
            if (ModelState.IsValid)
            {
                db.Security_Price.Add(security_Price);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_Price.Price_Curr);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Price.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Price.Security_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Price.Entity_ID);
            return View(security_Price);
        }

        // GET: SecurityPrice/Edit/5
        public ActionResult Edit(decimal EntityId, decimal SecurityId, string PriceCurr)
        {
            if (SecurityId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_Price security_Price = db.Security_Price.Find(EntityId, SecurityId, PriceCurr);

            if (security_Price == null)
            {
                return HttpNotFound();
            }

            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_Price.Price_Curr);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Price.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Price.Security_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_security_detail.Last_Update_User = User.Identity.Name; Name", security_Price.Entity_ID);
            return View(security_Price);
        }

        // POST: SecurityPrice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Entity_ID,Security_ID,Price_Curr,All_In_Price,Clean_Price,Accrued_Income_Price,Price_Source,Yield_To_Maturity,Discount_Rate,Last_Update_User,Last_Update_Date,Issued_Amount,Free_Float_Issued_Amount,Record_Date")] Security_Price security_Price)
        {
            if (ModelState.IsValid)
            {
                db.Entry(security_Price).State = EntityState.Modified;

                security_Price.Last_Update_Date = DateTime.Now;
                security_Price.Last_Update_User = User.Identity.Name;
                db.Database.Log = l => LogInfo(l);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_Price.Price_Curr);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Price.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Price.Security_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Price.Entity_ID);
            return View(security_Price);
        }

        // GET: SecurityPrice/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_Price security_Price = db.Security_Price.Find(id);
            if (security_Price == null)
            {
                return HttpNotFound();
            }
            return View(security_Price);
        }

        // POST: SecurityPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Security_Price security_Price = db.Security_Price.Find(id);
            db.Security_Price.Remove(security_Price);
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
