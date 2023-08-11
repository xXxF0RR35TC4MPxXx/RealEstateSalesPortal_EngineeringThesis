using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Common.Enums;
using System.Diagnostics;
using System.ComponentModel;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class RoomRentingReadOfferDTO : ReadOfferDTO
    {
        public string GetMedia()
        {
            List<string> single = new();
            if (Internet == true) single.Add("internet");
            if (CableTV == true) single.Add("telewizja kablowa");
            if (HomePhone == true) single.Add("telefon stacjonarny");

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

        [Description("Dodatkowe opłaty")]
        [Display(Order = 25)]
        public int? AdditionalFees { get; set; } //opłaty dodatkowe

        [Description("Kaucja")]
        [Display(Order = 26)]
        public int? Deposit { get; set; } //kaucja

        [Description("Liczba współlokatorów")]
        [Display(Order = 27)]
        public int? NumberOfPeopleInTheRoom { get; set; }

        [Description("Rodzaj zabudowy")]
        [Display(Order = 28)]
        public string? TypeOfBuilding { get; set; } //rodzaj zabudowy

        [Description("Piętro")]
        [Display(Order = 29)]
        public string? Floor { get; set; } //piętro

        [Description("Dostępne od")]
        [Display(Order = 30)]
        public string? AvailableFromDate { get; set; }

        [Description("Dostępne dla studentów")]
        [Display(Order = 31)]
        public bool? AvailableForStudents { get; set; }

        [Description("Tylko dla niepalących")]
        [Display(Order = 32)]
        public bool? OnlyForNonsmoking { get; set; }


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

        // ======= media ========
        [Description("Internet")]
        [Display(Order = 33)]
        public bool? Internet { get; set; }

        [Description("Telewizja kablowa")]
        [Display(Order = 34)]
        public bool? CableTV { get; set; }

        [Description("Telefon domowy")]
        [Display(Order = 35)]
        public bool? HomePhone { get; set; }

        public RoomRentingReadOfferDTO(string offerTitle, int price, string sellerType,
            string voivodeship, string city, string address, string description, bool? remoteControl, string offerStatus, string offerType,
            int? additionalFees, int? deposit, int? numberOfPeopleInTheRoom, string? typeOfBuilding,
            string? availableFromDate, bool? availableForStudents, bool? onlyForNonsmoking, bool? furniture, bool? washingMachine,
            bool? dishwasher, bool? fridge, bool? stove, bool? oven, bool? tV, bool? internet, bool? cableTV, bool? homePhone, string? floor)
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
            OfferType = offerType;

            AdditionalFees = additionalFees;
            Deposit = deposit;
            NumberOfPeopleInTheRoom = numberOfPeopleInTheRoom;
            TypeOfBuilding = typeOfBuilding;
            AvailableFromDate = availableFromDate;
            AvailableForStudents = availableForStudents;
            OnlyForNonsmoking = onlyForNonsmoking;
            Furniture = furniture;
            WashingMachine = washingMachine;
            Dishwasher = dishwasher;
            Fridge = fridge;
            Stove = stove;
            Oven = oven;
            TV = tV;
            Internet = internet;
            CableTV = cableTV;
            HomePhone = homePhone;
            Floor = floor;
        }

        public RoomRentingReadOfferDTO()
        {
        }
    }
}
