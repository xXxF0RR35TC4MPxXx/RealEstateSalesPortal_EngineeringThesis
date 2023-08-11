using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using Inżynierka.Shared;
using Inżynierka.Shared.Repositories;

namespace Inżynierka.Shared.Repositories
{
    [ScopedRegistrationWithInterface]
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public User? GetUserByRecoveryGuid(Guid guid)
        {
            var result = _dataContext.Users.Where(x => x.PasswordRecoveryGuid == guid).FirstOrDefault();
            return result;
        }

        public string? GetUserRoleById(int id)
        {
            var result = _dataContext.Users.Where(x => x.Id == id).FirstOrDefault()?.RoleName;
            return result;
        }
        public User? GetUserByConfirmationGuid(Guid guid)
        {
            var result = _dataContext.Users.Where(x => x.ConfirmationGuid == guid).FirstOrDefault();
            return result;
        }

        public User? GetUserByEmail(string mail)
        {
            var result = _dataContext.Users.Where(x => x.Email == mail).FirstOrDefault();
            return result;
        }

        public IQueryable<User> GetAllUsers()
        {
            var result = _dataContext.Users;
            return result;
        }

        public string GetUserEmail(int id)
        {
            var result = _dataContext.Users.Where(x => x.Id == id).Select(x => x.Email).FirstOrDefault();

            return result;
        }

        public string? GetUserPassword(string email)
        {
            var result = _dataContext.Users.Where(x => x.Email == email).Select(x => x.Password).FirstOrDefault();
            return result;
        }

        public Guid? GetUserGuidByEmail(string email)
        {
            var result = _dataContext.Users.Where(x => x.Email == email).Select(x => x.PasswordRecoveryGuid).FirstOrDefault();
            return result;
        }

        public bool CheckIfUserExist(string email)
        {
            var result = _dataContext.Users.Any(x => x.Email == email);
            return result;
        }

        public IQueryable<User> GetAllAgents(int agencyId)
        {
            var result = _dataContext.Users.Where(u => (u.AgentInAgencyId == agencyId || u.OwnerOfAgencyId == agencyId) && !u.DeletedDate.HasValue);
            return result;
        }

        public void ChangeUserPasswordByEmail(string email, string password)
        {
            var user = GetUserByEmail(email);
            user.Password = password;

            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();
        }
    }
}