using AutoMapper;
using YeahBuddy.Application.Services.AutoMapper;
using YeahBuddy.Application.Services.Cryptography;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;
using YeahBuddy.Exceptions.ExceptionsBase;

namespace YeahBuddy.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    public ResponseRegisterUserJson Execute(RequestRegisterUserJson request)
    {
        ValidateRequest(request);

        var autoMapper = new MapperConfiguration(options => { options.AddProfile(new AutoMapping()); }).CreateMapper();

        var user = autoMapper.Map<Domain.Entities.User>(request);

        var passwordHashing = new PasswordHasher();

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

        var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}