using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.DataAccess.Insurance
{
    public interface IInsurance
    {
        InsurancePatient FindPatient(string ahvNumber);
    }
}
