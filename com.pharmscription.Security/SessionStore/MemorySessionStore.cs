using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.Security.SessionStore
{
    public class MemorySessionStore : ISessionStore
    {
        private static readonly Dictionary<string, Session> sessionStore = new Dictionary<string, Session>(); 

        public Session AddSessionToken(Session session)
        {
            sessionStore.Add(session.Token, session);
            return session;
        }

        public void RemoveSessionToken(string token)
        {
            sessionStore.Remove(token);
        }

        public Session Session(string token)
        {
            return sessionStore.ContainsKey(token) ? sessionStore[token] : null;
        }
        
    }
}
