using Microsoft.AspNetCore.Mvc;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;

namespace YeahBuddy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponsesRegisterUserJson), StatusCodes.Status201Created)]
        public IActionResult Register(RequestRegisterUserJson request)
        {
            return Created();
        }
    }
}