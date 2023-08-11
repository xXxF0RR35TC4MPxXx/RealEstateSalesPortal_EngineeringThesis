using Inżynierka.Client.Pages.User;
using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.DTOs.UserPreferenceForm;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.ViewModels.Agency;
using Inżynierka.Shared.ViewModels.Offer.Create;
using Inżynierka.Shared.ViewModels.Offer.Filtering;
using Inżynierka.Shared.ViewModels.Offer.Update;
using Inżynierka.Shared.ViewModels.User;
using Inżynierka.Shared.ViewModels.UserEvents;
using Inżynierka.Shared.ViewModels.UserFavourites;
using Inżynierka.Shared.ViewModels.UserPreferenceForm;
using Inżynierka_Services.Listing;

namespace Inżynierka.Client.Logics
{
    public interface IApiLogic
    {
        Task<string> LoginAsync(SignInViewModel login);
        Task<string> ConfirmEmailAsync(Guid guid);

        Task<bool> CheckIfRecoveryGuidExists(Guid guid);
        Task<string> ForgotPassword(string email);
        Task<string> ForgotPassword2(RecoverPasswordForm2ViewModel model);
        Task<string> RegisterAsync(NewUserViewModel register);
        Task<(string Message, UserDTO? UserProfile)> UserProfileAsync();
        Task<Coordinates?> GetCoordinates(string address);
        Task<string> LogoutAsync();
        public Task<(bool, string)> RemoveAgentFromAgency(int agentId);

        
        public Task<UserOfferListing?> GetUserProfile(UserPageFilteringViewModel model, int id);
        public Task<UserFavouriteListing?> GetFavourites(UserFavouritesFilterViewModel model);
        public Task<string> UpdateUser(UserUpdateViewModel model);
        public Task<string> UpdateEvent(UserEventUpdateViewModel model, int id);
        public Task<IEnumerable<ReadEventDTO>?> GetUserEvents();
        public Task<UserEventUpdateViewModel?> GetEventById(int id);
        public Task<string> CreateUserEvent(UserEventCreateViewModel model);
        public Task<FormPageListing?> GetFormDataForReply(int id);
        public Task<string> SendReply(ReplyToSuggestionsViewModel model, int formId, Guid guid);

        public Task<string> ChangeOfferVisibility(int offerId, string restrictText);
        public Task<bool> CheckVisibility(int offerId);
        public Task<(IList<OfferVisibleForUserDTO>?, string)> GetAcceptedUsers(int offerId);

        #region Agency
        public Task<AgencyOfferListing?> GetAgencyProfile(AgencyPageFilteringViewModel model, int id);
        public Task<string?> UpdateAgency(AgencyUpdateViewModel model, int agencyId);
        public Task<AgentsListing?> GetAgents(int agencyId);
        public Task<string> LeaveAgency();
        public Task<string> DeleteAgency(int agencyId);
        public Task<string> CreateAgency(AgencyCreateViewModel model);
        public Task<string> JoinAgencyByCode(string invitationGuid);
        #endregion

        #region Forms
        public Task<string> CreatePreferenceForm(UserPreferenceFormCreateViewModel model);
        public Task<IList<UserPreferenceFormThumbnailDTO>?> GetForms();
        public Task<FormPageListing?> GetFormData(int id);
        public Task<string> DeleteFormSuggestion(int Id, int UserId);
        public Task<string> AddFormSuggestion(int OfferId, int FormId);
        public Task<string> SendSuggestion(int FormId);

        #endregion

        public Task<ReadOfferDTO?> GetOffer(int offerId, string type);

        #region Offer Create
        public Task<string> CreateApartmentRentOffer(ApartmentRentOfferCreateViewModel model);
        public Task<string> CreateApartmentSaleOffer(ApartmentSaleOfferCreateViewModel model);
        public Task<string> CreateHouseRentOffer(HouseRentOfferCreateViewModel model);
        public Task<string> CreateHouseSaleOffer(HouseSaleOfferCreateViewModel model);
        public Task<string> CreateHallRentOffer(HallRentOfferCreateViewModel model);
        public Task<string> CreateHallSaleOffer(HallSaleOfferCreateViewModel model);
        public Task<string> CreateGarageRentOffer(GarageRentOfferCreateViewModel model);
        public Task<string> CreateGarageSaleOffer(GarageSaleOfferCreateViewModel model);
        public Task<string> CreatePremisesRentOffer(BusinessPremisesRentOfferCreateViewModel model);
        public Task<string> CreatePremisesSaleOffer(BusinessPremisesSaleOfferCreateViewModel model);
        public Task<string> CreateRoomRentOffer(RoomRentingOfferCreateViewModel model);
        public Task<string> CreatePlotOffer(PlotOfferCreateViewModel model);
        #endregion

        #region Offer Update
        public Task<string> UpdateRoomRentOffer(RoomRentingOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdatePlotOffer(PlotOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateApartmentRentOffer(ApartmentRentOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateApartmentSaleOffer(ApartmentSaleOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdatePremisesRentOffer(BusinessPremisesRentOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdatePremisesSaleOffer(BusinessPremisesSaleOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateGarageRentOffer(GarageRentOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateGarageSaleOffer(GarageSaleOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateHallRentOffer(HallRentOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateHallSaleOffer(HallSaleOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateHouseRentOffer(HouseRentOfferUpdateViewModel model, int OfferId);
        public Task<string> UpdateHouseSaleOffer(HouseSaleOfferUpdateViewModel model, int OfferId);


        #endregion
    }
}
