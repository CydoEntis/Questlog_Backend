using AutoMapper;
using Questlog.Application.Common.DTOs.ApplicationUser.Response;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
            CreateMap<GetApplicationUserResponseDTO, ApplicationUser>().ReverseMap();
        }
    }
}