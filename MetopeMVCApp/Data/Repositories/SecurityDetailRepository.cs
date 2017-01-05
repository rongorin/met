﻿using MetopeMVCApp.Data;
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
    public class SecurityTypeRepository : GenericRepository<MetopeDbEntities, Security_Type>,
                                         ISecurityTypesRepository
    {
        //possibly override the Update ..here override and update the Version property:  
    }
    public class CurrencyRepository : GenericRepository<MetopeDbEntities, Currency>,
                                      ICurrencyRepository
    { 
    }
    public class CurrencyPairRepository : GenericRepository<MetopeDbEntities, Currency_Pair>,
                                  ICurrencyPairRepository
    { 
    }
    public class PartyRepository : GenericRepository<MetopeDbEntities, Party>,
                          IPartyRepository
    {

        public IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId)
        {
            return GetAll().Where(c => c.Party_Type == iType)
                                .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity); 

        } 
    }
    public class CodeMiscellaneousRepository : GenericRepository<MetopeDbEntities, Code_Miscellaneous>,
                                  ICodeMiscellaneous
    { 
    } 
}   