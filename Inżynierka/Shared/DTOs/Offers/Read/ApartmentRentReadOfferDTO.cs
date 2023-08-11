using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Common.Enums;
using System.ComponentModel;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class ApartmentRentReadOfferDTO : ReadOfferDTO
    {
        [Description("Typ budynku")]
        [Display(Order = 25)]
        public string? TypeOfBuilding { get; set; } //rodzaj zabudowy

        [Description("Piętro")]
        [Display(Order = 26)]
        public string? Floor { get; set; }

        [Description("Ilość pięter")]
        [Display(Order = 27)]
        public int? FloorCount { get; set; }

        [Description("Materiał budowlany")]
        [Display(Order = 28)]
        public string? BuildingMaterial { get; set; }

        [Description("Typ okien")]
        [Display(Order = 29)]
        public string? WindowsType { get; set; }

        [Description("Rodzaj ogrzewania")]
        [Display(Order = 30)]
        public string? HeatingType { get; set; }

        [Description("Czynsz")]
        [Display(Order = 31)]
        public int? Rent { get; set; }

        [Description("Dostępne od")]
        [Display(Order = 32)]
        public string? AvailableSinceDate { get; set; }

        // ======= media =========

        [Description("Internet")]
        [Display(Order = 33)]
        public bool? Internet { get; set; }

        [Description("Telewizja kablowa")]
        [Display(Order = 34)]
        public bool? CableTV { get; set; }

        [Description("Telefon domowy")]
        [Display(Order = 35)]
        public bool? HomePhone { get; set; }


        // ======== informacje dodatkowe ========
        [Description("Balkon")]
        [Display(Order = 36)]
        public bool? Balcony { get; set; }
        [Description("Pomieszczenie użytkowe")]
        [Display(Order = 37)]
        public bool? UtilityRoom { get; set; }
        [Description("Miejsce parkingowe")]
        [Display(Order = 38)]
        public bool? ParkingSpace { get; set; }
        [Description("Piwnica")]
        [Display(Order = 39)]
        public bool? Basement { get; set; }
        [Description("Ogród")]
        [Display(Order = 40)]
        public bool? Garden { get; set; }
        [Description("Taras")]
        [Display(Order = 41)]
        public bool? Terrace { get; set; }
        [Description("Winda")]
        [Display(Order = 42)]
        public bool? Elevator { get; set; }
        [Description("Dwupoziomowy")]
        [Display(Order = 43)]
        public bool? TwoLevel { get; set; }
        [Description("Oddzielna kuchnia")]
        [Display(Order = 43)]
        public bool? SeparateKitchen { get; set; }
        [Description("Klimatyzacja")]
        [Display(Order = 44)]
        public bool? AirConditioning { get; set; }
        

        // ========== zabezpieczenia ===========

        [Description("Rolety anty-włamaniowe")]
        [Display(Order = 67)]
        public bool? AntiBurglaryBlinds { get; set; }
        [Description("Drzwi lub okna anty-włamaniowe")]
        [Display(Order = 47)]
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        [Description("Domofon")]
        [Display(Order = 48)]
        public bool? IntercomOrVideophone { get; set; }
        [Description("Monitoring")]
        [Display(Order = 49)]
        public bool? MonitoringOrSecurity { get; set; }
        [Description("System alarmowy")]
        [Display(Order = 50)]
        public bool? AlarmSystem { get; set; }
        [Description("Teren zamknięty")]
        [Display(Order = 51)]
        public bool? ClosedArea { get; set; }

        // ======= wyposażenie =======
        [Description("Meble")]
        [Display(Order = 52)]
        public bool? Furniture { get; set; }
        [Description("Pralka")]
        [Display(Order = 53)]
        public bool? WashingMachine { get; set; }
        [Description("Zmywarka")]
        [Display(Order = 54)]
        public bool? Dishwasher { get; set; }
        [Description("Lodówka")]
        [Display(Order = 55)]
        public bool? Fridge { get; set; }
        [Description("Kuchenka")]
        [Display(Order = 56)]
        public bool? Stove { get; set; }
        [Description("Piekarnik")]
        [Display(Order = 57)]
        public bool? Oven { get; set; }
        [Description("Telewizor")]
        [Display(Order = 58)]
        public bool? TV { get; set; }
        [Description("Stan wykończenia")]
        [Display(Order = 97)]
        public string? ApartmentFinishCondition { get; set; }


        [Description("Dostępne dla studentów")]
        [Display(Order = 98)]


        public bool? AvailableForStudents { get; set; }
        [Description("Tylko dla niepalących")]
        [Display(Order = 99)]
        public bool? OnlyForNonsmoking { get; set; }



        public string GetMedia()
        {

            List<string> single = new();
            if (Internet == true) single.Add("internet");
            if (CableTV == true) single.Add("telewizja kablowa");
            if (HomePhone == true) single.Add("telefon stacjonarny");

            return string.Join(", ", single);

        }

        public string GetAdditionalInfo()
        {

            List<string> single = new();
            if (Balcony == true) single.Add("balkon");
            if (UtilityRoom == true) single.Add("pomieszczenie użytkowe");
            if (ParkingSpace == true) single.Add("miejsce parkingowe");
            if (Basement == true) single.Add("piwnica");
            if (Garden == true) single.Add("ogród");
            if (Terrace == true) single.Add("taras");
            if (Elevator == true) single.Add("winda");
            if (TwoLevel == true) single.Add("dwupoziomowy");
            if (SeparateKitchen == true) single.Add("oddzielna kuchnia");
            if (AirConditioning == true) single.Add("klimatyzacja");

            return string.Join(", ", single);

        }

        public string GetSafety()
        {

            List<string> single = new();
            if (AntiBurglaryBlinds == true) single.Add("rolety anty-włamaniowe");
            if (AntiBurglaryWindowsOrDoors == true) single.Add("drzwi lub okna anty-włamaniowe");
            if (IntercomOrVideophone == true) single.Add("domofon");
            if (MonitoringOrSecurity == true) single.Add("monitoring");
            if (AlarmSystem == true) single.Add("system alarmowy");
            if (ClosedArea == true) single.Add("teren zamknięty");

            return string.Join(", ", single);

        }

        public string GetEquipment()
        {

            List<string> single = new();
            if (Furniture == true) single.Add("meble");
            if (WashingMachine == true) single.Add("pralka");
            if (Dishwasher == true) single.Add("zmywarka");
            if (Fridge == true) single.Add("lodówka");
            if (Stove == true) single.Add("kuchenka");
            if (Oven == true) single.Add("piekarnik");
            if (TV == true) single.Add("telewizor");

            return string.Join(", ", single);

        }





        public ApartmentRentReadOfferDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus,
            string offerType, string? typeOfBuilding, string? floor, int? floorCount, string? buildingMaterial,
            string? windowsType, string? heatingType, int? rent, string? availableSinceDate, bool? internet, bool? cableTV,
            bool? homePhone, bool? balcony, bool? utilityRoom, bool? parkingSpace, bool? basement, bool? garden, bool? terrace, bool? elevator,
            bool? twoLevel, bool? separateKitchen, bool? airConditioning, bool? availableForStudents, bool? onlyForNonsmoking,
            bool? antiBurglaryBlinds, bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone, bool? monitoringOrSecurity,
            bool? alarmSystem, bool? closedArea, bool? furniture, bool? washingMachine, bool? dishwasher, bool? fridge, bool? stove,
            bool? oven, bool? tV, string? apartmentFinishCondition)
        {
            OfferTitle = offerTitle;
            Price = price;
            SellerType = sellerType;
            Voivodeship = voivodeship;
            City = city;
            Address = address;
            Description = description;
            RemoteControl = remoteControl;
            OfferStatus = offerStatus;

            SellerType = offerType;
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
            AvailableForStudents = availableForStudents;
            OnlyForNonsmoking = onlyForNonsmoking;
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
            ApartmentFinishCondition = apartmentFinishCondition;
        }

        public ApartmentRentReadOfferDTO()
        {
        }
    }
}
