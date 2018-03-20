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
    public class ClassificationIndustryController : Controller
    {
         
        private readonly IClassificationIndustryRepository db11;

        public ClassificationIndustryController(IClassificationIndustryRepository idb)
        {
            db11 = idb; 
        }  

        // GET: ClassificationIndustry
        //public ActionResult Index()
        //{
        //    var classification_Industry = db.Classification_Industry.Include(c => c.Classification).Include(c => c.Entity).Include(c => c.User);
        //    return View(classification_Industry.ToList());
        //}

        
        [ClassificationsFilter]
       public ActionResult Create(string ClassificationCode)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            ViewBag.myClassificationCode = ClassificationCode;
            var indust = new Classification_Industry
            {
                Entity_ID = EntityID
            };

            return View(indust);

            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            //ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name");
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name");
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name");

        }
        
        // POST: ClassificationIndustry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClassificationsFilter]
        public ActionResult Create([Bind(Include = "Classification_Code,Description,Industry_Code,Entity_ID,Last_Update_Date,Last_Update_User,System_Locked")] Classification_Industry classification_Industry)
        {
             
            decimal EntityID = (decimal)ViewBag.EntityId;
            classification_Industry.Entity_ID = EntityID;
            if (ModelState.IsValid)
            {
                if (Industry_Exists(classification_Industry.Entity_ID,
                                 classification_Industry.Classification_Code,
                                 classification_Industry.Industry_Code))
                {
                    ModelState.AddModelError("Name", "FAILED to create Industry code" + classification_Industry.Industry_Code + ". Record already exist for this Classification/ Industry.");

                }
                else
                {
                    classification_Industry.Last_Update_User = User.Identity.Name;
                    classification_Industry.Last_Update_Date = DateTime.Now;
                    db11.Add(classification_Industry);
                    db11.Save();
                    TempData["ResultMessage"] = "New Industry code " + classification_Industry.Industry_Code + "\" created successfully!";
                    return RedirectToAction("Edit", "Classifications", new { ClassificationCode = classification_Industry.Classification_Code, EntityId = classification_Industry.Entity_ID });
            
                }
            }
            ViewBag.myClassificationCode = classification_Industry.Classification_Code;
            return View(classification_Industry);

        }
        // GET: Classifications/Edit/5
        [ClassificationsFilter]
        public ActionResult Edit(string ClassificationCode, string IndustryCode)
        {

            var EntityID = (decimal)ViewBag.EntityId;

            Classification_Industry classification_Industry = db11.FindBy(r => r.Classification_Code == ClassificationCode)
                                           .MatchCriteria(c => c.Industry_Code == IndustryCode)
                                           .MatchCriteria(c => c.Entity_ID == EntityID).FirstOrDefault();

            if (classification_Industry == null)
            {
                return HttpNotFound();

            }
            ViewBag.myClassificationCode = classification_Industry.Classification_Code;
           //  ViewBag.myClassificationCode = ClassificationCode;

            return View(classification_Industry);
        }
        // GET: ClassificationIndustry/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Classification_Industry classification_Industry = db.Classification_Industry.Find(id);
        //    if (classification_Industry == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Classification_Code = new SelectList(db.Classifications, "Classification_Code", "Description", classification_Industry.Classification_Code);
        //    ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", classification_Industry.Entity_ID);
        //    ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", classification_Industry.Entity_ID);
        //    return View(classification_Industry);
        //}

        // POST: ClassificationIndustry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClassificationsFilter]
        public ActionResult Edit([Bind(Include = "Classification_Code,Description,Industry_Code,Entity_ID,Last_Update_Date,Last_Update_User,System_Locked")] Classification_Industry classification_Industry)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            if (ModelState.IsValid)
            {
                db11.Update(classification_Industry); //sets the modified status 
                classification_Industry.Last_Update_Date = DateTime.Now;
                classification_Industry.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Classification Industry for Industry cd \"" + classification_Industry.Industry_Code + "\" editied successfully!";

                return RedirectToAction("Edit", "Classifications", new { ClassificationCode = classification_Industry.Classification_Code, EntityId = classification_Industry.Entity_ID });
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Classification Industry ");
            ViewBag.EntityIdScope = EntityID;  
             
            //ViewBag.Classification_Code = new SelectList(db.Classifications, "Classification_Code", "Description", classification_Industry.Classification_Code);
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", classification_Industry.Entity_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", classification_Industry.Entity_ID);
            return View(classification_Industry);
        }

        // GET: ClassificationIndustry/Delete 
        public ActionResult Delete(string ClassificationCode, string IndustryCode)
        {
            if (ClassificationCode == null || IndustryCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var EntityID = (decimal)ViewBag.EntityId;

            Classification_Industry classification_Industry = db11.FindBy(r => r.Classification_Code == ClassificationCode &&
                                                r.Entity_ID == EntityID &&
                                                r.Industry_Code == IndustryCode
                                            ).FirstOrDefault();

            if (classification_Industry == null)
            {
                return HttpNotFound();
            }

            return View(classification_Industry); 
              
        }

        // POST: ClassificationIndustry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string ClassificationCode, string IndustryCode)  
        {  
            var EntityID = (decimal)ViewBag.EntityId;
            Classification_Industry classification_Industry = db11.FindBy(r => r.Classification_Code == ClassificationCode &&
                                                r.Entity_ID == EntityID &&
                                                r.Industry_Code == IndustryCode
                                            ).FirstOrDefault();

            db11.Delete(classification_Industry);
            db11.Save();
            TempData["ResultMessage"] = "Classification Industry code \"" + classification_Industry.Industry_Code + "\" deleted successfully!";

            return RedirectToAction("Edit", "Classifications", new { ClassificationCode = classification_Industry.Classification_Code, EntityId = classification_Industry.Entity_ID });

        }
        private bool Industry_Exists(decimal entityID, string ClassificationCd, string IndustryCd)
        {
            Classification_Industry checkExist = db11.FindBy(e => e.Entity_ID == entityID &&
                              e.Classification_Code == ClassificationCd &&
                              e.Industry_Code == IndustryCd).FirstOrDefault();
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
