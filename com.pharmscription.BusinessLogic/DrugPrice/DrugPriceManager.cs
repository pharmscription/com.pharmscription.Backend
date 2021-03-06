﻿

namespace com.pharmscription.BusinessLogic.DrugPrice
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.DatabaseSeeder;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using Infrastructure.Constants;
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

        public async Task SeedDataTables()
        {
            if (!_drugRepository.GetAll().Any())
            {
                await DatabaseSeeder.SeedDataTableAsync(Seed.Drug);
            }
            if (!_drugStoreRepository.GetAll().Any())
            {
                await DatabaseSeeder.SeedDataTableAsync(Seed.Address);
                await DatabaseSeeder.SeedDataTableAsync(Seed.DrugStore);
            }
            if (!_drugPriceRepository.GetAll().Any())
            {
                await DatabaseSeeder.SeedDataTableAsync(Seed.DrugPrice);
            }
        }

        private async Task CheckOrSeedPrices()
        {
            if (!_drugPriceRepository.GetAll().Any())
            {
                await SeedDataTables();
            }
        }
        public async Task<double> GetPrice(Guid drugStoreId, Guid drugId)
        {
            await CheckOrSeedPrices();
            return await _drugPriceRepository.GetPrice(drugStoreId, drugId);
        }

        public async Task<double> GetPrice(Guid drugId)
        {
            await CheckOrSeedPrices();
            return await _drugPriceRepository.GetPrice(drugId, new Guid(PharmscriptionConstants.DefaultDrugStoreId));
        }

        public async  Task<ReportDrugItem> GenerateDrugItemReport(DrugItem drugItem)
        {
            await CheckOrSeedPrices();
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
