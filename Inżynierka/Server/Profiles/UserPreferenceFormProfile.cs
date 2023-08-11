using AutoMapper;
using Inżynierka.Shared.ViewModels.UserPreferenceForm;
using Inżynierka.Shared.DTOs.UserPreferenceForm;
using Inżynierka.Shared.Entities;
using Inżynierka_Services.Listing;

namespace Inżynierka.Server.Profiles
{
    public class UserPreferenceFormProfile : Profile
    {
        public UserPreferenceFormProfile()
        {
            CreateMap<UserPreferenceFormCreateViewModel, UserPreferenceFormCreateDTO>();
            CreateMap<UserPreferenceFormCreateDTO, UserPreferenceForm>();
            CreateMap<UserPreferenceForm, UserPreferenceFormThumbnailDTO>();
            CreateMap<UserPreferenceForm, FormPageListing>();

        }
    }
}
