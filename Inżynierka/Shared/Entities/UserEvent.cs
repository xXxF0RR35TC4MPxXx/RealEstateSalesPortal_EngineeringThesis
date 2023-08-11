using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.Entities
{
    [Table("UserEvent")]
    public class UserEvent
    {
        [Key]
        public int Id { get; set; }

        public int OfferId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeletedDate { get; set; }


        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; }

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }
        

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

        [Required]
        [DataType(DataType.Text)]
        public string EventName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string EventCompletionStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DeadlineDate { get; set; }
    }
}
