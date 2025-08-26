namespace LimbooCards.Application.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using System.Linq;
    using System.Collections.Generic;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Subject, SubjectDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.CoOwners, opt => opt.MapFrom(src => src.CoOwners));

            CreateMap<SubjectDto, Subject>();

            CreateMap<Ofert, OfertDto>().ReverseMap();

            CreateMap<Content, ContentDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<SubjectPublisher, SubjectPublisherDto>().ReverseMap();

            CreateMap<CreateSubjectDto, Subject>()
                .ForMember(dest => dest.Oferts, opt => opt.MapFrom(src =>
                    src.Oferts != null ? src.Oferts.Select(o => new Ofert(o.Project, o.Module)).ToList() : new List<Ofert>()))
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src =>
                    src.Contents != null ? src.Contents.Select(c => new Content(c.Name, c.ChecklistItemTitle, c.ContentStatus)).ToList() : null))
                .ForMember(dest => dest.Publishers, opt => opt.MapFrom(src =>
                    src.Publishers != null ? src.Publishers.Select(p => new SubjectPublisher(p.Name, p.IsCurrent, p.IsExpect)).ToList() : null));
        }
    }
}
