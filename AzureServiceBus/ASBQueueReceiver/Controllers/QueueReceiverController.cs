using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASBQueueReceiver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueReceiverController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
