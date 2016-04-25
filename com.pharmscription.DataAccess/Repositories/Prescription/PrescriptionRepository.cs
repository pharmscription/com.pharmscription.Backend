using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.Repositories.Prescription
{
    using Entities.PrescriptionEntity;
    using BaseRepository;
    using UnitOfWork;

    public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual Task<List<Prescription>> GetByPatientId(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Prescription> GetWithAllNavs()
        {
            return GetSet().Include(e => e.Dispenses).Include(e => e.CounterProposals).Include(e => e.Patient);
        }
    }
}
