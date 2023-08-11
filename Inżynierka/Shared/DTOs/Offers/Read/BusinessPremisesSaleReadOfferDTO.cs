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
    public class BusinessPremisesSaleReadOfferDTO : ReadOfferDTO
    {
        [Description("Lokalizacja")]
        [Display(Order = 25)]
        public string? Location { get; set; }

        [Description("Piętro")]
        [Display(Order = 26)]
        public string? Floor { get; set; }

        [Description("Rok konstrukcji")]
        [Display(Order = 27)]
        public int? YearOfConstruction { get; set; }

        [Description("Stan wykończenia")]
        [Display(Order = 28)]
        public string? FinishCondition { get; set; }


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


        // ========== przeznaczenie lokalu ===========
        public string GetDesignation()
        {

            List<string> single = new();
            if (Service == true) single.Add("usługowy");
            if (Gastronomic == true) single.Add("gastronomiczny");
            if (Office == true) single.Add("biurowy");
            if (Industrial == true) single.Add("przemysłowy");
            if (Commercial == true) single.Add("handlowy");
            if (Hotel == true) single.Add("hotelowy");
            return string.Join(", ", single);
        }

        [Description("Usługowy")]
        [Display(Order = 29)]
        public bool? Service { get; set; }
        [Description("Gastronomiczny")]
        [Display(Order = 30)]
        public bool? Gastronomic { get; set; }
        [Description("Biurowy")]
        [Display(Order = 31)]
        public bool? Office { get; set; }
        [Description("Przemysłowy")]
        [Display(Order = 32)]
        public bool? Industrial { get; set; }
        [Description("Handlowy")]
        [Display(Order = 33)]
        public bool? Commercial { get; set; }
        [Description("Hotelowy")]
        [Display(Order = 34)]
        public bool? Hotel { get; set; }


        // ======== media =========
        public string GetMedia()
        {
            List<string> single = new();
            if (Internet == true) single.Add("internet");
            if (CableTV == true) single.Add("telewizja kablowa");
            if (HomePhone == true) single.Add("telefon stacjonarny");
            if (Water == true) single.Add("woda");
            if (Electricity == true) single.Add("prąd");
            if (SewageSystem == true) single.Add("kanalizacja");
            if (Gas == true) single.Add("gaz");
            if (SepticTank == true) single.Add("szambo");
            if (SewageTreatmentPlant == true) single.Add("oczyszczalnia ścieków");

            return string.Join(", ", single);
        }

        [Description("Internet")]
        [Display(Order = 35)]
        public bool? Internet { get; set; }

        [Description("Telewizja kablowa")]
        [Display(Order = 36)]
        public bool? CableTV { get; set; }

        [Description("Telefon")]
        [Display(Order = 37)]
        public bool? HomePhone { get; set; }
        [Description("Woda")]
        [Display(Order = 38)]
        public bool? Water { get; set; }
        [Description("Prąd")]
        [Display(Order = 39)]
        public bool? Electricity { get; set; }
        [Description("Kanalizacja")]
        [Display(Order = 40)]
        public bool? SewageSystem { get; set; }
        [Description("Gaz")]
        [Display(Order = 41)]
        public bool? Gas { get; set; }
        [Description("Szambo")]
        [Display(Order = 42)]
        public bool? SepticTank { get; set; }
        [Description("Oczyszczalnia ścieków")]
        [Display(Order = 43)]
        public bool? SewageTreatmentPlant { get; set; }


        public string GetAdditionalInfo()
        {
            List<string> single = new();
            if (ParkingSpace == true) single.Add("miejsce parkingowe");
            if (Elevator == true) single.Add("winda");
            if (AirConditioning == true) single.Add("klimatyzacja");
            if (Shopwindow == true) single.Add("witryna");
            if (AsphaltDriveway == true) single.Add("dojazd asfaltowy");
            if (Furnishings == true) single.Add("umeblowanie");
            if (Heating == true) single.Add("ogrzewanie");

            return string.Join(", ", single);
        }

        // ======== dodatkowe info ==========
        [Description("Witryna")]
        [Display(Order = 44)]
        public bool? Shopwindow { get; set; }
        [Description("Parking")]
        [Display(Order = 45)]
        public bool? ParkingSpace { get; set; }
        [Description("Dojazd asfalt.")]
        [Display(Order = 46)]
        public bool? AsphaltDriveway { get; set; }
        [Description("Ogrzewanie")]
        [Display(Order = 47)]
        public bool? Heating { get; set; }
        [Description("Winda")]
        [Display(Order = 48)]
        public bool? Elevator { get; set; }
        [Description("Umeblowanie")]
        [Display(Order = 49)]
        public bool? Furnishings { get; set; }
        [Description("Klimatyzacja")]
        [Display(Order = 50)]
        public bool? AirConditioning { get; set; }

        [Description("Rodzaj rynku")]
        [Display(Order = 99)]
        public string? TypeOfMarket { get; set; }

        public BusinessPremisesSaleReadOfferDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus,
            string? location, string? floor, int? yearOfConstruction, string? finishCondition,
            string offerType, bool? antiBurglaryBlinds, bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone,
            bool? monitoringOrSecurity, bool? alarmSystem, bool? closedArea, bool? service, bool? gastronomic, bool? office,
            bool? industrial, bool? commercial, bool? hotel, bool? internet, bool? cableTV, bool? homePhone, bool? water, bool? electricity,
            bool? sewageSystem, bool? gas, bool? septicTank, bool? sewageTreatmentPlant, bool? shopwindow, bool? parkingSpace,
            bool? asphaltDriveway, bool? heating, bool? elevator, bool? furnishings, bool? airConditioning, string? typeOfMarket)
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

            Location = location;
            Floor = floor;
            YearOfConstruction = yearOfConstruction;
            FinishCondition = finishCondition;
            OfferType = offerType;
            AntiBurglaryBlinds = antiBurglaryBlinds;
            AntiBurglaryWindowsOrDoors = antiBurglaryWindowsOrDoors;
            IntercomOrVideophone = intercomOrVideophone;
            MonitoringOrSecurity = monitoringOrSecurity;
            AlarmSystem = alarmSystem;
            ClosedArea = closedArea;
            Service = service;
            Gastronomic = gastronomic;
            Office = office;
            Industrial = industrial;
            Commercial = commercial;
            Hotel = hotel;
            Internet = internet;
            CableTV = cableTV;
            HomePhone = homePhone;
            Water = water;
            Electricity = electricity;
            SewageSystem = sewageSystem;
            Gas = gas;
            SepticTank = septicTank;
            SewageTreatmentPlant = sewageTreatmentPlant;
            Shopwindow = shopwindow;
            ParkingSpace = parkingSpace;
            AsphaltDriveway = asphaltDriveway;
            Heating = heating;
            Elevator = elevator;
            Furnishings = furnishings;
            AirConditioning = airConditioning;
            TypeOfMarket = typeOfMarket;
        }

        public BusinessPremisesSaleReadOfferDTO()
        {
        }
    }
}
