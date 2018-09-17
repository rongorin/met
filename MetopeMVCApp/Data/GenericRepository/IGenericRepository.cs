using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
// for this generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data.GenericRepository
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {

        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy2(Expression<Func<T, bool>> predicate);
        bool AnyExists(Expression<Func<T, bool>> predicate);
        T  Get(decimal id);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        void Save();
    }
}