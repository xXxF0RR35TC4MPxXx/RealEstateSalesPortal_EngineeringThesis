namespace Inżynierka.Shared.ViewModels.User
{
    public class ConfirmUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid ConfirmationGuid { get; set; }
    }
}
