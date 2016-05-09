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
            return Set.Where(e => e.Patient.Id == patientId).ToListAsync();
        }

        public IQueryable<Prescription> GetWithAllNavs()
        {
            return Set.Include(e => e.Dispenses).Include(e => e.CounterProposals).Include(e => e.Patient);
        }
    }
}
