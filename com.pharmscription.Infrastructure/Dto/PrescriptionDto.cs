using System.Collections.Generic;
using System.Runtime.Serialization;
namespace com.pharmscription.Infrastructure.Dto
{
    public class PrescriptionDto : BaseDto
    {
        public string Type { get; set; }
        public PatientDto Patient { get; set; }
        public string IssueDate { get; set; }
        public string EditDate { get; set; }
        public string ValidUntil { get; set; }
        public bool IsValid { get; set; }
        public ICollection<CounterProposalDto> CounterProposals { get; set; }
        public ICollection<DispenseDto> Dispenses { get; set; }
        public ICollection<DrugItemDto> Drugs { get; set; }
        public ICollection<PrescriptionDto> PrescriptionHistory { get; set; }
    }
}
