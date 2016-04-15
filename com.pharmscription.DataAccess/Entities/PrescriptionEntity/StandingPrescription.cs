namespace com.pharmscription.DataAccess.Entities.PrescriptionEntity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
    using com.pharmscription.DataAccess.Entities.DispenseEntity;
    using com.pharmscription.DataAccess.Entities.DrugItemEntity;

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
                           CounterProposal = CounterProposal.Select(cp => cp.Clone()).ToList(),
                           Doctor = Doctor,
                           Dispense = Dispense.Select(d => d.Clone()).ToList(),
                           Drug = Drug.Select(d => d.Clone()).ToList(),
                           PrescriptionHistory = PrescriptionHistory.Select(ph => ph.Clone()).ToList(),
                           ValidUntill = ValidUntill
                       };
        }
    }
}