namespace com.pharmscription.Infrastructure.Dto
{
    ﻿using System.Collections.Generic;
    ﻿using System.Diagnostics.CodeAnalysis;
    ﻿using System.Runtime.Serialization;
    [DataContract]
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public class PrescriptionDto : BaseDto
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public PatientDto Patient { get; set; }
        [DataMember]
        public string IssueDate { get; set; }
        [DataMember]
        public string EditDate { get; set; }
        [DataMember]
        public string SignDate { get; set; }
        [DataMember]
        public string ValidUntil { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public IReadOnlyCollection<CounterProposalDto> CounterProposals { get; set; }
        [DataMember]
        public IReadOnlyCollection<DispenseDto> Dispenses { get; set; }
        [DataMember]
        public IReadOnlyCollection<DrugItemDto> Drugs { get; set; }
        [DataMember]
        public IReadOnlyCollection<PrescriptionDto> PrescriptionHistory { get; set; }
    }
}
