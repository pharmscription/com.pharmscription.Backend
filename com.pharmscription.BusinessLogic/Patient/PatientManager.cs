using System;
using System.IO;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.BusinessLogic.Validation;
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
            AhvValidator ahvValidator = new AhvValidator();
            ahvValidator.Validate(ahvNumber);

            InsuranceConnector connector = new InsuranceConnector();
            InsurancePatient insurancePatient = await connector.GetInsuranceConnection().FindPatient(ahvNumber);
            return insurancePatient.ConvertToDto();
        }

        public async Task<PatientDto> Add(PatientDto patient)
        {
            AhvValidator ahvValidator = new AhvValidator();
            ahvValidator.Validate(patient);
            _patientRepository.Add(patient.ConvertToEntity());
            await _patientRepository.UnitOfWork.CommitAsync();
            return (await _patientRepository.GetByAhvNumber(patient.AhvNumber)).ConvertToDto();
        }

        public async Task<PatientDto> Edit(PatientDto patient)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientDto> Find(string ahvNumber)
        {
            if (_patientRepository.Exists(ahvNumber))
            {
                return (await _patientRepository.GetByAhvNumber(ahvNumber)).ConvertToDto();
            }

            return null;
        }

        public async Task<PatientDto> GetById(string id)
        {
            
            Guid gid;
            if (Guid.TryParse(id, out gid))
            {
                var patient = await _patientRepository.GetAsync(gid);
                return patient.ConvertToDto();
            }
            
            throw new InvalidDataException();
        }

        public Task<PatientDto> RemoveById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
