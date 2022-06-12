using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using SalesWebMvc.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SalesWebMvcApp.Services;
using SalesWebMvcApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace SalesWebMvc.Controllers
{
    [Authorize]
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        private readonly UserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;


        public SellersController(SellerService sellerService, DepartmentService departmentService, UserService userService, SignInManager<IdentityUser> signInManager)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
            _userService = userService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            User user = _userService.FindByEmail(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Oops.It looks like your username is not recognized, please try again later, if the problem persists, please register again" });
            }
            var objec = _sellerService.FindAll().FirstOrDefault();
            var list = _sellerService.FindAll().Where(obj => obj.UserId == user.Id);
            if (list == null)
            {
                return View();
            }
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync(); 
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(Seller obj)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
                return View(viewModel); //se o js for desabilitado e n puder fazer a validacao, ele manda voltar e terminar essa validacao antes de continuar retornando a mesma view
            }
            //inserimos o obj
            //User user = _userService.FindByEmail(ViewData["UserEmail"].ToString());
            User user = _userService.FindByEmail(User.Identity.Name);
            obj.UserId = user.Id;
            obj.Department = _departmentService.FindById(obj.DepartmentId);
            await _sellerService.InsertAsync(obj);
            user.AddSeller(obj);
            //redirecionamos para a acao index
            //obs: poderiamos colocar Red...("Index"), porem com o nameof, se 
            //mudarmos 
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) //int? = opcional
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value); 
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            User user = _userService.FindByEmail(User.Identity.Name);
            var obj = _sellerService.FindById(id.Value); //pra pegar o valor dele caso existe, por ser opcional
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" }); ;
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id) //esse ? ao lado do int é só pra nao dar erro de execucao, mas ele eh obrigatorio
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); //se o js for desabilitado e n puder fazer a validacao, ele manda voltar e terminar essa validacao antes de continuar retornando a mesma view
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e) //o App eh um super das excecoes personalizadas
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message) //justamente para retornar a view Error.cshtml
        {
            var viewModel = new Models.ViewModels.ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //serve para pegarmos o id interno da requisicao
                                                                                //?? significa que se for nulo, ira substituir pelo HttpContext
            };
            return View(viewModel);
        }

       

        
    }
}
