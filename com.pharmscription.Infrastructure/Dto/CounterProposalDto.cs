using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    public class CounterProposalDto : BaseDto
    {
        public string Date { get; set; }
        public string Message { get; set; }
    }
}
