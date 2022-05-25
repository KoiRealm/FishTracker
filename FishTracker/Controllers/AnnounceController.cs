using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FishTracker.Controllers
{
    [Route("api/announce")]
    [ApiController]
    public class AnnounceController : ControllerBase
    {
        [HttpGet("test")]
        public ActionResult Get()
        {
            return Ok("test");
        }
    }
}
