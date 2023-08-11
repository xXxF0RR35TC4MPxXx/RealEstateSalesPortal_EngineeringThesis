using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.DTOs.User
{

    public class UserEventDeleteDTO
    {
        public UserEventDeleteDTO(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
