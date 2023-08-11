using Inżynierka_Common.Enums;
using Inżynierka_Common.Listing;

namespace Inżynierka.Shared.ViewModels.UserEvents
{
    public class UserEventsFilterViewModel
    {
        public int OfferId { get; set; }

        public int SellerId { get; set; }

        public string? ClientName { get; set; }

        public string? ClientEmail { get; set; }

        public string? ClientPhoneNumber { get; set; }

        public string? EventName { get; set; }

        public DateTime DeadlineDate { get; set; }
    }
}
