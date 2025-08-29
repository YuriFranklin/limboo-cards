namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Infra.DTOs;

    public class ChecklistItemMappingProfile : Profile
    {
        public ChecklistItemMappingProfile()
        {
            CreateMap<ChecklistItemAutomateDto, ChecklistItem>()
            .ForCtorParam("title", opt => opt.MapFrom(src => src.value.title))
            .ForCtorParam("isChecked", opt => opt.MapFrom(src => src.value.isChecked))
            .ForCtorParam("orderHint", opt => opt.MapFrom(src => src.value.orderHint))
            .ForCtorParam("updatedAt", opt => opt.MapFrom(src => src.value.lastModifiedDateTime))
            .ForCtorParam("updatedBy", opt => opt.MapFrom(src => src.value.lastModifiedBy.user.id));
        }
    }
}