using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Oracon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        OraconContext cnx;

        public readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, OraconContext cnx, IConfiguration configuration)
        {
            _logger = logger;
            this.cnx = cnx;
        }

        public IActionResult Index()
        {
            var cursos = cnx.Cursos.Include(o => o.profesor).Include("etiquetas.categoria").ToList();
            ViewBag.Cursos = cursos;
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
