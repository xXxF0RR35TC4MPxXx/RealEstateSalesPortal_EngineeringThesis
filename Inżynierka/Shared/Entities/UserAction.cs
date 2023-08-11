using Inżynierka.Shared.IRepositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.Entities
{
    [Table("UserAction")]
    public class UserAction : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; } 

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Controller { get; set; }

        [Required]
        public string ControllerAction { get; set; }

        [Required]
        public string ActionParameters { get; set; }
    }
}