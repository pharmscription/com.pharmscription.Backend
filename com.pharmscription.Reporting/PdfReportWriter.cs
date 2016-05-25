
namespace com.pharmscription.Reporting
{
    using System.IO;
    using System.Threading.Tasks;
    using BusinessLogic.DrugPrice;
    using iTextSharp.text.pdf;
    using Infrastructure.Exception;

    public class PdfReportWriter
    {
        private readonly IDrugPriceManager _drugPriceManager;

        public PdfReportWriter(IDrugPriceManager drugPriceManager)
        {
            if (drugPriceManager == null)
            {
                throw new InvalidArgumentException("Not all Dependencies were fullfilled");
            }
            _drugPriceManager = drugPriceManager;
        }
        public async Task WriteReport(DispenseInformation dispenseInformation, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var generator = new PdfGenerator(_drugPriceManager);
                var document = generator.GetBaseDocument(
                    $"Abrechnung für {dispenseInformation.Patient.FirstName} {dispenseInformation.Patient.LastName}",
                    "Abrechnung");
                var reportWriter = PdfWriter.GetInstance(document, fileStream);
                await generator.FormatReport(document, dispenseInformation);
                reportWriter.Close();
            }   
        }
    }
}
