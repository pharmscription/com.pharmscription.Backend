using System;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Validation
{
    public class BirthDateValidator : IValidator<PatientDto>
    {
        public void Validate(PatientDto dto)
        {
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
