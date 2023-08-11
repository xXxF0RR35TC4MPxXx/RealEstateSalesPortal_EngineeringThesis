using AutoMapper;
using Inżynierka.Shared.ViewModels.User;
using Inżynierka.Shared.ViewModels.UserEvents;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;

namespace Inżynierka.Server.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserUpdateViewModel, UserUpdateDTO>().ForMember(s => s.Avatar, opt => opt.Ignore());
            CreateMap<UserEventCreateViewModel, UserEventCreateDTO>();
            CreateMap<UserEventUpdateViewModel, UserEventUpdateDTO>().ForMember(s=>s.EventCompletionStatus, opt=>opt.Ignore());
            CreateMap<UserEvent, ReadEventDTO>();
            CreateMap<UserEventCreateDTO, UserEvent>();
            
        }
    }
}
