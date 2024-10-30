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
            .ForMember(dest => dest.TotalMembers, opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members.Take(5)))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Owner")
                .Select(m => m.User.Id)
                .FirstOrDefault()))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Owner")
                .Select(m => m.User.DisplayName)
                .FirstOrDefault()));

        // .ForMember(dest => dest.Quests, opt => opt.MapFrom(src => src.Quests.Count)); 

        CreateMap<Campaign, CreateCampaignRequestDto>().ReverseMap();
        CreateMap<Campaign, UpdateCampaignResponseDto>().ReverseMap();
        CreateMap<Campaign, UpdateCampaignRequestDto>().ReverseMap();
    }
}