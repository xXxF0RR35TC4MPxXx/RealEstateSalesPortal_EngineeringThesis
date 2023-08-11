using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inżynierka.Shared.DTOs.User
{
    public class UserEventUpdateDTO
    {
        public string? ClientName { get; set; }

        public string? ClientEmail { get; set; }

        public string? ClientPhoneNumber { get; set; }

        public string? EventName { get; set; }

        public string? EventCompletionStatus { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public UserEventUpdateDTO(string? clientName, string? clientEmail, string? clientPhoneNumber, 
            string? eventName, string? eventCompletionStatus, DateTime? deadlineDate)
        {
            ClientName = clientName;
            ClientEmail = clientEmail;
            ClientPhoneNumber = clientPhoneNumber;
            EventName = eventName;
            EventCompletionStatus = eventCompletionStatus;
            DeadlineDate = deadlineDate;
        }

        public UserEventUpdateDTO()
        {
        }
    }
}
