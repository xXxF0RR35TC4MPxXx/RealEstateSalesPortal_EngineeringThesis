using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.DTOs.Agency
{
    public class AgencyUpdateDTO
    {

        public string AgencyName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string? Description { get; set; }

        public string? NIP { get; set; }

        public string? REGON { get; set; }

        public string? LicenceNumber { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
