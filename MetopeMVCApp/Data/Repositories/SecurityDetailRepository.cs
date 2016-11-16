using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data.Repositories
{
    public class SecurityDetailRepository : Repository<Security_Detail>
    {
        //possibly override the Update ..here override and update the Version property:

        //public override void Update(Security_Detail entity)
        //{
        //    base.Update(entity);
        //    SaveChanges();
        //    entity.Version++;
        //    base.Update(entity);
        //    SaveChanges();
        //}

    }
}