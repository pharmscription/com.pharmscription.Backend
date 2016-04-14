using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.AccountEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.Security.SessionStore;

namespace com.pharmscription.Security
{

    public class LoginProvider : ILoginProvider
    {
        private const int ROUNDS = 10000;
        private const int SALT_SIZE = 10;
        private const int TOKEN_VALIDITY_MILLISECONDS = 1000 * 60 * 60; //1 hour 
        private const int SESSION_ID_SIZE = 64;

        private readonly ISessionStore _sessionStore;
        private readonly SHA256Managed _crypt = new SHA256Managed();
        //private readonly List<IRepository<EntityWithAccount>> _repositories;
        private readonly IRepository<Patient> _patientRepository;

        public LoginProvider(/*List<IRepository<EntityWithAccount>> repositories, */IPatientRepository patientRepository, ISessionStore sessionStore)
        {
            _sessionStore = sessionStore;//new MemorySessionStore();
            _patientRepository = patientRepository;
            //_repositories = new List<IRepository<EntityWithAccount>>();
            //_repositories.Add(patientRepository);

        }

        public bool RegisterPatient(string username, string password)
        {
            string salt = CreateRandom(SALT_SIZE);
            string pwhash = CreatePasswordHash(password, salt);

            if (_patientRepository.GetFiltered(f => f.Username == username).Count() == 1)
            {
                var patient = _patientRepository.GetFiltered(f => f.Username == username).First();
                patient.PasswordHash = pwhash;
                _patientRepository.UnitOfWork.Commit();
                return true;
            }

            return false;
        }

        public async Task<Session> Authenticate(string username, string password)
        {
            return await Task.Factory.StartNew(() =>
            {
                var account = FindAccount(username, password, _patientRepository);
                if (account != null)
                {
                    return DoLogin(username, account);
                }

                return null;
            });
        }

        private Session DoLogin(string username, EntityWithAccount account)
        {
            string sessionId = CreateRandom(SESSION_ID_SIZE);

            Session session = new Session
            {
                Username = username,
                Token = sessionId,
                IssuedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddMilliseconds(TOKEN_VALIDITY_MILLISECONDS),
                Role = account.Role,
                Account = account
            };

            _sessionStore.AddSessionToken(session);
            return session;
        }

        private EntityWithAccount FindAccount(string username, string password, IRepository<Patient> repo)
        {
            foreach (var account in repo.GetFiltered(t => t.Username == username))
            {
                string pwhash = CreatePasswordHash(password, account.Salt);
                if (account.PasswordHash == pwhash)
                {
                    return account;
                }
            }
            return null;
        }


        public Session Authenticate(string token)
        {
            if (ValidateToken(token))
            {
                var session = _sessionStore.Session(token);
                if (session != null)
                {
                    return session;
                }
            }

            return null;
        }

        public bool ValidateToken(string token)
        {
            var session = _sessionStore.Session(token);
            if (session != null)
            {
                if ((DateTime.Now < session.ExpiresOn))
                {
                    session.ExpiresOn = session.ExpiresOn.AddSeconds(TOKEN_VALIDITY_MILLISECONDS);
                    return true;
                }
                _sessionStore.RemoveSessionToken(token);
            }
            return false;
        }

        public void Logout(string token)
        {
            _sessionStore.RemoveSessionToken(token);
        }

        private string CreateRandom(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        private string CreatePasswordHash(string pwd, string salt)
        {
            string password = string.Concat(salt, ".", pwd, ".", salt);

            for (var i = 0; i < ROUNDS; i++)
            {
                password = Sha256(password);
            }
            return password;
        }

        private string Sha256(string password)
        {
            var hash = new StringBuilder();
            byte[] crypto = _crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }


    }
}
