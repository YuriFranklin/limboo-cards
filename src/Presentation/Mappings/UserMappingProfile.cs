namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Presentation.GraphQL.Models;
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, UserModel>();
        }
    }
}