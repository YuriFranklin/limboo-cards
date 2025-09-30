
namespace LimbooCards.Application.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Application.DTOs;

    public class CardMappingProfile : Profile
    {
        public CardMappingProfile()
        {
            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<ChecklistItem, ChecklistItemDto>().ReverseMap();
        }
    }
}