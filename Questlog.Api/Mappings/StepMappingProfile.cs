using AutoMapper;
using Questlog.Application.Common.DTOs.Task.Request;
using Questlog.Application.Common.DTOs.Task.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

public class StepMappingProfile : Profile
{
    public StepMappingProfile()
    {
        CreateMap<Step, CreateStepRequestDto>().ReverseMap();
        CreateMap<Step, CreateStepResponseDto>().ReverseMap();
        CreateMap<Step, GetStepResponseDto>().ReverseMap();
    }
}