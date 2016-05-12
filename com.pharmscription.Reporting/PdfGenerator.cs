

namespace com.pharmscription.Reporting
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using BusinessLogic.DrugPrice;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.PatientEntity;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.draw;
    using Infrastructure.Constants;
    using Infrastructure.Exception;

    internal class PdfGenerator
    {
        private const int SideMargin = 36;

        private int _chapterNumber;

        private int ChapterNumber => _chapterNumber++;
        private readonly IDrugPriceManager _drugPriceManager;

        public PdfGenerator(IDrugPriceManager drugPriceManagerManager)
        {
            if (drugPriceManagerManager == null)
            {
                throw new InvalidArgumentException("Not all Dependencies were fullfilled");
            }
            _drugPriceManager = drugPriceManagerManager;
        }

        private static Document GetBaseLayout()
        {
            var baseLayout = new Rectangle(PageSize.A4);
            var document = new Document(baseLayout, SideMargin, SideMargin, SideMargin, SideMargin);
            return document;
        }

        public Document GetBaseDocument(string title, string subject)
        {
            var document = GetBaseLayout();
            document.AddTitle(title);
            document.AddSubject(subject);
            document.AddCreator(PharmscriptionConstants.CompanyName);
            document.AddAuthor(PharmscriptionConstants.CompanyName);
            return document;
        }

        private void FormatPatientInformation(IDocListener document, Patient patient)
        {
            document.Add(GetChapterTitle("Patienteninformation", 18, Font.NORMAL));
            document.Add(GetStandardParagraph($"{patient.FirstName} {patient.LastName}"));
            document.Add(GetStandardParagraph($"AHV: {patient.AhvNumber}"));
            document.Add(GetStandardParagraph($"{patient.Address.Street} {patient.Address.Number}"));
            document.Add(GetStandardParagraph($"{patient.Address.Location} {patient.Address.CityCode.CityCode}"));
        }

        private Chapter GetChapterTitle(string text, int fontSize, int fontStyle)
        {
            var titleParagraph = new Paragraph(text, GetFontConfiguration(fontSize, fontStyle));
            return new Chapter(titleParagraph, ChapterNumber) {NumberDepth = 0, TriggerNewPage = false};
        }
        private static Paragraph GetStandardParagraph(string text)
        {
            return new Paragraph(text, GetStandardFontConfiguration());
        }
        private static Font GetFontConfiguration(int fontSize, int fontStyle)
        {
            var bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            var times = new Font(bfTimes, fontSize, fontStyle, BaseColor.BLACK);
            return times;
        }

        private static Font GetStandardFontConfiguration()
        {
            return GetFontConfiguration(12, Font.NORMAL);
        }

        private void FormatReportTitle(IDocListener document)
        {
            var now = DateTime.Now.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
            var aMonthAgo = DateTime.Now.AddMonths(-1)
                .ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
            var title = $"Rechnungsperiode {aMonthAgo} - {now}";
            document.Add(GetChapterTitle(title, 24, Font.NORMAL));

        }

        private void FormatPrescriptionTitle(IDocListener document)
        {
            document.Add(GetChapterTitle("Rezepte:", 18, Font.NORMAL));
        }

        public async Task<Document> FormatReport(Document document, DispenseInformation dispenseInformation)
        {
            document.Open();
            FormatReportTitle(document);
            FormatPatientInformation(document, dispenseInformation.Patient);
            FormatPrescriptionTitle(document);
            foreach (var prescriptionDispensese in dispenseInformation.PrescriptionDispenseses)
            {
                await FormatPrescriptionDispenses(document, prescriptionDispensese);
            }
            document.Close();
            return document;
        }

        private async Task FormatPrescriptionDispenses(IDocListener document, PrescriptionDispenses prescriptionDispenses)
        {
            var title = $"Rezept {prescriptionDispenses.Prescription.Id}:";
            document.Add(GetChapterTitle(title, 16, Font.NORMAL));
            if (prescriptionDispenses.Prescription.CreatedDate != null)
            {
                var validTitle =
                $"Laufzeit: {prescriptionDispenses.Prescription.CreatedDate.Value.ToString(PharmscriptionConstants.DateFormat)} - {DateTime.Now.ToString(PharmscriptionConstants.DateFormat)}";
                document.Add(GetStandardParagraph(validTitle));
            }
            foreach (var dispense in prescriptionDispenses.Dispenses)
            {
                await FormatDispenses(document, dispense);
            }

        }

        private static void AddSeperator(IDocListener document)
        {
            var separator = new LineSeparator();
            var linebreak = new Chunk(separator);
            document.Add(linebreak);
            document.Add(new Paragraph(""));
        }

        private async Task FormatDispenses(IDocListener document, Dispense dispense)
        {
            AddSeperator(document);
            var title = $"Ausgabe vom {dispense.Date.ToString(PharmscriptionConstants.DateFormat)}";
            document.Add(GetChapterTitle(title, 14, Font.BOLD));
            if (!string.IsNullOrWhiteSpace(dispense.Remark))
            {
                document.Add(GetStandardParagraph($"Bemerkung: {dispense.Remark}"));
            }
            var table = new PdfPTable(4) {SpacingBefore = 20f,  PaddingTop = 20f};
            var cell = new PdfPCell(new Phrase("Abrechnung"))
            {
                Colspan = 4,
                HorizontalAlignment = 1
            };
            table.AddCell(cell);

            table.AddCell("Anzahl");

            table.AddCell("Beschreibung");

            table.AddCell("Preis");

            table.AddCell("Preis Total");
            var total = 0.0;
            foreach (var drugItem in dispense.DrugItems)
            {
                var reportDrugItem = await  _drugPriceManager.GenerateDrugItemReport(drugItem);
                table.AddCell(reportDrugItem.Quantity.ToString());
                table.AddCell(reportDrugItem.Description);
                table.AddCell(reportDrugItem.FormattedPrice());
                table.AddCell(reportDrugItem.TotalPrice());
                total += reportDrugItem.Quantity * reportDrugItem.Price;
            }
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell(total.ToString("F"));
            document.Add(table);
            AddSeperator(document);
        }
    }
}
