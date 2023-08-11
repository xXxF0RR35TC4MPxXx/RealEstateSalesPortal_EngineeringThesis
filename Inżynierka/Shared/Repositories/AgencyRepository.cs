using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using Inżynierka.Shared;
using Inżynierka.Shared.Repositories;

namespace Inżynierka.Shared.Repositories
{
    [ScopedRegistrationWithInterface]
    public class AgencyRepository : BaseRepository<Agency>, IAgencyRepository
    {
        private readonly DataContext _dataContext;

        public AgencyRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public Agency? GetAgencyByInvitationGuid(Guid guid)
        {
            return _dataContext.Agencies.Where(a=>a.InvitationGuid == guid).FirstOrDefault();
        }
    }
}