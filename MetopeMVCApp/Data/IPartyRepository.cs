using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MetopeMVCApp.Data
{
    public class IPartyRepository : IGenericRepository<Party>
    {
        //IQueryable<Party> GetAll(Expression<Func<Party, bool>> predicate);
        IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId);

    }
}