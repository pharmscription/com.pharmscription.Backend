﻿using System;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using System.Collections.Generic;
    using Entities.PatientEntity;
    using Entities.PrescriptionEntity;

    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient> GetByAhvNumber(string ahvNumber);
        bool Exists(string ahvNumber);
        Task<Patient> GetWithPrescriptions(Guid id);
        Task<IEnumerable<Patient>> GetWithAllNavs(Func<Patient, bool> predicate);
        Task<ICollection<Prescription>> GetPrescriptions(Guid id);
        Task<Patient> GetWithAllNavs(Guid id);

        Task<ICollection<Patient>> GetAllWithUnreportedDispenses(DateTime lastRespectedDate);
    }
}
