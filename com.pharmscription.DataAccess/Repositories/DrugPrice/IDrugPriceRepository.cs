

namespace com.pharmscription.DataAccess.Repositories.DrugPrice
{
    using System;
    using System.Threading.Tasks;
    using Entities.DrugPriceEntity;
    using SharedInterfaces;
    public interface IDrugPriceRepository : IRepository<DrugPrice>
    {
        Task<double> GetPrice(Guid drugId, Guid storeId);
    }
}
