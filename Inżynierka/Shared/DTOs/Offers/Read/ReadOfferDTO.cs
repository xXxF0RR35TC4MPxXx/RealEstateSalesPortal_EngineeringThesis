using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Inżynierka.Shared.DTOs.Offers.Read
{
    public class ReadOfferDTO
    {
        [Description("ID Ogłoszenia")]
        [Display(Order = 0)]
        public int Id { get; set; }

        [Description("Tytuł ogłoszenia")]
        [Display(Order = 21)]
        public string OfferTitle { get; set; }
        [Description("Miasto")]
        [Display(Order = 2)]
        public string? City { get; set; }
        [Description("Województwo")]
        [Display(Order = 3)]
        public string? Voivodeship { get; set; }
        [Description("Adres")]
        [Display(Order = 4)]
        public string? Address { get; set; }
        [Description("Cena")]
        [Display(Order = 5)]
        public int Price { get; set; } //cena całkowita
        [Description("Cena za m²")]
        [Display(Order = 6)]
        public int PriceForOneSquareMeter { get; set; } //cena za m2
        [Description("Liczba pokoi")]
        [Display(Order = 7)]
        public int? RoomCount { get; set; } //liczba pokoi
        [Description("Powierzchnia lokalu (m²)")]
        [Display(Order = 8)]
        public int? Area { get; set; } //powierzchnia domu
        [Description("Obsługa zdalna")]
        [Display(Order = 9)]
        public bool? RemoteControl { get; set; } //obsługa zdalna
        [Description("Opis tekstowy")]
        [Display(Order = 10)]
        public string? Description { get; set; } //opis
        [Description("ID Agencji")]
        [Display(Order = 11)]

        public int? AgencyId { get; set; } //id agencji
        [Description("Nazwa agencji")]
        [Display(Order = 12)]
        public string? AgencyName { get; set; } //nazwa agencji
        [Description("Logo agencji")]
        [Display(Order = 13)]
        public byte[]? AgencyLogo { get; set; } //logo agencji
        [Description("Telefon agencji")]
        [Display(Order = 14)]
        public string? AgencyPhone { get; set; } //logo agencji
        [Description("Dodano dnia")]
        [Display(Order = 15)]
        public DateTime AddingDate { get; set; } //data dodania
        [Description("ID Sprzedawcy")]
        [Display(Order = 16)]
        public int SellerId { get; set; }
        [Description("Sprzedawca")]
        [Display(Order = 17)]
        public string SellerName { get; set; }
        [Description("Numer telefonu")]
        [Display(Order = 18)]
        public string SellerPhone { get; set; }
        [Description("Avatar sprzedawcy")]
        [Display(Order = 19)]
        public byte[]? SellerAvatar { get; set; }
        [Description("Stan oferty")]
        [Display(Order = 20)]
        public string? OfferStatus { get; set; } //stan oferty
        [Description("Typ sprzedawcy")]
        [Display(Order = 1)]
        public string SellerType { get; set; } //rodzaj oferty (Priv / agent)
        [Description("Typ oferty")]
        [Display(Order = 22)]
        public string? OfferType { get; set; } // sell / rent
        [Description("Rodzaj zabudowy")]
        [Display(Order = 23)]
        public string? EstateType { get; set; } // House/Hall/Apartment
        [Description("Zdjęcia")]
        [Display(Order = 24)]
                
        public IEnumerable<byte[]>? Photos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReadOfferDTO dTO &&
                   OfferTitle == dTO.OfferTitle &&
                   Photos == dTO.Photos &&
                   City == dTO.City &&
                   AgencyId == dTO.AgencyId &&
                   AgencyName == dTO.AgencyName &&
                   AgencyLogo == dTO.AgencyLogo &&
                   Voivodeship == dTO.Voivodeship &&
                   Address == dTO.Address &&
                   RoomCount == dTO.RoomCount &&
                   Price == dTO.Price &&
                   Area == dTO.Area &&
                   SellerType == dTO.SellerType &&
                   AddingDate == dTO.AddingDate &&
                   PriceForOneSquareMeter == dTO.PriceForOneSquareMeter;
        }
    }
}
