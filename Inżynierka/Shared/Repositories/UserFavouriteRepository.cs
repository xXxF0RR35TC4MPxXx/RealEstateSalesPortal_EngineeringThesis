using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore.Storage;
using Inżynierka_Common.Enums;
using Inżynierka_Common.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Inżynierka.Shared.Repositories
{
    [ScopedRegistrationWithInterface]
    public class UserFavouriteRepository : BaseRepository<UserFavourite>, IUserFavouriteRepository
    {
        public IConfiguration Configuration { get; }
        private DataContext _dataContext;

        public UserFavouriteRepository(DataContext context, IConfiguration configuration) : base(context)
        {
            _dataContext = context;
            Configuration = configuration;
        }

        public UserFavourite? GetFav(int offerId, int userId)
        {
            UserFavourite? result = _dataContext.UserFavourites.Where(o => o.OfferId == offerId && o.UserId == userId).SingleOrDefault();
            return result;
        }

        public IQueryable<UserFavourite> GetUserFavourites(int userId)
        {
            var result = _dataContext.UserFavourites.Where(o => o.UserId == userId).OrderByDescending(o => o.LikeDate)
                .Include(uf=>uf.Offer)
                    .ThenInclude(o=>o.Seller)
                        .ThenInclude(s=>s.AgentInAgency)
                .Include(uf => uf.Offer)
                    .ThenInclude(o => o.Seller)
                        .ThenInclude(s => s.OwnerOfAgency);
            return result;
        }

        public bool RemoveFavByIdAndSaveChanges(int offerId, int userId)
        {
            UserFavourite? userFav = GetFav(offerId, userId);

            if (userFav != null)
            {
                try 
                {
                    _dataContext.UserFavourites.Remove(userFav);
                    _dataContext.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
