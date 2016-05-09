

namespace com.pharmscription.Reporting
{
    using System;
    using System.Globalization;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.PatientEntity;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.text.pdf.draw;
    using Infrastructure.Constants;

    internal class PdfGenerator
    {
        private const int SideMargin = 36;

        private int _chapterNumber;

        private int ChapterNumber => _chapterNumber++;

        private Document GetBaseLayout()
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
            document.Open();
            document.Add(GetChapterTitle("Patienteninformation", 18, Font.NORMAL));
            document.Add(GetStandardParagraph($"{patient.FirstName} {patient.LastName}"));
            document.Add(GetStandardParagraph($"AHV: {patient.AhvNumber}"));
            document.Add(GetStandardParagraph($"{patient.Address.Street} {patient.Address.Number}"));
            document.Add(GetStandardParagraph($"{patient.Address.Location} {patient.Address.CityCode.CityCode}"));
        }

        private Chapter GetChapterTitle(string text, int fontSize, int fontStyle)
        {
            var titleParagraph = new Paragraph(text, GetFontConfiguration(fontSize, fontStyle));
            return new Chapter(titleParagraph, ChapterNumber) {NumberDepth = 0};
        }
        private Paragraph GetStandardParagraph(string text)
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
            document.Open();
            var now = DateTime.Now.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
            var aMonthAgo = DateTime.Now.AddMonths(-1)
                .ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
            var title = $"Rechnungsperiode {aMonthAgo} - {now}";
            document.Add(GetChapterTitle(title, 24, Font.NORMAL));

        }

        private void FormatPrescriptionTitle(IDocListener document)
        {
            document.Open();
            document.Add(GetChapterTitle("Rezepte:", 18, Font.NORMAL));
        }

        public Document FormatReport(Document document, DispenseInformation dispenseInformation)
        {
            document.Open();
            FormatReportTitle(document);
            FormatPatientInformation(document, dispenseInformation.Patient);
            FormatPrescriptionTitle(document);
            foreach (var prescriptionDispensese in dispenseInformation.PrescriptionDispenseses)
            {
                FormatPrescriptionDispenses(document, prescriptionDispensese);
            }
            document.Close();
            return document;
        }

        private void FormatPrescriptionDispenses(IDocListener document, PrescriptionDispenses prescriptionDispenses)
        {
            document.Open();
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
                FormatDispenses(document, dispense);
            }

        }

        private static void AddSeperator(IDocListener document)
        {
            document.Open();
            var separator = new LineSeparator();
            var linebreak = new Chunk(separator);
            document.Add(linebreak);
            document.Add(new Paragraph(""));
        }

        private void FormatDispenses(IDocListener document, Dispense dispense)
        {
            document.Open();
            AddSeperator(document);
            document.Open();
            var title = $"Ausgabe vom {dispense.Date.ToString(PharmscriptionConstants.DateFormat)}";
            document.Add(GetChapterTitle(title, 14, Font.BOLD));
            if (!string.IsNullOrWhiteSpace(dispense.Remark))
            {
                document.Add(GetStandardParagraph($"Bemerkung: {dispense.Remark}"));
            }
            foreach (var drugItem in dispense.DrugItems)
            {
                document.Add(GetStandardParagraph($"Menge: {drugItem.Quantity}, Bezeichnung: {drugItem.Drug.DrugDescription}"));

            }
            AddSeperator(document);
        }
    }
}
