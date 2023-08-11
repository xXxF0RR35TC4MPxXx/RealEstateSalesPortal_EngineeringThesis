using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.Entities
{
    [Table("UserMessage")]
    public class UserMessage
    {
        [Key]
        public int Id { get; set; }

        public int OfferId { get; set; }
        public string EstateAndOfferType { get; set; }



        public int SellerId { get; set; }

        [ForeignKey("SellerId ")]
        public virtual User Seller { get; set; }

        

        [DataType(DataType.Text)]
        public string ClientName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression( "^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$", ErrorMessage = "Email is required and must be properly formatted.")]
        [MaxLength(60)]
        public string ClientEmail { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawdziwy numer telefonu.")]
        [Phone]
        public string ClientPhoneNumber { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(1000, ErrorMessage="Maksymalna długość wiadomości to 1000 znaków.")]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public bool IsResponded { get; set; }
    }
}
