using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserFavouriteTests
{
    public class RemoveFavTests : BaseUserFavouriteServiceTests
    {
        //Nie ma takiego polubienia
        [Fact]
        public void RemoveFav_ShouldReturnFalse_WhenOfferIsNotInFav()
        {
            UserFavouriteRepositoryMock.Setup(x => x.GetFav(offer1.Id, user1.Id)).Returns((UserFavourite?)null);

            bool result = testedOfferService.RemoveFromFavourites(offer1.Id, user1.Id, out errorMessage);

            Assert.False(result);
        }


        //działa normalnie
        [Fact]
        public void RemoveFav_ShouldReturnTrue_WhenOK()
        {
            UserFavouriteRepositoryMock.Setup(x => x.GetFav(offer1.Id, user1.Id)).Returns(fav1);

            bool result = testedOfferService.RemoveFromFavourites(offer1.Id, user1.Id, out errorMessage);

            Assert.False(result);
        }

        //wyrzuca wyjątek przy zapisywaniu
        [Fact]
        public void RemoveFav_ShouldReturnFalse_WhenException()
        {
            Exception ex = new Exception();
            UserFavouriteRepositoryMock.Setup(x => x.GetFav(offer1.Id, user1.Id)).Returns(fav1);
            UserFavouriteRepositoryMock.Setup(x => x.RemoveFavByIdAndSaveChanges(offer1.Id, user1.Id)).Throws(ex);

            bool result = testedOfferService.RemoveFromFavourites(offer1.Id, user1.Id, out errorMessage);

            Assert.False(result);
        }
    }
}
