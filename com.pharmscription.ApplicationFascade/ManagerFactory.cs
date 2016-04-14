
using com.pharmscription.BusinessLogic;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Security.SessionStore;

namespace com.pharmscription.ApplicationFascade
{
    public class ManagerFactory
    {
        private readonly Context _context;
        public ManagerFactory(Context context)
        {
            _context = context;
        }
        
        public IPatientManager PatientManager
        {
            get
            {
                IPharmscriptionUnitOfWork puow = new PharmscriptionDataAccess().UnitOfWork;
                IPatientRepository patientRepository = new PatientRepository(puow);
                return ProxyManager<IPatientManager>.Create(new PatientManager(_context, patientRepository),_context.Session);
            }
        }

        public IDrugManager DrugManager
        {
            get
            {
                IPharmscriptionUnitOfWork puow = new PharmscriptionDataAccess().UnitOfWork;
                IDrugRepository drugRepository = new DrugRepository(puow);
                return ProxyManager<IDrugManager>.Create(new DrugManager(_context, drugRepository), _context.Session);
            }
        }

    }
}
