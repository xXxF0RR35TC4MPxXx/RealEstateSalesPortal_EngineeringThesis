using Inżynierka.Shared.DTOs.Agency;
using Inżynierka.Shared.Entities;
using System.Collections;

namespace Inżynierka.UnitTests.AgencyTests
{
    public class UpdateTests:BaseAgencyServiceTest
    {
        // [done] nie ma takiej agencji 
        // [done] nie jestem właścicielem agencji
        // [done] dobry / zły numer telefonu
        // [done] dobry / zły email
        // [done] dobry / zły NIP
        // [done] dobry / zły REGON
        // [done] dobry / zły Kod pocztowy

        [Theory]
        [ClassData(typeof(Update1TestDataGenerator))]
        public void Update_ShouldReturnFalse_IfNoAgencyWithGivenId(int agencyId, int userId, bool expectedResult, Agency? returnedAgency)
        {
            AgencyRepositoryMock.Setup(x => x.GetById(agencyId)).Returns(returnedAgency);
            bool result = testedAgencyService.Update(agencyId, userId, updateDTO, out errorMessage);
            Assert.True(result == expectedResult);
        }

        //agency.OwnerId != currentUserId
        [Theory]
        [ClassData(typeof(Update2TestDataGenerator))]
        public void Update_ShouldReturnFalse_IfUserIsntTheOwnerOfAgency(int agencyId, int userId, bool expectedResult, Agency? returnedAgency)
        {
            AgencyRepositoryMock.Setup(x => x.GetById(agencyId)).Returns(returnedAgency);
            bool result = testedAgencyService.Update(agencyId, userId, updateDTO, out errorMessage);
            Assert.True(result == expectedResult);
        }

        //Regex match tests
        [Theory]
        [ClassData(typeof(Update3TestDataGenerator))]
        public void Update_ShouldReturnFalse_IfNewDataDoesntMatchRegex(int agencyId, int userId, bool expectedResult, AgencyUpdateDTO updateDTO)
        {
            User user1 = new()
            {
                Id = 1,
                Email = "admin@admin.com",
                UserStatus = "ACTIVE",
                RoleName = "ADMIN"
            };

            Agency agency = new Agency()
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                LicenceNumber = "111111111111111",
                REGON = "111111111",
                NIP = "1111111111",
                Description = "desc",
                PostalCode = "15-333",
                City = "Białystok",
                Address = "Zwierzyniecka 6",
                PhoneNumber = "111111111",
                Email = "test@test.com",
                AgencyName = "TestAgency",
                OwnerId = user1.Id,
                Owner = user1,
            };

            AgencyRepositoryMock.Setup(x => x.GetById(agencyId)).Returns(agency);
            bool result = testedAgencyService.Update(agencyId, userId, updateDTO, out errorMessage);
            Assert.True(result == expectedResult);
        }
    }

    public class Update1TestDataGenerator : IEnumerable<object[]>
    {
        static User user1 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN"
        };


        private readonly List<object[]> _data = new List<object[]>
        {
            
            new object[] {1, 1, true, new Agency()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    LicenceNumber = "111111111111111",
                    REGON = "111111111",
                    NIP = "1111111111",
                    Description = "desc",
                    PostalCode = "15-333",
                    City = "Białystok",
                    Address = "Zwierzyniecka 6",
                    PhoneNumber = "111111111",
                    Email = "test@test.com",
                    AgencyName = "TestAgency",
                    OwnerId = user1.Id,
                    Owner = user1,
                }
            },

            new object[] {1, 1, false, (Agency?)null}
        };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Update2TestDataGenerator : IEnumerable<object[]>
    {
        static User user1 = new()
        {
            Id = 1,
            Email = "admin@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN"
        };

        static User user2 = new()
        {
            Id = 2,
            Email = "admin2@admin.com",
            UserStatus = "ACTIVE",
            RoleName = "ADMIN"
        };

        private readonly List<object[]> _data = new List<object[]>
        {

            new object[] {1, 1, true, new Agency()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    LicenceNumber = "111111111111111",
                    REGON = "111111111",
                    NIP = "1111111111",
                    Description = "desc",
                    PostalCode = "15-333",
                    City = "Białystok",
                    Address = "Zwierzyniecka 6",
                    PhoneNumber = "111111111",
                    Email = "test@test.com",
                    AgencyName = "TestAgency",
                    OwnerId = user1.Id,
                    Owner = user1,
                }
            },

            new object[] {1, 2, false,new Agency()
                {
                    Id = 1,
                    CreatedDate = DateTime.Now,
                    LicenceNumber = "111111111111111",
                    REGON = "111111111",
                    NIP = "1111111111",
                    Description = "desc",
                    PostalCode = "15-333",
                    City = "Białystok",
                    Address = "Zwierzyniecka 6",
                    PhoneNumber = "111111111",
                    Email = "test@test.com",
                    AgencyName = "TestAgency",
                    OwnerId = user1.Id,
                    Owner = user1,
                }
            }
        };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Update3TestDataGenerator : IEnumerable<object[]>
    {
        
        private readonly List<object[]> _data = new()
        {
            //PhoneNumberTests
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "123123123",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "9999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "99999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, true,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },

            //NIP tests
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "99999919999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999x99999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "          ",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "😋😋😋😋😋😋😋😋😋😋",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },

            //REGON tests
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "99999999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "99999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "9999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO(){
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "9999999999999999999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "😋😋😋😋😋😋😋😋😋",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },

            //Email tests
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "a@wp.pl",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email.com@updated",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "emailupdated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.a",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },

            //Postal code tests
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "00-000",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, true, new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "999-99",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99-9999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99x999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            },
            new object[] {1, 1, false,new AgencyUpdateDTO()
                {
                    AgencyName = "UpdatedName",
                    Email = "email@updated.com",
                    PhoneNumber = "999999999",
                    City = "UpdatedCity",
                    Address = "UpdatedAddress",
                    PostalCode = "99 999",
                    Description = "UpdatedDesc",
                    NIP = "9999999999",
                    REGON = "999999999",
                    LicenceNumber = "999999999",
                    UpdatedDate = DateTime.Now
                }
            }
        };
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
