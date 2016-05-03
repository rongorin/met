using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Data;


namespace MetopeMVCApp.Controllers
{
    public class PortfolioController : Controller
    { 
        //private PortfolioRepository _repo = new PortfolioRepository( );
        private readonly IPortfolioRepository _repo; 

        private MetopeDbEntities db = new MetopeDbEntities(); //REMOVE this when done doing repository
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        // need parameterless constructor. This is poor mans constructor Dependency Injection (DI)/ 
        // Inversion of Control (IoC) like ninject or similar. http://stackoverflow.com/questions/12605445/mvc-no-parameterless-constructor-defined-for-this-object
            //public PortfolioController()
        //    : this(new PortfolioRepository())
        //{ 
        //}
        public PortfolioController() 
        {
            this._repo = new PortfolioRepository(new MetopeDbEntities());
        }
  
        public PortfolioController(IPortfolioRepository repo) {
            _repo = repo;
        }

        // GET: /Portfolio/
        public ActionResult Index()
        {   

            var currentUser = manager.FindById(User.Identity.GetUserId());
            var portfolios = _repo.GetPortfolios(currentUser.EntityIdScope); // db.Portfolios.Where(c => c.Entity_ID == currentUser.EntityIdScope).Include(p => p.Entity).Include(p => p.User);

            //var userId = User.Identity.GetUserId();
           //var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id; 


            manager.Dispose();
            return View(portfolios.ToList());
        }

        // GET: /Portfolio/Details/ 5,'abc'
        public ActionResult Details(decimal EntityId, string PortfolioCode)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
             
            if (EntityId == null || PortfolioCode == null) 
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
            if (currentUser.EntityIdScope != EntityId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!
            } 

            Portfolio portfolio = _repo.GetPortfolioById(EntityId, PortfolioCode);
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        // GET: /Portfolio/Create
        public ActionResult Create()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            ViewBag.Entity_ID  =  new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name");
            ViewBag.entityId = currentUser.EntityIdScope;
            return View();
        }

        // POST: /Portfolio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude="Entity_ID")] Portfolio portfolio)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            portfolio.Entity_ID = currentUser.EntityIdScope;

            if ( ModelState.IsValid)
            {  
                // first check if this PortfolioCode already used !
                Portfolio checkPortf = _repo.GetPortfolioById(portfolio.Entity_ID, portfolio.Portfolio_Code);
              
                if (checkPortf != null)
                {
                    TempData.Add("ResultMessage", "Failed to create portfolio \"" + portfolio.Portfolio_Name + "\" code:\"" + portfolio.Portfolio_Code + "\". Code already exists!"); 
                }
                else
                { 

                    _repo.CreatePortfolio(portfolio);
                    _repo.Save();
                    TempData.Add("ResultMessage", "new portfolio \"" + portfolio.Portfolio_Name + "\" code:\"" + portfolio.Portfolio_Code + "\" created successfully!");
                } 
                return RedirectToAction("Index");

            } 

            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID) 
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name");
             
         
            return View(portfolio);
        }

        // GET: /Portfolio/Edit/5
        public ActionResult Edit(decimal EntityId, string PortfolioCode)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());


            if (EntityId == null || PortfolioCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = _repo.GetPortfolioById(EntityId, PortfolioCode);
             
            if (portfolio == null)
            {
                return HttpNotFound();
            }

            if (currentUser.EntityIdScope != EntityId)
            { 
                 return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!
            }

    
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name", portfolio.Manager);

            return View(portfolio);
        }

        // POST: /Portfolio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Edit([Bind(Include="Entity_ID,Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End,Custodian_Code,Active_Flag,System_Locked")] 
                                    Portfolio portfolio)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid )
            {
                if (currentUser.EntityIdScope != portfolio.Entity_ID)
                {
                      ModelState.AddModelError("Error", "An error occurred trying to edit. Portfolio isnt in scope");
                    //return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!
                }
                else
                {  
                    _repo.UpdatePortfolio(portfolio); // this is  EntityState.Modified;
                    _repo.Save();
                    TempData.Add("ResultMessage", "Portfolio \"" + portfolio.Portfolio_Name + "\" editied successfully!");
                    return RedirectToAction("Index");
               
                }
            } 
            ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name", portfolio.Manager);
            return View(portfolio);
        }

        // GET: /Portfolio/Delete/5
        public ActionResult Delete(decimal EntityId, string PortfolioCode)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (EntityId == null || PortfolioCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Portfolio portfolio = _repo.GetPortfolioById(EntityId, PortfolioCode);  
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        }

        // POST: /Portfolio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal EntityId, string PortfolioCode)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
             
            if (currentUser.EntityIdScope != EntityId) 
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!

            Portfolio portfolio = _repo.GetPortfolioById(EntityId, PortfolioCode); //db.Portfolios.Find(EntityId, PortfolioCode);
         
            _repo.DeletePortfolio(EntityId, PortfolioCode);
            _repo.Save();
            return RedirectToAction("Index");
        }
        //For DropDowns populating
        public IQueryable<User> LoadManagers(decimal iEntityId)
        { 
            //return db.Users.Where(r => r.Entity_ID == iEntityId);
            return _repo.GetUsers(iEntityId);  

            //return from t in db.Users
            //       where t.Entity_ID == 1
            //       select t;
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
