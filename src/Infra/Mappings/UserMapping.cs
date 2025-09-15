namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Infra.DTOs;
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserAutomateDto, User>()
            .ForCtorParam("id", opt => opt.MapFrom(dto =>
                string.IsNullOrWhiteSpace(dto.ID) ? Guid.Empty : Guid.Parse(dto.ID)
            ))
            .ForCtorParam("fullName", opt => opt.MapFrom(dto => dto.FULLNAME ?? string.Empty))
            .ForCtorParam("email", opt => opt.MapFrom(dto => dto.EMAIL));
        }
    }
}