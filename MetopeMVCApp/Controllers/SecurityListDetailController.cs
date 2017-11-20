using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Data;

namespace MetopeMVCApp.Controllers
{

    [SetAllowedEntityIdAttribute]
    public class SecurityListDetailController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly ISecurityListDetailRepository  db11;

        public SecurityListDetailController(ISecurityListDetailRepository iDb)
        {
            db11 = iDb; 
        }
        // GET: SecurityListDetail
        public ActionResult Index()
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            var viewModel = new SecurityListDetailIndexVM();

            var security_List_Detailxx = db.Security_List_Detail.Include(s => s.Entity);
             
            var sld = db11.GetAll()
                      .MatchCriteria(c => c.Entity_ID == EntityID)

              .Select(g => new SecurityListDetailIndexVM
                     {
                         Security_List_Code = g.Security_List_Code, 
                         Entity_ID = g.Entity_ID,
                         Description = g.Description,
                         Security_List_Name = g.Security_List_Name ,
                         SecurityList = g.Select(c => new SecurityListVM {
                             Security_List_Code = c.Security_List_Code,
                             Security_ID = c.Security_ID

                         //SecurityList = db.Security_List.Select(g => new DatabaseGroupViewModel
                         // {
                         //   Group = g,
                         //   Selected =  g.Databases .Any(db => db.Id == databaseId)
                         // }).ToList();
        
                          
                      }) 
                     .OrderBy(s => s.Security_List_Code);
                     


            //viewModel.SecurityListDetail = db11.GetAll().AsNoTracking()
            //           .MatchCriteria(c => c.Entity_ID == EntityID);

            //viewModel.SecurityListDetail = db.Security_List_Detail.Find(s => s.Entity_ID); 

            return View(sld.ToList() );
        }

        // GET: SecurityListDetail/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_List_Detail security_List_Detail = db.Security_List_Detail.Find(id);
            if (security_List_Detail == null)
            {
                return HttpNotFound();
            }
            return View(security_List_Detail);
        }

        // GET: SecurityListDetail/Create
        public ActionResult Create()
        {
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            return View();
        }

        // POST: SecurityListDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Entity_ID,Security_List_Code,Security_List_Name,Description,System_Locked")] Security_List_Detail security_List_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Security_List_Detail.Add(security_List_Detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_List_Detail.Entity_ID);
            return View(security_List_Detail);
        }

        // GET: SecurityListDetail/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_List_Detail security_List_Detail = db.Security_List_Detail.Find(id);
            if (security_List_Detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_List_Detail.Entity_ID);
            return View(security_List_Detail);
        }

        // POST: SecurityListDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Entity_ID,Security_List_Code,Security_List_Name,Description,System_Locked")] Security_List_Detail security_List_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(security_List_Detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_List_Detail.Entity_ID);
            return View(security_List_Detail);
        }

        // GET: SecurityListDetail/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_List_Detail security_List_Detail = db.Security_List_Detail.Find(id);
            if (security_List_Detail == null)
            {
                return HttpNotFound();
            }
            return View(security_List_Detail);
        }

        // POST: SecurityListDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Security_List_Detail security_List_Detail = db.Security_List_Detail.Find(id);
            db.Security_List_Detail.Remove(security_List_Detail);
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
