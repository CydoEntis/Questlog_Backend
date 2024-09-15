using AutoMapper;
using Questlog.Application.Common.DTOs;
using Questlog.Domain.Entities;

namespace Questlog.Api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();
        }
    }
}
