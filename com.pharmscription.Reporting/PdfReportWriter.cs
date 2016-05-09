
namespace com.pharmscription.Reporting
{
    using System.IO;
    using iTextSharp.text.pdf;

    public class PdfReportWriter
    {
        public void WriteReport(DispenseInformation dispenseInformation)
        {
            var fileStream = new FileStream("Chapter1_Example1.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            var generator = new PdfGenerator();
            var document = generator.GetBaseDocument(
                $"Abrechnung für {dispenseInformation.Patient.FirstName} {dispenseInformation.Patient.LastName}",
                "Abrechnung");
            PdfWriter reportWriter = PdfWriter.GetInstance(document, fileStream);
             generator.FormatReport(document, dispenseInformation);

            var x = reportWriter;
            reportWriter.Close();

        }



    }
}
