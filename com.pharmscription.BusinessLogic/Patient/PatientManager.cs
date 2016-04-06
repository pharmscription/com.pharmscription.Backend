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

        public PatientDto Lookup(string ahvNumber)
        {
            InsuranceConnector connector = new InsuranceConnector();
            InsurancePatient insurancePatient = connector.GetInsuranceConnection().FindPatient(ahvNumber);
            return PatientConverter.Convert(insurancePatient);
        }

        public PatientDto Add(PatientDto patient)
        {
            _patientRepository.Add(PatientConverter.Convert(patient));
            var addedPatient = _patientRepository.GetByAhvNumber(patient.AhvNumber).Result;
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

        public PatientDto GetById(string id)
        {
            List<DataAccess.Entities.PatientEntity.Patient> list = null;
            Guid gid;
            if (Guid.TryParse(id, out gid))
            {
                list = _patientRepository.Find(gid).ToList();
            }
            if (list != null && list.Capacity == 1)
            {
                return PatientConverter.Convert(list[0]);
            }
            throw new InvalidDataException();
        }

        public PatientDto RemoveById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
