using System.Threading.Tasks;
using com.pharmscription.Security.SessionStore;

namespace com.pharmscription.Security
{
    public interface ILoginProvider
    {
        Task<Session> Authenticate(string username, string password);
        Session Authenticate(string token);
        void Logout(string token);
        bool RegisterPatient(string username, string password);
    }
}
