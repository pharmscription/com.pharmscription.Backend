using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess
{
    using com.pharmscription.DataAccess.Identity;
    
    public interface IPharmscriptionDataAccess
    {
        PhaIdentityDbContext IdentityDbContext { get; }
    }
}
