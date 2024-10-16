using AutoMapper;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class PartyMappingProfile : Profile
{
    public PartyMappingProfile()
    {
        CreateMap<Party, GetPartyResponseDto>()
            .ForMember(dest => dest.NumberOfMembers, opt => opt.MapFrom(src => src.PartyMembers.Count))
            .ForMember(dest => dest.PartyLeader, opt => opt.MapFrom(src => src.PartyMembers
                .Where(m => m.Role == "Leader")
                .Select(m => m.GuildMember.User.DisplayName)
                .FirstOrDefault()));
        // .ForMember(dest => dest.Quests, opt => opt.MapFrom(src => src.Quests.Count)); 
        CreateMap<CreatePartyRequestDto, Party>().ReverseMap();
        CreateMap<CreatePartyResponseDTO, Party>().ReverseMap();
    }
}