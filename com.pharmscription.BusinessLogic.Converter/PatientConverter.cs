using System;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public class PatientConverter
    {
        public static PatientDto Convert(InsurancePatient patient)
        {
            if (patient == null) return null;
            return new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate,
                AhvNumber = patient.AhvNumber,
                InsuranceNumber = patient.InsuranceNumber,
                LastName = patient.LastName,
                FirstName = patient.FirstName,
                Address = new AddressDto
                {
                  Street = patient.Street,
                  Number = patient.StreetNumber,
                  City = patient.City,
                  CityCode = patient.CityCode
                } ,
                Insurance = patient.Insurance
            };
        }

        public static PatientDto Convert(Patient patient)
        {
            if (patient == null) return null;
            var patientDto = new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate,
                AhvNumber = patient.AhvNumber,
                InsuranceNumber = patient.InsuranceNumber,
                LastName = patient.LastName,
                FirstName = patient.FirstName
            };
            if (patient.Address != null)
            {
                patientDto.Address = new AddressDto
                {
                    Street = patient.Address.Street,
                    Number = patient.Address.Number,
                    City = patient.Address.Location,
                    CityCode = patient.Address.CityCode.CityCode
                };
            }
            return patientDto;
        }
    }
}
