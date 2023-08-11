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
    public interface IUserFavouriteRepository : IBaseRepository<UserFavourite>
    {
        public bool RemoveFavByIdAndSaveChanges(int offerId, int userId);
        public UserFavourite? GetFav(int offerId, int userId);
        public IQueryable<UserFavourite> GetUserFavourites(int userId);
    }
}
