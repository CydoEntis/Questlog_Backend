﻿using AutoMapper;
using Questlog.Application.Common;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.ApplicationUser.Response;
using Questlog.Domain.Entities;

namespace Questlog.Api.Mappings;

    public class QueryParamMappingProfile : Profile
    {
        public QueryParamMappingProfile()
        {
            CreateMap<QueryParamsDto, CampaignQueryOptions>().ReverseMap();
        }
    }