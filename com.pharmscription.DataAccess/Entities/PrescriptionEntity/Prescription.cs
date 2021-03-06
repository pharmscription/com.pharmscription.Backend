﻿namespace com.pharmscription.DataAccess.Entities.PrescriptionEntity
{
    using System;
    using System.Collections.Generic;
    using BaseEntity;
    using CounterProposalEntity;
    using DispenseEntity;
    using DoctorEntity;
    using DrugItemEntity;
    using PatientEntity;
    using SharedInterfaces;
    public abstract class Prescription : Entity, ICloneable<Prescription>, IEquatable<Prescription>
    {
        public virtual Patient Patient { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsValid { get; set; }
        public virtual ICollection<CounterProposal> CounterProposals { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Dispense> Dispenses { get; set; }
        public virtual ICollection<DrugItem> DrugItems { get; set; }
        public virtual ICollection<Prescription> PrescriptionHistory { get; set; }

        public abstract Prescription Clone();
        public abstract string GetPrescriptionType();
        public abstract bool Equals(Prescription other);
    }
}
