using AutoMapper;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<ApplicationUser, LoginDto>().ReverseMap();
    }
}

