using Inżynierka.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.UnitTests.UserFavouriteTests
{
    public class AddFavTests : BaseUserFavouriteServiceTests
    {
        //wyrzuca wyjątek przy zapisywaniu
        [Fact]
        public void AddToFav_ShouldReturnFalse_WhenException()
        {
            Exception ex = new Exception();
            UserFavouriteRepositoryMock.Setup(x => x.AddAndSaveChanges(It.IsAny<UserFavourite>())).Throws(ex);

            bool result = testedOfferService.AddToFavourites(offer1.Id, user1.Id, out errorMessage);

            Assert.False(result);
        }

        //działa ok
        [Fact]
        public void AddToFav_ShouldReturnTrue_WhenOK()
        {
            Exception ex = new Exception();
            UserFavouriteRepositoryMock.Setup(x => x.AddAndSaveChanges(It.IsAny<UserFavourite>())).Returns(fav1);

            bool result = testedOfferService.AddToFavourites(offer1.Id, user1.Id, out errorMessage);

            Assert.True(result);
        }
    }
}
