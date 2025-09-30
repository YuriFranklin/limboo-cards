namespace LimbooCards.Application.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>()
            .ConstructUsing(dto => new User(dto.Id ?? Guid.CreateVersion7().ToString(), dto.FullName, dto.Email));
        }
    }

}