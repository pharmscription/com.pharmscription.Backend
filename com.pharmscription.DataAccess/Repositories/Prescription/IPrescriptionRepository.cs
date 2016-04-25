using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.Repositories.Prescription
{
    using Entities.PrescriptionEntity;
    using SharedInterfaces;

    /// <summary>
    /// The PrescriptionRepository interface.
    /// </summary>
    public interface IPrescriptionRepository: IRepository<Prescription>
    {
        Task<List<Prescription>> GetByPatientId(Guid patientId);
        IQueryable<Prescription> GetWithAllNavs();
    }
}
