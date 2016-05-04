using System;
using System.Diagnostics.CodeAnalysis;
using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.BusinessLogic.Converter
{
    using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
    using com.pharmscription.Infrastructure.Exception;

    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:StatementMustNotBeOnSingleLine", Justification = "Newline would not increase readability of code")]
    public static class PatientConversionExtensions
    {
        public static PatientDto ConvertToDto(this InsurancePatient patient)
        {
            if (patient == null) { return null; }
            return new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate.ToString(PharmscriptionConstants.DateFormat),
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
            if (patient == null) { return null; }
            var patientDto = new PatientDto
            {
                PhoneNumber = patient.PhoneNumber,
                BirthDate = patient.BirthDate.ToString(PharmscriptionConstants.DateFormat),
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
                    Id = patient.Address.Id.ToString(),
                    Street = patient.Address.Street,
                    Number = patient.Address.Number,
                    Location = patient.Address.Location,
                    CityCode = patient.Address.CityCode.CityCode,
                    StreetExtension = patient.Address.StreetExtension,
                    State = patient.Address.State
                };   
            }

            if (patient.User != null)
            {
                patientDto.Identity = new IdentityDto
                {
                    Id = patient.User.Id.ToString(),
                    UserName = patient.User.UserName
                };
            }

            return patientDto;
        }

        public static Patient ConvertToEntity(this PatientDto patientDto)
        {
            if (patientDto == null) { return null; }
            var patient = new Patient
            {
                PhoneNumber = patientDto.PhoneNumber,
                BirthDate = DateTime.ParseExact(patientDto.BirthDate, PharmscriptionConstants.DateFormat, System.Globalization.CultureInfo.InvariantCulture),
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
                    Id = Guid.Parse(patientDto.Address.Id),
                    Street = patientDto.Address.Street,
                    Number = patientDto.Address.Number,
                    Location = patientDto.Address.Location,
                    CityCode = SwissCityCode.CreateInstance(patientDto.Address.CityCode),
                    StreetExtension = patientDto.Address.StreetExtension,
                    State = patientDto.Address.State,
                };
                if (patientDto.Address.Id != null)
                {
                    patient.Address.Id = Guid.Parse(patientDto.Address.Id);
                }
            }
            if (patientDto.Identity != null && !string.IsNullOrEmpty(patientDto.Identity.UserName))
            {
                patient.User = new IdentityUser
                                   {
                                       Id = Guid.Parse(patientDto.Identity.Id),
                                       UserName = patientDto.Identity.UserName
                                   };
            }

            return patient;
        }
    }
}
