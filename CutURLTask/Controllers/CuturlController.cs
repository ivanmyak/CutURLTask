using System.Diagnostics;
using CutURLTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace CutURLTask.Controllers
{
    public class CuturlController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "CutURL";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
