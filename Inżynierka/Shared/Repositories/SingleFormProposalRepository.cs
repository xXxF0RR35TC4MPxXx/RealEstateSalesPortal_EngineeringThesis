using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;

namespace Inżynierka.Shared.Repositories
{
    [ScopedRegistrationWithInterface]
    public class SingleFormProposalRepository : BaseRepository<SingleFormProposal>, ISingleFormProposalRepository
    {
        private readonly DataContext _dataContext;

        public SingleFormProposalRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public SingleFormProposal? GetProposal(int formId, int offerId) 
        {
            var result = _dataContext.SingleFormProposals.Where(x => x.FormId == formId && x.OfferId == offerId).FirstOrDefault();
            return result;
        }

        public void DeleteAndSaveChanges(SingleFormProposal proposal)
        {
            Remove(proposal);
            SaveChanges();
        }
    }
}