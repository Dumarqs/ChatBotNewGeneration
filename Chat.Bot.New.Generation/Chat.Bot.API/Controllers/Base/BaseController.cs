using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.API.Controllers.Base
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
    }
}
