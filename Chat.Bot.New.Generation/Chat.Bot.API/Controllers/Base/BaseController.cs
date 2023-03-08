using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.API.Controllers.Base
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected new IActionResult Response(object result = null)
        {
            return Ok(new
            {
                success = true,
                data = result
            });
        }

    }
}
