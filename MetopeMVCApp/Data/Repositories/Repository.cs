

using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetopeMVCApp.Data.Repositories
{
    public class Repository<T> : IDisposable where T : class
    {
        private bool disposed = false;
        private MetopeDbEntities context = null;

        protected DbSet<T> DbSet
        {
            get;
            set;
        }

        public Repository()
        {
            context = new MetopeDbEntities();
            DbSet = context.Set<T>();
        }

        public Repository(MetopeDbEntities context)
        {
            this.context = context;
        }
        public IQueryable<T> GetAll()
        {
            //return DbSet.ToList(); 
            return DbSet.AsQueryable(); 

        }
 
        public T Get(decimal id)
        {
            return DbSet.Find(id);
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(decimal id)
        {
            DbSet.Remove(DbSet.Find(id));
        } 

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                context.Dispose();
                disposed = true;
            }
        }
  
    }
}