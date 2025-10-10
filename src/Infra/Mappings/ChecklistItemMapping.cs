namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Infra.DTOs;
    using System; // Adicionado para Guid

    public class ChecklistItemMappingProfile : Profile
    {
        public ChecklistItemMappingProfile()
        {
            CreateMap<ChecklistItemAutomateDto, ChecklistItem>()
                .ForCtorParam("title", opt => opt.MapFrom(src => src.Value.Title))
                .ForCtorParam("isChecked", opt => opt.MapFrom(src => src.Value.IsChecked))
                .ForCtorParam("orderHint", opt => opt.MapFrom(src => src.Value.OrderHint))
                .ForCtorParam("updatedAt", opt => opt.MapFrom(src => src.Value.LastModifiedDateTime))
                .ForCtorParam("updatedBy", opt => opt.MapFrom(src => src.Value.LastModifiedBy.User.Id.ToString()));
        }
    }
}