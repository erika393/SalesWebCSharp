using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models.ViewModels
{
    public class SalesRecordFormViewModel
    {
        //mantenha o mesmo name de attrs para ajudar o framework a reconhecer
        public SalesRecord SalesRecord { get; set; }
        public int SellerId { get; set; }
    }
}
