using AutoMapper;

namespace Tragate.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings(){
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToModelMappingProfile());
                cfg.AddProfile(new ModelToDomainMappingProfile());
                cfg.AddProfile(new CommandToEntityMappingProfile());
                cfg.AddProfile(new DomainToDomainMappingProfile());
                cfg.AddProfile(new DomainToEventMappingProfile());
            });
        }
    }
}