using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.DTOs.User
{
    public class ReadEventDTO
    {
        public int Id { get; set; }
        public int OfferId { get; set; }

        public int SellerId { get; set; }

        public string? ClientName { get; set; }

        public string? ClientEmail { get; set; }

        public string? ClientPhoneNumber { get; set; }
        public string? EventCompletionStatus { get; set; }
        public string? EventName { get; set; }

        public DateTime DeadlineDate { get; set; }

        public DateTime EndDate { get; set; }

        public ReadEventDTO()
        {

        }
    }
}
