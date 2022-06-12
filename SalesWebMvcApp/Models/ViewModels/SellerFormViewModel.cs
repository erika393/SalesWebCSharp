using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models.ViewModels
{
    public class SellerFormViewModel
    {
        //mantenha o mesmo name de attrs para ajudar o framework a reconhecer
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<SalesRecord> Sales { get; set; }
    }
}
