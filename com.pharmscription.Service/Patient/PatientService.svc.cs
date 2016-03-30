using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.Service.Patient
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "PatientService" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    // HINWEIS: Wählen Sie zum Starten des WCF-Testclients zum Testen dieses Diensts PatientService.svc oder PatientService.svc.cs im Projektmappen-Explorer aus, und starten Sie das Debuggen.
    public class PatientService : IPatientService
    {
        public PatientDto CreatePatient(PatientDto dto)
        {
            throw new NotImplementedException();
        }

        public PatientDto DeletePatient(Guid id)
        {
            throw new NotImplementedException();
        }

        public AddressDto GetAddress(Guid patientId)
        {
            throw new NotImplementedException();
        }

        public PatientDto GetPatient(Guid id)
        {
            throw new NotImplementedException();
        }

        public PatientDto GetPatientByAhv(string ahv)
        {
            throw new NotImplementedException();
        }

        public PatientDto ModifyPatient(PatientDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
