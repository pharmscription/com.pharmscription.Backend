namespace com.pharmscription.DataAccess.Entities.PrescriptionEntity
{
   using System.Linq;

    public class SinglePrescription: Prescription
    {
        public override Prescription Clone()
        {
            return new SinglePrescription
                       {
                           Patient = Patient,
                           IssueDate = IssueDate,
                           EditDate = EditDate,
                           IsValid = IsValid,
                           CounterProposals = CounterProposals.Select(cp => cp.Clone()).ToList(),
                           Doctor = Doctor,
                           Dispenses = Dispenses.Select(d => d.Clone()).ToList(),
                           DrugItems = DrugItems.Select(d => d.Clone()).ToList(),
                           PrescriptionHistory = PrescriptionHistory.Select(ph => ph.Clone()).ToList()
                       };
        }

        public override string GetPrescriptionType()
        {
            return "N";
        }
    }
}