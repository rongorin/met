using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using System.Configuration; 
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework; 
using MetopeMVCApp.Data; 
using MetopeMVCApp.Filters;
using MetopeMVCApp.Data.GenericRepository;
using Metope.DAL;
namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class PortfolioController : Controller
    { 
        //private PortfolioRepository _repo = new PortfolioRepository( );

        private MetopeDbEntities db = new MetopeDbEntities(); //REMOVE this when done doing repository
        private UserManager<ApplicationUser> manager = 
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        //using  proper DI : only one constructor 

        private readonly IPortfolioRepository3 _repo;

        public PortfolioController(IPortfolioRepository3 repo)
        {
            _repo = repo;
        }

        // need parameterless constructor. This is poor mans constructor Dependency Injection (DI)/ 
        // so this just passes in an implementation of the IPortfolioRep interface (which is the parm required by the constructor)
                // The MVC runtime can't instantiate your controller as it can't provide an implementation of IAccommodationService. You either need to do poor man's constructor injection like this:
                //  public FeaturedAccommodationController()
                //     : this(new AccommodationService())
                //  {
                //  }
                //  public FeaturedAccommodationController(IAccommodationService accommodationService)
                //  { 
                //  } 
        // Inversion of Control (IoC) like ninject or similar. http://stackoverflow.com/questions/12605445/mvc-no-parameterless-constructor-defined-for-this-object
        //public PortfolioController()
        //    : this(new PortfolioRepository(new MetopeDbEntities())) 
        //{
        //    GetUserId = () => User.Identity.GetUserId();
        //}
          
        //public PortfolioController() 
        //{
        //    this._repo = new PortfolioRepository(new MetopeDbEntities());
        //}

  
        public Func<string> GetUserId; //For testing 
         
        public ActionResult Index(int page = 1, string searchTerm = null)
        { 
            decimal EntityID = (decimal)ViewBag.EntityId;
            var portfolios = _repo.GetPortfolios(EntityID, page, searchTerm);
           
            // db.Portfolios.Where(c => c.Entity_ID == currentUser.EntityIdScope).Include(p => p.Entity).Include(p => p.User);


            ////return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId).Include(p => p.Entity).Include(p => p.User);
            //// var security_detail = db.Security_Detail.Include(s => s.Country).Include(s => s.Country1).Include(s => s.Currency).Include(s => s.Currency1).Include(s => s.Currency2).Include(s => s.Currency3) ;

            //var userId = User.Identity.GetUserId();
           //var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == userId).First().Id; 

            if(Request.IsAjaxRequest())
            {
                return PartialView("_Portfolios", portfolios);
            }
            manager.Dispose();
            return View(portfolios);
        }

        // GET: /Portfolio/Details/ 5,'abc'
        [CustomEntityAuthoriseFilter] 
        public ActionResult Details(decimal EntityId, string PortfolioCode)
        {  
            Portfolio portfolio = null;
            try
            {
                portfolio = _repo.GetPortfolioById(EntityId, PortfolioCode); 
            }
            catch { 
            }
            if (portfolio == null)
            {
                return HttpNotFound();
            }
            return View(portfolio);
        } 
        // GET: /Portfolio/Create 
       
        [LogAttribuite] 
        public ActionResult Create()
        {  
            var currentUser = manager.FindById(User.Identity.GetUserId());
   
            ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name");
            ViewBag.Portfolio_Base_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Portfolio_Report_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.PortfolIo_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name");
            ViewBag.Portfolio_Types = new SelectList(GetCodeMiscellVals("PORTTYP"), "MisCode", "MisCode_Description");
            ViewBag.PortfolioStatus = new SelectList(GetCodeMiscellVals("PFSTATUS"), "MisCode", "MisCode_Description");
            ViewBag.Custodians = new SelectList(GetPartyValues(currentUser.EntityIdScope), "Party_Code", "Party_Name");

            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
            ViewBag.MyActiveFlagList = new SelectList(selectListItems, "Value", "Text" );
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text" );

            var portfolio = new Portfolio
            {
                Inception_Date =  DateTime.Now ,
                Entity_ID =  currentUser.EntityIdScope    
            };
             
            return View(portfolio);
        }

        // POST: /Portfolio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Create([Bind(Include = "Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End, Portfolio_Status ,Custodian_Code,Active_Flag,System_Locked")] Portfolio portfolio)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId()); 
            portfolio.Entity_ID = currentUser.EntityIdScope;

            //var errors = ModelState.Values.SelectMany(v => v.Errors);  
            /*----------------------------------------------
            first check if this PortfolioCode already used ! 
            ---------------------------------------------- */
            Portfolio checkPortf = _repo.GetPortfolioById(portfolio.Entity_ID, portfolio.Portfolio_Code); 
            if (ModelState.IsValid) {
                if (checkPortf != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Portfolio \"" + portfolio.Portfolio_Name + "\" code:\"" + portfolio.Portfolio_Code + "\". Code already exists!"); 
                }
            }

            if ( ModelState.IsValid)
            {
                _repo.CreatePortfolio(portfolio);
                _repo.Save ();
                TempData.Add("ResultMessage", "new portfolio \"" + portfolio.Portfolio_Name + "\" code:\"" + portfolio.Portfolio_Code + "\" created successfully!");
           
                return RedirectToAction("Index"); 
            }

            //ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name");
            //ViewBag.Portfolio_Base_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            //ViewBag.Portfolio_Report_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            //ViewBag.PortfolIo_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name");
            //ViewBag.Portfolio_Types = new SelectList(GetCodeMiscellVals("PORTTYP"), "MisCode", "MisCode_Description");
            //ViewBag.PortfolioStatus = new SelectList(GetCodeMiscellVals("PFSTATUS"), "MisCode", "MisCode_Description");
            //ViewBag.Custodians = new SelectList(GetPartyValues(currentUser.EntityIdScope), "Party_Code", "Party_Name");
            //var selectListItems = new List<SelectListItem>();
            //selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            //selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
            //ViewBag.MyActiveFlagList = new SelectList(selectListItems, "Value", "Text");
            //ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text");
              
            return View(portfolio);
        }

        // GET: /Portfolio/Edit/5
        [CustomEntityAuthoriseFilter]
        public ActionResult Edit(decimal EntityId, string PortfolioCode)
        { 
            Portfolio portfolio = _repo.GetPortfolioById(EntityId, PortfolioCode);

            if (portfolio == null)
            { 
                return HttpNotFound();
            }  
            ViewBag.PortfolioBaseCurrency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Base_Currency);
            ViewBag.Portfolio_Report_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Report_Currency);
            ViewBag.PortfolIo_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", portfolio.PortfolIo_Domicile);
            ViewBag.Portfolio_Types = new SelectList(GetCodeMiscellVals("PORTTYP"), "MisCode", "MisCode_Description", portfolio.Portfolio_Type);
            ViewBag.PortfolioStatus = new SelectList(GetCodeMiscellVals("PFSTATUS"), "MisCode", "MisCode_Description", portfolio.Portfolio_Status);
            ViewBag.Custodians = new SelectList(GetPartyValues(EntityId), "Party_Code", "Party_Name", portfolio.Custodian_Code);
             
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString }); 
            ViewBag.MyActiveFlagList = new SelectList(selectListItems, "Value", "Text", portfolio.Active_Flag);
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", portfolio.System_Locked);

            ViewBag.managers = new SelectList(LoadManagers(EntityId), "User_Code", "User_Name", portfolio.Manager);

            return View(portfolio);
        }

        // POST: /Portfolio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Edit([Bind(Include="Entity_ID,Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End, Portfolio_Status ,Custodian_Code,Active_Flag,System_Locked")] 
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

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.PortfolioBaseCurrency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Base_Currency);
            ViewBag.Portfolio_Report_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Report_Currency);
            ViewBag.PortfolIo_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", portfolio.PortfolIo_Domicile);
            ViewBag.Portfolio_Types = new SelectList(GetCodeMiscellVals("PORTTYP"), "MisCode", "MisCode_Description", portfolio.Portfolio_Type);
            ViewBag.PortfolioStatus = new SelectList(GetCodeMiscellVals("PFSTATUS"), "MisCode", "MisCode_Description", portfolio.Portfolio_Status);
            ViewBag.Custodians = new SelectList(GetPartyValues(currentUser.EntityIdScope), "Party_Code", "Party_Name", portfolio.Custodian_Code);
             
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString }); 
            ViewBag.MyActiveFlagList = new SelectList(selectListItems, "Value", "Text", portfolio.Active_Flag);
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", portfolio.System_Locked);

             ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name", portfolio.Manager);
            return View(portfolio);
        }

        // GET: /Portfolio/Delete/5
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(decimal EntityId, string PortfolioCode)
        { 
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
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(decimal EntityId, string PortfolioCode)
        {  
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
 
        }
        public IQueryable<Code_Miscellaneous> GetCodeMiscellVals(string iSettings)
        {
            //return db.Users.Where(r => r.Entity_ID == iEntityId);
            return _repo.GetCodeMiscVals(iSettings);

        }
         public IQueryable<Party> GetPartyValues(decimal iEntityId)
        {
            decimal refGenericEntity = Convert.ToDecimal(ConfigurationManager.AppSettings["GenericEntityId"]); 
            //return db.Users.Where(r => r.Entity_ID == iEntityId);
            return _repo.GetPartyValues(iEntityId, "CUSTODIAN", refGenericEntity);

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
