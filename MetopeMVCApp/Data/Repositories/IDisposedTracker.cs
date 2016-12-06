using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data.Repositories
{
    public interface IDisposedTracker
    {
        bool IsDisposed { get; set; }
    }
}