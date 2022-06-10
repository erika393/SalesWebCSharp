using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvcApp.Data;
using SalesWebMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvcApp.Services
{
    public class UserService
    {
        private readonly SalesWebMvcContext _context;

        public UserService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(User obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> FindAllAsync()
        {
            //precisa colocar await para falar ao compilador q essa chamada eh assincrona
            return await _context.User.ToListAsync();
        }

        public User FindById(int id)
        {
            var result =  _context.User.FirstOrDefault(obj => obj.Id == id);
            return result;
        }

        public User FindByEmail(string email)
        {
            var result = _context.User.FirstOrDefault(obj => obj.Email == email);
            return result;
        }

        public async Task<int> GetNewId()
        {
            var listCount = _context.User.ToList().Count;
            if(listCount < 1)
            {
                return 1;
            }
            var lastId = (await _context.User.LastAsync()).Id;
            //precisa colocar await para falar ao compilador q essa chamada eh assincrona
            return lastId + 1;
        }
    }
}
