using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    [DataContract]
    public class CounterProposalDto : BaseDto
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
