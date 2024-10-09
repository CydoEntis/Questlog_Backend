using AutoMapper;
using Questlog.Application.Common.DTOs.Character;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class CharacterMappingProfile : Profile
    {
        public CharacterMappingProfile()
        {
            CreateMap<Character, CharacterResponseDTO>().ReverseMap();
            CreateMap<Character, CharacterRequestDTO>().ReverseMap();
        }
    }
}