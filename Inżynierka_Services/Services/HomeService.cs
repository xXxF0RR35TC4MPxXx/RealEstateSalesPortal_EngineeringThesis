using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class HomeService
    {
        private readonly ILogger<HomeService> _logger;
        private readonly IOfferRepository _offerRepository;

        public HomeService(ILogger<HomeService> logger, IOfferRepository offerRepository)
        {
            _logger = logger;
            _offerRepository = offerRepository;
        }

        public IEnumerable<HomepageOffersDTO>? GetHomepageOffersList(string path)
        {
            List<Offer>? offers = _offerRepository.GetHomepageOffers();

            IEnumerable<HomepageOffersDTO> homepageOffersDTOcollection = offers.Select(offer => new HomepageOffersDTO
            {
                Id = offer.Id,
                OfferTitle = offer.OfferTitle,
                OfferCategory = offer.EstateType,
                ForSaleOrForRent = offer.OfferType,
                Voivodeship = offer.Voivodeship,
                City = offer.City,
                Area = offer.Area,
                RoomCount = offer.RoomCount,
                Price = offer.Price,
                Photo = File.Exists(Path.Combine(path, offer.Id.ToString(), "1.jpg"))?File.ReadAllBytes(Path.Combine(path, offer.Id.ToString(), "1.jpg")) :null
            }); ;


            return homepageOffersDTOcollection;
        }
    }
}
