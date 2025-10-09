using AutoMapper;
using YeahBuddy.Communication.Requests;
using YeahBuddy.Domain.Entities;

namespace YeahBuddy.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(
                dest => dest.Password,
                opt => opt.Ignore()
            );
    }
}