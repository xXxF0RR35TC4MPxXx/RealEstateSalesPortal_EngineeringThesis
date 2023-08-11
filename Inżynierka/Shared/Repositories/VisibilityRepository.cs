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
using Inżynierka.Shared.DTOs.Offers;

namespace Inżynierka.Shared.Repositories
{
    [ScopedRegistrationWithInterface]
    public class VisibilityRepository : BaseRepository<OfferVisibleForUser>, IVisibilityRepository
    {
        private readonly DataContext _dataContext;

        public VisibilityRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public bool CheckVisibility(int offerId, int userId)
        {
            return _dataContext.OfferVisibleForUsers.Where(o=> o.OfferId == offerId && o.UserId == userId).Any();
        }


        public OfferVisibleForUser? GetVisibility(int userId, int offerId)
        {
            return _dataContext.OfferVisibleForUsers.Where(o=>o.UserId==userId && o.OfferId == offerId).FirstOrDefault();
        }

        public IQueryable<OfferVisibleForUser>? GetAcceptedUsers(int offerId)
        {
            return _dataContext.OfferVisibleForUsers.Where(o => o.OfferId == offerId);
        }
    }
}