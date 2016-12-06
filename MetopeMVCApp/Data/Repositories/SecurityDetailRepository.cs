using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace MetopeMVCApp.Data.Repositories
{
    public class SecurityDetailRepository : GenericRepository<MetopeDbEntities, Security_Detail>, 
                                        ISecurityDetailRepository
    {
    
    }
    public class ExchangeRepository : GenericRepository<MetopeDbEntities, Exchange>,
                                    IExchangeRepository
    {
        //possibly override the Update ..here override and update the Version property:  
    } 
    public class CountryRepository : GenericRepository<MetopeDbEntities, Country>,
                                    ICountryRepository
    {
        //possibly override the Update ..here override and update the Version property:  
    }
}   