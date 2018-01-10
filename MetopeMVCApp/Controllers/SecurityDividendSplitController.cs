using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Metope.DAL;
using MetopeMVCApp.Services;
using System.Text;
using MetopeMVCApp.Filters;
///
///                                                     this is Client
/// 
namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityDividendSplitController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();

        private void LogInfo(string path , string logmessage)
        {
            System.IO.File.AppendAllText(path, logmessage);
        } 

        // GET: this is Client
        public ActionResult Index()
        {
            ClientApiService<Security_Dividend_Split> svc = new ClientApiService<Security_Dividend_Split>();
            IEnumerable<Security_Dividend_Split> allSplits = Enumerable.Empty<Security_Dividend_Split>();

            string FilePath = HttpContext.Server.MapPath("~/Data/Repositories/LoggerRepository/LoggerFile.txt");
            decimal EntityID = (decimal)ViewBag.EntityId;
            try
            {
                LogInfo(FilePath, "RC started. \r\n");
                allSplits = svc.findAll<Security_Dividend_Split>("SecurityDividendSplit", EntityID, FilePath);
          

                if (allSplits == null)
                    ModelState.AddModelError(string.Empty, "Server error occurred reading the data.");

                return View(allSplits);

            }
            catch (AggregateException ex) //occurs on failure in a api client fail
            {
                LogInfo(FilePath, "\r\n RC Exception caught." + ex.Message + "\n");

                var sb = new StringBuilder();
                sb.AppendLine("  An Error Occurred:");

                foreach (var exception in ex.InnerExceptions)
                { 
                    sb.AppendLine(exception.Message.ToString());
                    ModelState.AddModelError(string.Empty, sb.ToString()); 

                }
                LogInfo(FilePath, "\r\n  RC Success done.");
                return View(allSplits);

            }

        }

        // GET: http://localhost:53133/SecurityDividendSplit/Details?entityID=2&securityID=57&dividendAnnNumber=1
        // 
        public ActionResult Details(decimal entityID, decimal securityID, decimal dividendAnnNumber)
        {  
            if (entityID == null || securityID == null || dividendAnnNumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ClientApiService<Security_Dividend_Split> svc = new ClientApiService<Security_Dividend_Split>();
            Security_Dividend_Split split = new Security_Dividend_Split();
            try
            {
                split = svc.find<Security_Dividend_Split>("SecurityDividendSplit", entityID, securityID, dividendAnnNumber);

                if (split == null)
                    ModelState.AddModelError(string.Empty, "Server error occurred reading the data.");

                //if (security_Dividend_Split == null)
                //{
                //    return HttpNotFound();
                //}

                return View(split); 

            }
            catch (AggregateException ex) //occurs on failure in a api client fail
            {
                var sb = new StringBuilder();
                sb.AppendLine("  An Error Occurred:");

                foreach (var exception in ex.InnerExceptions)
                {
                    sb.AppendLine(exception.Message.ToString());
                    ModelState.AddModelError(string.Empty, sb.ToString());

                }
                return View(split);

            }

 
            //Security_Dividend_Split security_Dividend_Split = db.Security_Dividend_Split.Find(id);
            //if (security_Dividend_Split == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(security_Dividend_Split);
        }

        // GET: SecurityDividendSplit/Create
        public ActionResult Create()
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Short_Name");
            var secDivSplit = new Security_Dividend_Split
            {
                Entity_ID = EntityID 
            };

            return View(secDivSplit);
        }

        // POST: SecurityDividendSplit/Create       this is CLIent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Dividend_Annual_Number,Dividend_Split,Entity_ID,Security_ID")] Security_Dividend_Split security_Dividend_Split)
        {
            ClientApiService<Security_Dividend_Split> svc = new ClientApiService<Security_Dividend_Split>();
            security_Dividend_Split.Entity_ID = ViewBag.EntityId;
            try
            {
                if (ModelState.IsValid)
                {
                    //security_Dividend_Split.Last_Update_Date = DateTime.Now;
                    //security_Div idend_Split.Last_Update_User = "METOPEADMIN";

                    bool success = svc.create(security_Dividend_Split, "SecurityDividendSplit");

                    if (success == true)
                        return RedirectToAction("Index"); 

                }
            }
            catch (AggregateException ex) //occurs on failure in a api client fail
            {
                var sb = new StringBuilder();
                sb.AppendLine("  An Error Occurred:");

                foreach (var exception in ex.InnerExceptions)
                {
                    sb.AppendLine(exception.Message.ToString());
                    ModelState.AddModelError(string.Empty, sb.ToString());

                } 

            }
            ModelState.AddModelError(string.Empty, "Server error. Please check inputted values.");
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Dividend_Split.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Short_Name", security_Dividend_Split.Security_ID);
 
            return View(security_Dividend_Split);
        }

        // GET: SecurityDividendSplit/Edit/5
        public ActionResult Edit(decimal entityID, decimal securityID, decimal dividendAnnNumber)
        { 
            if (entityID == null || securityID == null || dividendAnnNumber == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ClientApiService<Security_Dividend_Split> svc = new ClientApiService<Security_Dividend_Split>();
            Security_Dividend_Split split = new Security_Dividend_Split();
       
            split = svc.find<Security_Dividend_Split>("SecurityDividendSplit", entityID, securityID, dividendAnnNumber);
 
            if (split == null)
            {
                ModelState.AddModelError(string.Empty, "Server error occurred reading the data.");
                return HttpNotFound();
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", split.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Short_Name", split.Security_ID);
            return View(split);
        }

        // POST: SecurityDividendSplit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dividend_Annual_Number,Dividend_Split,Entity_ID,Security_ID")] Security_Dividend_Split security_Dividend_Split)
        {
            ClientApiService<Security_Dividend_Split> svc = new ClientApiService<Security_Dividend_Split>();
            try
            {
             if (ModelState.IsValid)
                {
                    //security_Dividend_Split.Last_Update_Date = DateTime.Now;
                    //security_Div idend_Split.Last_Update_User = "METOPEADMIN";

                    bool success = svc.edit(security_Dividend_Split, "SecurityDividendSplit");

                    if (success == true)
                    {
                        TempData.Add("ResultMessage", "Split record for Security Id \"" + security_Dividend_Split.Security_ID + "\" deleted successfully!");
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "Server error. Please check inputted values.");
                    return View(security_Dividend_Split);

                }
            }

            catch (AggregateException ex) //occurs on failure in a api client fail
            {
                var sb = new StringBuilder();
                sb.AppendLine("  An Error Occurred:");

                foreach (var exception in ex.InnerExceptions)
                {
                    sb.AppendLine(exception.Message.ToString());
                    ModelState.AddModelError(string.Empty, sb.ToString());

                }
                return View(security_Dividend_Split); 
            }

            //if (ModelState.IsValid)
            //{
            //    db.Entry(security_Dividend_Split).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            ModelState.AddModelError(string.Empty, "Server error attempting to save. Please check input values.");
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Dividend_Split.Entity_ID);
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Short_Name", security_Dividend_Split.Security_ID);
            return View(security_Dividend_Split);
        }

        //[HttpGet, Route("GetByID/{entityID},{securityID},{dividendAnnNumber}")]
        public ActionResult Delete(decimal entityID, decimal securityID, decimal dividendAnnNumber)
        {
            if (entityID == null || securityID == null || dividendAnnNumber == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 

            ClientApiService<Security_Dividend_Split> svc = new ClientApiService<Security_Dividend_Split>();
            Security_Dividend_Split split = new Security_Dividend_Split();

            split = svc.find<Security_Dividend_Split>("SecurityDividendSplit", entityID, securityID, dividendAnnNumber);
             
            if (split == null)
            {
                ModelState.AddModelError(string.Empty, "Server error occurred reading the data.");
                return HttpNotFound();
            }

            return View(split);
        }

        // POST: SecurityDividendSplit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal entityID, decimal securityID, decimal dividendAnnNumber)
        {
            ClientApiService<Security_Dividend_Split> svc = new ClientApiService<Security_Dividend_Split>();

            bool success = svc.delete("SecurityDividendSplit", entityID, securityID, dividendAnnNumber);
            if (success == true)
                TempData.Add("ResultMessage", "Split record for Security Id \"" + securityID + "\" deleted successfully!"); 
            else
                TempData.Add("ResultMessage", "Failure occurred attempting to delete Split record for Security Id \"" + securityID  );

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
