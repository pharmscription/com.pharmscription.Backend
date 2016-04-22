namespace com.pharmscription.Service.ControllerInterfaces
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using com.pharmscription.Infrastructure.Dto;

    public interface IPatiensController
    {
        /// <summary>
        /// GET patients/id
        /// </summary>
        /// <param name="id">
        ///     The id of the patient.
        /// </param>
        /// <returns>
        /// The <see cref="PatientDto"/> with the id.
        /// </returns>
        Task<PatientDto> Get(string id);

        [Route("patients/ahv-number/{ahv}")]
        Task<PatientDto> GetByAhvNumber(string ahv);

        [Route("patients/lookup/{ahv}")]
        Task<PatientDto> GetLookupByAhv(string ahv);

        Task<PatientDto> Put([FromBody] PatientDto patient);

    }
}