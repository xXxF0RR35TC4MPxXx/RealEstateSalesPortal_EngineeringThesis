using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.DTOs.Offers.Read;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.DTOs.UserPreferenceForm;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.ViewModels;
using Inżynierka.Shared.ViewModels.Agency;
using Inżynierka.Shared.ViewModels.Offer.Create;
using Inżynierka.Shared.ViewModels.Offer.Filtering;
using Inżynierka.Shared.ViewModels.Offer.Update;
using Inżynierka.Shared.ViewModels.User;
using Inżynierka.Shared.ViewModels.UserEvents;
using Inżynierka.Shared.ViewModels.UserFavourites;
using Inżynierka.Shared.ViewModels.UserPreferenceForm;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Inżynierka_Services.Listing;
using MatBlazor;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Inżynierka.Client.Logics
{
    public class ApiLogic : IApiLogic
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ApiLogic(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> CreatePreferenceForm(UserPreferenceFormCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("UserPreferenceController/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }


        public async Task<string> ChangeOfferVisibility(int offerId, string restrictText)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"VisibilityController/ChangeVisibility/{offerId}/{restrictText}");
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseViewModel?>();
                return responseContent.Value;
            }
        }

        public async Task<bool> CheckVisibility(int offerId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"VisibilityController/CheckVisibility/{offerId}");
            return !response.IsSuccessStatusCode;
            
        }

        public async Task<(IList<OfferVisibleForUserDTO>?, string)> GetAcceptedUsers(int offerId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"VisibilityController/AcceptedUsers/{offerId}");
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<IList<OfferVisibleForUserDTO>?>(), "");
            }
            else
            {
                return (null, await response.Content.ReadAsStringAsync());
            }

        }

        //await Http.GetAsync($"VisibilityController/ChangeVisibility/{offerId}/{restrictText}");

        public async Task<ReadOfferDTO?> GetOffer(int offerId, string type)
        {
            var Http = _httpClientFactory.CreateClient("API");
            if (type.Contains(EnumHelper.GetDescriptionFromEnum(EstateType.ROOM)))
            {
                return await Http.GetFromJsonAsync<RoomRentingReadOfferDTO?>($"OfferController/Get/{offerId}");
            }
            else if (type.Contains(EnumHelper.GetDescriptionFromEnum(EstateType.PLOT)))
            {
                return await Http.GetFromJsonAsync<PlotReadOfferDTO?>($"OfferController/Get/{offerId}");
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                return await Http.GetFromJsonAsync<HouseSaleReadOfferDTO?>($"OfferController/Get/{offerId}");
            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HOUSE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                return await Http.GetFromJsonAsync<HouseRentReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                return await Http.GetFromJsonAsync<HallRentReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.HALL) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                return await Http.GetFromJsonAsync<HallSaleReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                return await Http.GetFromJsonAsync<BusinessPremisesRentReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.PREMISES) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                return await Http.GetFromJsonAsync<BusinessPremisesSaleReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                return await Http.GetFromJsonAsync<GarageRentReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.GARAGE) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                return await Http.GetFromJsonAsync<GarageSaleReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.RENT))
            {
                return await Http.GetFromJsonAsync<ApartmentRentReadOfferDTO?>($"OfferController/Get/{offerId}");

            }
            else if (type == EnumHelper.GetDescriptionFromEnum(EstateType.APARTMENT) + EnumHelper.GetDescriptionFromEnum(OfferType.SALE))
            {
                return await Http.GetFromJsonAsync<ApartmentSaleReadOfferDTO?>($"OfferController/Get/{offerId}");
            }
            else return null;
        }

        public async Task<string> CreateUserEvent(UserEventCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("UserEventsController/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> JoinAgencyByCode(string invitationGuid)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"AgencyController/JoinAgencyByCode/{invitationGuid}");
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseViewModel?>();
                return responseContent.Value;
            }
        }

        public async Task<string> CreateAgency(AgencyCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("AgencyController/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> SendReply(ReplyToSuggestionsViewModel model, int formId, Guid guid)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"UserPreferenceController/ReplyToSuggestions/{formId}/{guid}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        #region Offer Create
        public async Task<string> CreateApartmentRentOffer(ApartmentRentOfferCreateViewModel model)
        {

            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Rent/Apartment/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreateApartmentSaleOffer(ApartmentSaleOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Sale/Apartment/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreateHouseRentOffer(HouseRentOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Rent/House/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreateHouseSaleOffer(HouseSaleOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Sale/House/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreateGarageRentOffer(GarageRentOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Rent/Garage/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreateGarageSaleOffer(GarageSaleOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Sale/Garage/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreateHallRentOffer(HallRentOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Rent/Hall/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreateHallSaleOffer(HallSaleOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Sale/Hall/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreatePremisesRentOffer(BusinessPremisesRentOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Rent/Premises/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreatePremisesSaleOffer(BusinessPremisesSaleOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Sale/Premises/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }


        public async Task<string> CreateRoomRentOffer(RoomRentingOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Rent/Room/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> CreatePlotOffer(PlotOfferCreateViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("OfferController/Plot/Create", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        #endregion

        public async Task<IEnumerable<ReadEventDTO>?> GetUserEvents()
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"UserEventsController/GetAll");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<ReadEventDTO>?>();
            }
            else
            {
                return null;
            }
        }

        public async Task<IList<UserPreferenceFormThumbnailDTO>?> GetForms()
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"UserPreferenceController/MyForms");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IList<UserPreferenceFormThumbnailDTO>?>();
            }
            else
            {
                return null;
            }
        }

        public async Task<string> LeaveAgency()
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"AgencyController/Leave/");
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseViewModel?>();
                return responseContent.Value;
            }
        }

        public async Task<string> DeleteAgency(int agencyId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"AgencyController/Delete/{agencyId}");
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseViewModel?>();
                return responseContent.Value;
            }
        }

        public async Task<string> DeleteFormSuggestion(int OfferId, int FormId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"UserPreferenceController/RemoveSuggestion/{FormId}/{OfferId}");
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseViewModel?>();
                return responseContent.Value;
            }
        }

        public async Task<string> AddFormSuggestion(int OfferId, int FormId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"UserPreferenceController/AddSuggestion/{FormId}/{OfferId}");
            if (response.IsSuccessStatusCode)
            {
                return "OK";
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseViewModel?>();
                return responseContent.Value;
            }
        }

        public async Task<string> SendSuggestion(int FormId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"UserPreferenceController/SendSuggestionEmail/{FormId}");
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseViewModel?>();
                return responseContent.Value;
            }
        }

        public async Task<FormPageListing?> GetFormData(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"UserPreferenceController/MyForms/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FormPageListing?>();
            }
            else
            {
                return null;
            }
        }

        public async Task<(bool,string)> RemoveAgentFromAgency(int agentId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"AgencyController/RemoveAgent/{agentId}");
            if (response.IsSuccessStatusCode)
            {
                return (true,"");
            }
            else
            {
                string responseMessage = await response.Content.ReadAsStringAsync();
                return (false, responseMessage);
            }
        }

        public async Task<string?> UpdateAgency(AgencyUpdateViewModel model, int agencyId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"AgencyController/Update/{agencyId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<FormPageListing?> GetFormDataForReply(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"UserPreferenceController/GetReplyData/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FormPageListing?>();
            }
            else
            {
                return null;
            }
        }


        public async Task<UserEventUpdateViewModel?> GetEventById(int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"User/Events/GetUpdateViewModel/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserEventUpdateViewModel?>();
            }
            else
            {
                return null;
            }
        }

        public async Task<string> UpdateEvent(UserEventUpdateViewModel model, int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"UserEventsController/Update/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> UpdateUser(UserUpdateViewModel model)
        {

            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("UserController/Update", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        

        public async Task<UserOfferListing?> GetUserProfile(UserPageFilteringViewModel model, int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"UserController/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserOfferListing?>();
            }
            else
            {
                return null;
            }
        }

        public async Task<AgencyOfferListing?> GetAgencyProfile(AgencyPageFilteringViewModel model, int id)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"AgencyController/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AgencyOfferListing?>();
            }
            else
            {
                return null;
            }
        }

        public async Task<AgentsListing?> GetAgents(int agencyId)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync($"AgencyController/{agencyId}/Agents/");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AgentsListing?>();
            }
            else
            {
                return null;
            }
        }

        public async Task<UserFavouriteListing?> GetFavourites(UserFavouritesFilterViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/User/GetFavourites/", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserFavouriteListing?>();
            }
            else
            {
                return null;
            }
        }

        #region Offer Update
        public async Task<string> UpdateRoomRentOffer(RoomRentingOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/Room/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdatePlotOffer(PlotOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/Plot/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateApartmentRentOffer(ApartmentRentOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/ApartmentRent/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateApartmentSaleOffer(ApartmentSaleOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/ApartmentSale/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdatePremisesRentOffer(BusinessPremisesRentOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/PremisesRent/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdatePremisesSaleOffer(BusinessPremisesSaleOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/PremisesSale/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateGarageRentOffer(GarageRentOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/GarageRent/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateGarageSaleOffer(GarageSaleOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/GarageSale/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateHallRentOffer(HallRentOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/HallRent/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateHallSaleOffer(HallSaleOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/HallSale/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateHouseRentOffer(HouseRentOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/HouseRent/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        public async Task<string> UpdateHouseSaleOffer(HouseSaleOfferUpdateViewModel model, int OfferId)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"OfferController/Update/HouseSale/{OfferId}", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }
        #endregion

        public async Task<string> ForgotPassword(string email)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("AuthController/PasswordRecoveryMail", email);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> ForgotPassword2(RecoverPasswordForm2ViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("AuthController/RecoverPassword", model);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<bool> CheckIfRecoveryGuidExists(Guid guid)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("AuthController/CheckIfRecoveryGuidExists", guid);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> ConfirmEmailAsync(Guid guid)
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("AuthController/ConfirmAccount", guid);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                return "failed";
            }
        }

        public async Task<string> LoginAsync(SignInViewModel login)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(login);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("AuthController/SignIn", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<string> RegisterAsync(NewUserViewModel register)
        {
            var client = _httpClientFactory.CreateClient("API");
            string payload = JsonSerializer.Serialize(register);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("AuthController/Register", content);
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            else
            {
                var responseJson = await response.Content.ReadFromJsonAsync<ResponseViewModel>();
                return responseJson.Value;
            }
        }

        public async Task<(string Message, UserDTO? UserProfile)> UserProfileAsync()
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync("AuthController/user-profile");
            if (response.IsSuccessStatusCode)
            {
                return ("Success", await response.Content.ReadFromJsonAsync<UserDTO>());
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return ("Unauthorized", null);
                }
                else
                {
                    return ("Failed", null);
                }
            }
        }

        public async Task<string> LogoutAsync()
        {
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.GetAsync("AuthController/LogOut");
            if (response.IsSuccessStatusCode)
            {
                return "Success";
            }
            return "Failed";
        }

        public async Task<Coordinates?> GetCoordinates(string address)
        {

            var client = _httpClientFactory.CreateClient("API");
            GetCoordsViewModel vm = new GetCoordsViewModel() { address = address };
            var response = await client.GetAsync($"OfferController/GetCoordinates/{vm}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Coordinates?>();
            }
            else
            {
                return null;
            }
        }
    }
}
