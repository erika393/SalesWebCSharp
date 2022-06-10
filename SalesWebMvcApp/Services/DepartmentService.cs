using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context;
        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //Operacao sincrona para chamada no db
        //public List<Department> FindAll()
        //{
        //    //essa expressao linq so eh executada quando realmente for preciso, no caso, tolist logo depois e esse tolist eh uma operacao sincrona
        //    //porem existe uma outra operacao tolistasync, mas ela nao eh do asp net e sim do framework
        //    return _context.Department.OrderBy(x => x.Name).ToList();
        //}

        //operacao assincrona para chamada no db (recomendavel)
        public async Task<List<Department>> FindAllAsync()
        {
            //precisa colocar await para falar ao compilador q essa chamada eh assincrona
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();

        }

        public Department FindById(int id)
        {
            //precisa colocar await para falar ao compilador q essa chamada eh assincrona
            return _context.Department.FirstOrDefault(obj => obj.Id == id);

        }
    }
}
