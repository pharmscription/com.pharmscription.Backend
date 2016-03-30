using System;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Patient
{
    public interface IPatientManager
    {
        PatientDto Lookup(String ahvNumber);
        PatientDto Add(PatientDto patient);
        PatientDto Edit(PatientDto patient);
        Task<PatientDto> Find(String ahvNumber);
    }
}

