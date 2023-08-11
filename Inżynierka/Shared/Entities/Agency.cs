using Inżynierka.Shared.IRepositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace Inżynierka.Shared.Entities
{
    [Table("Agency")]
    public class Agency : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }

        public virtual ICollection<User>? Agents { get;set; }

        [Required]
        [DataType(DataType.Text)]
        public string AgencyName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression("^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$", ErrorMessage = "Ciąg nie został rozpoznany jako prawidłowy adres email.")]
        [MaxLength(60)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawdziwy numer telefonu.")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        public Guid InvitationGuid { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(6)]
        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawidłowy kod pocztowy w Polsce.")]
        public string PostalCode { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [MinLength(10)]
        [MaxLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawidłowy numer NIP.")]
        public string? NIP { get; set; }

        [MinLength(9)]
        [MaxLength(14)]
        [RegularExpression(@"^\d+$", ErrorMessage="Podany ciąg nie został rozpoznany jako prawidłowy numer REGON.")]
        public string? REGON { get; set; }

        [DataType(DataType.Text)]
        public string? LicenceNumber { get; set; }

        [DataType(DataType.Text)]
        public string? AvatarFilePath { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastUpdatedDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedDate { get; set; }

    }
}