namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Presentation.GraphQL.Models;
    public class SubjectMappingProfile : Profile
    {
        public SubjectMappingProfile()
        {
            CreateMap<SubjectDto, SubjectModel>();
        }
    }
}