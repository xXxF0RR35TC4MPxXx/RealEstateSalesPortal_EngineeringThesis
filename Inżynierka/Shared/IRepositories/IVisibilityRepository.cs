using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Repositories;
using Inżynierka_Common.ServiceRegistrationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.IRepositories
{
    [ScopedRegistrationWithInterface]
    public interface IVisibilityRepository : IBaseRepository<OfferVisibleForUser>
    {
        public bool CheckVisibility(int offerId, int userId);
        public OfferVisibleForUser? GetVisibility(int userId, int offerId);
        IQueryable<OfferVisibleForUser>? GetAcceptedUsers(int offerId);
    }
}
