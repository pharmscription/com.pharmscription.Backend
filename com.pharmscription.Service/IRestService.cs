using System.ServiceModel;
using System.ServiceModel.Web;

using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.Service
{
    using System.Threading.Tasks;

    // http://stackoverflow.com/questions/20206069/restful-web-service-body-format
    [ServiceContract]
    public interface IRestService
    {
        #region patients
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        Task<PatientDto> GetPatient(string id);

        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients")]
        [OperationContract]
        Task<PatientDto> CreatePatient(PatientDto dto);

        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        Task<PatientDto> ModifyPatient(string id, PatientDto newPatientDto);

        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{patientId}/address")]
        [OperationContract]
        Task<AddressDto> GetAddress(string patientId);

        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/ahv-number/{ahv}")]
        [OperationContract]
        Task<PatientDto> GetPatientByAhv(string ahv);

        [WebInvoke(Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        Task<PatientDto> DeletePatient(string id);

        #endregion

        #region drugs

        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "drugs/{id}")]
        [OperationContract]
        Task<DrugDto> GetDrug(string id);

        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "drugs/search/{keyword}")]
        [OperationContract]
        Task<DrugDto[]> SearchDrugs(string keyword);

        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Wrapped, 
            UriTemplate = "drugs/{id}/price")]
        [OperationContract]
        Task<double> GetDrugPrice(string id);

        #endregion
    }
}
