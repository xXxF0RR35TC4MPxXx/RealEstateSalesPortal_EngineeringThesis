using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Inżynierka.Shared.DTOs.Offers
{
    public class OfferDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OfferTitle { get; set; }
        public string OfferCategory { get; set; }
        public string ForSaleOrForRent { get; set; }
        public IEnumerable<byte[]>? Photos { get; set; }
        public string City { get; set; }
        public string Voivodeship { get; set; }
        public string ZIPCode { get; set; }
        public string Street { get; set; }
        public int? HouseArea { get; set; } //powierzchnia domu
        public int? PlotArea { get; set; } //powierzchnia działki
        public string? BuildingType { get; set; } //rodzaj zabudowy
        public int? RoomCount { get; set; } //liczba pokoi
        public bool? RemoteControl { get; set; } //zdalna obsługa
        public string? HeatingType { get; set; } //rodzaj ogrzewania
        public string? FinishCondition { get; set; } //stan wykończenia
        public int? YearOfConstruction { get; set; } //rok budowy
        public bool? ParkingSpace { get; set; } //miejsce parkingowe
        public string? BuildingLocation { get; set; } //położenie budynku (za miastem itp.)
        public decimal TotalPrice { get; set; } //cena całkowita
        public decimal? PriceForOneSquareMeter { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OfferDTO dTO &&
                   UserId == dTO.UserId &&
                   OfferTitle == dTO.OfferTitle &&
                   OfferCategory == dTO.OfferCategory &&
                   ForSaleOrForRent == dTO.ForSaleOrForRent &&
                   City == dTO.City &&
                   Voivodeship == dTO.Voivodeship &&
                   ZIPCode == dTO.ZIPCode &&
                   Street == dTO.Street &&
                   HouseArea == dTO.HouseArea &&
                   PlotArea == dTO.PlotArea &&
                   BuildingType == dTO.BuildingType &&
                   RoomCount == dTO.RoomCount &&
                   RemoteControl == dTO.RemoteControl &&
                   HeatingType == dTO.HeatingType &&
                   FinishCondition == dTO.FinishCondition &&
                   YearOfConstruction == dTO.YearOfConstruction &&
                   ParkingSpace == dTO.ParkingSpace &&
                   BuildingLocation == dTO.BuildingLocation &&
                   TotalPrice == dTO.TotalPrice &&
                   PriceForOneSquareMeter == dTO.PriceForOneSquareMeter;
        }
    }
}
