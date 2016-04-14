using System;
using com.pharmscription.DataAccess.Entities.AccountEntity;

namespace com.pharmscription.Service
{
    internal class AuthorizedAttribute : Attribute
    {
        public Role Role { get; set; }
    }
}