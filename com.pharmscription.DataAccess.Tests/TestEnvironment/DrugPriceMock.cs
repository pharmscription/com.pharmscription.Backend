namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLogic.Drug;
    using DataAccess.Entities.DrugPriceEntity;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using Infrastructure.Exception;

    [ExcludeFromCodeCoverage]
    public class DrugPriceMock
    {
        private readonly IDrugRepository _drugRepository;
        private readonly IDrugStoreRepository _drugStoreRepository;
        private readonly IDrugPriceRepository _drugPriceRepository;
        private readonly IDrugManager _drugManager;
        private const string AllDrugsSelector = "a";

        public DrugPriceMock(IDrugRepository drugRepository, IDrugStoreRepository drugStoreRepository,
            IDrugPriceRepository drugPriceRepository)
        {
            if (drugRepository == null || drugStoreRepository == null || drugPriceRepository == null)
            {
                throw new InvalidArgumentException("Not all Dependency were fulfilled");
            }
            _drugRepository = drugRepository;
            _drugStoreRepository = drugStoreRepository;
            _drugPriceRepository = drugPriceRepository;
            _drugManager = new DrugManager(_drugRepository);
        }

        public async Task PreloadDrugs()
        {
            await _drugManager.Count(AllDrugsSelector);
        }

        public async Task CreateTestDrugStore()
        {
            var drugStore = DrugStoreTestEnvironment.GetTestDrugStore();
            _drugStoreRepository.Add(drugStore);
            await _drugStoreRepository.UnitOfWork.CommitAsync();
        }

        public async Task GenerateRandomDrugPrices()
        {
            var drugStore = _drugStoreRepository.GetAll().FirstOrDefault();
            foreach (var drug in _drugRepository.GetAll())
            {
                var drugPrice = new DrugPrice
                {
                    Price = GetRandomPrice(),
                    Drug = drug,
                    DrugStore = drugStore
                };
                _drugPriceRepository.Add(drugPrice);
            }
            await _drugPriceRepository.UnitOfWork.CommitAsync();
        }

        public double GetRandomPrice()
        {
            var random = new Random();
            return random.NextDouble() * (100 - 1) + 1;
        }

        public async Task MockEnvironment()
        {
            await PreloadDrugs();
            await CreateTestDrugStore();
            await GenerateRandomDrugPrices();
        }
    }
}
