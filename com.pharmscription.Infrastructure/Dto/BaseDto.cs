
using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    [DataContract]
    public class BaseDto
    {
        [DataMember]
        public string Id { get; set; }
    }
}
