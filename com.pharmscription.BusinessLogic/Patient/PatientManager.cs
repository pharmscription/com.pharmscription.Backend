using System;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.BusinessLogic.Patient
{
    public class PatientManager : CoreWorkflow, IPatientManager
    {
        private readonly IPatientRepository _patientRepository;

        public PatientManager(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public PatientDto Lookup(string ahvNumber)
        {
            InsuranceConnector connector = new InsuranceConnector();
            InsurancePatient insurancePatient = connector.GetInsuranceConnection().FindPatient(ahvNumber);
            return PatientConverter.Convert(insurancePatient);
        }

        public PatientDto Add(PatientDto patient)
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionDataAccess().UnitOfWork;
            
            IPatientRepository patientRepository = new PatientRepository(puow);
            
            patientRepository.Add(PatientConverter.Convert(patient));

            var addedPatient = patientRepository.GetByAhvNumber(patient.AhvNumber).Result;
            return PatientConverter.Convert(addedPatient);
        }

        public PatientDto Edit(PatientDto patient)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientDto> Find(string ahvNumber)
        {
            if (_patientRepository.Exists(ahvNumber))
            {
                return PatientConverter.Convert(await _patientRepository.GetByAhvNumber(ahvNumber));
            }

            return null;
        }
    }
}
