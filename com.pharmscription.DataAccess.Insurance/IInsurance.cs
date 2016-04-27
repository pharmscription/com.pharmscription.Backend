using System.Threading.Tasks;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.DataAccess.Insurance
{
    /// <summary>
    /// The Insurance interface to fake the connection to an insurance.
    /// </summary>
    public interface IInsurance
    {
        /// <summary>
        /// Find a patient in the patient store of the insurance.
        /// </summary>
        /// <param name="ahvNumber">
        /// AHV number.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> which returns a <see cref="InsurancePatient"/>/>.
        /// </returns>
        Task<InsurancePatient> FindPatient(string ahvNumber);
    }
}
