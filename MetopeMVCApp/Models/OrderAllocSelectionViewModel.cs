using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Models
{
    public class OrderAllocSelectionViewModel
    {
        public List<SelectOrderAllocEditorViewModel> OrderAllocations { get; set; }

        public OrderAllocSelectionViewModel ()
        {
            this.OrderAllocations = new  List<SelectOrderAllocEditorViewModel>();
        }
        public IEnumerable<decimal> getSelectedIds()
        {
            return this.OrderAllocations.Where(a => a.Selected == true)
                                               .Select(td => td.Order_ID).ToList();

        }
    }
}