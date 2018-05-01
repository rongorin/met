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
    public class ClassificationsController : Controller
    { 
        private readonly IClassificationRepository db11;
        private readonly IClassificationIndustryRepository db2;

        public ClassificationsController(IClassificationRepository idb, IClassificationIndustryRepository idb2)
        {
            db11 = idb;
            db2 = idb2; //we need this because in case of Delete, check the Industries that are referenced .
        } 
        // GET: Classifications
        public ActionResult Index()
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            var classifications = db11.GetAll().Include(c => c.Classification_Industry)
                    .MatchCriteriaEnum(c => ( c.Entity_ID == EntityID) );
               
            return View(classifications );
        }
         

        // GET: Classifications/Create
        public ActionResult Create()
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
             
            var secPerf = new Classification
            {
                Entity_ID = EntityID
            };

            return View(secPerf);
       
        }

        // POST: Classifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Classification_Code,Description,Entity_ID,Last_Update_Date,Last_Update_User,System_Locked")] Classification classification)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            classification.Entity_ID = EntityID;
            if (ModelState.IsValid)
            {
                if (Classification_Exists(classification.Entity_ID, classification.Classification_Code))
                {
                    ModelState.AddModelError("Name", "FAILED to create Classification" + classification.Classification_Code + ". Record already exists!");

                }
                else
                {
                    classification.Last_Update_User = User.Identity.Name;
                    classification.Last_Update_Date = DateTime.Now;
                    db11.Add(classification);
                    db11.Save();
                    TempData["ResultMessage"] = "New Classification " + classification.Classification_Code + "\" created successfully!";
                    return RedirectToAction("Index" );
                }
            }
            ViewBag.myClassificationCode = classification.Classification_Code;
            return View(classification);
              
        }

        // GET: Classifications/Edit 
        [CustomEntityAuthoriseFilter]
        public ActionResult Edit(string ClassificationCode, decimal EntityId)
        { 
            Classification cc = db11.FindBy(r => r.Classification_Code == ClassificationCode)
                                           .MatchCriteria(c => c.Entity_ID == EntityId).FirstOrDefault();

            if (cc == null)
            {
                return HttpNotFound();

            }  
            return View(cc);
        }

        // POST: Classifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit ([Bind(Include = "Classification_Code,Description,Entity_ID,Last_Update_Date,Last_Update_User,System_Locked")] Classification classification)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(classification); //sets the modified status 
                classification.Last_Update_Date = DateTime.Now;
                classification.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Classification code \"" + classification.Classification_Code + "\" editied successfully!";

                return RedirectToAction("Edit", new { ClassificationCode = classification.Classification_Code, EntityId = classification.Entity_ID });
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Classification");
            ViewBag.EntityIdScope = EntityID;   
            return View(classification);
        } 
        // GET: Classifications/Delete/5
        public ActionResult Delete(string ClassificationCode )
        {
            var EntityID = (decimal)ViewBag.EntityId;
            Classification classification = db11.FindBy(r => r.Classification_Code == ClassificationCode &&
                                                r.Entity_ID == EntityID  
                                                   ).FirstOrDefault();

            if (classification == null)
            {
                return HttpNotFound();
            }

            return View(classification);

        } 

        // POST: Classifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public ActionResult DeleteConfirmed(string ClassificationCode)
        {
            var EntityID = (decimal)ViewBag.EntityId;
            Classification classification = db11.FindBy(r => r.Classification_Code == ClassificationCode &&
                                                r.Entity_ID == EntityID  
                                            ).FirstOrDefault();

            if (Industry_Exists(classification.Entity_ID, classification.Classification_Code))
                TempData["ResultMessage"] = "Delete FAILED!!  " + "You must first delete the related Industry codes before deleting the Classification " + classification.Classification_Code;
            else
            {
                db11.Delete(classification);
                db11.Save();
                TempData["ResultMessage"] = "Classification Code" + classification.Classification_Code + " deleted successfully!";
                 
            }
         
            return RedirectToAction("Index");
        } 

        private bool Classification_Exists(decimal entityID, string classifCode )
        {
            Classification checkExist = db11.FindBy(e => e.Entity_ID == entityID &&
                              e.Classification_Code == classifCode).FirstOrDefault();
            if (checkExist != null)

                return true;
            else
                return false;
        }

        private bool Industry_Exists(decimal entityID, string ClassificationCd )
        {
            Classification_Industry checkExist = db2.FindBy(e => e.Entity_ID == entityID &&
                              e.Classification_Code == ClassificationCd  ).FirstOrDefault() ;
  
            if (checkExist != null)

                return true;
            else
                return false;
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
