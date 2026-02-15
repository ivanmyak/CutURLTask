using CutURLTask.Data;
using CutURLTask.Entities;
using CutURLTask.Models;
using CutURLTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace CutURLTask.Controllers
{
    public class CuturlController : Controller
    {
        private readonly CutUrlDbContext _context;
        public CuturlController(CutUrlDbContext context)
        {
            _context = context;
        }

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
        /// <returns>        
        /// если найдёт - страница с информацией по ссылке.
        /// если нет (и ссылка была короткой) - сообщение о не-нахождении.
        /// если нет (и ссылка была длинной) - генерация новой и страница с информацией 
        /// </returns>
        public async Task<IActionResult> LookupAsync(UrlViewModel model)
        {
            if (string.IsNullOrEmpty(model.ShortURL?.OriginalString) && string.IsNullOrEmpty(model.LongURL?.OriginalString))
            {
                ModelState.AddModelError("ShortUrl", "Введите короткую или длинную ссылку");
                return View("Generate", model);
            }
            string code;
            UrlRecord? urlRecord = null;

            // Если пришёл короткий код
            if (!string.IsNullOrEmpty(model.ShortURL?.OriginalString))
            {
                code = GeneratorService.ExtractCode(model.ShortURL);
                urlRecord = await _context.UrlRecords
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Code == code);

                if (urlRecord == null)
                {
                    ModelState.AddModelError("ShortUrl", "Такой короткой ссылки не найдено");
                    return View("Generate", model);
                }
            }
            // Если пришёл длинный URL
            else if (model.LongURL != null)
            {
                string longUrl = model.LongURL.OriginalString;

                urlRecord = await _context.UrlRecords
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.LongUrl == longUrl);

                if (urlRecord == null)
                {
                    // Генерация нового кода
                    code = await GeneratorService.GenerateCodeAsync(_context);

                    urlRecord = new UrlRecord
                    {
                        LongUrl = longUrl,
                        Code = code,
                        UsedCount = 0,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.AddAsync(urlRecord);
                    await _context.SaveChangesAsync();
                }
            }
            string host = HttpContext.Request.Host.Value ?? "localhost";
            TempData["DetailsModel"] = JsonSerializer.Serialize(new DetailsUrlViewModel(urlRecord!, host));
            return RedirectToAction("Details", new { code = urlRecord?.Code });
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(string code)
        {
            ViewData["Title"] = "Подробности";
            DetailsUrlViewModel? model;
            if (TempData["DetailModel"] is string json)
            {
                model = JsonSerializer.Deserialize<DetailsUrlViewModel>(json);
            }
            else
            {
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest();
                }
                var urlrec = await _context.UrlRecords.FirstOrDefaultAsync(u => u.Code == code) ?? throw new("Нет такой записи в БД!");

                string host = HttpContext.Request.Host.Value ?? "localhost";
                model = new DetailsUrlViewModel(urlrec, host);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DetailsAsync(DetailsUrlViewModel model)
        {
            ViewData["Title"] = "Подробности";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditUrlViewModel model)
        {
            UrlRecord urlRecord = await _context.UrlRecords.FirstOrDefaultAsync(u => u.Id == model.Id) ?? throw new("Нет такой записи в БД!");

            if (urlRecord.LongUrl != model.LongUrl)
            {
                urlRecord.LongUrl = model.LongUrl;
                _context.UrlRecords.Update(urlRecord);
                await _context.SaveChangesAsync();
            }

            string host = HttpContext.Request.Host.Value ?? "localhost";
            return View("Details", new DetailsUrlViewModel(urlRecord, host));
        }

        /// <summary>
        /// Наша переадресация по коротким ссылкам
        /// </summary>
        /// <param name="code">Наша "короткая" ссылка</param>
        /// <returns></returns>
        /// <remarks>И тут я задумался - нужно ли мне влазить в данный контроллер или стоит вынести данный функционал ре-роутинга в иной сервис...</remarks>
        public async Task<IActionResult> RedirectToUrl(string code)
        {
            var record = await _context.UrlRecords.FirstOrDefaultAsync(u => u.Code == code);
            if (record == null)
                return NotFound();
            else
            {
                record.UsedCount++;
                _context.Update(record);
                await _context.SaveChangesAsync();
            }

            return Redirect(record.LongUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewData["Title"] = "CutURL ОШИБКИ";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
