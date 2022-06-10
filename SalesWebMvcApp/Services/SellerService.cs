using System;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Data;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        //readonly faz com que nao seja alterado
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //assinc operation
        public List<Seller> FindAll()
        {
            //precisa colocar await para falar ao compilador q essa chamada eh assincrona
            return _context.Seller.ToList();

        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);
           await _context.SaveChangesAsync();
        }
        public Seller FindById(int? id)
        {
            //add esse Include (parte do Microsoft Entity Framework) pq se vermos os detalhes do Seller nao aparecera o Department
            //pois o Department eh outro obj e nao um Seller, para isso temos que fazer o Include
            return _context.Seller.Include(obj => obj.Department).Include(obj => obj.Sales).FirstOrDefault(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public void Update(Seller obj)
        {
            //o any serve pra falar se existe algum dado na condicao colocada
            try
            {
                var result = _context.Seller.FirstOrDefault(x => x.Id == obj.Id);
                if (result == null)
                {
                    throw new NotFoundException("Id not found!");
                }

                result.Name = obj.Name;
                result.DepartmentId = obj.DepartmentId;
                result.BirthDate = obj.BirthDate;
                result.BaseSalary = obj.BaseSalary;
                result.Email = obj.Email;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message); //excecao de nivel de acesso a dados
                //Aqui eh capturado a excecao em nivel de acesso a dados e relancada em nivel de servico
            }

        }
    }
}
