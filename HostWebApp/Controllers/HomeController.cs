using Microsoft.AspNetCore.Mvc;

namespace HostWebApp.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAsync() => Ok("Hello from HostWebApp");
    }
}
