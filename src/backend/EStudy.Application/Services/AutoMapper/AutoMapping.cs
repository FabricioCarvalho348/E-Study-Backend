using AutoMapper;
using EStudy.Communication.Requests.Events;
using EStudy.Communication.Requests.UserTasks;
using EStudy.Communication.Requests.Users;
using EStudy.Communication.Responses.Events;
using EStudy.Communication.Responses.UserTasks;
using EStudy.Communication.Responses.Users;
using Sqids;

namespace EStudy.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;

    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;

        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestCreateUserTaskJson, Domain.Entities.UserTask>();
        CreateMap<RequestCreateEventJson, Domain.Entities.Event>();
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();

        CreateMap<Domain.Entities.UserTask, ResponseUserTaskJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<Domain.Entities.Event, ResponseEventJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}