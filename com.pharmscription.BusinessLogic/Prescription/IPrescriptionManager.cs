namespace com.pharmscription.BusinessLogic.Prescription
{
     using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Dto;
    public interface IPrescriptionManager
    {
        Task<ICollection<PrescriptionDto>> Get(string patientId);
        Task<PrescriptionDto> Get(string patientId, string prescriptionId);
        Task<PrescriptionDto> Add(string patientId, PrescriptionDto prescriptionDto);
        Task<PrescriptionDto> Update(string patientId, string prescriptionId, PrescriptionDto prescriptionDto);
        Task<ICollection<CounterProposalDto>> GetCounterProposals(string patientId, string prescriptionId);
        Task<CounterProposalDto> GetCounterProposals(string patientId, string prescriptionId, string proposalId);

        Task<CounterProposalDto> AddCounterProposal(string patientId, string prescriptionId,
            CounterProposalDto counterProposal);

        Task<CounterProposalDto> EditCounterProposal(string patientId, string prescriptionId,
            CounterProposalDto counterProposalDto);

        Task<ICollection<DispenseDto>> GetDispenses(string patientId, string prescriptionId);
        Task<DispenseDto> GetDispenses(string patientId, string prescriptionId, string dispenseId);

        Task<DispenseDto> AddDispense(string patientId, string prescriptionId, DispenseDto dispenseDto);


        Task<ICollection<DrugItemDto>> GetPrescriptionDrugs(string patientId, string prescriptionId);
    }
}
