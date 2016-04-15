namespace com.pharmscription.DataAccess.Entities.CounterProposalEntity
{
    using System;

    using com.pharmscription.DataAccess.Entities.BaseEntity;
    using com.pharmscription.DataAccess.SharedInterfaces;

    public class CounterProposal : Entity, ICloneable<CounterProposal>
    {
        public DateTime Date { get; set; } 
        public string Message { get; set; }

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