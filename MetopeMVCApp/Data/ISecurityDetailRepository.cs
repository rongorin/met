using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Data.Repositories;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
// for this generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data
{
    public interface ISecurityDetailRepository : IGenericRepository<Security_Detail>
    {
        IQueryable<Security_Detail> GetAll(Expression<Func<Security_Detail, bool>> predicate);
        IQueryable<Security_Detail> GetAllActive( );

    }
    public interface IExchangeRepository : IGenericRepository<Exchange>
    {
    }
    public interface ICountryRepository : IGenericRepository<Country>
    {
    }
    public interface ISecurityTypesRepository : IGenericRepository<Security_Type>
    { 
    }
    public interface ICurrencyPairRepository : IGenericRepository<Currency_Pair>
    {

    }
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {

    }
    public interface IPartyRepository : IGenericRepository<Party>
    {
          IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId);
    }
    public interface ICodeMiscellaneous : IGenericRepository<Code_Miscellaneous>
    {
    }
}