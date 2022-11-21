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
    [UserFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsContext _context;

        public HomeController(ILogger<HomeController> logger, NewsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> AddCategory(Category category){
            // Eger category.Id 0 ise, kategori kayitli bir kategori degildir. Ekleme yapilir.
            // id'si var ise kategori kayitlidir ve guncellenir. 
            if(category.Id==0){
                await _context.AddAsync(category);
            }
            else{
                _context.Update(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Category));
        }

        public async Task<IActionResult> AddAuthor(Author author){
            if(author.Id==0){
                await _context.AddAsync(author);
            }
            else{
                _context.Update(author);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Author));
        }

        public async Task<IActionResult> CategoryDetails(int Id){
            // Id bilgisi ile categori detayi cekilir
            var category = await _context.Category.FindAsync(Id);
            return Json(category);
        }

        public async Task<IActionResult> AuthorDetails(int Id){
            var author = await _context.Author.FindAsync(Id);
            return Json(author);
        }

        public IActionResult Category()
        {
            // kategori listeleme
            List<Category> list = _context.Category.ToList();
            return View(list);
        }

        public IActionResult Author()
        {
            List<Author> list = _context.Author.ToList();
            return View(list);
        }

        public async Task<IActionResult> DeleteCategory(int? Id)
        {
            // silme islemi icin id bilgisiyle ilgili kategoriyi bulduk ve sildik.
            Category category = await _context.Category.FindAsync(Id);
            _context.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Category));
        }

        public async Task<IActionResult> DeleteAuthor(int? Id)
        {
            Author author = await _context.Author.FindAsync(Id);
            _context.Remove(author);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Author));
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
