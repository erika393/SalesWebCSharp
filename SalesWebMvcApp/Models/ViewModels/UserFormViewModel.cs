using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvcApp.Models.ViewModels
{
    public class UserFormViewModel
    {
        //mantenha o mesmo name de attrs para ajudar o framework a reconhecer
        public User User { get; set; }
        public ICollection<Seller> Sellers { get; set; }
    }
}
