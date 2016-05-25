using com.pharmscription.DataAccess.Entities.PrescriptionEntity;

namespace com.pharmscription.DataAccess.Entities.CounterProposalEntity
{
    using System;

    using BaseEntity;
    using SharedInterfaces;

    public class CounterProposal : Entity, ICloneable<CounterProposal>, IEquatable<CounterProposal>
    {
        public DateTime Date { get; set; } 
        public string Message { get; set; }
        //// Diese Properties wird vom EF benötigt um das entsprechende Prescription Objekt in das CounterProposal zu speichern
        public virtual StandingPrescription StandingPrescription { get; set; }
        public virtual SinglePrescription SinglePrescription { get; set; }
        public virtual Prescription Prescription { get; set; }
        //// Um endlose Zyklen zu vermeiden, wird Prescription nicht kopiert
        public CounterProposal Clone()
        {
            return new CounterProposal
                       {
                           Date = Date,
                           Message = Message
                       };
        }

        public bool Equals(CounterProposal other)
        {
            if (other == null)
            {
                return false;
            }
            return Date.Equals(other.Date) && Message == other.Message
                   && Prescription == other.Prescription;
        }
    }
}