namespace Inżynierka.Shared.DTOs.Agency
{
    public class AgentsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[]? Avatar { get; set; }
        public int? ActiveOffers { get; set; }

        public AgentsDTO(int id, string fullName, string email, byte[]? avatar, string phone, int activeOffers)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Avatar = avatar;
            Phone = phone;
            ActiveOffers = activeOffers;
        }

        public AgentsDTO() { }
    }
}