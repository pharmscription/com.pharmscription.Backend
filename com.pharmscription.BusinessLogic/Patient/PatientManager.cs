using System;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.BusinessLogic.Validation;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.BusinessLogic.Patient
{
    using Infrastructure.Exception;

    public class PatientManager : CoreWorkflow, IPatientManager
    {
        private readonly IPatientRepository _patientRepository;

        public PatientManager(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientDto> Lookup(string ahvNumber)
        {
            if (ahvNumber == null)
            {
                throw new InvalidArgumentException("ahvNumber must not be null");
            }
            AhvValidator ahvValidator = new AhvValidator();
            ahvValidator.Validate(ahvNumber);

            InsuranceConnector connector = new InsuranceConnector();
            InsurancePatient insurancePatient = await connector.GetInsuranceConnection().FindPatient(ahvNumber);
            return insurancePatient.ConvertToDto();
        }

        public async Task<PatientDto> Add(PatientDto patient)
        {
            if (patient == null)
            {
                throw new InvalidArgumentException("DTO must not be null");
            }
            AhvValidator ahvValidator = new AhvValidator();
            BirthDateValidator birthDateValidator = new BirthDateValidator();
            ahvValidator.Validate(patient);
            birthDateValidator.Validate(patient);
            _patientRepository.Add(patient.ConvertToEntity());
            await _patientRepository.UnitOfWork.CommitAsync();
            return (await _patientRepository.GetByAhvNumber(patient.AhvNumber)).ConvertToDto();
        }

        public Task<PatientDto> Edit(PatientDto patient)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientDto> Find(string ahvNumber)
        {
            if (ahvNumber == null)
            {
                throw new InvalidArgumentException("ahvNumber must not be null");
            }
            if (_patientRepository.Exists(ahvNumber))
            {
                return (await _patientRepository.GetByAhvNumber(ahvNumber)).ConvertToDto();
            }
            throw new NotFoundException("Patient with AHV number " + ahvNumber + " not found");
        }

        public async Task<PatientDto> GetById(string id)
        {
            Guid gid;
            if (Guid.TryParse(id, out gid))
            {
                var patient = await _patientRepository.GetAsyncOrThrow(gid);
                return patient.ConvertToDto();
            }
            throw new InvalidArgumentException("Id " + id + " not found");
        }

        public Task<PatientDto> RemoveById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
