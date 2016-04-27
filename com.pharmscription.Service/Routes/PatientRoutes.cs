namespace com.pharmscription.Service.Routes
{
    public static class PatientRoutes
    {
        public const string GetPatientById = "patients/{id}";
        public const string AddPatient = "patients";
        public const string GetPatientByAhvNumber = "patients/ahv-number/{ahv}";
        public const string LookupPatientByAhvNumber = "patients/lookup/{ahv}";
    }
}