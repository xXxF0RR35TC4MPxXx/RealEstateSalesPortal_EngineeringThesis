using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities
{
    [Table("UserPreferenceForm")]
    public class UserPreferenceForm
    {
        [Key]
        public int Id { get; set; }

        public Guid EmailVerificationGuid { get; set; }

        public int AgentId { get; set; }

        [ForeignKey("AgentId")]
        public virtual User Agent { get; set; }
        public virtual ICollection<SingleFormProposal>? SingleFormProposals{ get; set; }

        [DataType(DataType.Text)]
        public string? City { get; set; }

        [DataType(DataType.Text)]
        public string? OfferType { get; set; } //Sale or rent

        [DataType(DataType.Text)]
        public string EstateType { get; set; } //house, apartment etc.

        public int? MinArea { get; set; }
        public int? MaxArea { get; set; }

        public int? RoomCount { get; set; }
        public int? MaxPrice { get; set; }

        [DataType(DataType.Text)]
        public string ClientEmail { get; set; }

        [DataType(DataType.Text)]
        public string ClientPhone{ get; set; }

        public string? ClientComment { get; set; }
    }
}
