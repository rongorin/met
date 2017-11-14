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
    public class SecurityListController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();

        // GET: SecurityList
        public ActionResult Index()
        {
            var security_List = db.Security_List.Include(s => s.Security_Detail);
            return View(security_List.ToList());
        }

        // GET: SecurityList/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_List security_List = db.Security_List.Find(id);
            if (security_List == null)
            {
                return HttpNotFound();
            }
            return View(security_List);
        }

        // GET: SecurityList/Create
        public ActionResult Create()
        {
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name");
            return View();
        }

        // POST: SecurityList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Security_ID,Security_List_Code,Entity_ID")] Security_List security_List)
        {
            if (ModelState.IsValid)
            {
                db.Security_List.Add(security_List);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_List.Security_ID);
            return View(security_List);
        }

        // GET: SecurityList/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_List security_List = db.Security_List.Find(id);
            if (security_List == null)
            {
                return HttpNotFound();
            }
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_List.Security_ID);
            return View(security_List);
        }

        // POST: SecurityList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Security_ID,Security_List_Code,Entity_ID")] Security_List security_List)
        {
            if (ModelState.IsValid)
            {
                db.Entry(security_List).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_List.Security_ID);
            return View(security_List);
        }

        // GET: SecurityList/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_List security_List = db.Security_List.Find(id);
            if (security_List == null)
            {
                return HttpNotFound();
            }
            return View(security_List);
        }

        // POST: SecurityList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Security_List security_List = db.Security_List.Find(id);
            db.Security_List.Remove(security_List);
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
