namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Presentation.GraphQL.Models;
    public class CardMappingProfile : Profile
    {
        public CardMappingProfile()
        {
            CreateMap<CardDto, CardModel>();
        }
    }
}