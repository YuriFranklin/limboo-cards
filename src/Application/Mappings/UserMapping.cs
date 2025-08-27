namespace LimbooCards.Application.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<CreateUserDto, User>()
        .ConstructUsing(dto => new User(dto.Id ?? Guid.NewGuid(), dto.FullName, dto.Email));
        }
    }

}