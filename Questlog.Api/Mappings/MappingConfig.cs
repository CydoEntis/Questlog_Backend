﻿using AutoMapper;

namespace Questlog.Api.Mappings;

public class MappingConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(cfg =>

        {
            cfg.AddProfile(new UserMappingProfile());
            cfg.AddProfile(new AuthMappingProfile());
            cfg.AddProfile(new PartyMappingProfile());
            cfg.AddProfile(new MemberMappingProfile());
            cfg.AddProfile(new QuestMappingProfile());
            cfg.AddProfile(new StepMappingProfile());
            cfg.AddProfile(new MemberQuestMappingProfile());
            cfg.AddProfile(new AvatarMappingProfile());
        });
    }
}