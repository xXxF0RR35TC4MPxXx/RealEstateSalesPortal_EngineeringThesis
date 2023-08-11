using Inżynierka.Shared.IRepositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.Entities
{
    [Table("Offer")]
    public class Offer : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(100, ErrorMessage="Tytuł ogłoszenia może mieć max. 100 znaków.")]
        public string OfferTitle { get; set; }

        [Required]
        public int Price { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [MaxLength(30)]
        public string OfferType { get; set; } //sale or rent

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20)]
        public string EstateType { get; set; } //rodzaj budynku (dom, mieszkanie itp.)

        public int? RoomCount { get; set; }

        public int? Area { get; set; } //powierzchnia domu

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(20)]
        public string SellerType { get; set; }

        [Required]
        public bool VisibilityRestricted { get; set; } = false;

        [NotMapped]
        public IEnumerable<byte[]>? Photos { get; set; }

        [Required]
        [MaxLength(30)]
        [DataType(DataType.Text)]
        public string Voivodeship { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string Address { get; set; }


        [Required]
        [MaxLength(5000, ErrorMessage = "Maksymalna długość opisu oferty to 5000 znaków!")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        public bool? RemoteControl { get; set; } //zdalna obsługa

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string OfferStatus { get; set; }

        public int SellerId { get; set; }

        [Required]
        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LastEditedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeletedDate { get; set; }
    }
}
