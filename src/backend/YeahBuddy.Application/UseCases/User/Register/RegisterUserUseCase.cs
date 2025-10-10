using AutoMapper;
using YeahBuddy.Application.Services.Cryptography;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Communication.Responses;
using YeahBuddy.Domain.Repositories.User;
using YeahBuddy.Exceptions.ExceptionsBase;

namespace YeahBuddy.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly PasswordHasher _passwordHashing;

    public RegisterUserUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository, IMapper mapper, PasswordHasher passwordHashing)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _mapper = mapper;
        _passwordHashing = passwordHashing;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        ValidateRequest(request);

        var user = _mapper.Map<Domain.Entities.User>(request);

        user.Password = _passwordHashing.Encrypt(request.Password);

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