using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetopeMVCApp.Test
{
    class FakeMetopeDb : IMetopeDbEntities
    {
        public IQueryable<T> Restaurants
        {
            get { return _map.Get<Restaurant>().AsQueryable(); }
            set { _map.Use<Restaurant>(value); }
        }

        public IQueryable<Review> Reviews
        {
            get { return _map.Get<Review>().AsQueryable(); }
            set { _map.Use<Review>(value); }
        }

        public int SaveChanges()
        {
            ChangesSaved = true;
            return 0;
        }

        public bool ChangesSaved { get; set; }

        public T Attach<T>(T entity) where T : class
        {
            _map.Get<T>().Add(entity);
            return entity;
        }

        public T Add<T>(T entity) where T : class
        {
            _map.Get<T>().Add(entity);
            return entity;
        }

        public T Delete<T>(T entity) where T : class
        {
            _map.Get<T>().Remove(entity);
            return entity;
        }

        
    }
}
