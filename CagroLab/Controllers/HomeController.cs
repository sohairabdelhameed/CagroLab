using CagroLab.Context;
using CagroLab.Models;
using CagroLab.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CagroLab.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly CagroLabDbContext _dbContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(CagroLabDbContext context , ILogger<HomeController> logger)
        {
            _dbContext = context;
            _logger = logger;
                
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
