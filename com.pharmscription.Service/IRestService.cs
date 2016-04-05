using System;
using System.ServiceModel;
using System.ServiceModel.Web;

using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.Service
{
    // http://stackoverflow.com/questions/20206069/restful-web-service-body-format
    [ServiceContract]
    //TODO: Check what line below influences
    /*[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]*/
    public interface IRestService
    {
        #region patients
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        PatientDto GetPatient(string id);

        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients")]
        [OperationContract]
        PatientDto CreatePatient(PatientDto dto);

        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        PatientDto ModifyPatient(string id, PatientDto newPatientDto);

        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{patientId}/address")]
        [OperationContract]
        AddressDto GetAddress(string patientId);

        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/ahv-number/{ahv}")]
        [OperationContract]
        PatientDto GetPatientByAhv(string ahv);

        [WebInvoke(Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        PatientDto DeletePatient(string id);

        #endregion

        #region drugs

        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "drugs/{id}")]
        [OperationContract]
        DrugDto GetDrug(string id);

        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, 
            UriTemplate = "drugs/search/{keyword}")]
        [OperationContract]
        DrugDto[] SearchDrugs(string keyword);

        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Wrapped, 
            UriTemplate = "drugs/{id}/price")]
        [OperationContract]
        double GetDrugPrice(string id);

        #endregion
    }
}
