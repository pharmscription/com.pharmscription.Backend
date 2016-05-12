using System;
using System.Threading.Tasks;

namespace com.pharmscription.BusinessLogic.DrugPrice
{
    using DataAccess.Entities.DrugItemEntity;

    public interface IDrugPriceManager
    {
        Task<double> GetPrice(Guid drugStoreId, Guid drugId);
        Task<double> GetPrice(Guid drugId);
        Task<ReportDrugItem> GenerateDrugItemReport(DrugItem drugItem);

    }
}
