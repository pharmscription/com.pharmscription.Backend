using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    [DataContract]
    public class DrugDto: BaseDto
    {
        [DataMember]
        public string DrugDescription { get; set; }
        [DataMember]
        public string PackageSize { get; set; }
        [DataMember]
        public string Unit { get; set; }
        [DataMember]
        public string Composition { get; set; }
        [DataMember]
        public string NarcoticCategory { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
    }
}
