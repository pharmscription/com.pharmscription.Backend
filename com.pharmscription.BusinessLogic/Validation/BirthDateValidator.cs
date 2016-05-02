using System;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Validation
{
    using System.Globalization;

    public class BirthDateValidator : IValidator<PatientDto>
    {
        public void Validate(PatientDto dto)
        {
            if (dto == null)
            {
                throw new InvalidArgumentException("Null Patient is not valid");
            }
            if (dto.BirthDate == "01.01.0001")
            {
                throw new InvalidArgumentException("Date must be initialized first");
            }
            try
            {
                 DateTime.Parse(dto.BirthDate, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                throw new InvalidArgumentException("No valid Birthdate was supplied");
            }
        }
    }
}
