using Inżynierka.Shared.Entities;
using Inżynierka.Shared.Repositories;
using Inżynierka_Common.ServiceRegistrationAttributes;

namespace Inżynierka.Shared.IRepositories
{
    [ScopedRegistrationWithInterface]
    public interface ISingleFormProposalRepository : IBaseRepository<SingleFormProposal>
    {
        public SingleFormProposal? GetProposal(int formId, int offerId);

        public void DeleteAndSaveChanges(SingleFormProposal proposal);
    }
}

