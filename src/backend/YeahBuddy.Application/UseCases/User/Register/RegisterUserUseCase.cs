using AutoMapper;
using YeahBuddy.Application.Services.AutoMapper;
using YeahBuddy.Application.Services.Cryptography;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;
using YeahBuddy.Domain.Repositories.User;
using YeahBuddy.Exceptions.ExceptionsBase;

namespace YeahBuddy.Application.UseCases.User.Register;

public class RegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        var passwordHashing = new PasswordHasher();
        var autoMapper = new MapperConfiguration(options => { options.AddProfile(new AutoMapping()); }).CreateMapper();

        ValidateRequest(request);

        var user = autoMapper.Map<Domain.Entities.User>(request);

        user.Password = passwordHashing.Encrypt(request.Password);

        await _userWriteOnlyRepository.Add(user);

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