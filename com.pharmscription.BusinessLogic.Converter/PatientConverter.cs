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
                Address = new AddressDto
                {
                  Street = patient.Street,
                  Number = patient.StreetNumber
                } ,
                FirstName = patient.FirstName
            };
        }


    }
}
