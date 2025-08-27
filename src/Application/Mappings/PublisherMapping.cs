namespace LimbooCards.Application.Mappings {
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Application.DTOs;

    public class PublisherMappingProfile : Profile
    {
        public PublisherMappingProfile()
        {
            CreateMap<Publisher, PublisherDto>();
            CreateMap<PublisherDto, Publisher>();
        }
    }
}