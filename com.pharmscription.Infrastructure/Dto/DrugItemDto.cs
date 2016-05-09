using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    [DataContract]
    public class DrugItemDto : BaseDto
    {
        [DataMember]
        public DrugDto Drug { get; set; }
        [DataMember]
        public string DosageDescription { get; set; }
        [DataMember]
        public int Quantity { get; set; }
    }

}
