using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using YeahBuddy.Application.Services.AutoMapper;
using YeahBuddy.Application.Services.Cryptography;
using YeahBuddy.Application.UseCases.User.Register;

namespace YeahBuddy.Application;

public static class DependenceInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddPasswordHasher(services);
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option =>
            new MapperConfiguration(options => { options.AddProfile(new AutoMapping()); }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }

    private static void AddPasswordHasher(IServiceCollection services)
    {
        services.AddScoped(options => new PasswordHasher());
    }
}