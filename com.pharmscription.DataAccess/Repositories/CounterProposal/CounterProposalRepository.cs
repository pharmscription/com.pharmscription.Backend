namespace com.pharmscription.DataAccess.Repositories.CounterProposal
{
    using Entities.CounterProposalEntity;
    using BaseRepository;
    using UnitOfWork;

    public class CounterProposalRepository : Repository<CounterProposal>, ICounterProposalRepository
    {
        public CounterProposalRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}