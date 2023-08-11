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
    public interface IUserEventRepository : IBaseRepository<UserEvent>
    {
        public IEnumerable<UserEvent>? GetAllUserEvents(int id);
    }
}
