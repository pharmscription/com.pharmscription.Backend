﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Validation
{
    public class AHVValidator : IValidator<PatientDto>
    {
        public void Validate(PatientDto dto)
        {
            Validate(dto.AhvNumber);
        }

        public void Validate(string socialNumber)
        {
            return;
            
            if (socialNumber.Length != 13)
            {
                throw new InvalidAhvNumberException("Invalid Lenght");
            }

            try
            {
                int checksum = socialNumber[socialNumber.Length - 1];
                int sum3 = 0;
                int sum1 = 0;
                for (int i = 0; i < socialNumber.Length - 1; i = i + 2)
                {
                    sum3 = sum3 + socialNumber[i];
                }
                for (int j = 1; j < socialNumber.Length - 1; j = j + 2)
                {
                    sum1 = sum1 + (socialNumber[j] * 3);
                }
                int calculatedChecksum = 10 - ((sum3 + sum1) % 10);
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
