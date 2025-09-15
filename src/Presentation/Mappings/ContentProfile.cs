namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Presentation.GraphQL.Models;
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<ContentDto, ContentModel>();
        }
    }
}