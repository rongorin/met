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
        IQueryable<Security_Type> ListSecTypeCode();
         
        IQueryable<Exchange> ListExchanges();
        IEnumerable<SelectListItem> ListMiscellanousTypes(string iCodeType);
        IEnumerable<SelectListItem> ListPortfolios(decimal iUser );

        IEnumerable<SelectListItem> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId);
        //IEnumerable<Country> List();
        void Dispose();
         
    }

    public class Services : IDisposable, IServices   
    {
        MetopeMVCApp.Services.IServices isvc;

        MetopeDbEntities _context;
        private bool disposed = false;

        public Services(bool createCntxt)
        {
            if (createCntxt)
                 _context = new MetopeDbEntities();
        }

        public IQueryable<Country> ListCountry()
        {

            return  
                _context.Countries;
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
            return _context.Security_Type;
        } 
   
        public IQueryable<Currency> ListCurrencies()
        {
            return _context.Currencies
                //.Select(x => new SelectListItem { Text = x.Currency_Name, Value = x.Currency_Code })
                    .OrderBy(s => s.Currency_Name); 
        } 
        //public IEnumerable<SelectListItem> ListCountry()
        //{ 
        //    return
        //         _context.Countries
        //         .Select(x => new SelectListItem { Text = x.Country_Name, Value = x.Country_Code })
        //          .ToList();
        //}
        public IQueryable<Exchange> ListExchanges()
        {
          IExchangeRepository dbCntx = new  ExchangeRepository ();

          return dbCntx.GetAll()
                .OrderBy(s => s.Exchange_Name);

            //return _context.Exchanges   
            //    //.Where(p => p.Entity_ID == 1)
            //    // .Select(x => new SelectListItem { Text = x.Exchange_Name, Value = x.Exchange_Code })
            //     .OrderBy(s => s.Exchange_Name);
        }
        public IEnumerable<SelectListItem> ListMiscellanousTypes(string iCodeType)
        {
            return _context.Code_Miscellaneous
                 .Where(c => c.MisCode_Type == iCodeType)
                 .Select(x => new SelectListItem { Text = x.MisCode_Description, Value = x.MisCode })
                 .ToList();
        }
        public IEnumerable<SelectListItem> ListPortfolios(decimal iUser )
        {

            IPortfolioRepository3 PortfolioRepo = new PortfolioRepository3();

            return PortfolioRepo.GetPortfolios(iUser )
                     .Select(x => new SelectListItem { Text = x.Portfolio_Name, Value = x.Portfolio_Code }) ; 
             
        }


        public IEnumerable<SelectListItem> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId)
        {
            return _context.Parties.Where(c => c.Party_Type == iType)
                    .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity)
                    .Select(x => new SelectListItem { Text = x.Party_Name, Value = x.Party_Code }) ;
             
        }
        public IEnumerable<SelectListItem> ListCurrencyPairs()
        {
            return _context.Currency_Pair
                    .Select(x => new SelectListItem { Text = x.Currency_Pair_Code, Value = x.Currency_Pair_Code })
                    .OrderBy(s => s.Text);
             
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