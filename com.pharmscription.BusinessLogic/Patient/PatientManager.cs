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
    using DataAccess.Entities.PatientEntity;
    using GuidHelper;


    public class PatientManager : CoreWorkflow, IPatientManager
    {
        private readonly IPatientRepository _patientRepository;

        public PatientManager(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientDto> Lookup(string ahvNumber)
        {
            AhvValidator.Validate(ahvNumber);
            InsurancePatient insurancePatient = await InsuranceConnector.InsuranceConnection.FindPatient(ahvNumber);
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

        public async Task<PatientDto> Find(string ahvNumber)
        {
            AhvValidator.Validate(ahvNumber);
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
                if (!await _patientRepository.CheckIfEntityExists(gid))
                {
                    throw new NotFoundException("Patient with id: " + id + " not found");
                }
                var patient = await _patientRepository.GetWithAllNavs(gid);
                return patient.ConvertToDto();
            }
            throw new InvalidArgumentException("Id " + id + " not found");
        }

        public async Task<PatientDto> Update(PatientDto patient)
        {
            var oldPatient = await _patientRepository.GetWithAllNavs(GuidParser.ParseGuid(patient.Id));
            UpdatePatientInformation(oldPatient, patient);
            await _patientRepository.UnitOfWork.CommitAsync();
            return oldPatient.ConvertToDto();
        }

        private static void UpdatePatientInformation(Patient oldPatient, PatientDto patient)
        {
            oldPatient.EMailAddress = patient.EMailAddress;
            oldPatient.FirstName = patient.FirstName;
            oldPatient.Insurance = patient.Insurance;
            oldPatient.InsuranceNumber = patient.InsuranceNumber;
            oldPatient.LastName = patient.LastName;
            oldPatient.PhoneNumber = patient.PhoneNumber;
            oldPatient.Address = patient.Address.ConvertToEntity();
        }
    }
}
