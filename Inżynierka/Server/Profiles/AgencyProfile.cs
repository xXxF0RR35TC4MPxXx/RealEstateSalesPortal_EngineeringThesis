using AutoMapper;
using Inżynierka.Shared.ViewModels.Agency;
using Inżynierka.Shared.ViewModels.Offer.Filtering;
using Inżynierka.Shared.DTOs.Agency;
using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka.Shared.DTOs.Offers.Filtering;
using Inżynierka.Shared.Entities;

namespace Inżynierka.Server.Profiles
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            CreateMap<AgencyCreateViewModel, AgencyCreateDTO>();
            CreateMap<AgencyCreateDTO, Agency>();
            CreateMap<AgencyUpdateViewModel, AgencyUpdateDTO>();
            CreateMap<AgencyDeleteViewModel, AgencyDeleteDTO>();
            CreateMap<AgencyPageFilteringViewModel, AgencyOffersFilteringDTO>();
        }
    }
}
