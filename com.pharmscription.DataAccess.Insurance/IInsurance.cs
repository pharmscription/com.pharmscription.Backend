using System.Threading.Tasks;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.DataAccess.Insurance
{
    public interface IInsurance
    {
        Task<InsurancePatient> FindPatient(string ahvNumber);
    }
}
