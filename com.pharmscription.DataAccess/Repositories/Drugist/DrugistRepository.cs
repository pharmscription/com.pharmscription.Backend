namespace com.pharmscription.DataAccess.Repositories.Drugist
{
    using com.pharmscription.DataAccess.Entities.DrugistEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class DrugistRepository : Repository<Drugist>, IDrugistRepository
    {
        public DrugistRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
