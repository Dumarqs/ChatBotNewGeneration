using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.API.Controllers.Base
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected new IActionResult Response(object result = null)
        {
            return Ok(result);
        }

    }
}
