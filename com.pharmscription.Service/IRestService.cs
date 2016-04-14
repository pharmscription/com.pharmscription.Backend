namespace com.pharmscription.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Threading.Tasks;

    using Infrastructure.Dto;

    /// <summary>
    /// The main interface to communicate with the backend
    /// </summary>
    [ServiceContract]
    public interface IRestService
    {
        #region patients

        /// <summary>
        /// Get a patient by id
        /// </summary>
        /// <param name="id">
        /// The id of a patient
        /// </param>
        /// <returns>
        /// <see cref="Task"/> which returns a patient .
        /// </returns>
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        [FaultContract(typeof(WebFaultException<ErrorMessage>))]
        Task<PatientDto> GetPatient(string id);

        /// <summary>
        /// Create a new Patient.
        /// </summary>
        /// <param name="dto">
        /// The patient DTO.
        /// </param>
        /// <returns>
        /// <see cref="Task"/> which returns the new created patient.
        /// </returns>
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients")]
        [OperationContract]
        [FaultContract(typeof(WebFaultException<ErrorMessage>))]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "DTO is not a spelling mistake")]
        Task<PatientDto> CreatePatient(PatientDto dto);
        
        /// <summary>
        /// Search a patient by its AHV(social security number) Number.
        /// </summary>
        /// <param name="ahv">
        /// The AHV number of the patient.
        /// </param>
        /// <returns>
        /// <see cref="Task"/> whicht returns the patient which the corresponding AHV.
        /// </returns>
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/ahv-number/{ahv}")]
        [OperationContract]
        [FaultContract(typeof(WebFaultException<ErrorMessage>))]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "AHV Number is not a spelling mistake")]
        Task<PatientDto> GetPatientByAhv(string ahv);

        /// <summary>
        /// Search for a patient in his insurance database.
        /// </summary>
        /// <param name="ahv">
        /// AHV number of the patient.
        /// </param>
        /// <returns>
        /// <see cref="Task"/> which returns a patient from an insurance.
        /// </returns>
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/lookup/{ahv}")]
        [OperationContract]
        [FaultContract(typeof(WebFaultException<ErrorMessage>))]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "AHV Number is not a spelling mistake")]
        Task<PatientDto> LookupPatient(string ahv);
        
        #endregion

        #region drugs

        /// <summary>
        /// Get a drug by its id.
        /// </summary>
        /// <param name="id">
        /// The id of the drug.
        /// </param>
        /// <returns>
        /// <see cref="Task"/> which returns a drug.
        /// </returns>
        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "drugs/{id}")]
        [OperationContract]
        [FaultContract(typeof(WebFaultException<ErrorMessage>))]
        Task<DrugDto> GetDrug(string id);

        /// <summary>
        /// Search a drug by keywords.
        /// </summary>
        /// <param name="keyword">
        /// The keyword which has already been entered.
        /// </param>
        /// <returns>
        /// <see cref="Task"/> which returns the amount of fount results.
        /// </returns>
        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "drugs/search/{keyword}")]
        [OperationContract]
        [FaultContract(typeof(WebFaultException<ErrorMessage>))]
        Task<int> SearchDrugs(string keyword);

        /// <summary>
        /// Search a drug by keywords an get a page from the result set
        /// </summary>
        /// <param name="keyword">The keyword which has been entered</param>
        /// <param name="page">The page of the result set</param>
        /// <param name="amount">Amount which will be returnes per page</param>
        /// <returns>
        /// <see cref="Task"/> which returns the page of the result set
        /// </returns>
        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "drugs/search/{keyword}/{page}/{amount}")]
        [OperationContract]
        [FaultContract(typeof(WebFaultException<ErrorMessage>))]
        Task<DrugDto[]> SearchDrugs(string keyword, string page, string amount);

        #endregion
    }
}
