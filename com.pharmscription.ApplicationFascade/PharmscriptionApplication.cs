
using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic;
using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Entities.AccountEntity;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Security;
using com.pharmscription.Security.SessionStore;

namespace com.pharmscription.ApplicationFascade
{
    public class PharmscriptionApplication : IPharmscriptionApplication
    {
        private readonly ILoginProvider _loginProvider;
        private readonly ISessionStore _sessionStore = new MemorySessionStore();

        public PharmscriptionApplication()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionDataAccess().UnitOfWork;
            IPatientRepository patientRepository = new PatientRepository(puow);
            _loginProvider = new LoginProvider(patientRepository, _sessionStore);
        }

        public ManagerFactory ManagerFactory(string sessionId)
        {
            Session session = _loginProvider.Authenticate(sessionId);
            return new ManagerFactory(new Context
            {
                Session = session
            });
        }

        public ManagerFactory ManagerFactory()
        {
            return new ManagerFactory(new Context());
        }


        public async Task<Session> Authenticate(string username, string password)
        {
            return await _loginProvider.Authenticate(username, password);
        }
    }
}
