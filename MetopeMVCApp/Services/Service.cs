using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetopeMVCApp.Services
{
    public interface IServices
    {
        IEnumerable<SelectListItem> ListCountry();
        IEnumerable<SelectListItem> ListExchanges(); 
        IEnumerable<SelectListItem> ListSecTypeCode();
        IEnumerable<Country> List();

        // Other methods of user service
    }

    public class Services : IServices   
    { 
        MetopeDbEntities _context;

        public Services()
        {
            _context = new MetopeDbEntities();
        }

        public IEnumerable<SelectListItem> ListCountry()
        {
            return _context.Countries
                 .Select(x => new SelectListItem { Text = x.Country_Name, Value = x.Country_Code })
                 .ToList(); 
            ////return   _context.Countries.ToList() ;
            //return _context.Countries.OrderBy(album => album.Country_Name)
            // .Select(album =>
            //     new SelectListItem
            //     { 
            //         Text = album.Country_Name,
            //         Value = album.Country_Code
            //     });
              
        }
        public IEnumerable<SelectListItem> ListSecTypeCode()
        {
            return _context.Security_Type
                 .Select(x => new SelectListItem { Text = x.Name, Value = x.Security_Type_Code })
                 .ToList(); 
        }
        public IEnumerable<SelectListItem> ListExchanges()
        {
            return _context.Exchanges
                //.Where(p => p.Entity_ID == 1)
                 .Select(x => new SelectListItem { Text = x.Exchange_Name, Value = x.Exchange_Code })
                 .ToList();
        } 
 
        public IEnumerable<Country> List()
        {
            return new List<Country>
                                        {
                                            new Country { Country_Code = "AK", Country_Name = "Alaska" },
                                            new Country { Country_Code = "AL", Country_Name = "Alabama" }
                                        };
        }
    }

 
}