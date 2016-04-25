using com.pharmscription.DataAccess.Entities.PrescriptionEntity;

namespace com.pharmscription.DataAccess.Entities.CounterProposalEntity
{
    using System;

    using BaseEntity;
    using SharedInterfaces;

    public class CounterProposal : Entity, ICloneable<CounterProposal>
    {
        public DateTime Date { get; set; } 
        public string Message { get; set; }

        public virtual StandingPrescription StandingPrescription { get; set; }
        public virtual SinglePrescription SinglePrescription { get; set; }
        public virtual Prescription Prescription
        {
            get;
            set;
        }
        public CounterProposal Clone()
        {
            return new CounterProposal
                       {
                           Date = Date,
                           Message = Message
                       };
        }
    }
}