using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.DataAccess.Repositories.CounterProposal;
using com.pharmscription.DataAccess.Repositories.Dispense;
using com.pharmscription.DataAccess.Repositories.DrugItem;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Repositories.Prescription;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.Prescription
{
    public class PrescriptionManager : IPrescriptionManager
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ICounterProposalRepository _counterProposalRepository;
        private readonly IDispenseRepository _dispenseRepository;
        private readonly IDrugItemRepository _drugItemRepository;
        public PrescriptionManager(IPrescriptionRepository prescriptionRepository, IPatientRepository patientRepository, ICounterProposalRepository counterProposalRepository, IDispenseRepository dispenseRepository, IDrugItemRepository drugItemRepository)
        {
            if (prescriptionRepository == null || patientRepository == null || counterProposalRepository == null || dispenseRepository == null || drugItemRepository == null)
            {
                throw new InvalidArgumentException("Depended Upon Arguments were null");
            }
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
            _counterProposalRepository = counterProposalRepository;
            _dispenseRepository = dispenseRepository;
            _drugItemRepository = drugItemRepository;
        }
        public async Task<List<PrescriptionDto>> Get(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId))
            {
                throw new InvalidArgumentException("Patient Id was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException )
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw  new NotFoundException("Patient Does not Exist");
            }
            return (await _prescriptionRepository.GetByPatientId(patientGuid)).ConvertToDtos();
        }

        public async Task<PrescriptionDto> Get(string patientId, string prescriptionId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }
            return (await _prescriptionRepository.GetByPatientId(patientGuid)).FirstOrDefault(e => e.Id == prescriptionGuid).ConvertToDto();
        }

        public async Task<PrescriptionDto> Add(string patientId, PrescriptionDto prescriptionDto)
        {
            if (string.IsNullOrWhiteSpace(patientId) || prescriptionDto == null)
            {
                throw new InvalidArgumentException("Patient Id or prescriptionDto was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }

            
            var patient = await _patientRepository.GetAsync(patientGuid);
            if (patient.Prescriptions == null)
            {
                patient.Prescriptions = new List<DataAccess.Entities.PrescriptionEntity.Prescription>();
            }
            var prescription = prescriptionDto.ConvertToEntity();
            prescription.Patient = patient;
            _prescriptionRepository.Add(prescription);
            patient.Prescriptions.Add(prescription);

            await _prescriptionRepository.UnitOfWork.CommitAsync();
            return prescription.ConvertToDto();
        }

        public async Task<List<CounterProposalDto>> GetCounterProposal(string patientId, string prescriptionId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }
            return (await _prescriptionRepository.GetAsync(prescriptionGuid)).CounterProposals.ConvertToDtos();
        }

        public async Task<CounterProposalDto> GetCounterProposal(string patientId, string prescriptionId, string proposalId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || string.IsNullOrWhiteSpace(proposalId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or proposalid was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }
            Guid counterProposalGuid;
            try
            {
                counterProposalGuid = new Guid(proposalId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Counter Proposal Id");
            }
            if (await _counterProposalRepository.GetAsync(counterProposalGuid) == null)
            {
                throw new NotFoundException("Counter Proposal Does not Exist");
            }
            return (await _counterProposalRepository.GetAsync(counterProposalGuid)).ConvertToDto();
        }

        public async Task<CounterProposalDto> AddCounterProposal(string patientId, string prescriptionId, CounterProposalDto counterProposalDto)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || counterProposalDto == null)
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or counterproposal was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }

            var counterProposal = counterProposalDto.ConvertToEntity();
            _counterProposalRepository.Add(counterProposal);
            var prescription = await _prescriptionRepository.GetAsync(prescriptionGuid);
            prescription.CounterProposals.Add(counterProposal);
            await _counterProposalRepository.UnitOfWork.CommitAsync();
            return counterProposal.ConvertToDto();
        }

        public async Task<CounterProposalDto> EditCounterProposal(string patientId, string prescriptionId, CounterProposalDto counterProposalDto)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || string.IsNullOrWhiteSpace(counterProposalDto?.Id))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or counterproposal was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }
            Guid counterProposalGuid;
            try
            {
                counterProposalGuid = new Guid(counterProposalDto.Id);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Counter Proposal Id");
            }
            if (await _counterProposalRepository.GetAsync(counterProposalGuid) == null)
            {
                throw new NotFoundException("Counter Proposal Does not Exist");
            }
            var counterProposal = await _counterProposalRepository.GetAsync(counterProposalGuid);
            counterProposal.Message = counterProposalDto.Message;
            counterProposal.Date = DateTime.Parse(counterProposalDto.Date);
            await _counterProposalRepository.UnitOfWork.CommitAsync();
            return counterProposal.ConvertToDto();
        }

        public async Task<List<DispenseDto>> GetDispense(string patientId, string prescriptionId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }
            return (await _prescriptionRepository.GetAsync(prescriptionGuid)).Dispenses.ConvertToDtos();
        }

        public async Task<DispenseDto> GetDispense(string patientId, string prescriptionId, string dispenseId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || string.IsNullOrWhiteSpace(dispenseId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or dispenseDto id was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }
            Guid dispenseGuid;
            try
            {
                dispenseGuid = new Guid(dispenseId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Dispense Id");
            }
            if (await _dispenseRepository.GetAsync(dispenseGuid) == null)
            {
                throw new NotFoundException("Dispense Does not Exist");
            }
            return (await _dispenseRepository.GetAsync(dispenseGuid)).ConvertToDto();
        }

        public async Task<DispenseDto> AddDispense(string patientId, string prescriptionId, DispenseDto dispenseDto)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || dispenseDto == null)
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or counterproposal was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }

            var dispense = dispenseDto.ConvertToEntity();
            _dispenseRepository.Add(dispense);
            var prescription = await _prescriptionRepository.GetAsync(prescriptionGuid);
            prescription.Dispenses.Add(dispense);
            await _dispenseRepository.UnitOfWork.CommitAsync();
            return dispense.ConvertToDto();
        }

        public async Task<DrugItemDto> GetPrescriptionDrug(string patientId, string prescriptionId, string drugItemId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or drug item id was null or empty");
            }
            Guid patientGuid;
            try
            {
                patientGuid = new Guid(patientId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Patient Id");
            }
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw new NotFoundException("Patient Does not Exist");
            }
            Guid prescriptionGuid;
            try
            {
                prescriptionGuid = new Guid(prescriptionId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Prescription Id");
            }
            if (await _prescriptionRepository.GetAsync(prescriptionGuid) == null)
            {
                throw new NotFoundException("Prescription Does not Exist");
            }
            Guid drugItemGuid;
            try
            {
                drugItemGuid = new Guid(drugItemId);
            }
            catch (FormatException)
            {
                throw new NotFoundException("Inavlid Drugitem Id");
            }
            if (await _drugItemRepository.GetAsync(drugItemGuid) == null)
            {
                throw new NotFoundException("Drugitem Does not Exist");
            }
            return (await _drugItemRepository.GetAsync(drugItemGuid)).ConvertToDto();
        }

    }
}
