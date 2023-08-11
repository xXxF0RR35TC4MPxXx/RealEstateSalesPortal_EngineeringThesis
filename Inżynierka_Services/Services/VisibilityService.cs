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
using Inżynierka_Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class VisibilityService
    {
        private readonly ILogger<VisibilityService> _logger;
        private readonly IOfferRepository _offerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVisibilityRepository _visibilityRepository;

        public VisibilityService(ILogger<VisibilityService> logger, IOfferRepository offerRepository, IVisibilityRepository visibilityRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _offerRepository = offerRepository;
            _visibilityRepository = visibilityRepository;
            _userRepository = userRepository;
        }

        public bool CheckVisibility(int offerId, out string _errorMessage)
        {
            _errorMessage = "";

            var offerType = _offerRepository.GetTypeOfOffer(offerId);
            if (offerType == null)
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return false;
            }


            Offer? offer = _offerRepository.GetOffer(offerId, offerType);
            if (offer == null) 
            { 
                _errorMessage = ErrorMessageHelper.NoOffer; 
                return false; 
            }

            if(offer.VisibilityRestricted == false) { return true; }
            else
            {
                return false;
            }

        }

        public IList<OfferVisibleForUserDTO>? GetAcceptedUsers(int userId, int offerId, out string _errorMessage)
        {
            _errorMessage = "";
            User? user = _userRepository.GetById(userId);
            if(user == null) 
            {
                _errorMessage = ErrorMessageHelper.NoUser;
                return null;
            }

            var offerType = _offerRepository.GetTypeOfOffer(offerId);
            if(offerType == null) 
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return null;
            }

            Offer? offer = _offerRepository.GetOffer(offerId, offerType);
            if(offer == null) 
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return null;
            }

            if(offer.SellerId!=userId)
            {
                _errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer;
                return null;
            }

            IQueryable<OfferVisibleForUser>? acceptedUsers = _visibilityRepository.GetAcceptedUsers(offerId);
            if (acceptedUsers == null) { return null; }

            IList<OfferVisibleForUserDTO> result = acceptedUsers.Select(x => new OfferVisibleForUserDTO() 
            {
                userId = x.UserId,
                offerId = x.OfferId,
                userEmail = _userRepository.GetUserEmail(x.UserId)
            }).ToList();
            return result;

        }


        public bool CheckIfEligible(int offerId, int userId)
        {
            User? user = _userRepository.GetById(userId);
            if (user == null) { return false; }

            string? type = _offerRepository.GetTypeOfOffer(offerId);
            if (type == null) { return false; }

            Offer? offer = _offerRepository.GetOffer(offerId, type);
            if (offer == null) { return false; }

            return _visibilityRepository.CheckVisibility(offerId, userId);
        } 

        public bool ChangeVisibility(int userId, int offerId, bool restrict, out string _errorMessage)
        {
            _errorMessage = "";

            User? user = _userRepository.GetById(userId);
            if (user == null) { _errorMessage = ErrorMessageHelper.NoUser;  return false; }

            string? type = _offerRepository.GetTypeOfOffer(offerId);
            if (type == null) { _errorMessage = ErrorMessageHelper.NoOffer; return false; }

            Offer? offer = _offerRepository.GetOffer(offerId, type);
            if (offer == null) { _errorMessage = ErrorMessageHelper.NoOffer; return false; }

            if (offer.SellerId != userId) { _errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer; return false; }

            try
            {
                offer.VisibilityRestricted = restrict;
                _offerRepository.UpdateAndSaveChanges(offer);
            }
            catch(Exception ex)
            {
                _errorMessage = "Błąd";
                return false;
            }
            return true;
        }

        public bool AddUserToDetails(int ownerId, int offerId, string userEmail, out string _errorMessage)
        {
            _errorMessage = "";

            User? user = _userRepository.GetUserByEmail(userEmail);
            if (user == null) { _errorMessage = ErrorMessageHelper.NoUser; return false; }

            string? type = _offerRepository.GetTypeOfOffer(offerId);
            if (type == null) { _errorMessage = ErrorMessageHelper.NoOffer; return false; }

            Offer? offer = _offerRepository.GetOffer(offerId, type);
            if (offer == null) { _errorMessage = ErrorMessageHelper.NoOffer; return false; }

            if (offer.SellerId != ownerId) { _errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer; return false; }

            if(_visibilityRepository.CheckVisibility(offerId, user.Id))
            {
                _errorMessage = "Użytkownik już może wyświetlić ofertę";
                return false;
            }

            try
            {
                OfferVisibleForUser visibility = new()
                {     
                    UserId = user.Id,
                    OfferId = offerId
                };

                _visibilityRepository.AddAndSaveChanges(visibility);
            }
            catch (Exception ex)
            {
                _errorMessage = "Błąd";
                return false;
            }
            return true;
        }

        public bool RemoveUserFromDetails(int ownerId, int offerId, int userId, out string _errorMessage)
        {
            _errorMessage = "";

            User? user = _userRepository.GetById(userId);
            if (user == null) { _errorMessage = ErrorMessageHelper.NoUser; return false; }

            string? type = _offerRepository.GetTypeOfOffer(offerId);
            if (type == null) { _errorMessage = ErrorMessageHelper.NoOffer; return false; }

            Offer? offer = _offerRepository.GetOffer(offerId, type);
            if (offer == null) { _errorMessage = ErrorMessageHelper.NoOffer; return false; }

            if (offer.SellerId != ownerId) { _errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer; return false; }

            OfferVisibleForUser? visibility = _visibilityRepository.GetVisibility(userId, offerId);
            if (visibility == null)
            {
                _errorMessage = "Ten użytkownik nie mógł już wyświetlić tej oferty";
                return false;
            }

            try
            {
                _visibilityRepository.Remove(visibility);
                _visibilityRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                _errorMessage = "Błąd";
                return false;
            }
            return true;
        }
    }
}
