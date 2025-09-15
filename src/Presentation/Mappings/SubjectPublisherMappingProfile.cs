namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Presentation.GraphQL.Models;
    public class SubjectPublisherMappingProfile : Profile
    {
        public SubjectPublisherMappingProfile()
        {
            CreateMap<SubjectPublisherDto, SubjectPublisherModel>();
        }
    }
}