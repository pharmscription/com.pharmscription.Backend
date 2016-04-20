﻿using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    [DataContract]
    public class DrugItemDto: BaseDto
    {
        [DataMember]
        public DrugDto Drug { get; set; }
        [DataMember]
        public DispenseDto Dispense { get; set; }
        [DataMember]
        public PrescriptionDto Prescription { get; set; }
        [DataMember]
        public string DosageDescription { get; set; }
    }
}
