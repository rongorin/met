using MetopeMVCApp.Controllers;
using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Data.Repositories;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace MetopeMVCApp.Services
{
            /*  -------------------------------------------------------------
             * This service is to populate the various dropdowns and lookups
             ------------------------------------------------------------*/ 
    public interface IServices
    {
        IQueryable<Country> ListCountry();
        IQueryable<Currency> ListCurrencies();
        IQueryable<Currency_Pair> ListCurrencyPairs();
        IQueryable<Security_Type> ListSecTypeCode();

        IQueryable<Exchange> ListExchanges();
        IQueryable<Code_Miscellaneous> ListMiscellanousTypes(string iCodeType);
        IQueryable<Portfolio> ListPortfolios(decimal iUser );

        IQueryable<Party> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId);
        //IEnumerable<Country> List();
        void Dispose();
         
    }

    public class Services : IDisposable, IServices   
    { 

        MetopeDbEntities _context;
        private bool disposed = false;

        public Services(bool createContxt)
        {
            if (createContxt)
                 _context = new MetopeDbEntities();
        }

        public IQueryable<Country> ListCountry()
        {
            ICountryRepository dbCntx = new CountryRepository();
              
            return dbCntx.GetAll().OrderBy(s => s.Country_Name);

            //return  _context.Countries;
             
                     //.Select(x => new SelectListItem { Text = x.Country_Name, Value = x.Country_Code })
                    // .ToList();
             
                //var newItem = new SelectListItem { Text = "a select me", Value = "01" };
                //xxx.Add(newItem); 
              
            ////return   _context.Countries.ToList() ;
            //return _context.Countries.OrderBy(album => album.Country_Name)
            // .Select(album =>

            //     new SelectListItem
            //     { 
            //         Text = album.Country_Name,
            //         Value = album.Country_Code
            //     });
              
        }
        public IQueryable<Security_Type> ListSecTypeCode()
        {
            ISecurityTypesRepository dbCntx = new SecurityTypeRepository(); 
            return dbCntx.GetAll(); 
        } 
   
        public IQueryable<Currency> ListCurrencies()
        {
            ICurrencyRepository dbCntx = new CurrencyRepository(); 
            return dbCntx.GetAll()
                  .OrderBy(s => s.Currency_Name);
           // return _context.Currencies
                //.Select(x => new SelectListItem { Text = x.Currency_Name, Value = x.Currency_Code })
                  
        } 
 
        public IQueryable<Exchange> ListExchanges()
        {
          IExchangeRepository dbCntx = new  ExchangeRepository (); 
          //using (IExchangeRepository dbCntx = new  ExchangeRepository ())
       
              return dbCntx.GetAll()
                    .OrderBy(s => s.Exchange_Name);
        
            //return _context.Exchanges   
            //    //.Where(p => p.Entity_ID == 1)
            //    // .Select(x => new SelectListItem { Text = x.Exchange_Name, Value = x.Exchange_Code })
            //     .OrderBy(s => s.Exchange_Name);
        }

        public IQueryable<Code_Miscellaneous> ListMiscellanousTypes(string iCodeType)
        {

            ICodeMiscellaneous dbCntx = new CodeMiscellaneousRepository();
            return dbCntx.GetAll().Where(c => c.MisCode_Type == iCodeType); 
         
        }
        public IQueryable<Portfolio> ListPortfolios(decimal iUser)
        {
            IPortfolioRepository3 dbCntx = new PortfolioRepository3();  
            return dbCntx.GetAll().Where(c => c.Entity_ID == iUser);
             
        }
        public IQueryable<Security_Detail> ListSecurities(decimal iEntity)
        {
            ISecurityDetailRepository dbCntx  ;
            dbCntx = new  SecurityDetailRepository();
            return dbCntx.GetAll(r => r.Security_Type_Code =="FXRATE")
                     .MatchEntityID(c => c.Entity_ID == iEntity);

            //return dbCntx.GetAll().Where(c => c.Entity_ID == iUser);

        }
        public IQueryable<Party> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId)
        {
            IPartyRepository dbCntx = new PartyRepository();

            return dbCntx.GetPartyValues(iEntity, iType, iGenericEntityId)
                      .OrderBy(s => s.Party_Name);  
        }

        //public IEnumerable<SelectListItem> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId)
        //{ 
        //    return _context.Parties.Where(c => c.Party_Type == iType)
        //            .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity)
        //            .Select(x => new SelectListItem { Text = x.Party_Name, Value = x.Party_Code }) ;
             
        //}
        public IQueryable<Currency_Pair> ListCurrencyPairs()
        {
            ICurrencyPairRepository dbCntx = new CurrencyPairRepository();
            return dbCntx.GetAll();
        }

        public List<SelectListItem> ListTrueFalse()
        {  
            return  new List<SelectListItem>
                    {
                        new SelectListItem { Text = "True", Value = bool.TrueString },
                        new SelectListItem { Text = "False", Value = bool.TrueString }
                    }; 
        }
        public void Dispose()
        {
            if (!disposed)
            {
                _context.Dispose();
                disposed = true;
            }
        }
        //public IEnumerable<Country> List()
        //{
        //    return new List<Country>
        //            {
        //                new Country { Country_Code = "AK", Country_Name = "Alaska" },
        //                new Country { Country_Code = "AL", Country_Name = "Alabama" }
        //            };
        //}
    }

 
}