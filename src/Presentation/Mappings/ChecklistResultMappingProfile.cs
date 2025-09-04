namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Events;
    using LimbooCards.Presentation.GraphQL.Models;
    public class ChecklistResultMappingProfile : Profile
    {
        public ChecklistResultMappingProfile()
        {
            CreateMap<ChecklistResultDto, ChecklistResultModel>();
            CreateMap<ChecklistItemCompleted, ChecklistItemCompletedModel>();
            CreateMap<ChecklistItemNotFounded, ChecklistItemNotFoundedModel>();
        }
    }
}