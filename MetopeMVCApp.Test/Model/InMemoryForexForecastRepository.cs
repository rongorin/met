using Metope.DAL;
using MetopeMVCApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetopeMVCApp.Test.Model
{
    class InMemoryForexForecastRepository : IForexForecastRepository 
    {
        private readonly IList<Forex_Forecast> _db = new List<Forex_Forecast>();
         
       
        public void Add(Forex_Forecast ff)
        {
            if (_db.Any(x => x.Entity_ID == ff.Entity_ID &&
                              x.Security_ID == ff.Security_ID &&
                              x.Month_Year == ff.Month_Year))
            {
                _db.Remove(FindById(ff.Entity_ID, ff.Security_ID, ff.Month_Year));
            }
            _db.Add(ff);
        }
        public void Save()
        {

            //do nothing
        }

       
        public IEnumerable<Forex_Forecast> GetAllRecs(System.Linq.Expressions.Expression<Func<Forex_Forecast, bool>> predicate)
        {
            return _db.ToList() ;
        }

        public Forex_Forecast FindById(decimal EntityId, decimal SecurityId, string MonthYear)
        {
            return _db.FirstOrDefault(x => x.Entity_ID == EntityId && x.Security_ID == SecurityId && 
                                        x.Month_Year == MonthYear);
        }

        IQueryable<Forex_Forecast> Data.GenericRepository.IGenericRepository<Forex_Forecast>.GetAll()
        {
            throw new NotImplementedException();
        }

        IQueryable<Forex_Forecast> Data.GenericRepository.IGenericRepository<Forex_Forecast>.FindBy(
                                                System.Linq.Expressions.Expression<Func<Forex_Forecast, bool>> predicate)
        {
            return _db.AsQueryable().Where(predicate);

        }
       
        public IEnumerable<Forex_Forecast> FindBy(System.Linq.Expressions.Expression<Func<Forex_Forecast, bool>> predicate)
        {
            return _db.AsQueryable().Where(predicate);
              
        }

        public IEnumerable<Forex_Forecast> FindAll()
        {
            return _db.ToList();
        }

        public IEnumerable<Forex_Forecast> FindBy2(System.Linq.Expressions.Expression<Func<Forex_Forecast, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool AnyExists(System.Linq.Expressions.Expression<Func<Forex_Forecast, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Forex_Forecast Get(decimal id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Forex_Forecast entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Forex_Forecast entity)
        {
            throw new NotImplementedException();
             
        }
        
         
         
        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
