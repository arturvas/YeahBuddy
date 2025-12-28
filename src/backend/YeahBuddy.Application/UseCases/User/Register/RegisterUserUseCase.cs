using AutoMapper;
using YeahBuddy.Application.Services.Cryptography;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;
using YeahBuddy.Domain.Repositories;
using YeahBuddy.Domain.Repositories.User;
using YeahBuddy.Exceptions;
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
        await ValidateRequest(request);

        var user = mapper.Map<Domain.Entities.User>(request);

        user.Password = passwordHashing.Encrypt(request.Password);

        await userWriteOnlyRepository.Add(user);

        await unitOfWork.CommitAsync();

        return new ResponseRegisterUserJson
        {
            Name = request.Name
        };
    }

    private async Task ValidateRequest(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);
        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExist)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourcesMessagesException.EMAIL_ALREADY_REGISTERED));
        
        if (result.IsValid) return;

        var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}