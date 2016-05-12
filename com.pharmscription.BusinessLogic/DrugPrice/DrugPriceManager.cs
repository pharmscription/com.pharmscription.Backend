

namespace com.pharmscription.BusinessLogic.DrugPrice
{
    using System;
    using System.Threading.Tasks;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using Infrastructure.Exception;

    public class DrugPriceManager: IDrugPriceManager
    {
        private readonly IDrugPriceRepository _drugPriceRepository;
        private readonly IDrugStoreRepository _drugStoreRepository;
        private readonly IDrugRepository _drugRepository;

        public DrugPriceManager(IDrugPriceRepository drugPriceRepository, IDrugStoreRepository drugStoreRepository,
            IDrugRepository drugRepository)
        {
            if (drugPriceRepository == null || drugStoreRepository == null || drugRepository == null)
            {
                throw new InvalidArgumentException("Not all dependencies were provided");
            }
            _drugRepository = drugRepository;
            _drugPriceRepository = drugPriceRepository;
            _drugStoreRepository = drugStoreRepository;
        }
        public async Task<double> GetPrice(Guid drugStoreId, Guid drugId)
        {
            return await _drugPriceRepository.GetPrice(drugStoreId, drugId);
        }

        public async Task<double> GetPrice(Guid drugId)
        {
            return await _drugPriceRepository.GetPrice(drugId, new Guid("d7b84f98-d923-ca54-d1b0-08d3783e0110"));
        }

        public async  Task<ReportDrugItem> GenerateDrugItemReport(DrugItem drugItem)
        {
            var price = await GetPrice(drugItem.Drug.Id);
            var reportDrugItem = new ReportDrugItem
            {
                Price = price,
                Quantity = drugItem.Quantity,
                Description = drugItem.Drug.DrugDescription
            };
            return reportDrugItem;
        }

    }
}
