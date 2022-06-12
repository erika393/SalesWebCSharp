using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System;
using System.Threading.Tasks;
using SalesWebMvc.Services.Exceptions;


namespace SalesWebMvc.Controllers
{
    [Authorize]
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd"); //usamos na view

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd"); //usamos na view

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }

        public IActionResult NewSale([FromRoute] int? id)
        {
            var viewModel = new SalesRecordFormViewModel { SellerId = id ?? -1};
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewSale(int? id, SalesRecord obj)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Index), new { message = "Id not provided" });
            }
            if (!ModelState.IsValid)
            {
                var viewModel = new SalesRecordFormViewModel { SalesRecord = obj};
                return View(viewModel); 
            }
            obj.Id = (_salesRecordService.ExistId(obj.Id)) ? _salesRecordService.GetNewId() : obj.Id;

            var seller = _sellerService.FindById(id);
            seller.AddSales(obj);
            var sellerCount = seller.Sales.Count;
            _salesRecordService.Insert(obj);
            return RedirectToAction(nameof(Success));
        }
        
    }
}