﻿using Inżynierka.Shared.Entities;
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
    public class BusinessPremisesSaleUpdateOfferDTO: OfferUpdateDTO
    {
        public PremisesLocation? Location { get; set; }

        public Floor? Floor { get; set; }

        public int? YearOfConstruction { get; set; }

        public PremisesFinishCondition? FinishCondition { get; set; }


        // ========== zabezpieczenia ===========
        public bool? AntiBurglaryBlinds { get; set; }
        public bool? AntiBurglaryWindowsOrDoors { get; set; }
        public bool? IntercomOrVideophone { get; set; }
        public bool? MonitoringOrSecurity { get; set; }
        public bool? AlarmSystem { get; set; }
        public bool? ClosedArea { get; set; }


        // ========== przeznaczenie lokalu ===========
        public bool? Service { get; set; }
        public bool? Gastronomic { get; set; }
        public bool? Office { get; set; }
        public bool? Industrial { get; set; }
        public bool? Commercial { get; set; }
        public bool? Hotel { get; set; }


        // ======== media =========
        public bool? Internet { get; set; }
        public bool? CableTV { get; set; }
        public bool? HomePhone { get; set; }
        public bool? Water { get; set; }
        public bool? Electricity { get; set; }
        public bool? SewageSystem { get; set; }
        public bool? Gas { get; set; }
        public bool? SepticTank { get; set; }
        public bool? SewageTreatmentPlant { get; set; }


        // ======== dodatkowe info ==========
        public bool? Shopwindow { get; set; }
        public bool? ParkingSpace { get; set; }
        public bool? AsphaltDriveway { get; set; }
        public bool? Heating { get; set; }
        public bool? Elevator { get; set; }
        public bool? Furnishings { get; set; }
        public bool? AirConditioning { get; set; }

        public TypeOfMarket? TypeOfMarket { get; set; }

        public BusinessPremisesSaleUpdateOfferDTO(string offerTitle, int price,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus,
            PremisesLocation? location, Floor? floor, int? yearOfConstruction, PremisesFinishCondition? finishCondition,
            bool? antiBurglaryBlinds, bool? antiBurglaryWindowsOrDoors, bool? intercomOrVideophone,
            bool? monitoringOrSecurity, bool? alarmSystem, bool? closedArea, bool? service, bool? gastronomic, bool? office,
            bool? industrial, bool? commercial, bool? hotel, bool? internet, bool? cableTV, bool? homePhone, bool? water, bool? electricity,
            bool? sewageSystem, bool? gas, bool? septicTank, bool? sewageTreatmentPlant, bool? shopwindow, bool? parkingSpace,
            bool? asphaltDriveway, bool? heating, bool? elevator, bool? furnishings, bool? airConditioning, TypeOfMarket? typeOfMarket)
        {
            OfferTitle = offerTitle;
            Price = price;
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

        public BusinessPremisesSaleUpdateOfferDTO()
        {
        }
    }
}
