using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Data.Repositories;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// for this generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data
{
    public interface ISecurityDetailRepository : IGenericRepository<Security_Detail>
    { 
    }
    public interface IExchangeRepository : IGenericRepository<Exchange>
    {
    }
    public interface ICountryRepository : IGenericRepository<Country>
    {
    }
}