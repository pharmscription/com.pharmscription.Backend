using System;
using com.pharmscription.DataAccess.Entities.AccountEntity;

namespace com.pharmscription.Security
{
    public class AuthorizedAttribute : Attribute
    {
        public Role Role { get; set; }
    }
}