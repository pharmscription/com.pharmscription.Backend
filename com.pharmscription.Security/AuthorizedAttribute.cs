using System;
using com.pharmscription.DataAccess.Entities.AccountEntity;

namespace com.pharmscription.Security
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizedAttribute : Attribute
    {
        public Role Role { get; set; }
    }
}