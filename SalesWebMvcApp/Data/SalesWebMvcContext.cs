using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvcApp.Models;
using System;
using System.Threading.Tasks;

namespace SalesWebMvc.Data
{
    public class SalesWebMvcContext : DbContext
    {
        public SalesWebMvcContext (DbContextOptions<SalesWebMvcContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<SalesRecord> SalesRecord { get; set; }
        public DbSet<User> User { get; set; }

        internal Task<User> Where()
        {
            throw new NotImplementedException();
        }
    }
}
