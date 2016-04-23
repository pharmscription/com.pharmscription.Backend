using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Converter;
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
        public PrescriptionManager(IPrescriptionRepository prescriptionRepository, IPatientRepository patientRepository)
        {
            if (prescriptionRepository == null || patientRepository == null)
            {
                throw new InvalidArgumentException("Depended Upon Arguments were null");
            }
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
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

            var prescription = prescriptionDto.ConvertToEntity();
            _prescriptionRepository.Add(prescription);
            var patient = await _patientRepository.GetAsync(patientGuid);
            patient.Prescriptions.Add(prescription);
            await _prescriptionRepository.UnitOfWork.CommitAsync();
            return prescriptionDto;
        }

        public Task<List<CounterProposalDto>> GetCounterProposal(string patientId, string prescriptionId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id was null or empty");
            }
            return null;
        }

        public Task<CounterProposalDto> GetCounterProposal(string patientId, string prescriptionId, string proposalId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || string.IsNullOrWhiteSpace(proposalId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or proposalid was null or empty");
            }
            return null;
        }

        public Task<CounterProposalDto> AddCounterProposal(string patientId, string prescriptionId, CounterProposalDto counterProposal)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || counterProposal == null)
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or counterproposal was null or empty");
            }
            return null;
        }

        public Task<CounterProposalDto> EditCounterProposal(string patientId, string prescriptionId, CounterProposalDto counterProposal)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || counterProposal == null)
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or counterproposal was null or empty");
            }
            return null;
        }

        public Task<List<DispenseDto>> GetDispense(string patientId, string prescriptionId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id was null or empty");
            }
            return null;
        }

        public Task<DispenseDto> GetDispense(string patientId, string prescriptionId, string dispenseId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || string.IsNullOrWhiteSpace(dispenseId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or dispense id was null or empty");
            }
            return null;
        }

        public Task<DispenseDto> AddDispense(string patientId, string prescriptionId, DispenseDto dispense)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId) || dispense == null)
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or counterproposal was null or empty");
            }
            return null;
        }

        public Task<List<DrugItemDto>> GetPrescriptionDrugs(string patientId, string prescriptionId)
        {
            if (string.IsNullOrWhiteSpace(patientId) || string.IsNullOrWhiteSpace(prescriptionId))
            {
                throw new InvalidArgumentException("Patient Id or prescription Id or drug item id was null or empty");
            }
            return null;
        }
    }
}
