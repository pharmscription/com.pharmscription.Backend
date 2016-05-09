using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.Identity
{
    using System.Security.Claims;

    using com.pharmscription.DataAccess.Entities.PhaIdentiyUser;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class PhaIdentityDbContext : IdentityDbContext<PhaIdentityUser>
    {
        public PhaIdentityDbContext() : base("IdentityDB")
        {

        }
    }
    
}
