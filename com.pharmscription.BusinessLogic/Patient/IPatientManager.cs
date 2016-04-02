using System;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Patient
{
    public interface IPatientManager
    {
        PatientDto Lookup(string ahvNumber);
        PatientDto Add(PatientDto patient);
        PatientDto Edit(PatientDto patient);
        Task<PatientDto> Find(string ahvNumber);
        PatientDto GetById(string id);
        PatientDto RemoveById(string id);
    }
}

