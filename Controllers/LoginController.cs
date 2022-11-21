using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminNewsModule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;  
using Microsoft.Extensions.Logging;
using AdminNewsModule.Models;
using AdminNewsModule.Filter;

namespace AdminNewsModule.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly NewsContext _context;

        public LoginController(ILogger<LoginController> logger, NewsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Login(string Email, string Password)
        {
           // Eger girilen email ve password yazarinkine esitse giris yapar, degilse ayni sayfada kalir, giriş yapamaz
           var author = _context.Author.FirstOrDefault(w=>w.Email == Email && w.Password == Password);
           if(author == null)
           {
               return RedirectToAction(nameof(Index));    
           }
           
           HttpContext.Session.SetInt32("id", author.Id);
           
            return RedirectToAction("", "News", new { area = "" });    
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
