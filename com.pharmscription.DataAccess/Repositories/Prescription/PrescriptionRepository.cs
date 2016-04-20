using System;
using System.Collections.Generic;
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
    }
}
