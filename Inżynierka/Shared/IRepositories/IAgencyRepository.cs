using Inżynierka.Shared.Entities;
using Inżynierka_Common.ServiceRegistrationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.IRepositories
{
    [ScopedRegistrationWithInterface]
    public interface IAgencyRepository : IBaseRepository<Agency>
    {
        public Agency? GetAgencyByInvitationGuid(Guid guid);
    }
}
