using CodinGame.MarsLander.Storage;
using Microsoft.AspNetCore.Mvc;

namespace CodinGame.Application.Controllers
{
    [ApiController]
    [Route("mars-lander")]
    public class MarsLanderController : Controller
    {
        [HttpGet("maps")]
        public IActionResult GetMaps()
        {
            return Ok(Maps.Get());
        }
    }
}