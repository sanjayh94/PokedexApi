using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

namespace PokedexApi.Controllers
{
    /// <summary>
    /// Controller for Default ('/') routes. Currently just set up for healthchecks. Not necessary at this point to be honest.
    /// </summary>
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        #region PrivateVariables
        private readonly ILogger<DefaultController> _logger; 
        #endregion


        public DefaultController(ILogger<DefaultController> logger)
        {
            // Setting up logger using Dependency Injection
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
