namespace com.pharmscription.Service.Routes
{
    public static class PrescriptionRoutes
    {
        internal const string GetPrescriptions = "patients/{patientid}/prescriptions/";

        internal const string GetPrescriptionById = "patients/{patientid}/prescriptions/{id}";

        internal const string CreatePrescription = "patients/{patientid}/prescriptions/";

        internal const string UpdatePrescription = "patients/{patientid}/prescriptions/{prescriptionid}";

        internal const string GetCounterProposals = "patients/{patientid}/prescriptions/{prescriptionid}/counterproposals";

        internal const string CreateCounterProposal = GetCounterProposals;

        internal const string GetDispenses = "patients/{patientid}/prescriptions/{prescriptionid}/dispenses";

        internal const string CreateDispense = "patients/{patientid}/prescriptions/{prescriptionid}/dispenses";

        internal const string EditDispense =
            "patients/{patientid}/prescriptions/{prescriptionid}/dispenses/{dispenseid}";

        internal const string GetDrugs = "patiens/{patientid}/prescriptions/{prescriptionid}/drugs";

    }
}