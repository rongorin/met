using Metope.DAL;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetopeMVCApp.Test
{
    class SecurityDetailHistory
    {
        private  Security_Detail Security;

        public SecurityDetailHistory( Security_Detail secDetail)
        {
            // Complete member initialization
            this.Security = secDetail;
        }

       public HistoryResult CreateNewOne()
        {
            var res = new HistoryResult();
            res.PriceHist = 5;

            return res;
        }
    }
}
