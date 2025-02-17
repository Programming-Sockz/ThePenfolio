using Microsoft.AspNetCore.Mvc;

namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public async Task<string> Get()
        {
            await Task.Delay(1);
            return "Hello from the server!";
        }
    }
}
