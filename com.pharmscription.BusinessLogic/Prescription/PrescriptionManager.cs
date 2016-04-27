using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.BusinessLogic.GuidHelper;
using com.pharmscription.BusinessLogic.Validation;
using com.pharmscription.DataAccess.Repositories.CounterProposal;
using com.pharmscription.DataAccess.Repositories.Dispense;
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
        public PrescriptionManager(IPrescriptionRepository prescriptionRepository, IPatientRepository patientRepository, ICounterProposalRepository counterProposalRepository, IDispenseRepository dispenseRepository)
        {
            if (prescriptionRepository == null || patientRepository == null || counterProposalRepository == null || dispenseRepository == null)
            {
                throw new InvalidArgumentException("Depended Upon Arguments were null");
            }
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
            _counterProposalRepository = counterProposalRepository;
            _dispenseRepository = dispenseRepository;
        }
        public async Task<List<PrescriptionDto>> Get(string patientId)
        {
            var patientGuid = GuidParser.ParseGuid(patientId);
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw  new NotFoundException("Patient Does not Exist");
            }
            return (await _prescriptionRepository.GetByPatientId(patientGuid)).ConvertToDtos();
        }

        public async Task<PrescriptionDto> Get(string patientId, string prescriptionId)
        {
            var patientGuid = GuidParser.ParseGuid(patientId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            return (await _prescriptionRepository.GetByPatientId(patientGuid)).FirstOrDefault(e => e.Id == prescriptionGuid).ConvertToDto();
        }

        public async Task<PrescriptionDto> Add(string patientId, PrescriptionDto prescriptionDto)
        {
            if (prescriptionDto == null)
            {
                throw new InvalidArgumentException("Patient Id or prescriptionDto was null or empty");
            }
            var prescriptionValidator = new PrescriptionValidator();
            prescriptionValidator.Validate(prescriptionDto);
            var patientGuid = GuidParser.ParseGuid(patientId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            var patient = await _patientRepository.GetWithPrescriptions(patientGuid);
            var prescription = prescriptionDto.ConvertToEntity();
            prescription.Patient = patient;
            _prescriptionRepository.Add(prescription);
            patient.Prescriptions.Add(prescription);
            await _prescriptionRepository.UnitOfWork.CommitAsync();
            return prescription.ConvertToDto();
        }

        public async Task<List<CounterProposalDto>> GetCounterProposals(string patientId, string prescriptionId)
        {
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            return (await _prescriptionRepository.GetAsync(prescriptionGuid)).CounterProposals.ConvertToDtos().ToList();
        }

        public async Task<CounterProposalDto> GetCounterProposals(string patientId, string prescriptionId, string proposalId)
        {
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            await _prescriptionRepository.CheckIfEntityExists(GuidParser.ParseGuid(prescriptionId));
            var counterProposalGuid = GuidParser.ParseGuid(proposalId);
            await _counterProposalRepository.CheckIfEntityExists(counterProposalGuid);
            return (await _counterProposalRepository.GetAsync(counterProposalGuid)).ConvertToDto();
        }

        public async Task<CounterProposalDto> AddCounterProposal(string patientId, string prescriptionId, CounterProposalDto counterProposalDto)
        {
            if (counterProposalDto == null)
            {
                throw new InvalidArgumentException("counterproposal was null or empty");
            }
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            var counterProposal = counterProposalDto.ConvertToEntity();
            _counterProposalRepository.Add(counterProposal);
            var prescription = await _prescriptionRepository.GetAsync(prescriptionGuid);
            prescription.CounterProposals.Add(counterProposal);
            await _counterProposalRepository.UnitOfWork.CommitAsync();
            return counterProposal.ConvertToDto();
        }

        public async Task<CounterProposalDto> EditCounterProposal(string patientId, string prescriptionId, CounterProposalDto counterProposalDto)
        {
            if (counterProposalDto == null)
            {
                throw new InvalidArgumentException("counterproposal was null");
            }
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            await _prescriptionRepository.CheckIfEntityExists(GuidParser.ParseGuid(prescriptionId));
            var counterProposalGuid = GuidParser.ParseGuid(counterProposalDto.Id);
            await _counterProposalRepository.CheckIfEntityExists(counterProposalGuid);
            var counterProposal = await _counterProposalRepository.GetAsync(counterProposalGuid);
            counterProposal.Message = counterProposalDto.Message;
            counterProposal.Date = DateTime.Parse(counterProposalDto.Date);
            await _counterProposalRepository.UnitOfWork.CommitAsync();
            return counterProposal.ConvertToDto();
        }

        public async Task<List<DispenseDto>> GetDispense(string patientId, string prescriptionId)
        {
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            return (await _prescriptionRepository.GetAsync(prescriptionGuid)).Dispenses.ConvertToDtos();
        }

        public async Task<DispenseDto> GetDispense(string patientId, string prescriptionId, string dispenseId)
        {
            var dispenseGuid = GuidParser.ParseGuid(dispenseId);
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            await _prescriptionRepository.CheckIfEntityExists(GuidParser.ParseGuid(prescriptionId));
            await _dispenseRepository.CheckIfEntityExists(dispenseGuid);
            return (await _dispenseRepository.GetAsync(dispenseGuid)).ConvertToDto();
        }

        public async Task<DispenseDto> AddDispense(string patientId, string prescriptionId, DispenseDto dispenseDto)
        {
            if (dispenseDto == null)
            {
                throw new InvalidArgumentException("dispense was null or empty");
            }
            var patientGuid = GuidParser.ParseGuid(patientId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
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

        public async Task<List<DrugItemDto>> GetPrescriptionDrugs(string patientId, string prescriptionId)
        {
            var patientGuid = GuidParser.ParseGuid(patientId);
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            return (await _prescriptionRepository.GetAsync(prescriptionGuid)).DrugItems.ConvertToDtos();
        }
    }
}
