using System;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Validation
{
    using System.Globalization;

    public class PrescriptionValidator: IValidator<PrescriptionDto>
    {
        public void Validate(PrescriptionDto dto)
        {
            if (dto == null)
            {
                throw new InvalidArgumentException("Null Prescription is not allowed");
            }
            if (dto.Type != "N" && dto.Type != "S")
            {
                throw new InvalidArgumentException("No valid Type was provided");
            }
            if (dto.ValidUntil == "01.01.0001")
            {
                throw new InvalidArgumentException("No valid ValidUntil Date was supplied");
            }
            try
            {
                DateTime.Parse(dto.ValidUntil, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                throw new InvalidArgumentException("No valid ValidUntil Date was supplied");
            }
        }
    }
}
