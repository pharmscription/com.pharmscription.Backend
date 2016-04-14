namespace com.pharmscription.DataAccess.Repositories.CounterProposal
{
    using com.pharmscription.DataAccess.Entities.CounterProposal;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.SharedInterfaces;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class CounterProposalRepository : Repository<CounterProposalEntity>, ICounterProposalRepository
    {
        public CounterProposalRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}