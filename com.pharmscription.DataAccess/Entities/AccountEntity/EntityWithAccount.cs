using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.BaseEntity;

namespace com.pharmscription.DataAccess.Entities.AccountEntity
{
    public class EntityWithAccount : Entity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string SharedSecret { get; set; }
        public Role Role { get; set; }
    }
}
