namespace com.pharmscription.DataAccess.Repositories.DrugstoreEmployee
{
    using com.pharmscription.DataAccess.Entities.DrugstoreEmployeeEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.Repositories.DrugStoreEmployee;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class DrugstoreEmployeeRepository : Repository<DrugstoreEmployee>, IDrugstoreEmployeeRepository
    {
        public DrugstoreEmployeeRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
