namespace com.pharmscription.DataAccess.Entities.PhaIdentiyUser
{
    using System;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class PhaIdentityUser : IdentityUser
    {
        public Guid UserId { get; set; }
    }
}
