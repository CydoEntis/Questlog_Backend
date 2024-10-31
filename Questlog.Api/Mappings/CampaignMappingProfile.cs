using AutoMapper;
using Questlog.Application.Common.DTOs.Campaign;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class CampaignMappingProfile : Profile
{
    public CampaignMappingProfile()
    {
        CreateMap<Campaign, CampaignDto>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Owner")
                .Select(m => m.User.Id)
                .FirstOrDefault()))
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Members
                .Where(m => m.Role == "Owner")
                .Select(m => m.User.DisplayName)
                .FirstOrDefault()));


        CreateMap<Campaign, CreateCampaignDto>().ReverseMap();
        CreateMap<Campaign, UpdateCampaignDto>().ReverseMap();
    }
}