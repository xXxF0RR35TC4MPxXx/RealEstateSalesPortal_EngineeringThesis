using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Offers.Update
{
    public class ApartmentSaleUpdateOfferDTO:OfferUpdateDTO
    {
        public TypeOfBuilding? TypeOfBuilding { get; set; } //rodzaj zabudowy

        public string? ApartmentFinishCondition { get; set; }
        public Floor? Floor { get; set; }

        public int? FloorCount { get; set; }

        public BuildingMaterial? BuildingMaterial { get; set; }

        public WindowType? WindowsType { get; set; }

        public ApartmentHeating? HeatingType { get; set; }

        public int? Rent { get; set; }

        public int? YearOfConstruction { get; set; }
        public string? AvailableSinceDate { get; set; }


        //======== media =============
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }

        //========== ============
        public bool? Balcony { get; set; }
        public bool? UtilityRoom { get; set; }
        public bool? ParkingSpace { get; set; }
        public bool? Basement { get; set; }
        public bool? Garden { get; set; }
        public bool? Terrace { get; set; }
        public bool? Elevator { get; set; }
        public bool? TwoLevel { get; set; }
        public bool? SeparateKitchen { get; set; }
        public bool? AirConditioning { get; set; }



        // ========== zabezpieczenia ===========
        public bool? AntiBurglaryBlinds { get; set; }
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        public bool? IntercomOrVideophone { get; set; }
        public bool? MonitoringOrSecurity { get; set; }
        public bool? AlarmSystem { get; set; }
        public bool? ClosedArea { get; set; }



        // ======= wyposażenie =======
        public bool? Furniture { get; set; }
        public bool? WashingMachine { get; set; }
        public bool? Dishwasher { get; set; }
        public bool? Fridge { get; set; }
        public bool? Stove { get; set; }
        public bool? Oven { get; set; }
        public bool? TV { get; set; }


        public TypeOfMarket? TypeOfMarket { get; set; }

        public string? FormOfProperty { get; set; }

        public ApartmentSaleUpdateOfferDTO(string offerTitle, int price, string voivodeship, string city, string address, string description, 
            bool? remoteControl, string offerStatus,
            TypeOfBuilding? typeOfBuilding, Floor? floor, int? floorCount, BuildingMaterial? buildingMaterial, WindowType? windowsType,
            ApartmentHeating? heatingType, int? rent, string? availableSinceDate, bool? internet, bool? cableTV, bool? homePhone,
            bool? balcony, bool? utilityRoom, bool? parkingSpace, bool? basement, bool? garden, bool? terrace, bool? elevator, bool? twoLevel,
            bool? separateKitchen, bool? airConditioning, bool? antiBurglaryBlinds, bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone,
            bool? monitoringOrSecurity, bool? alarmSystem, bool? closedArea, bool? furniture, bool? washingMachine, bool? dishwasher, bool? fridge,
            bool? stove, bool? oven, bool? tV, TypeOfMarket? typeOfMarket, string? formOfProperty, int? yearOfConstruction, string? apartmentFinishCondition)
        {
            OfferTitle = offerTitle;
            Price = price;
            Voivodeship = voivodeship;
            City = city;
            Address = address;
            Description = description;
            RemoteControl = remoteControl;
            OfferStatus = offerStatus;

            TypeOfBuilding = typeOfBuilding;
            Floor = floor;
            FloorCount = floorCount;
            BuildingMaterial = buildingMaterial;
            WindowsType = windowsType;
            HeatingType = heatingType;
            Rent = rent;
            AvailableSinceDate = availableSinceDate;
            Internet = internet;
            CableTV = cableTV;
            HomePhone = homePhone;
            Balcony = balcony;
            UtilityRoom = utilityRoom;
            ParkingSpace = parkingSpace;
            Basement = basement;
            Garden = garden;
            Terrace = terrace;
            Elevator = elevator;
            TwoLevel = twoLevel;
            SeparateKitchen = separateKitchen;
            AirConditioning = airConditioning;
            AntiBurglaryBlinds = antiBurglaryBlinds;
            AntiBurglaryWindowsOrDoors = antiBurglaryWindowsOrDoors;
            IntercomOrVideophone = intercomOrVideophone;
            MonitoringOrSecurity = monitoringOrSecurity;
            AlarmSystem = alarmSystem;
            ClosedArea = closedArea;
            Furniture = furniture;
            WashingMachine = washingMachine;
            Dishwasher = dishwasher;
            Fridge = fridge;
            Stove = stove;
            Oven = oven;
            TV = tV;
            TypeOfMarket = typeOfMarket;
            FormOfProperty = formOfProperty;
            YearOfConstruction = yearOfConstruction;
            ApartmentFinishCondition = apartmentFinishCondition;
        }

        public ApartmentSaleUpdateOfferDTO()
        {
        }
    }
}
