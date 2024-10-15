using AutoMapper;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class PartyMappingProfile : Profile
{
    public PartyMappingProfile()
    {
        CreateMap<CreatePartyRequestDTO, Party>().ReverseMap();
        CreateMap<CreatePartyResponseDTO, Party>().ReverseMap();
    }
}