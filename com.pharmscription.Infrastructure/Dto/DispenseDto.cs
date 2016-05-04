

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    using System.Diagnostics.CodeAnalysis;

    [DataContract]
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
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
