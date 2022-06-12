using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;
        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; //pegara do tipo DbSet e construir o tipo IQuerible
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller) //faz o join
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; //pegara do tipo DbSet e construir o tipo IQuerible
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller) //faz o join
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }

        public int GetNewId()
        {
            var listCount = _context.SalesRecord.ToList().Count;
            if (listCount < 1)
            {
                return 1;
            }
            var lastId = _context.SalesRecord.LastAsync().Id;
            return lastId + 1;
        }

        public bool ExistId(int id)
        {
            return _context.SalesRecord.Any(obj => obj.Id == id);
        }
        public void Insert(SalesRecord obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public int FindAllBySellerId(int id)
        {
            return _context.SalesRecord.ToList().FindAll(obj => obj.Seller.Id == id).Count;
        }
    }
}
