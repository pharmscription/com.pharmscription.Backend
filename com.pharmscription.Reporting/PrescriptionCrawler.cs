
namespace com.pharmscription.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities.PatientEntity;
    using DataAccess.Repositories.Patient;
    using Infrastructure.Exception;

    public class PrescriptionCrawler
    {
        private readonly IPatientRepository _patientRepository;
        public PrescriptionCrawler(IPatientRepository patientRepository)
        {
            if (patientRepository == null)
            {
                throw new InvalidArgumentException("Depended on Arguments were not provided");
            }
            _patientRepository = patientRepository;
        }

        private async Task<ICollection<Patient>> GetAllToReport()
        {
            var now = DateTime.Now;
            var aMonthAgo = now.AddMonths(-1);
            return await _patientRepository.GetAllWithUnreportedDispenses(aMonthAgo);
        }

        public async Task<ICollection<DispenseInformation>> GetRawDataForReport()
        {
            var patients = await GetAllToReport();
            var list = new List<DispenseInformation>(patients.Count);
            foreach (var patient in patients)
            {
                var dispenseInformation = new DispenseInformation
                {
                    Patient = patient
                };
                var prescriptionDispenses = new List<PrescriptionDispenses>(patient.Prescriptions.Count);
                prescriptionDispenses.AddRange(patient.Prescriptions.Select(prescription => new PrescriptionDispenses
                {
                    Prescription = prescription, Dispenses = prescription.Dispenses
                }));
                dispenseInformation.PrescriptionDispenseses = prescriptionDispenses;
                list.Add(dispenseInformation);
            }
            return list;
        }
    }
}
