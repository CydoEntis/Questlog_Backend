﻿using AutoMapper;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();

        }
    }
}