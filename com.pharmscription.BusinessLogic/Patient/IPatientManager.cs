using System;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Patient
{
    public interface IPatientManager
    {
        PatientDto Lookup(String ahvNumber);
        PatientDto Add(PatientDto patient);
        PatientDto Edit(PatientDto patient);
        PatientDto Find(String ahvNumber);
    }
}

