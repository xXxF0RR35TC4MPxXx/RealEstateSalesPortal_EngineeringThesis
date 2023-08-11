using Inżynierka.Shared.IRepositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace Inżynierka.Shared.Entities
{
    [Table("User")]
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression( "^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email is required and must be properly formatted.")]
        [MaxLength(60)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string RoleName { get; set; }

        [DataType(DataType.Text)]
        public string? City { get; set; }

        [DataType(DataType.Text)]
        public string? PostalCode { get;set; }

        [DataType(DataType.Text)]
        public string? Street { get; set; }
               

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{9})$", ErrorMessage = "Podany ciąg nie został rozpoznany jako prawdziwy numer telefonu.")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        //when user is an agent of an agency

        public int? AgentInAgencyId { get; set; }

        [ForeignKey("AgentInAgencyId")]
        public virtual Agency? AgentInAgency { get; set; }

        //when user is an OWNER of an agency

        public int? OwnerOfAgencyId { get; set; }

        [ForeignKey("OwnerOfAgencyId")]
        public virtual Agency? OwnerOfAgency { get; set; }

        public virtual ICollection<UserFavourite>? UserFavourites { get; set; }
        public virtual ICollection<UserPreferenceForm>? UserPreferenceForm { get; set; }

        public virtual ICollection<Offer>? UserOffers { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<UserEvent>? UserEvents { get; set; }
        public virtual ICollection<UserMessage>? UserMessages { get; set; }

        [DataType(DataType.Text)]
        public string? AvatarFilePath { get; set; }


        public string UserStatus { get; set; }


        [Required]
        public Guid PasswordRecoveryGuid { get; set; }

        [Required]
        public Guid ConfirmationGuid { get; set; }
        


        public DateTime? LastUpdatedDate { get; set; }

        public int? DeletedById { get; set; }

        public DateTime? DeletedDate { get; set; }

        [ForeignKey("DeletedById")]
        public virtual User? DeletedBy { get; set; }
    }
}