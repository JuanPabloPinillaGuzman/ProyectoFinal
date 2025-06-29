using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiProject.Controllers
{
    [Route("[controller]")]
    public class DetailsDiagnosticController : Controller
    {
        private readonly ILogger<DetailsDiagnosticController> _logger;

        public DetailsDiagnosticController(ILogger<DetailsDiagnosticController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}