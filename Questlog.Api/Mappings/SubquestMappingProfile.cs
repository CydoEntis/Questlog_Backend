using AutoMapper;
using Questlog.Application.Common.DTOs.Subquest.Request;
using Questlog.Application.Common.DTOs.Subquest.Response;
using Questlog.Domain.Entities;
using Task = Questlog.Domain.Entities.Task;

namespace Questlog.Api.Mappings;

public class SubquestMappingProfile : Profile
{
    public SubquestMappingProfile()
    {
        CreateMap<Task, CreateTaskRequestDto>().ReverseMap();
        CreateMap<Task, CreateTaskResponseDto>().ReverseMap();
        CreateMap<Task, GetTaskResponseDto>().ReverseMap();
    }
}