

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{

    [DataContract]
    public class DispenseDto  : BaseDto
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public IReadOnlyCollection<DrugItemDto> DrugItems { get; set; }



    }
}
