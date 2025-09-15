namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Presentation.GraphQL.Models;
    public class OfertMappingProfile : Profile
    {
        public OfertMappingProfile()
        {
            CreateMap<OfertDto, OfertModel>();
        }
    }
}