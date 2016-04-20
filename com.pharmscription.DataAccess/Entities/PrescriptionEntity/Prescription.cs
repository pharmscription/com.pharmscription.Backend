namespace com.pharmscription.DataAccess.Entities.PrescriptionEntity
{
    using System;
    using System.Collections.Generic;

    using com.pharmscription.DataAccess.Entities.BaseEntity;
    using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
    using com.pharmscription.DataAccess.Entities.DispenseEntity;
    using com.pharmscription.DataAccess.Entities.DoctorEntity;
    using com.pharmscription.DataAccess.Entities.DrugItemEntity;
    using com.pharmscription.DataAccess.Entities.PatientEntity;
    using com.pharmscription.DataAccess.SharedInterfaces;
    public abstract class Prescription : Entity, ICloneable<Prescription>
    {
        public Patient Patient { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime SignDate { get; set; }
        public bool IsValid { get; set; }
        public List<CounterProposal> CounterProposals { get; set; }
        public Doctor Doctor { get; set; }
        public List<Dispense> Dispenses { get; set; }  
        public List<DrugItem> DrugItems { get; set; } 
        public List<Prescription> PrescriptionHistory { get; set; }

        public abstract Prescription Clone();
    }
}
