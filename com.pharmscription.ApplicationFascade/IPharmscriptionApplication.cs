
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
    public interface IPharmscriptionApplication
    {
        ManagerFactory ManagerFactory(string sessionId);
        ManagerFactory ManagerFactory();
        Task<Session> Authenticate(string username, string password);

    }
}
