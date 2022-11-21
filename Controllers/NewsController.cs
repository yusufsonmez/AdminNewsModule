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
    [UserFilter]
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private readonly NewsContext _context;

        public NewsController(ILogger<NewsController> logger, NewsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.News.ToList();
            return View(list);
        }
        
        public IActionResult Publish(int Id)
        {
            // yayinlama bu kisimdan yapilir.
            // id bilgisiyle news bulunur ve IsPublish=true yapilir 
            var news = _context.News.Find(Id);
            news.IsPublish=true;
            _context.Update(news);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult News(int id)
        {
            ViewBag.Categories = _context.Category.Select(w =>
                new SelectListItem{
                    Text = w.Name,
                    Value = w.Id.ToString() 
                }
            ).ToList();
            
            // id 0 a esit degilse, yani yeni bir news ise news ekleme sayfasi acilir.model bununla birlikte iletilir
            // 0 a esitse view, model olmadan acilir. yeni bir ekleme olacagi anlamına gelir.
            if(id!=0){
                            
            var news = _context.News.Find(id);
            if(news is not null){
                return View(news);
                 }
        
            }
            else{
                return View(); 
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(News model)
        {
            if(model != null)
            {
                // file alinir, kaydedilir
                var file = Request.Form.Files.First();
                string savePath = Path.Combine("C:","Users","yusuf","source","repos","NewsModule","NewsModule","wwwroot","img");                
                var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
                var fileUrl = Path.Combine(savePath, fileName);
                using(var fileStream = new FileStream(fileUrl, FileMode.Create)){
                    await file.CopyToAsync(fileStream);
                }
                // veritabanına eklenir
                model.ImagePath = fileName;
                model.AuthorId = (int)HttpContext.Session.GetInt32("id");
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(true);
            }   
            return Json(false);
        }

        [HttpPut]
        public async Task<IActionResult> Update(News model)
        {
            if(model != null)
            {
                // file alinir, kaydedilir
                var file = Request.Form.Files.First();
                string savePath = Path.Combine("C:","Users","yusuf","source","repos","NewsModule","NewsModule","wwwroot","img");
                var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
                var fileUrl = Path.Combine(savePath, fileName);
                using(var fileStream = new FileStream(fileUrl, FileMode.Create)){
                    await file.CopyToAsync(fileStream);
                }
                // bilgiler guncellenir.
                var theModel = _context.News.Find(model.Id);
                theModel.Title = model.Title;
                theModel.Subtitle = model.Subtitle;
                theModel.ImagePath = fileName;
                theModel.AuthorId = (int)HttpContext.Session.GetInt32("id");
                _context.Update(theModel);
                _context.SaveChanges();
                return Json(true);
            }   
            return Json(false);
        }

        public async Task<IActionResult> DeleteNews(int? Id)
        {
            // silme islemi yapilip ayni sayfaya yonlendirme yapilir
            News news = await _context.News.FindAsync(Id);
            _context.Remove(news);
            await _context.SaveChangesAsync();

            return RedirectToAction("", "News", new { area = "" });    
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
