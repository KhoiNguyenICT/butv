using Microsoft.AspNetCore.Mvc;
using BUTV.Infrastructure;
using BUTV.Services.Movie;
using BUTV.Services.Media;
using BUTV.Services.Catalog;
using Microsoft.Extensions.Caching.Memory;
using System;
using ImageMagick;
using static BUTV.Startup;
using System.IO;

namespace BUTV.Controllers
{
    public class HomeController : Controller
    {
        
        public HomeController()
        {
           
        }        
        public IActionResult Index()
        {
          
            return View();
        }        
      
        public IActionResult news()
        {
            return View();
        }
        public IActionResult moviedetail()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
