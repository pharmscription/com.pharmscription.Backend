namespace com.pharmscription.Reporting
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using DataAccess.Repositories.Dispense;

    public class Reporter
    {
        private readonly PdfReportWriter _pdfReportWriter;
        private readonly PrescriptionCrawler _prescriptionCrawler;
        private readonly string _outputDirectory = Directory.GetCurrentDirectory();
        private readonly IDispenseRepository _dispenseRepository;

        public Reporter(PdfReportWriter pdfReportWriter, PrescriptionCrawler prescriptionCrawler, IDispenseRepository dispenseRepository)
        {
            _pdfReportWriter = pdfReportWriter;
            _prescriptionCrawler = prescriptionCrawler;
            _dispenseRepository = dispenseRepository;
        }

        public async Task WriteReports()
        {
            var patients = await _prescriptionCrawler.GetRawDataForReport();
            var now = DateTime.Now;
            foreach (var patient in patients)
            {
                var path = Path.Combine(
                    Path.Combine(Path.Combine(_outputDirectory, patient.Patient.Insurance),
                        now.ToString("dd.MM.yyyy")), patient.Patient.AhvNumber);
                Directory.CreateDirectory(path);
                await _pdfReportWriter.WriteReport(patient, Path.Combine(path, "report.pdf"));
                foreach (var prescriptionDispensese in patient.PrescriptionDispenseses)
                {
                    foreach (var dispense in prescriptionDispensese.Dispenses)
                    {
                        dispense.Reported = true;
                    }
                    _dispenseRepository.UnitOfWork.Commit();
                }
            }
        }
    }
}
