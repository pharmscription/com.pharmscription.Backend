﻿using System;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.BusinessLogic.Patient
{
    public class PatientManager : CoreWorkflow, IPatientManager
    {
        private IPatientRepository _patientRepository;

        public PatientManager(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

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
            if (_patientRepository.Exists(ahvNumber))
            {
                return PatientConverter.Convert(_patientRepository.GetByAhvNumber(ahvNumber));
            }

            return null;
        }
    }
}
