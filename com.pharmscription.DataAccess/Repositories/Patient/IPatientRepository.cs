﻿using System;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    public interface IPatientRepository : IRepository<Entities.PatientEntity.Patient>
    {
        Task<Entities.PatientEntity.Patient> GetByAhvNumber(string ahvNumber);
        bool Exists(string ahvNumber);
        Task<Entities.PatientEntity.Patient> GetWithPrescriptions(Guid id);
        IQueryable<Entities.PatientEntity.Patient> GetWithAllNavs();

    }
}
