namespace com.pharmscription.Infrastructure.Dto
{
    ﻿using System.Collections.Generic;
    ﻿using System.Runtime.Serialization;
    [DataContract]
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
        public ICollection<CounterProposalDto> CounterProposals { get; set; }
        [DataMember]
        public ICollection<DispenseDto> Dispenses { get; set; }
        [DataMember]
        public ICollection<DrugItemDto> Drugs { get; set; }
        [DataMember]
        public ICollection<PrescriptionDto> PrescriptionHistory { get; set; }
    }
}
