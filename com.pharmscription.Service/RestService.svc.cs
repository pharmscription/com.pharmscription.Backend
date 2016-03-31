using System;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.Service
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "PatientService" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    // HINWEIS: Wählen Sie zum Starten des WCF-Testclients zum Testen dieses Diensts PatientService.svc oder PatientService.svc.cs im Projektmappen-Explorer aus, und starten Sie das Debuggen.
    public class RestService : IRestService
    {
        #region patient

        public PatientDto GetPatient(string id)
        {
            throw new NotImplementedException();
        }

        public PatientDto CreatePatient(PatientDto dto)
        {
            throw new NotImplementedException();
        }

        public PatientDto ModifyPatient(string id, PatientDto newPatientDto)
        {
            throw new NotImplementedException();
        }

        public AddressDto GetAddress(string patientId)
        {
            throw new NotImplementedException();
        }

        public PatientDto GetPatientByAhv(string ahv)
        {
            throw new NotImplementedException();
        }

        public PatientDto DeletePatient(string id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
