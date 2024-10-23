using AutoMapper;
using Questlog.Application.Common.DTOs.Task.Request;
using Questlog.Application.Common.DTOs.Task.Response;
using Questlog.Domain.Entities;
using Task = Questlog.Domain.Entities.Task;

namespace Questlog.Api.Mappings;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<Task, CreateTaskRequestDto>().ReverseMap();
        CreateMap<Task, CreateTaskResponseDto>().ReverseMap();
        CreateMap<Task, GetTaskResponseDto>().ReverseMap();
    }
}