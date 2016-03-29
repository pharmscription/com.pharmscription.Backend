using System;
using com.pharmscription.Infrastructure.Insurance;

namespace com.pharmscription.DataAccess.Insurance
{
    public interface IInsurance
    {
        InsurancePatient FindPatient(String ahvNumber);
    }
}
