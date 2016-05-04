namespace com.pharmscription.Service.Routes
{
    public class PrescriptionRoutes
    {
        protected internal const string GetPrescriptions = "patients/{patientid}/prescriptions/";

        protected internal const string GetPrescriptionById = "patients/{patientid}/prescriptions/{id}";

        protected internal const string CreatePrescription = "patients/{patientid}/prescriptions/";

        protected internal const string GetCounterProposals = "patients/{patientid}/prescriptions/{prescriptionid}/counterproposals";

        protected internal const string CreateCounterProposal = GetCounterProposals;

        protected internal const string GetDispenses = "patients/{patientid}/prescriptions/{prescriptionid}/dispenses";

        protected internal const string CreateDispense = "patients/{patientid}/prescriptions/{prescriptionid}/dispense";

        protected internal const string GetDrugs = "patiens/{patientid}/prescriptions/{prescriptionid}/drugs";

    }
}