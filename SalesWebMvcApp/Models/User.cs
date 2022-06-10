using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvcApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public string EmailUserAtt { get; set; }

        public User(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public void AddSeller(Seller sr)
        {
            Sellers.Add(sr);
        }

        public void RemoveSeller(Seller sr)
        {
            Sellers.Remove(sr);
        }
    }
}
