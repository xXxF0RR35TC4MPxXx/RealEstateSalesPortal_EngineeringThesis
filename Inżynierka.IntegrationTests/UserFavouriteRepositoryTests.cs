using Inżynierka.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.IntegrationTests
{
    public class UserFavouriteRepositoryTests : BaseIntegrationTest
    {
        //base methods

        [Test, Isolated]
        public void Add_IfValidUserAndOffer_ShouldAddUserFavToDatabase()
        {
            UserFavourite userFavourite = new UserFavourite()
            {
                UserId = 8,
                OfferId = 28,
                LikeDate = DateTime.Now
            };

            userFavRepo.AddAndSaveChanges(userFavourite);

            var usersFavCount = context.UserFavourites.Count(x => x.OfferId == 28 && x.UserId == 8);
            Assert.That(usersFavCount, Is.EqualTo(1));
        }

        [Theory, Isolated]
        [TestCase(7, -1)]
        [TestCase(-1, 24)]
        [TestCase(-1, -1)]

        public void Add_IfInvalidUserOrOffer_ShouldThrowDbUpdateException(int userId, int offerId)
        {
            UserFavourite userFavourite = new UserFavourite()
            {
                UserId = userId,
                OfferId = offerId,
                LikeDate = DateTime.Now
            };

            var ex = Assert.Throws<DbUpdateException>(() => userFavRepo2.AddAndSaveChanges(userFavourite));
            Assert.That(ex, Is.Not.Null);
        }


        [Theory, Isolated]
        [TestCase(7, 25)]
        [TestCase(7, 31)]
        [TestCase(8, 22)]
        [TestCase(8, 26)]
        public void RemoveFavById_IfFound_ShouldRemoveUserFavFromDatabase(int userId, int offerId)
        {
            UserFavourite userFavourite = new UserFavourite()
            {
                UserId = userId,
                OfferId = offerId,
                LikeDate = DateTime.Now
            };

            userFavRepo.RemoveFavByIdAndSaveChanges(userId, offerId);

            var userFavCount = context2.UserFavourites.Count(x => x.UserId == userId && x.OfferId == offerId);

            Assert.That(userFavCount, Is.Not.EqualTo(0));
        }

        [Theory, Isolated]
        [TestCase(-1, 25)]
        [TestCase(7, -1)]
        [TestCase(-1, -1)]
        [TestCase(Int32.MaxValue, Int32.MaxValue)]
        public void RemoveFavById_IfInvalidData_ShouldReturnFalse(int userId, int offerId)
        {
            bool result = userFavRepo.RemoveFavByIdAndSaveChanges(userId, offerId);
            Assert.That(result, Is.False);
        }

        [Test, Isolated]
        public void GetFav_IfValidParams_ShouldReturnUserFav()
        {
            var userFav = userFavRepo.GetFav(25, 7);
            Assert.That(userFav, Is.Not.Null);
        }
        
        [Theory, Isolated]
        [TestCase(-1, 25)]
        [TestCase(7, -1)]
        [TestCase(-1, -1)]
        [TestCase(Int32.MaxValue, Int32.MaxValue)]
        public void GetFav_IfInvalidData_ShouldReturnNull(int userId, int offerId)
        {
            var result = userFavRepo.GetFav(userId, offerId);
            Assert.That(result, Is.Null);
        }

        [Test, Isolated]
        public void GetUserFavourites_IfAny_ShouldReturnIQueryable()
        {
            var iqueryable = userFavRepo.GetUserFavourites(7);
            Assert.That(iqueryable.Count(), Is.Not.EqualTo(0));
        }
    }
}
