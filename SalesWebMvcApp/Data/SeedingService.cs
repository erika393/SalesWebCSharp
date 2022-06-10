using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using SalesWebMvcApp.Data;
using SalesWebMvcApp.Models;
using System;
using System.Linq;

namespace SalesWebMvc.Data
{
    public class SeedingService
    {
        private SalesWebMvcContext _context;
        private ApplicationDbContext _appContext;

        public SeedingService(SalesWebMvcContext context, ApplicationDbContext appContext)
        {
            _context = context;
            _appContext = appContext;
        }

        //essa operacao sera responsavel por popular a app (Seed)
        public void Seed()
        {
            
            if (_context.Department.Any() || _context.Seller.Any() || _context.SalesRecord.Any())
            {
                //o db ja foi populado
                return;
            }

            Department d1 = new Department(1, "Computers");

            Department d2 = new Department(2, "Electronics");

            Department d3 = new Department(3, "Fashion");

            Department d4 = new Department(4, "Books");

            User user = new User(1, "erika@erika.com", "12345");

            //abaixo mandamos add esses objetos no db usando o entity framework
            //o addRange permite que add varios obj de uma vez
            _context.Department.AddRange(d1, d2, d3, d4);
            _context.User.AddRange(user);

            //precisamos fazer abaixo para salvar tudo
            _context.SaveChanges();
        }
    }
}
