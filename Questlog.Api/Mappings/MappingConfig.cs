using AutoMapper;

namespace Questlog.Api.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserMappingProfile());
                cfg.AddProfile(new CharacterMappingProfile());
                cfg.AddProfile(new GuildMappingProfile());
                cfg.AddProfile(new GuildMemberMappingProfile());
                cfg.AddProfile(new PartyMappingProfile());
                cfg.AddProfile(new PartyMemberMappingProfile());

                cfg.AddProfile(new QuestMappingProfile());
            });
        }

    }
}
