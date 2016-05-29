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
    using System.Globalization;

    using DataAccess.Entities.DispenseEntity;
    using Infrastructure.Constants;

    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.PrescriptionEntity;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugItem;
    using Infrastructure.EntityHelper;

    public class PrescriptionManager : IPrescriptionManager
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ICounterProposalRepository _counterProposalRepository;
        private readonly IDispenseRepository _dispenseRepository;
        private readonly IDrugRepository _drugRepository;
        private readonly IDrugItemRepository _drugItemRepository;

        public PrescriptionManager(IPrescriptionRepository prescriptionRepository, IPatientRepository patientRepository, ICounterProposalRepository counterproposalRepository, IDispenseRepository dispenseRepository, IDrugRepository drugRepository, IDrugItemRepository drugItemRepository)
        {
            if (prescriptionRepository == null || patientRepository == null || counterproposalRepository == null || dispenseRepository == null || drugRepository == null || drugItemRepository == null)
            {
                throw new InvalidArgumentException("Depended Upon Arguments were null");
            }
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
            _counterProposalRepository = counterproposalRepository;
            _dispenseRepository = dispenseRepository;
            _drugRepository = drugRepository;
            _drugItemRepository = drugItemRepository;
        }
        public async Task<ICollection<PrescriptionDto>> Get(string patientId)
        {
            var patientGuid = GuidParser.ParseGuid(patientId);
            if (await _patientRepository.GetAsync(patientGuid) == null)
            {
                throw  new NotFoundException("Patient Does not Exist");
            }
            return (await _patientRepository.GetPrescriptions(patientGuid)).ConvertToDtos();
        }

        public async Task<PrescriptionDto> Get(string patientId, string prescriptionId)
        {
            var patientGuid = GuidParser.ParseGuid(patientId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            var prescription = (await _prescriptionRepository.GetByPatientId(patientGuid)).
                FirstOrDefault(e => e.Id == prescriptionGuid).ConvertToDto();
            var patient = (await _patientRepository.GetAsync(patientGuid)).ConvertToDto();
            prescription.Patient = patient;
            return (prescription);
        }

        public async Task<PrescriptionDto> Add(string patientId, PrescriptionDto prescriptionDto)
        {
             if (prescriptionDto == null)
            {
                throw new InvalidArgumentException("prescriptionDto was null or empty");
            }
            var prescriptionValidator = new PrescriptionValidator();
            prescriptionValidator.Validate(prescriptionDto);
            var patientGuid = GuidParser.ParseGuid(patientId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            var patient = await _patientRepository.GetWithPrescriptions(patientGuid);
            var prescription = await MapNewPrescriptionToEntity(prescriptionDto);
            prescription.IsValid = true;
            var addedPrescription = _prescriptionRepository.Add(prescription);
            patient.Prescriptions.Add(addedPrescription);
            await _prescriptionRepository.UnitOfWork.CommitAsync();
            return addedPrescription.ConvertToDto();
        }

        public async Task<PrescriptionDto> Update(string patientId, string prescriptionId, PrescriptionDto prescriptionDto)
        {
            if (prescriptionDto == null)
            {
                throw new InvalidArgumentException("prescriptionDto was null or empty");
            }
            var prescriptionValidator = new PrescriptionValidator();
            prescriptionValidator.Validate(prescriptionDto);
            if (prescriptionDto.CounterProposals == null || !prescriptionDto.CounterProposals.Any())
            {
                throw new InvalidArgumentException("Updated Prescription must at least have one counterproposal");
            }

            var patientGuid = GuidParser.ParseGuid(patientId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            var patient = await _patientRepository.GetWithPrescriptions(patientGuid);
            var newPrescription = await MapNewPrescriptionToEntity(prescriptionDto);
            var oldPrecription =
                await _prescriptionRepository.GetWithAllNavsAsync(GuidParser.ParseGuid(prescriptionId));
            newPrescription.PrescriptionHistory.Add(oldPrecription);
            oldPrecription.IsValid = false;
            _prescriptionRepository.Add(newPrescription);
            patient.Prescriptions.Add(newPrescription);
            await _prescriptionRepository.UnitOfWork.CommitAsync();
            return newPrescription.ConvertToDto();
        }

        public async Task<ICollection<CounterProposalDto>> GetCounterProposals(string patientId, string prescriptionId)
        {
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            return (await _prescriptionRepository.GetAsync(prescriptionGuid)).CounterProposals.ConvertToDtos();
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

        public async Task<ICollection<DispenseDto>> GetDispenses(string patientId, string prescriptionId)
        {
            await _patientRepository.CheckIfEntityExists(GuidParser.ParseGuid(patientId));
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            return (await _prescriptionRepository.GetWithAllNavsAsync(prescriptionGuid)).Dispenses.ConvertToDtos();
        }

        public async Task<DispenseDto> GetDispenses(string patientId, string prescriptionId, string dispenseId)
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
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            var dispense = dispenseDto.ConvertToEntity();
            if (dispenseDto.DrugItems == null)
            {
                dispense.DrugItems = new List<DrugItem>();
                dispenseDto.DrugItems = new List<DrugItemDto>();
            }
            dispense.DrugItems.Clear();
            foreach (var drugItem in dispenseDto.DrugItems)
            {
                //dispense.DrugItems.Add(await this._drugItemRepository.GetWithDrugAsync(new Guid(drugItem.Id)));
                var newDrugItem = drugItem.ConvertToEntity();
                newDrugItem.Id = IdentityGenerator.NewSequentialGuid();
                newDrugItem.Drug = await _drugRepository.GetAsync(newDrugItem.Drug.Id);
                dispense.DrugItems.Add(newDrugItem);
            }
            _dispenseRepository.Add(dispense);
            var prescription = await _prescriptionRepository.GetWithAllNavsAsync(prescriptionGuid);
            if (prescription.Dispenses == null)
            {
                prescription.Dispenses = new List<Dispense>();
            }

            prescription.Dispenses.Add(dispense);
            await _dispenseRepository.UnitOfWork.CommitAsync();
            return dispense.ConvertToDto();
        }

        public async Task<DispenseDto> ModifyDispense(
            string patientId,
            string prescriptionId,
            string dispenseId,
            DispenseDto dispenseDto)
        {
            if (dispenseDto == null)
            {
                throw new InvalidArgumentException("dispense was null or empty");
            }

            if (dispenseId != dispenseDto.Id)
            {
                throw new InvalidArgumentException("dispenseid does not match DispenseDto");
            }

            if (dispenseDto.Date == DateTime.MinValue.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture))
            {
                throw new InvalidArgumentException("Dispense already signed. Cannot edit");
            }

            var patientGuid = GuidParser.ParseGuid(patientId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            var dispenseGuid = GuidParser.ParseGuid(dispenseId);
            await _dispenseRepository.CheckIfEntityExists(dispenseGuid);
            var dispense = dispenseDto.ConvertToEntity();
            var oldDispense = await _dispenseRepository.GetAsync(dispenseGuid);
            oldDispense.Update(dispense);
            _dispenseRepository.Modify(oldDispense);
            await _dispenseRepository.UnitOfWork.CommitAsync();
            return dispense.ConvertToDto();
        }

        public async Task<ICollection<DrugItemDto>> GetPrescriptionDrugs(string patientId, string prescriptionId)
        {
            var patientGuid = GuidParser.ParseGuid(patientId);
            var prescriptionGuid = GuidParser.ParseGuid(prescriptionId);
            await _patientRepository.CheckIfEntityExists(patientGuid);
            await _prescriptionRepository.CheckIfEntityExists(prescriptionGuid);
            return (await _prescriptionRepository.GetAsync(prescriptionGuid)).DrugItems.ConvertToDtos();
        }

        private async Task<Prescription> MapNewPrescriptionToEntity(PrescriptionDto prescriptionDto)
        {
            Prescription prescription;
            if (prescriptionDto.Type == "N")
            {
                prescription = new SinglePrescription();
            }
            else
            {
                prescription = new StandingPrescription 
                {
                    ValidUntil = DateTime.Parse(prescriptionDto.ValidUntil)
                };
            }

            prescription.IsValid = true;
            if (prescriptionDto.CounterProposals != null && prescriptionDto.CounterProposals.Any())
            {
                prescription.CounterProposals = new List<CounterProposal>();
                foreach (var counterProposalDto in prescriptionDto.CounterProposals)
                {
                    var counterProposal = new CounterProposal
                    {
                        Date = DateTime.Now,
                        Message = counterProposalDto.Message
                    };
                    prescription.CounterProposals.Add(counterProposal);
                }
            }
            if (prescriptionDto.Drugs != null && prescriptionDto.Drugs.Any())
            {
                prescription.DrugItems = new List<DrugItem>();
                foreach (var drugItemDto in prescriptionDto.Drugs)
                {
                    var drugItem = new DrugItem
                    {
                        DosageDescription = drugItemDto.DosageDescription,
                        Drug = await _drugRepository.GetAsyncOrThrow(GuidParser.ParseGuid(drugItemDto.Drug.Id)),
                        Quantity = drugItemDto.Quantity
                    };
                    prescription.DrugItems.Add(drugItem);
                }
            }
            prescription.PrescriptionHistory = new List<Prescription>();
            if (prescriptionDto.PrescriptionHistory != null)
            {
                foreach (var oldPrescription in prescriptionDto.PrescriptionHistory)
                {
                    var oldPrescriptionInDatabase =
                        await _prescriptionRepository.GetAsyncOrThrow(GuidParser.ParseGuid(oldPrescription.Id));
                    prescription.PrescriptionHistory.Add(oldPrescriptionInDatabase);
                }
            }
            prescription.EditDate = DateTime.Now;
            prescription.IssueDate = DateTime.Now;
            return prescription;
        }
    }
}
