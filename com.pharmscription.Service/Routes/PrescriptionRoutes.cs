namespace com.pharmscription.Service.Routes
{
    public static class PrescriptionRoutes
    {
        public const string GetPrescriptions = "patients/{patientid}/prescriptions/";

        public const string GetPrescriptionById = "patients/{patientid}/prescriptions/{id}";

        public const string CreatePrescription = "patients/{patientid}/prescriptions/";

        public const string GetCounterProposals = "patients/{patientid}/prescriptions/{prescriptionid}/counterproposals";

        public const string CreateCounterProposal = GetCounterProposals;

        public const string GetDispenses = "patients/{patientid}/prescriptions/{prescriptionid}/dispenses";

        public const string CreateDispense = GetDispenses;

        public const string GetDrugs = "patiens/{patientid}/prescriptions/{prescriptionid}/drugs";

    }
}