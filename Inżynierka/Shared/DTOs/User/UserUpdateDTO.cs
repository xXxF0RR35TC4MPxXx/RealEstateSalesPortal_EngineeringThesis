using Inżynierka.Shared.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inżynierka.Shared.DTOs.User
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public byte[]? Avatar { get; set; }

        
        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? Street { get; set; }

        public UserUpdateDTO(int id, string? phoneNumber, string? firstName, string? lastName, string? description, byte[]? avatar, string? city, string? postalCode, string? street)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Description = description;
            Avatar = avatar;
            City = city;
            PostalCode = postalCode;
            Street = street;
        }

        public UserUpdateDTO()
        {

        }
    }
}