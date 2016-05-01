

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Repositories.DrugItem;
    using Moq;

    [ExcludeFromCodeCoverage]
    public class DrugItemTestEnvironment
    {
        public const string DrugItemOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e39";
        public static List<DrugItem> GetTestDrugItems()
        {
            return new List<DrugItem>
            {
                new DrugItem
                {
                    Id = Guid.Parse(DrugItemOneId),
                    Drug = new Drug
                    {
                        Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e40"),
                        IsValid = true,
                        DrugDescription = "Aspirin"
                    },
                    DosageDescription = "2/3/2",
                    Dispense = new Dispense
                    {
                        Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e41"),
                        Remark = "Did it"
                    }
                }
            };
        }

        public static Mock<DrugItemRepository> GetMockedDrugItemsRepository()
        {
            var drugItems = GetTestDrugItems();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(drugItems);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.DrugItems).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<DrugItem, DrugItemRepository>(mockPuow, mockSet, drugItems);
            return mockedRepository;
        }
    }
}
