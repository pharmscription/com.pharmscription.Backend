using System;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.BusinessLogic.Validation;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;
using com.pharmscription.Infrastructure.Exception;

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
            if (patient == null)
            {
                throw new InvalidArgumentException("DTO must not be null");
            }
            AhvValidator ahvValidator = new AhvValidator();
            BirthDateValidator birthDateValidator = new BirthDateValidator();
            ahvValidator.Validate(patient);
            birthDateValidator.Validate(patient);
            if (_patientRepository.Exists(patient.AhvNumber))
            {
                throw new InvalidArgumentException("Patient with such an Ahv Number already exists");
            }
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
            AhvValidator ahvValidator = new AhvValidator();
            ahvValidator.Validate(ahvNumber);
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
                var patient = await _patientRepository.GetWithAllNavs(gid);
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
