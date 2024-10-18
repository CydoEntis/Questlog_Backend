using AutoMapper;
using Questlog.Application.Common.DTOs.Campaign.Requests;
using Questlog.Application.Common.DTOs.Campaign.Responses;
using Questlog.Application.Common.DTOs.Guild.Requests;
using Questlog.Application.Common.DTOs.Guild.Responses;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class CampaignMappingProfile : Profile
{
    public CampaignMappingProfile()
    {
        CreateMap<Campaign, CreateCampaignResponseDto>().ReverseMap();
        CreateMap<Campaign, GetCampaignResponseDto>()
            .ForMember(dest => dest.NumberOfMembers, opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Leader")
                .Select(m => m.User.Id)
                .FirstOrDefault()))
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Leader")
                .Select(m => m.User.DisplayName)
                .FirstOrDefault()));

        // .ForMember(dest => dest.Quests, opt => opt.MapFrom(src => src.Quests.Count)); 

        CreateMap<Campaign, CreateCampaignRequestDto>().ReverseMap();
    }
}