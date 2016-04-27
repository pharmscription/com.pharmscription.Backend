using System;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Validation
{
    public class BirthDateValidator : IValidator<PatientDto>
    {
        public void Validate(PatientDto dto)
        {
            if (dto.BirthDate == "01.01.0001")
            {
                throw new InvalidArgumentException("No valid Birthdate was supplied");
            }
            try
            {
                 DateTime.Parse(dto.BirthDate);
            }
            catch (Exception)
            {
                throw new InvalidArgumentException("No valid Birthdate was supplied");
            }
        }
    }
}
