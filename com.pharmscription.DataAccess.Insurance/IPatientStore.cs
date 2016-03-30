using System.Collections.Generic;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.DataAccess.Insurance
{
    public interface IPatientStore
    {
        List<InsurancePatient> Patients { get; }
    }
}