using System;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Validation
{
    public class AhvValidator : IValidator<PatientDto>
    {
        public void Validate(PatientDto dto)
        {
            Validate(dto.AhvNumber);
        }

        public void Validate(string socialNumber)
        {
            socialNumber = socialNumber.Replace(".", "");
            if (socialNumber.Length != 13)
            {
                throw new InvalidAhvNumberException("Invalid Lenght");
            }

            try
            {
                var checksum = socialNumber[socialNumber.Length - 1] - '0';
                var sum3 = 0;
                var sum1 = 0;
                for (var i = 0; i < socialNumber.Length - 1; i = i + 2)
                {
                    sum3 = sum3 + (socialNumber[i] - '0');
                }
                for (var j = 1; j < socialNumber.Length - 1; j = j + 2)
                {
                    sum1 = sum1 + ((socialNumber[j] - '0') * 3);
                }
                var calculatedChecksum = 10 - ((sum3 + sum1) % 10);
                if (calculatedChecksum != checksum)
                {
                    throw new InvalidAhvNumberException("Invalid Checksum. Was: " + calculatedChecksum + ", Expected:" + checksum);
                }
            }
            catch (Exception e)
            {
                throw new InvalidAhvNumberException("Error", e);
            }
        }
    }
}
