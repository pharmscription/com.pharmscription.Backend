using System;
using com.pharmscription.business;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Insurance;

namespace com.pharmscription.BusinessLogic.Patient
{
    public class PatientManager : CoreWorkflow, IPatientManager
    {
      
        public PatientDto Lookup(String ahvNumber)
        {
            InsuranceConnector connector = new InsuranceConnector();
            InsurancePatient insurancePatient = connector.GetInsuranceConnection().FindPatient(ahvNumber);
            return PatientConverter.Convert(insurancePatient);
        }

        public PatientDto Add(PatientDto patient)
        {
            throw new NotImplementedException();
        }

        public PatientDto Edit(PatientDto patient)
        {
            throw new NotImplementedException();
        }

        public PatientDto Find(string ahvNumber)
        {
            throw new NotImplementedException();
        }
    }
}
