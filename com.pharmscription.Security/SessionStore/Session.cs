using System;
using com.pharmscription.DataAccess.Entities.AccountEntity;

namespace com.pharmscription.Security.SessionStore
{
    public class Session
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public EntityWithAccount Account { get; set; }
        public Role Role { get; set; }
        public DateTime IssuedOn { get; set; } 
        public DateTime ExpiresOn { get; set; }
        
    }
}
