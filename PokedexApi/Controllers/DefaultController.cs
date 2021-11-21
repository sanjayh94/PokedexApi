using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokedexApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Basic fault healthcheck page. Returns simple 200 when the endpoint is called
        /// </summary>
        /// <returns>HTTP Status code 200</returns>
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Default Route...");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Front page under construction ;)");
            return StatusCode(200, sb.ToString());
        }
    }
}
