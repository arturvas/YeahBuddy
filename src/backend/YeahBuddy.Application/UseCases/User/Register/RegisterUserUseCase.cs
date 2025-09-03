using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;
using YeahBuddy.Exceptions.ExceptionsBase;

namespace YeahBuddy.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    public ResponseRegisterUserJson Execute(RequestRegisterUserJson request)
    {
        ValidateRequest(request);

        // TODO mapear a request em uma entidade

        // TODO criptografar a senha

        // TODO salvar no banco de dados

        return new ResponseRegisterUserJson
        {
            Name = request.Name
        };
    }

    private static void ValidateRequest(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        if (result.IsValid) return;

        var errorMessages = string.Join("; ", result.Errors.Select(x => x.ErrorMessage));

        throw new YeahBuddyException();
    }
}