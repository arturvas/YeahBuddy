using AutoMapper;
using YeahBuddy.Application.Services.Cryptography;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;
using YeahBuddy.Domain.Repositories;
using YeahBuddy.Domain.Repositories.User;
using YeahBuddy.Exceptions.ExceptionsBase;

namespace YeahBuddy.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserReadOnlyRepository userReadOnlyRepository,
    IUserWriteOnlyRepository userWriteOnlyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    PasswordHasher passwordHashing)
    : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        ValidateRequest(request);

        var user = mapper.Map<Domain.Entities.User>(request);

        user.Password = passwordHashing.Encrypt(request.Password);

        await userWriteOnlyRepository.Add(user);

        await unitOfWork.CommitAsync();

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