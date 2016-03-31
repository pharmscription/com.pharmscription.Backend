using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.Service.Patient
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Schnittstellennamen "IPatientService" sowohl im Code als auch in der Konfigurationsdatei ändern.
    [ServiceContract]
    /*[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]*/
    public interface IPatientService
    {
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        PatientDto GetPatient(Guid id);

        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "patients")]
        [OperationContract]
        PatientDto CreatePatient(PatientDto dto);

        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        PatientDto ModifyPatient(PatientDto dto);

        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "patients/{id}/address")]
        [OperationContract]
        AddressDto GetAddress(Guid patientId);

        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "patients/ahv-number/{ahv}")]
        [OperationContract]
        PatientDto GetPatientByAhv(string ahv);

        [WebInvoke(Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "patients/{id}")]
        [OperationContract]
        PatientDto DeletePatient(Guid id);
    }
}

