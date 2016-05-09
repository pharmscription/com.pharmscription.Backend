using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess
{
    using com.pharmscription.DataAccess.Identity;

    public class PharmscriptionDataAccess : IPharmscriptionDataAccess
    {
        public PhaIdentityDbContext IdentityDbContext => new PhaIdentityDbContext();
    }
}
