using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using Inżynierka.Shared;
using Inżynierka.Shared.Repositories;
using Inżynierka.Shared.DTOs.UserPreferenceForm;
using System.Threading.Tasks.Dataflow;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;

namespace Inżynierka.Shared.Repositories
{
    [ScopedRegistrationWithInterface]
    public class UserPreferenceFormRepository : BaseRepository<UserPreferenceForm>, IUserPreferenceFormRepository
    {
        private readonly DataContext _dataContext;
        private readonly IOfferRepository _offerRepository;

        public UserPreferenceFormRepository(DataContext context, IOfferRepository offerRepository) : base(context)
        {
            _dataContext = context;
            _offerRepository = offerRepository;
        }

        public IEnumerable<UserPreferenceForm>? GetMyForms(int agentId)
        {
            return _dataContext.UserPreferenceForms.Where(x => x.AgentId == agentId);
        }

        public IEnumerable<SingleFormProposal>? GetFormsProposals(int formId)
        {
            return _dataContext.SingleFormProposals.Where(x => x.FormId == formId);
        }

        public UserPreferenceForm? GetForm(int formId)
        {
            return _dataContext.UserPreferenceForms
                .Include(x => x.Agent)
                .Include(x => x.SingleFormProposals)
                    .ThenInclude(p => p.Offer)
                .Single(x => x.Id == formId);
        }

        public List<int>? GetThreeAgentsForUserPreferenceForm(UserPreferenceFormCreateDTO dto, out string extraMessage)
        {
            //get all offers
            var allOffers = _offerRepository.GetAll().Where(x => x.SellerType == UserRoles.AGENCY.ToString() && x.DeletedDate == null 
                && x.OfferStatus != EnumHelper.GetDescriptionFromEnum(OfferStatus.ENDED));

            //get all offers owned by agents
            var matchingOffers = allOffers;

            //select by given criteria
            matchingOffers = (dto.City != null && dto.City != "") ? matchingOffers.Where(x=>x.City == dto.City) : matchingOffers;
            matchingOffers = (dto.OfferType != null && dto.OfferType != "" && dto.EstateType!=EnumHelper.GetDescriptionFromEnum(EstateType.ROOM)) 
                ? matchingOffers.Where(x=>x.OfferType == dto.OfferType) : matchingOffers;
            matchingOffers = (dto.EstateType != null && dto.EstateType != "") 
                ? matchingOffers.Where(x=>x.EstateType == dto.EstateType) : matchingOffers;
            matchingOffers = (dto.MinArea != null) ? matchingOffers.Where(x=>x.Area >= dto.MinArea) : matchingOffers;
            matchingOffers = (dto.MaxArea != null) ? matchingOffers.Where(x=>x.Area <= dto.MaxArea) : matchingOffers;
            matchingOffers = (dto.RoomCount != null) ? matchingOffers.Where(x=>x.RoomCount == dto.RoomCount) : matchingOffers;
            matchingOffers = (dto.MaxPrice != null) ? matchingOffers.Where(x=>x.Price <= dto.MaxPrice) : matchingOffers;

            //if not - get the ones meeting SOME of the criteria
            if (!matchingOffers.Any())
            {
                matchingOffers = allOffers;
                matchingOffers = (dto.City != null) ? matchingOffers.Where(x => x.City == dto.City) : matchingOffers;
                matchingOffers = (dto.OfferType != null) ? matchingOffers.Where(x => x.OfferType == dto.OfferType) : matchingOffers;
                matchingOffers = (dto.EstateType != null) ? matchingOffers.Where(x => x.EstateType == dto.EstateType) : matchingOffers;
                matchingOffers = (dto.MaxPrice != null) ? matchingOffers.Where(x => x.Price <= dto.MaxPrice) : matchingOffers;
                extraMessage = ErrorMessageHelper.SimilarOffersFound;
            }

            //if not - return null
            if (!matchingOffers.Any())
            {
                extraMessage = ErrorMessageHelper.NoOffersMatchingPreferences;
                return null;
            }

            //if I'm here - we have some matching offers.
            //Now let's group them by the sellerId and let's check which agents have the most of them
            var ThreeMostPopulatedGroups = matchingOffers.GroupBy(b => b.SellerId).ToList().OrderByDescending(x => x.Count()).Take(3);

            List<int>? result = new();
            foreach(var group in ThreeMostPopulatedGroups)
            {
                result.Add(group.Key);
            }

            extraMessage = ""; return result;
        }
    }
}