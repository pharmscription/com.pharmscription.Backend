using System;
using com.pharmscription.ApplicationFascade;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.BusinessLogic.Drug;
namespace com.pharmscription.Service
{
    using System.Threading.Tasks;

    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "PatientService" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    // HINWEIS: Wählen Sie zum Starten des WCF-Testclients zum Testen dieses Diensts PatientService.svc oder PatientService.svc.cs im Projektmappen-Explorer aus, und starten Sie das Debuggen.
    public class RestService : IRestService
    {
        private readonly IPatientManager _patientManager;

        private readonly IDrugManager _drugManager;

        public RestService(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        public RestService(IDrugManager drugManager)
        {
            _drugManager = drugManager;
        }

        public RestService()
        {
            _patientManager = new ManagerFactory().PatientManager;
        }

        #region patient
        public async Task<PatientDto> GetPatient(string id)
        {
            return await _patientManager.GetById(id);
        }

        public async Task<PatientDto> CreatePatient(PatientDto dto)
        {
            return await _patientManager.Add(dto);
        }

        public async Task<PatientDto> ModifyPatient(string id, PatientDto newPatientDto)
        {
            throw new NotImplementedException();
        }

        public async Task<AddressDto> GetAddress(string patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientDto> GetPatientByAhv(string ahv)
        {
            return _patientManager.Lookup(ahv).Result;
        }

        public async Task<PatientDto> DeletePatient(string id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region drugs
        public async Task<DrugDto> GetDrug(string id)
        {
            return await _drugManager.GetById(id);
        }

        public async Task<DrugDto[]> SearchDrugs(string keyword)
        {
            return (await _drugManager.Search(keyword)).ToArray();
        }

        public Task<double> GetDrugPrice(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
