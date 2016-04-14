namespace com.pharmscription.Security.SessionStore
{
    public interface ISessionStore
    {
        Session AddSessionToken(Session session);
        void RemoveSessionToken(string token);
        Session Session(string token);
    }
}
