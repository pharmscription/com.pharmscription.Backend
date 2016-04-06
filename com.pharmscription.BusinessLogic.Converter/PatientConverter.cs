using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
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
                FirstName = patient.FirstName,
                Insurance = patient.Insurance
            };
            if (patient.Address != null)
            {
                patientDto.Address = new AddressDto
                {
                    Street = patient.Address.Street,
                    Number = patient.Address.Number,
                    City = patient.Address.Location,
                    CityCode = patient.Address.CityCode.CityCode,
                    StreetExtension = patient.Address.StreetExtension
                };
            }
            return patientDto;
        }

        public static Patient Convert(PatientDto patientDto)
        {
            if (patientDto == null) return null;
            var patient = new Patient
            {
                PhoneNumber = patientDto.PhoneNumber,
                BirthDate = patientDto.BirthDate,
                AhvNumber = patientDto.AhvNumber,
                InsuranceNumber = patientDto.InsuranceNumber,
                LastName = patientDto.LastName,
                FirstName = patientDto.FirstName,
                Insurance = patientDto.Insurance
            };
            if (patientDto.Address != null)
            {
                patient.Address = new Address
                {
                    Street = patientDto.Address.Street,
                    Number = patientDto.Address.Number,
                    Location = patientDto.Address.City,
                    CityCode = SwissCityCode.CreateInstance(patientDto.Address.CityCode),
                    StreetExtension = patientDto.Address.StreetExtension

                };
            }
            return patient;
        }
    }
}
