using In¿ynierka_Common.ServiceRegistrationAttributes;
using In¿ynierka.Shared.Entities;

namespace In¿ynierka.Shared.Repositories
{
    [ScopedRegistration]
    public class UserActionRepository : BaseRepository<UserAction>
    {
        public UserActionRepository(DataContext context) : base(context)
        {
        }
    }
}