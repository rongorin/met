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
using MetopeMVCApp.Filters;
using Microsoft.AspNet.Identity.EntityFramework;

using Metope.DAL;
using MetopeMVCApp.Models;
using Newtonsoft.Json;
namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityClassificationIndustryController : Controller
    {
        private readonly ISecurityClassificationIndustryRepository db11;
        private readonly IClassificationIndustryRepository dbIndustry;

        public SecurityClassificationIndustryController(ISecurityClassificationIndustryRepository iDb, IClassificationIndustryRepository iDb2)
        {
            db11 = iDb;
            dbIndustry = iDb2;
        }
        // GET: SecurityClassificationIndustry
        public ActionResult Index(decimal? SecurityId, string Ticker="")
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            ViewBag.SecurityID = SecurityId;
            ViewBag.ticker = Ticker;

            var vm = db11.GetAllRecs(x => x.Entity_ID == EntityID
                                    && x.Security_ID == SecurityId);

            Security_Classification_Industry firsRecord = vm.FirstOrDefault(); 
            ViewBag.mySecurity  = firsRecord != null ? firsRecord.Security_Detail.Short_Name : null;

            return View(vm);
        }

        // GET: SecurityClassificationIndustry/Create
        [SecuritiesForInScopeFilter]
        [ClassificationsFilter]
         [IndustryFilter] 
        public ActionResult Create(decimal SecurityId)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            ViewBag.SecuritiesAllScope = SecurityId;

            var sci = new Security_Classification_Industry
           {
               Entity_ID = EntityID, 
               Security_ID = (decimal)SecurityId, 
               Effective_Date = DateTime.Now
           };
            //ViewBag.myClassificationCode = "";
            return View(sci); 

        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClassificationsFilter]
          [IndustryFilter]
        public ActionResult Create([Bind(Include = "Classification_Code,Effective_Date,Entity_ID,Industry_Code,Last_Update_Date,Last_Update_User,Security_ID")] Security_Classification_Industry security_Classification_Industry)
 
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            security_Classification_Industry.Entity_ID = EntityID;
            /*------------------------------------------ 
             first check if already used ! 
             ------------------------------------------*/
            Security_Classification_Industry check = db11.FindBy(r => r.Classification_Code == security_Classification_Industry.Classification_Code 
                                        &&  r.Entity_ID == security_Classification_Industry.Entity_ID
                                        && r.Security_ID == security_Classification_Industry.Security_ID 
                                        && r.Industry_Code == security_Classification_Industry.Industry_Code 
                                        && r.Effective_Date == security_Classification_Industry.Effective_Date).FirstOrDefault<Security_Classification_Industry>();
            if (ModelState.IsValid)
            {
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create a Security Classification Industry for " + security_Classification_Industry.Security_ID + " as it Already exists!");
                }
                else
                {
                    security_Classification_Industry.Last_Update_Date = DateTime.Now;
                    security_Classification_Industry.Last_Update_User = User.Identity.Name;

                    db11.Add(security_Classification_Industry);
                    db11.Save();
                    TempData["ResultMessage"] = "Security Classification Industry successfully added for Security " 
                                                + security_Classification_Industry.Security_ID.ToString() + " for Classification "
                                                + security_Classification_Industry.Classification_Code+ " !";
                     
                    return RedirectToAction("Index", new { SecurityId = security_Classification_Industry.Security_ID });
                    //return RedirectToAction("Index", "Portfolio");
                }
            }
            ViewBag.myClassificationCode = security_Classification_Industry.Classification_Code;
            ViewBag.myIndustryCode = security_Classification_Industry.Industry_Code;

            return View(security_Classification_Industry);
        }

        // GET: SecurityClassificationIndustry/Delete/5 
        [CustomEntityAuthoriseFilter]
        [ClassificationsFilter]
        [IndustryFilter]
        public ActionResult Edit(decimal SecurityId, string IndustryCode, string ClassificationCode, DateTime EffectiveDate, string Nav = "")
        {
            var EntityId = (decimal)ViewBag.EntityId;

            Security_Classification_Industry Security_Classification_Industry = db11.FindBy(r => r.Classification_Code == ClassificationCode
                                   && r.Entity_ID == EntityId
                                   && r.Security_ID == SecurityId
                                   && r.Industry_Code == IndustryCode
                                   && r.Effective_Date == EffectiveDate).FirstOrDefault();

            if (Security_Classification_Industry == null)
            {
                return HttpNotFound();
            }
            ViewBag.myClassificationCode = Security_Classification_Industry.Classification_Code;
            ViewBag.myIndustryCode = Security_Classification_Industry.Industry_Code;
            
            return View(Security_Classification_Industry);
        }

        // POST: SecurityClassificationIndustry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClassificationsFilter]
        [IndustryFilter]
        public ActionResult Edit([Bind(Include = "Classification_Code,Effective_Date,Entity_ID,Industry_Code,Last_Update_Date,Last_Update_User,Security_ID")] Security_Classification_Industry security_Classification_Industry)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(security_Classification_Industry); //sets the modified status 
                security_Classification_Industry.Last_Update_Date = DateTime.Now;
                security_Classification_Industry.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Security Classification Industry for " + security_Classification_Industry.Security_ID + " edited successfully!";

                return RedirectToAction("Index", new { SecurityId = security_Classification_Industry.Security_ID, });

            }

            ViewBag.myClassificationCode = security_Classification_Industry.Classification_Code;
            ViewBag.myIndustryCode = security_Classification_Industry.Industry_Code;

            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", Security_Classification_Industry.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", Security_Classification_Industry.Security_ID);
            //ViewBag.Discount_Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", Security_Classification_Industry.Discount_Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", Security_Classification_Industry.Entity_ID);
            return View(security_Classification_Industry);
        }
   
        public ActionResult Delete(decimal SecurityId, string IndustryCode, string ClassificationCode, DateTime EffectiveDate, string Nav = "")
        {
            var EntityId = (decimal)ViewBag.EntityId;

            Security_Classification_Industry Security_Classification_Industry = db11.FindBy(r => r.Classification_Code == ClassificationCode
                                   && r.Entity_ID == EntityId
                                   && r.Security_ID == SecurityId
                                   && r.Industry_Code == IndustryCode
                                   && r.Effective_Date == EffectiveDate).FirstOrDefault(); 
             
            if (Security_Classification_Industry == null)
            {
                return HttpNotFound();
            }

            return View(Security_Classification_Industry);
        }
        
        // POST: SecurityClassificationIndustry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]  
        [CustomEntityAuthoriseFilter] 
        public ActionResult DeleteConfirmed( decimal SecurityId, string IndustryCode, string ClassificationCode, DateTime EffectiveDate, string Nav = "")
        {
            var EntityID = (decimal)ViewBag.EntityId;
 
            Security_Classification_Industry Security_Classification_Industry = db11.FindBy(r => r.Classification_Code == ClassificationCode
                            && r.Entity_ID == EntityID
                            && r.Security_ID == SecurityId
                            && r.Industry_Code == IndustryCode
                            && r.Effective_Date == EffectiveDate).FirstOrDefault();

            db11.Delete(Security_Classification_Industry);
            db11.Save();
            TempData["ResultMessage"] = "Classification Industry for Security " + Security_Classification_Industry.Security_ID + " deleted successfully!";

            return RedirectToAction("Index", new { securityId = Security_Classification_Industry.Security_ID});
        }
        /// <summary>
        /// called by Ajax call onchange from ddl    
        /// </summary>  
        [HttpPost]
        [IndustryJSONFilter] 
        public JsonResult LoadIndustryClassifications(string classificationCd)
        {
            string role = classificationCd ;
            //ViewBag.selectClassificationCode = classificationCd; 
            //HttpContext.Items["ClassificationCd"] = classificationCd;
             
            return Json(ViewBag.Industry_Code);
             
            //if (!Request.IsAjaxRequest())
            //    return RedirectToAction("Index", new { securityid = xxxx.securityid });

            //return Json(new
            //{
            //    CurrentPrice = bid.Amount.ToString("C"),
            //    BidCount = auction.BidCount
            //}); 
        }
        private List<ClassificationIndustryDDLViewModel> GetIndustries(decimal entityID, string ClassificationCd = "")
        {

            var industries = dbIndustry.GetAll((c => c.Entity_ID == entityID &&
                    (ClassificationCd != "") ? c.Classification_Code == ClassificationCd : c.Classification_Code != ""))
                    .OrderBy(r => r.Description).ToList().
                   Select(r => new ClassificationIndustryDDLViewModel
                    {
                        Industry_Code = r.Industry_Code,
                        Description = r.Description
                    }); 
            //List<Classification_Industry> res = (List<Classification_Industry>)industries.ToList(); 
            return industries.ToList() ;

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
