namespace com.pharmscription.Service.Routes
{
    public static class PatientRoutes
    {
        internal const string GetPatientById = "patients/{id}";
        internal const string AddPatient = "patients";
        internal const string GetPatientByAhvNumber = "patients/ahv-number/{ahv}";
        internal const string LookupPatientByAhvNumber = "patients/lookup/{ahv}";
    }
}