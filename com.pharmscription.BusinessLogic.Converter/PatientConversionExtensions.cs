using System;
using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public static class PatientConversionExtensions
    {
        public static PatientDto ConvertToDto(this InsurancePatient patient)
        {
            if (patient == null) return null;
            return new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate.ToString(@"dd.MM.yyyy"),
                AhvNumber = patient.AhvNumber,
                InsuranceNumber = patient.InsuranceNumber,
                LastName = patient.LastName,
                FirstName = patient.FirstName,
                Address = new AddressDto
                {
                  Street = patient.Street,
                  Number = patient.StreetNumber,
                  Location = patient.City,
                  CityCode = patient.CityCode
                } ,
                Insurance = patient.Insurance
            };
        }

        public static PatientDto ConvertToDto(this Patient patient)
        {
            if (patient == null) return null;
            var patientDto = new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate.ToString(@"dd.MM.yyyy"),
                AhvNumber = patient.AhvNumber,
                InsuranceNumber = patient.InsuranceNumber,
                LastName = patient.LastName,
                FirstName = patient.FirstName,
                Insurance = patient.Insurance,
                EMailAddress = patient.EMailAddress,
                Id = patient.Id.ToString()
            };
            if (patient.Address != null)
            {
                patientDto.Address = new AddressDto
                {
                    Street = patient.Address.Street,
                    Number = patient.Address.Number,
                    Location = patient.Address.Location,
                    CityCode = patient.Address.CityCode.CityCode,
                    StreetExtension = patient.Address.StreetExtension
                };
            }
            return patientDto;
        }

        public static Patient ConvertToEntity(this PatientDto patientDto)
        {
            if (patientDto == null) return null;
            var patient = new Patient
            {
                PhoneNumber = patientDto.PhoneNumber,
                BirthDate = DateTime.ParseExact(patientDto.BirthDate, @"dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture),
                AhvNumber = patientDto.AhvNumber,
                InsuranceNumber = patientDto.InsuranceNumber,
                LastName = patientDto.LastName,
                FirstName = patientDto.FirstName,
                Insurance = patientDto.Insurance, 
                EMailAddress = patientDto.EMailAddress
            };
            if (patientDto.Id != null)
            {
                patient.Id = Guid.Parse(patientDto.Id);
            }
            if (patientDto.Address != null)
            {
                patient.Address = new Address
                {
                    Street = patientDto.Address.Street,
                    Number = patientDto.Address.Number,
                    Location = patientDto.Address.Location,
                    CityCode = SwissCityCode.CreateInstance(patientDto.Address.CityCode),
                    StreetExtension = patientDto.Address.StreetExtension

                };
            }
            return patient;
        }
    }
}
