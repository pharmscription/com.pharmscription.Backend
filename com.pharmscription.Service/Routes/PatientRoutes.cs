namespace com.pharmscription.Service.Routes
{
    public class PatientRoutes
    {
        protected internal const string GetPatientById = "patients/{id}";
        protected internal const string AddPatient = "patients";
        protected internal const string GetPatientByAhvNumber = "patients/ahv-number/{ahv}";
        protected internal const string LookupPatientByAhvNumber = "patients/lookup/{ahv}";
    }
}