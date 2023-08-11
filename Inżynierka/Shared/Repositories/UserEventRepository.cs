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
    public class UserEventRepository : BaseRepository<UserEvent>, IUserEventRepository
    {
        private readonly DataContext _dataContext;

        public UserEventRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public IEnumerable<UserEvent>? GetAllUserEvents(int id)
        {
            var result = _dataContext.UserEvents.Where(e => e.SellerId == id && !e.DeletedDate.HasValue);
            return result;
        }
    }
}