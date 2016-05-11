namespace com.pharmscription.DataAccess.Entities.PrescriptionEntity
{
    using System;
    using System.Linq;

    using com.pharmscription.Infrastructure.Constants;

    public class StandingPrescription : Prescription
    {
        public DateTime ValidUntill { get; set; }

        public override Prescription Clone()
        {
            return new StandingPrescription
            {
                Patient = Patient,
                IssueDate = IssueDate,
                EditDate = EditDate,
                SignDate = SignDate,
                IsValid = IsValid,
                CounterProposals = CounterProposals.Select(cp => cp.Clone()).ToList(),
                Doctor = Doctor,
                Dispenses = Dispenses.Select(d => d.Clone()).ToList(),
                DrugItems = DrugItems.Select(d => d.Clone()).ToList(),
                PrescriptionHistory = PrescriptionHistory.Select(ph => ph.Clone()).ToList(),
                ValidUntill = ValidUntill
            };
        }

        public override string GetPrescriptionType()
        {
            return PharmscriptionConstants.StandingPrescriptionString;
        }

        public override bool Equals(Prescription other)
        {
            if (other == null)
            {
                return false;
            }
            return Patient.Equals(other.Patient) && IssueDate.Equals(other.IssueDate) && EditDate.Equals(other.EditDate)
                   && SignDate.Equals(other.SignDate) && IsValid == other.IsValid
                   && CounterProposals.SequenceEqual(other.CounterProposals) && Doctor.Equals(other.Doctor)
                   && Dispenses.SequenceEqual(other.Dispenses) && DrugItems.SequenceEqual(other.DrugItems)
                   && PrescriptionHistory.SequenceEqual(other.PrescriptionHistory) && ValidUntill.Equals(ValidUntill);
        }
    }
}