using API.Core.Domain.DTO.Note;
using API.Core.Domain.Entities;
using AutoMapper;

namespace API.Core.Domain.Mapping;

public class MappingProfile : Profile {
    public MappingProfile() {

        CreateMap<Note, NoteDto>();
        /*
        // User
        CreateMap<NewUserDto, ApplicationUser>();
        CreateMap<OptionalPasswordNewUserDto, ApplicationUser>();
        CreateMap<UpdateUserDto, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<PaginatedList<ApplicationUser>, PaginatedList<UserDto>>()
            .ConvertUsing<PagedListConverter<ApplicationUser, UserDto>>();

        */
    }
}
