using System;
using com.pharmscription.ApplicationFascade;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.Service
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "PatientService" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    // HINWEIS: Wählen Sie zum Starten des WCF-Testclients zum Testen dieses Diensts PatientService.svc oder PatientService.svc.cs im Projektmappen-Explorer aus, und starten Sie das Debuggen.
    public class RestService : IRestService
    {
        private IPatientManager _patientManager;
        public RestService(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        public RestService()
        {
            _patientManager = new ManagerFactory().PatientManager;
        }

        #region patient
        public PatientDto GetPatient(string id)
        {
            return _patientManager.GetById(id);
        }

        public PatientDto CreatePatient(PatientDto dto)
        {
            return _patientManager.Add(dto);
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
            return _patientManager.Lookup(ahv);
        }

        public PatientDto DeletePatient(string id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
