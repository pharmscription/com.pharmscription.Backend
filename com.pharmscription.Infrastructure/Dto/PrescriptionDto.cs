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
        //TODO: remove
        //public string SignDate { get; set; }
        public string ValidUntil { get; set; }
        public bool IsValid { get; set; }
        public IReadOnlyCollection<CounterProposalDto> CounterProposals { get; set; }
        public IReadOnlyCollection<DispenseDto> Dispenses { get; set; }
        public IReadOnlyCollection<DrugItemDto> Drugs { get; set; }
        public IReadOnlyCollection<PrescriptionDto> PrescriptionHistory { get; set; }
    }
}
