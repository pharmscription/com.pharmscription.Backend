using System.Collections.Generic;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Prescription
{
    public interface IPrescriptionManager
    {
        Task<List<PrescriptionDto>> Get(string patientId);
        Task<PrescriptionDto> Get(string patientId, string prescriptionId);
        Task<PrescriptionDto> Add(string patientId, PrescriptionDto prescriptionDto);
        Task<List<CounterProposalDto>> GetCounterProposal(string patientId, string prescriptionId);
        Task<CounterProposalDto> GetCounterProposal(string patientId, string prescriptionId, string proposalId);

        Task<CounterProposalDto> AddCounterProposal(string patientId, string prescriptionId,
            CounterProposalDto counterProposal);

        Task<CounterProposalDto> EditCounterProposal(string patientId, string prescriptionId,
            CounterProposalDto counterProposalDto);

        Task<List<DispenseDto>> GetDispense(string patientId, string prescriptionId);
        Task<DispenseDto> GetDispense(string patientId, string prescriptionId, string dispenseId);

        Task<DispenseDto> AddDispense(string patientId, string prescriptionId, DispenseDto dispenseDto);
        Task<DrugItemDto> GetPrescriptionDrug(string patientId, string prescriptionId, string drugItemId);
    }
}
