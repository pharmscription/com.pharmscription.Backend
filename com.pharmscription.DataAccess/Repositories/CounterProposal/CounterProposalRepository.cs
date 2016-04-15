namespace com.pharmscription.DataAccess.Repositories.CounterProposal
{
    using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.SharedInterfaces;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class CounterProposalRepository : Repository<CounterProposal>, ICounterProposalRepository
    {
        public CounterProposalRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}