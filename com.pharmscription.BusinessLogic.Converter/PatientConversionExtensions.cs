using System;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.BusinessLogic.Converter
{
    using System.Globalization;

    public static class PatientConversionExtensions
    {
        public static PatientDto ConvertToDto(this InsurancePatient patient)
        {
            if (patient == null) return null;
            return new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
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
                },
                Insurance = patient.Insurance
            };
        }

        public static PatientDto ConvertToDto(this Patient patient)
        {
            if (patient == null) return null;
            var patientDto = new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
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
                patientDto.Address = patient.Address.ConvertToDto();
            }
            return patientDto;
        }

        public static Patient ConvertToEntity(this PatientDto patientDto)
        {
            if (patientDto == null) return null;
            var patient = new Patient
            {
                PhoneNumber = patientDto.PhoneNumber,
                BirthDate = DateTime.ParseExact(patientDto.BirthDate, PharmscriptionConstants.DateFormat, CultureInfo.InvariantCulture),
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

                patient.Address = patientDto.Address.ConvertToEntity();
            }
            return patient;
        }
    }
}
