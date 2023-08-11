using Inżynierka.Shared.DTOs.UserPreferenceForm;
using Inżynierka.Shared.Entities;
using Inżynierka_Common.ServiceRegistrationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.IRepositories
{
    [ScopedRegistrationWithInterface]
    public interface IUserPreferenceFormRepository : IBaseRepository<UserPreferenceForm>
    {
        public IEnumerable<UserPreferenceForm>? GetMyForms(int agentId);
        public IEnumerable<SingleFormProposal>? GetFormsProposals(int formId);
        public List<int>? GetThreeAgentsForUserPreferenceForm(UserPreferenceFormCreateDTO dto, out string extraMessage);
        public UserPreferenceForm? GetForm(int formId);
    }
}

