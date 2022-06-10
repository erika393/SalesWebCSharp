using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SalesWebMvc.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SalesWebMvcApp.Services;
using SalesWebMvcApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SalesWebMvcApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        public UsersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost] //para dizermos q eh um metodo post, se fosse get n precisaria colocar
        [ValidateAntiForgeryToken] //metodo de segurança p q n usem o token de validacao para
        //enviar dados maliciosos
        public async Task<IActionResult> Create(User obj)
        {
            if (!ModelState.IsValid)
            {
                return View(); //se o js for desabilitado e n puder fazer a validacao, ele manda voltar e terminar essa validacao antes de continuar retornando a mesma view
            }
            //inserimos o obj
            await _userService.InsertAsync(obj);
            //redirecionamos para a acao index
            //obs: poderiamos colocar Red...("Index"), porem com o nameof, se 
            //mudarmos 
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
