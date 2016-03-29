using System.Collections.Generic;
using com.pharmscription.Infrastructure.Insurance;

namespace com.pharmscription.DataAccess.Insurance
{
    public interface IPatientStore
    {
        List<InsurancePatient> Patients { get; }
    }
}