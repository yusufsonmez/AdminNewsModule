using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminNewsModule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;  
using Microsoft.Extensions.Logging;
using System.IO;
using AdminNewsModule.Filter;

namespace AdminNewsModule.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly NewsContext _context;

        public RegisterController(ILogger<RegisterController> logger, NewsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register(Author author){

            var existAuthor = _context.Author.Where(e => e.Email == author.Email).FirstOrDefault<Author>();;
            try
            {
            if(!(existAuthor.Email == String.Empty)){
                // eger bu sekilde bir kullanici varsa ayni sayfada kal
                return RedirectToAction("Index", "Register", new { area = "" });
                }
            }
            catch (System.Exception)
            {
                // eger yeni kullaniciysa login sayfasina git
                _context.Update(author);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Login", new { area = "" });
            }
                return RedirectToAction("Index", "Register", new { area = "" });
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
