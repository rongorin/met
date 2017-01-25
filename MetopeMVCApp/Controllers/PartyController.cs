using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Data;

namespace MetopeMVCApp.Controllers
{
    public class PartyController : Controller
    {
        private readonly IPartyRepository db11; 
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private MetopeDbEntities db = new MetopeDbEntities(); 
        public PartyController(IPartyRepository iDb)
        {
            db11 = iDb; 
        } 

        // GET: Party
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId()); 
             
            var parties = db.Parties.Include(p => p.Country).Include(p => p.Entity);
            return View(parties.ToList());
        }

        // GET: Party/Details/5
        public ActionResult Details(decimal EntityId, string PartyCode)
        {
         
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (PartyCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
            if (currentUser.EntityIdScope != EntityId)
            {
                throw new Exception("Not Acceptable");
                //return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!
            } 

            Party party = db.Parties.Find(PartyCode);
            if (party == null)
            {
                return HttpNotFound();
            }
            return View(party);
        }

        // GET: Party/Create
        public ActionResult Create()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId()); 

            ViewBag.Country_Code = new SelectList(db.Countries, "Country_Code", "Country_Name");
          
            ViewBag.entityId = currentUser.EntityIdScope;
            return View();
             
        }

        // POST: Party/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Entity_ID,Party_Code,Party_Name,Party_Type,Financial_Year_End,Country_Code,System_Locked,SWIFT_ID,BIC_Code")] Party party)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            party.Entity_ID = currentUser.EntityIdScope;

            /*------------------------------------------ 
            first check if this party is already used ! 
            ----------------------------------------  */ 
            Party check = db11.FindBy(r => r.Party_Code == party.Party_Code).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (check != null && ModelState.IsValid)
                {
                    ModelState.AddModelError("Name", "FAILED to create Party \"" + party.Party_Name + "\" code:\"" + party.Party_Code + "\". Already exists!");
                }
            }
             
            if (ModelState.IsValid)
            {
                db11.Add(party);
                db11.Save();
                return RedirectToAction("Index");
            }

            ViewBag.Country_Code = new SelectList(db.Countries, "Country_Code", "Country_Name", party.Country_Code); 
            return View(party);
        }

        // GET: Party/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Party party = db.Parties.Find(id);
            if (party == null)
            {
                return HttpNotFound();
            }
            ViewBag.Country_Code = new SelectList(db.Countries, "Country_Code", "Country_Name", party.Country_Code); 
            return View(party);
        }

        // POST: Party/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Entity_ID,Party_Code,Party_Name,Party_Type,Financial_Year_End,Country_Code,System_Locked,SWIFT_ID,BIC_Code")] Party party)
        {
            if (ModelState.IsValid)
            {
                db.Entry(party).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Country_Code = new SelectList(db.Countries, "Country_Code", "Country_Name", party.Country_Code);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", party.Entity_ID);
            return View(party);
        }

        // GET: Party/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Party party = db.Parties.Find(id);
            if (party == null)
            {
                return HttpNotFound();
            }
            return View(party);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Party party = db.Parties.Find(id);
            db.Parties.Remove(party);
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
