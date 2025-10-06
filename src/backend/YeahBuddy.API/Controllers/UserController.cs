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
        public IActionResult Register(RequestRegisterUserJson request)
        {
            var useCase = new RegisterUserUseCase();

            var result = useCase.Execute(request);

            return Created(string.Empty, result);
        }
    }
}