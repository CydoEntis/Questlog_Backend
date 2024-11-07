using AutoMapper;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class PartyMappingProfile : Profile
{
    public PartyMappingProfile()
    {
        CreateMap<Party, PartyDto>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Owner")
                .Select(m => m.User.Id)
                .FirstOrDefault()))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Owner")
                .Select(m => m.User.DisplayName)
                .FirstOrDefault()));


        CreateMap<Party, CreatePartyDto>().ReverseMap();
        CreateMap<Party, UpdatePartyDto>().ReverseMap();
    }
}