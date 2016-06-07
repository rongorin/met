using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;

namespace MetopeMVCApp.Controllers
{
     [Authorize(Roles = "Admin")]
    public class EntityController : Controller
    {

        private MetopeDbEntities db = new MetopeDbEntities();

        // GET: /Entity/
        public ActionResult Index()
        {

            return View(db.Entities.ToList());
        }

        // GET: /Entity/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Entity entity = db.Entities.Find(id);  // or :   db.Entities.Single(r => r.Entity_ID == id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // GET: /Entity/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Entity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Entity_ID,Entity_Code,Entity_Name,Import_Folder,Export_Folder")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                db.Entities.Add(entity);
                db.SaveChanges();
                TempData.Add("ResultMessage", "new Entity \"" + entity.Entity_Name + "\" created successfully!");

                return RedirectToAction("Index");
            }

            return View(entity);
        }

        // GET: /Entity/Edit/5

        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.Entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: /Entity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Entity_ID,Entity_Code,Entity_Name,Import_Folder,Export_Folder")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                TempData.Add("ResultMessage", "Entity \"" + entity.Entity_Name + "\" edited successfully!");

                return RedirectToAction("Index");
            }
            return View(entity);
        }

        // GET: /Entity/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entity entity = db.Entities.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        // POST: /Entity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Entity entity = db.Entities.Find(id);
            db.Entities.Remove(entity);
            db.SaveChanges();
            TempData.Add("ResultMessage", "Entity \"" + entity.Entity_Name + "\" Deleted successfully!");

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
