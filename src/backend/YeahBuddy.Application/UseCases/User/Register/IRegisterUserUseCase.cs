using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;

namespace YeahBuddy.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request);
}