using Microsoft.AspNetCore.Mvc;
using YeahBuddy.Application.UseCases.User.Register;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;

namespace YeahBuddy.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] RequestRegisterUserJson request)
        {
            var result = await useCase.Execute(request);

            return Created(string.Empty, result);
        }
    }
}