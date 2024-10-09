using AutoMapper;
using Questlog.Application.Common.DTOs.PartyMember;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class PartyMemberMappingProfile : Profile
    {
        public PartyMemberMappingProfile()
        {
            CreateMap<PartyMember, CreatePartyMemberResponseDTO>().ReverseMap();
        }
    }
}