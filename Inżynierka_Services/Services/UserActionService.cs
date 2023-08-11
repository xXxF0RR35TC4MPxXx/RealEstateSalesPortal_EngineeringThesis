using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Repositories;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class UserActionService
    {
        private readonly UserActionRepository _userActionRepository;

        public UserActionService(UserActionRepository userActionRepository)
        {
            _userActionRepository = userActionRepository;
        }

        public void CreateUserAction(UserAction userAction)
        {
            _userActionRepository.AddAndSaveChanges(userAction);
        }
    }
}