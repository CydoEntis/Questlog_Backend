using AutoMapper;
using Questlog.Application.Common.DTOs.User;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
    }
}