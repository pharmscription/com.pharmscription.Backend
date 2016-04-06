using System;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.DrugEntity;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace com.pharmscription.DataAccess.SwissMedic
{
    class SwissMedicMock : ISwissMedic
    {
        public Task<Drug> SearchDrug(string partialDescription)
        {
/*            using (SpreadsheetDocument spreadsheetDocument =
    SpreadsheetDocument.Open("Drugs.xlsx", false))
            {
                // Code removed here.
            }*/
            throw new NotImplementedException();
        }

        public Task<Drug> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
