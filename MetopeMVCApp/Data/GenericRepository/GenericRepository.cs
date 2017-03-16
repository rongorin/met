using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

// for this generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data.GenericRepository
{
    public abstract class GenericRepository<C, T> :
        IGenericRepository<T>
        where T : class
        where C : DbContext, new()
    { 

        private C _entities = new C();
        public C Context
        {

            get { return _entities; }
            set { _entities = value; }
        }

        public virtual IQueryable<T> GetAll()
        { 
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public virtual T Get(decimal id)
        {
            return  _entities.Set<T>().Find(id);
        } 

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        { 
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        public virtual void Update(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified; 
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {

            if (!this.disposed)
                if (disposing)
                    _entities.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}