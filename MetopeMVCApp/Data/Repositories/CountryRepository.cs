using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetopeMVCApp.Data.Repositories
{
    public class CountryRepository : Repository<Country>
    {
        //possibly override the Update ..here override and update the Version property:
 
            //IEnumerable<Country> List();
    }


}