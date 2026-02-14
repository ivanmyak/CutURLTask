using System.Diagnostics;
using CutURLTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace CutURLTask.Controllers
{
    public class CuturlController : Controller
    {
        public IActionResult Generate()
        {
            ViewData["Title"] = "CutURL Main";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "О чём";
            return View();
        }

        /// <summary>
        /// Проверка ссылок
        /// </summary>
        /// <param name="longurl">Длинный URL</param>
        /// <param name="shorturl">Короткий URL</param>
        /// <returns>        
        /// если найдёт - страница с информацией по ссылке.
        /// если нет (и ссылка была короткой)- сообщение о не-нахождении.
        /// если нет (и ссылка была длинной)- генерация новой и страница с информацией 
        /// </returns>
        public IActionResult Lookup(UrlViewModel model)
        {
            if (model.ShortURL != null && model.LongURL == null)
            {
                ModelState.AddModelError("ShortUrl", "Такой короткой строки не найдено");
                return View("Generate", model);
            }
            else
                return View("Details");
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Edit()
        {

            return View();
        }

        /// <summary>
        /// Наша переадресация по коротким ссылкам
        /// </summary>
        /// <param name="code">Наша "короткая" ссылка</param>
        /// <returns></returns>
        public async Task<IActionResult> RedirectToUrl(string code)
        {
            //var record = await _context.FindByShortUrlAsync(code);
            //if (record == null) 
            //    return NotFound();
            //record.ClickCount++; 
            //await _context.UpdateAsync(record);
            //return Redirect(record.LongUrl);

            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
