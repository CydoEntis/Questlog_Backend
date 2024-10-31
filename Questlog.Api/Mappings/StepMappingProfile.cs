using AutoMapper;
using Questlog.Application.Common.DTOs.Step;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class StepMappingProfile : Profile
{
    public StepMappingProfile()
    {
        CreateMap<Step, CreateStepDto>().ReverseMap();
        CreateMap<Step, UpdateStepDto>().ReverseMap();
        CreateMap<Step, StepDto>().ReverseMap();
    }
}