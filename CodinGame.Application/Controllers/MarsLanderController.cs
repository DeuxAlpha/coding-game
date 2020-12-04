using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CodinGame.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarsLanderController : Controller
    {
        [HttpGet("maps")]
        public async Task<IActionResult> GetMaps()
        {
            return BadRequest("");
        }
    }
}