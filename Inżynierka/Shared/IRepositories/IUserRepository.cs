using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;

namespace Inżynierka.Shared.IRepositories
{
    [ScopedRegistrationWithInterface]
    public interface IUserRepository : IBaseRepository<User>
    {
        string? GetUserRoleById(int id);
        void ChangeUserPasswordByEmail(string email, string password);

        IQueryable<User> GetAllAgents(int agencyId);
        bool CheckIfUserExist(string email);

        User? GetUserByRecoveryGuid(Guid guid);
        IQueryable<User> GetAllUsers();

        User? GetUserByEmail(string mail);

        User? GetUserByConfirmationGuid(Guid guid);

        string GetUserEmail(int id);

        Guid? GetUserGuidByEmail(string email);

        string? GetUserPassword(string email);

    }
}