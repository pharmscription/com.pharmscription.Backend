using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.DataAccess.Repositories.Patient;
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

        public async Task<PatientDto> Lookup(string ahvNumber)
        {
            AHVValidator ahvValidator = new AHVValidator();
            ahvValidator.Validate(ahvNumber);

            InsuranceConnector connector = new InsuranceConnector();
            InsurancePatient insurancePatient = await connector.GetInsuranceConnection().FindPatient(ahvNumber);
            return PatientConverter.Convert(insurancePatient);
        }

        public async Task<PatientDto> Add(PatientDto patient)
        {
            AHVValidator ahvValidator = new AHVValidator();
            ahvValidator.Validate(patient);

            _patientRepository.Add(PatientConverter.Convert(patient));
            return PatientConverter.Convert(await _patientRepository.GetByAhvNumber(patient.AhvNumber));
        }

        public async Task<PatientDto> Edit(PatientDto patient)
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

        public async Task<PatientDto> GetById(string id)
        {
            
            Guid gid;
            if (Guid.TryParse(id, out gid))
            {
                var patient = await _patientRepository.GetAsync(gid);
                return PatientConverter.Convert(patient);
            }
            
            throw new InvalidDataException();
        }

        public Task<PatientDto> RemoveById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
