using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inżynierka.Shared.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    AgencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NIP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    REGON = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    LicenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControllerAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionParameters = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgentInAgencyId = table.Column<int>(type: "int", nullable: true),
                    OwnerOfAgencyId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvatarFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordRecoveryGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfirmationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Agency_AgentInAgencyId",
                        column: x => x.AgentInAgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_Agency_OwnerOfAgencyId",
                        column: x => x.OwnerOfAgencyId,
                        principalTable: "Agency",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_User_DeletedById",
                        column: x => x.DeletedById,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    OfferType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EstateType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: true),
                    Area = table.Column<int>(type: "int", nullable: true),
                    SellerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Voivodeship = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    RemoteControl = table.Column<bool>(type: "bit", nullable: true),
                    OfferStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offer_User_SellerId",
                        column: x => x.SellerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFavourite",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    EstateAndOfferType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LikeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavourite", x => new { x.OfferId, x.EstateAndOfferType, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserFavourite_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<int>(type: "int", nullable: false),
                    EstateAndOfferType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientEmail = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ClientPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsResponded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessage_User_SellerId",
                        column: x => x.SellerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentRentOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TypeOfBuilding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FloorCount = table.Column<int>(type: "int", nullable: true),
                    BuildingMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WindowsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeatingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rent = table.Column<int>(type: "int", nullable: true),
                    AvailableSinceDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    CableTV = table.Column<bool>(type: "bit", nullable: true),
                    HomePhone = table.Column<bool>(type: "bit", nullable: true),
                    Balcony = table.Column<bool>(type: "bit", nullable: true),
                    UtilityRoom = table.Column<bool>(type: "bit", nullable: true),
                    ParkingSpace = table.Column<bool>(type: "bit", nullable: true),
                    Basement = table.Column<bool>(type: "bit", nullable: true),
                    Garden = table.Column<bool>(type: "bit", nullable: true),
                    Terrace = table.Column<bool>(type: "bit", nullable: true),
                    Elevator = table.Column<bool>(type: "bit", nullable: true),
                    TwoLevel = table.Column<bool>(type: "bit", nullable: true),
                    SeparateKitchen = table.Column<bool>(type: "bit", nullable: true),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: true),
                    AvailableForStudents = table.Column<bool>(type: "bit", nullable: true),
                    OnlyForNonsmoking = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    Furniture = table.Column<bool>(type: "bit", nullable: true),
                    WashingMachine = table.Column<bool>(type: "bit", nullable: true),
                    Dishwasher = table.Column<bool>(type: "bit", nullable: true),
                    Fridge = table.Column<bool>(type: "bit", nullable: true),
                    Stove = table.Column<bool>(type: "bit", nullable: true),
                    Oven = table.Column<bool>(type: "bit", nullable: true),
                    TV = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentRentOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApartmentRentOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentSaleOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TypeOfBuilding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FloorCount = table.Column<int>(type: "int", nullable: true),
                    BuildingMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WindowsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeatingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rent = table.Column<int>(type: "int", nullable: true),
                    AvailableSinceDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    CableTV = table.Column<bool>(type: "bit", nullable: true),
                    HomePhone = table.Column<bool>(type: "bit", nullable: true),
                    Balcony = table.Column<bool>(type: "bit", nullable: true),
                    UtilityRoom = table.Column<bool>(type: "bit", nullable: true),
                    ParkingSpace = table.Column<bool>(type: "bit", nullable: true),
                    Basement = table.Column<bool>(type: "bit", nullable: true),
                    Garden = table.Column<bool>(type: "bit", nullable: true),
                    Terrace = table.Column<bool>(type: "bit", nullable: true),
                    Elevator = table.Column<bool>(type: "bit", nullable: true),
                    TwoLevel = table.Column<bool>(type: "bit", nullable: true),
                    SeparateKitchen = table.Column<bool>(type: "bit", nullable: true),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    Furniture = table.Column<bool>(type: "bit", nullable: true),
                    WashingMachine = table.Column<bool>(type: "bit", nullable: true),
                    Dishwasher = table.Column<bool>(type: "bit", nullable: true),
                    Fridge = table.Column<bool>(type: "bit", nullable: true),
                    Stove = table.Column<bool>(type: "bit", nullable: true),
                    Oven = table.Column<bool>(type: "bit", nullable: true),
                    TV = table.Column<bool>(type: "bit", nullable: true),
                    TypeOfMarket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormOfProperty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentSaleOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApartmentSaleOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPremisesRentOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOfConstruction = table.Column<int>(type: "int", nullable: true),
                    FinishCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    Service = table.Column<bool>(type: "bit", nullable: true),
                    Gastronomic = table.Column<bool>(type: "bit", nullable: true),
                    Office = table.Column<bool>(type: "bit", nullable: true),
                    Industrial = table.Column<bool>(type: "bit", nullable: true),
                    Commercial = table.Column<bool>(type: "bit", nullable: true),
                    Hotel = table.Column<bool>(type: "bit", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    CableTV = table.Column<bool>(type: "bit", nullable: true),
                    HomePhone = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Electricity = table.Column<bool>(type: "bit", nullable: true),
                    SewageSystem = table.Column<bool>(type: "bit", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    SepticTank = table.Column<bool>(type: "bit", nullable: true),
                    SewageTreatmentPlant = table.Column<bool>(type: "bit", nullable: true),
                    Shopwindow = table.Column<bool>(type: "bit", nullable: true),
                    ParkingSpace = table.Column<bool>(type: "bit", nullable: true),
                    AsphaltDriveway = table.Column<bool>(type: "bit", nullable: true),
                    Heating = table.Column<bool>(type: "bit", nullable: true),
                    Elevator = table.Column<bool>(type: "bit", nullable: true),
                    Furnishings = table.Column<bool>(type: "bit", nullable: true),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: true),
                    AvailableFromDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPremisesRentOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPremisesRentOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPremisesSaleOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearOfConstruction = table.Column<int>(type: "int", nullable: true),
                    FinishCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    Service = table.Column<bool>(type: "bit", nullable: true),
                    Gastronomic = table.Column<bool>(type: "bit", nullable: true),
                    Office = table.Column<bool>(type: "bit", nullable: true),
                    Industrial = table.Column<bool>(type: "bit", nullable: true),
                    Commercial = table.Column<bool>(type: "bit", nullable: true),
                    Hotel = table.Column<bool>(type: "bit", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    CableTV = table.Column<bool>(type: "bit", nullable: true),
                    HomePhone = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Electricity = table.Column<bool>(type: "bit", nullable: true),
                    SewageSystem = table.Column<bool>(type: "bit", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    SepticTank = table.Column<bool>(type: "bit", nullable: true),
                    SewageTreatmentPlant = table.Column<bool>(type: "bit", nullable: true),
                    Shopwindow = table.Column<bool>(type: "bit", nullable: true),
                    ParkingSpace = table.Column<bool>(type: "bit", nullable: true),
                    AsphaltDriveway = table.Column<bool>(type: "bit", nullable: true),
                    Heating = table.Column<bool>(type: "bit", nullable: true),
                    Elevator = table.Column<bool>(type: "bit", nullable: true),
                    Furnishings = table.Column<bool>(type: "bit", nullable: true),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: true),
                    TypeOfMarket = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPremisesSaleOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPremisesSaleOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarageRentOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Construction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lighting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heating = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarageRentOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarageRentOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GarageSaleOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Construction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lighting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfMarket = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarageSaleOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GarageSaleOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HallRentOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Construction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParkingSpace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flooring = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heating = table.Column<bool>(type: "bit", nullable: true),
                    Fencing = table.Column<bool>(type: "bit", nullable: true),
                    HasOfficeRooms = table.Column<bool>(type: "bit", nullable: true),
                    HasSocialFacilities = table.Column<bool>(type: "bit", nullable: true),
                    HasRamp = table.Column<bool>(type: "bit", nullable: true),
                    Storage = table.Column<bool>(type: "bit", nullable: true),
                    Production = table.Column<bool>(type: "bit", nullable: true),
                    Office = table.Column<bool>(type: "bit", nullable: true),
                    Commercial = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    ThreePhaseElectricPower = table.Column<bool>(type: "bit", nullable: true),
                    Phone = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Electricity = table.Column<bool>(type: "bit", nullable: true),
                    Sewerage = table.Column<bool>(type: "bit", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    SepticTank = table.Column<bool>(type: "bit", nullable: true),
                    SewageTreatmentPlant = table.Column<bool>(type: "bit", nullable: true),
                    FieldDriveway = table.Column<bool>(type: "bit", nullable: true),
                    PavedDriveway = table.Column<bool>(type: "bit", nullable: true),
                    AsphaltDriveway = table.Column<bool>(type: "bit", nullable: true),
                    AvailableFromDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallRentOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallRentOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HallSaleOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Construction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParkingSpace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flooring = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heating = table.Column<bool>(type: "bit", nullable: true),
                    Fencing = table.Column<bool>(type: "bit", nullable: true),
                    HasOfficeRooms = table.Column<bool>(type: "bit", nullable: true),
                    HasSocialFacilities = table.Column<bool>(type: "bit", nullable: true),
                    HasRamp = table.Column<bool>(type: "bit", nullable: true),
                    Storage = table.Column<bool>(type: "bit", nullable: true),
                    Production = table.Column<bool>(type: "bit", nullable: true),
                    Office = table.Column<bool>(type: "bit", nullable: true),
                    Commercial = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    ThreePhaseElectricPower = table.Column<bool>(type: "bit", nullable: true),
                    Phone = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Electricity = table.Column<bool>(type: "bit", nullable: true),
                    Sewerage = table.Column<bool>(type: "bit", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    SepticTank = table.Column<bool>(type: "bit", nullable: true),
                    SewageTreatmentPlant = table.Column<bool>(type: "bit", nullable: true),
                    FieldDriveway = table.Column<bool>(type: "bit", nullable: true),
                    PavedDriveway = table.Column<bool>(type: "bit", nullable: true),
                    AsphaltDriveway = table.Column<bool>(type: "bit", nullable: true),
                    TypeOfMarket = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallSaleOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallSaleOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseRentOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LandArea = table.Column<int>(type: "int", nullable: true),
                    TypeOfBuilding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FloorCount = table.Column<int>(type: "int", nullable: true),
                    BuildingMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionYear = table.Column<int>(type: "int", nullable: true),
                    AtticType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoofType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoofingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WindowsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsARecreationHouse = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    BrickFence = table.Column<bool>(type: "bit", nullable: true),
                    Net = table.Column<bool>(type: "bit", nullable: true),
                    MetalFence = table.Column<bool>(type: "bit", nullable: true),
                    WoodenFence = table.Column<bool>(type: "bit", nullable: true),
                    ConcreteFence = table.Column<bool>(type: "bit", nullable: true),
                    Hedge = table.Column<bool>(type: "bit", nullable: true),
                    OtherFencing = table.Column<bool>(type: "bit", nullable: true),
                    Geothermics = table.Column<bool>(type: "bit", nullable: true),
                    OilHeating = table.Column<bool>(type: "bit", nullable: true),
                    ElectricHeating = table.Column<bool>(type: "bit", nullable: true),
                    DistrictHeating = table.Column<bool>(type: "bit", nullable: true),
                    TileStoves = table.Column<bool>(type: "bit", nullable: true),
                    GasHeating = table.Column<bool>(type: "bit", nullable: true),
                    CoalHeating = table.Column<bool>(type: "bit", nullable: true),
                    Biomass = table.Column<bool>(type: "bit", nullable: true),
                    HeatPump = table.Column<bool>(type: "bit", nullable: true),
                    SolarCollector = table.Column<bool>(type: "bit", nullable: true),
                    OtherHeating = table.Column<bool>(type: "bit", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    CableTV = table.Column<bool>(type: "bit", nullable: true),
                    HomePhone = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Electricity = table.Column<bool>(type: "bit", nullable: true),
                    SewageSystem = table.Column<bool>(type: "bit", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    SepticTank = table.Column<bool>(type: "bit", nullable: true),
                    SewageTreatmentPlant = table.Column<bool>(type: "bit", nullable: true),
                    FieldDriveway = table.Column<bool>(type: "bit", nullable: true),
                    PavedDriveway = table.Column<bool>(type: "bit", nullable: true),
                    AsphaltDriveway = table.Column<bool>(type: "bit", nullable: true),
                    SwimmingPool = table.Column<bool>(type: "bit", nullable: true),
                    ParkingSpace = table.Column<bool>(type: "bit", nullable: true),
                    Basement = table.Column<bool>(type: "bit", nullable: true),
                    Attic = table.Column<bool>(type: "bit", nullable: true),
                    Garden = table.Column<bool>(type: "bit", nullable: true),
                    Furnishings = table.Column<bool>(type: "bit", nullable: true),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: true),
                    AvailableForStudents = table.Column<bool>(type: "bit", nullable: true),
                    OnlyForNonsmoking = table.Column<bool>(type: "bit", nullable: true),
                    Rent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseRentOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseRentOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HouseSaleOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LandArea = table.Column<int>(type: "int", nullable: true),
                    TypeOfBuilding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FloorCount = table.Column<int>(type: "int", nullable: true),
                    BuildingMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionYear = table.Column<int>(type: "int", nullable: true),
                    AtticType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoofType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoofingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinishCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WindowsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsARecreationHouse = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryBlinds = table.Column<bool>(type: "bit", nullable: true),
                    AntiBurglaryWindowsOrDoors = table.Column<bool>(type: "bit", nullable: true),
                    IntercomOrVideophone = table.Column<bool>(type: "bit", nullable: true),
                    MonitoringOrSecurity = table.Column<bool>(type: "bit", nullable: true),
                    AlarmSystem = table.Column<bool>(type: "bit", nullable: true),
                    ClosedArea = table.Column<bool>(type: "bit", nullable: true),
                    BrickFence = table.Column<bool>(type: "bit", nullable: true),
                    Net = table.Column<bool>(type: "bit", nullable: true),
                    MetalFence = table.Column<bool>(type: "bit", nullable: true),
                    WoodenFence = table.Column<bool>(type: "bit", nullable: true),
                    ConcreteFence = table.Column<bool>(type: "bit", nullable: true),
                    Hedge = table.Column<bool>(type: "bit", nullable: true),
                    OtherFencing = table.Column<bool>(type: "bit", nullable: true),
                    Geothermics = table.Column<bool>(type: "bit", nullable: true),
                    OilHeating = table.Column<bool>(type: "bit", nullable: true),
                    ElectricHeating = table.Column<bool>(type: "bit", nullable: true),
                    DistrictHeating = table.Column<bool>(type: "bit", nullable: true),
                    TileStoves = table.Column<bool>(type: "bit", nullable: true),
                    GasHeating = table.Column<bool>(type: "bit", nullable: true),
                    CoalHeating = table.Column<bool>(type: "bit", nullable: true),
                    Biomass = table.Column<bool>(type: "bit", nullable: true),
                    HeatPump = table.Column<bool>(type: "bit", nullable: true),
                    SolarCollector = table.Column<bool>(type: "bit", nullable: true),
                    OtherHeating = table.Column<bool>(type: "bit", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    CableTV = table.Column<bool>(type: "bit", nullable: true),
                    HomePhone = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Electricity = table.Column<bool>(type: "bit", nullable: true),
                    SewageSystem = table.Column<bool>(type: "bit", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    SepticTank = table.Column<bool>(type: "bit", nullable: true),
                    SewageTreatmentPlant = table.Column<bool>(type: "bit", nullable: true),
                    FieldDriveway = table.Column<bool>(type: "bit", nullable: true),
                    PavedDriveway = table.Column<bool>(type: "bit", nullable: true),
                    AsphaltDriveway = table.Column<bool>(type: "bit", nullable: true),
                    SwimmingPool = table.Column<bool>(type: "bit", nullable: true),
                    ParkingSpace = table.Column<bool>(type: "bit", nullable: true),
                    Basement = table.Column<bool>(type: "bit", nullable: true),
                    Attic = table.Column<bool>(type: "bit", nullable: true),
                    Garden = table.Column<bool>(type: "bit", nullable: true),
                    Furnishings = table.Column<bool>(type: "bit", nullable: true),
                    AirConditioning = table.Column<bool>(type: "bit", nullable: true),
                    TypeOfMarket = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseSaleOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseSaleOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlotOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PlotType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFenced = table.Column<bool>(type: "bit", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dimensions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldDriveway = table.Column<bool>(type: "bit", nullable: true),
                    PavedDriveway = table.Column<bool>(type: "bit", nullable: true),
                    AsphaltDriveway = table.Column<bool>(type: "bit", nullable: true),
                    Phone = table.Column<bool>(type: "bit", nullable: true),
                    Water = table.Column<bool>(type: "bit", nullable: true),
                    Electricity = table.Column<bool>(type: "bit", nullable: true),
                    Sewerage = table.Column<bool>(type: "bit", nullable: true),
                    Gas = table.Column<bool>(type: "bit", nullable: true),
                    SepticTank = table.Column<bool>(type: "bit", nullable: true),
                    SewageTreatmentPlant = table.Column<bool>(type: "bit", nullable: true),
                    Forest = table.Column<bool>(type: "bit", nullable: true),
                    Lake = table.Column<bool>(type: "bit", nullable: true),
                    Mountains = table.Column<bool>(type: "bit", nullable: true),
                    Sea = table.Column<bool>(type: "bit", nullable: true),
                    OpenArea = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlotOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomRentingOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AdditionalFees = table.Column<int>(type: "int", nullable: true),
                    Deposit = table.Column<int>(type: "int", nullable: true),
                    NumberOfPeopleInTheRoom = table.Column<int>(type: "int", nullable: true),
                    TypeOfBuilding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableForStudents = table.Column<bool>(type: "bit", nullable: true),
                    OnlyForNonsmoking = table.Column<bool>(type: "bit", nullable: true),
                    Furniture = table.Column<bool>(type: "bit", nullable: true),
                    WashingMachine = table.Column<bool>(type: "bit", nullable: true),
                    Dishwasher = table.Column<bool>(type: "bit", nullable: true),
                    Fridge = table.Column<bool>(type: "bit", nullable: true),
                    Stove = table.Column<bool>(type: "bit", nullable: true),
                    Oven = table.Column<bool>(type: "bit", nullable: true),
                    TV = table.Column<bool>(type: "bit", nullable: true),
                    Internet = table.Column<bool>(type: "bit", nullable: true),
                    CableTV = table.Column<bool>(type: "bit", nullable: true),
                    HomePhone = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRentingOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomRentingOffer_Offer_Id",
                        column: x => x.Id,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offer_SellerId",
                table: "Offer",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AgentInAgencyId",
                table: "User",
                column: "AgentInAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedById",
                table: "User",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_User_OwnerOfAgencyId",
                table: "User",
                column: "OwnerOfAgencyId",
                unique: true,
                filter: "[OwnerOfAgencyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavourite_UserId",
                table: "UserFavourite",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_SellerId",
                table: "UserMessage",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentRentOffer");

            migrationBuilder.DropTable(
                name: "ApartmentSaleOffer");

            migrationBuilder.DropTable(
                name: "BusinessPremisesRentOffer");

            migrationBuilder.DropTable(
                name: "BusinessPremisesSaleOffer");

            migrationBuilder.DropTable(
                name: "GarageRentOffer");

            migrationBuilder.DropTable(
                name: "GarageSaleOffer");

            migrationBuilder.DropTable(
                name: "HallRentOffer");

            migrationBuilder.DropTable(
                name: "HallSaleOffer");

            migrationBuilder.DropTable(
                name: "HouseRentOffer");

            migrationBuilder.DropTable(
                name: "HouseSaleOffer");

            migrationBuilder.DropTable(
                name: "PlotOffer");

            migrationBuilder.DropTable(
                name: "RoomRentingOffer");

            migrationBuilder.DropTable(
                name: "UserAction");

            migrationBuilder.DropTable(
                name: "UserFavourite");

            migrationBuilder.DropTable(
                name: "UserMessage");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Agency");
        }
    }
}
