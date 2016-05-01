
namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Repositories.Drug;
    using Moq;

    [ExcludeFromCodeCoverage]
    public class DrugTestEnvironment
    {
        public const string DrugOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e40";
        public const string DrugOneDescription = "Aspirin";
        public const string DrugTwoId = "1bbf86b0-1f14-4f4c-c05a-5c9dd00e8e40";
        public const string DrugTwoDescription = "Mebucain";
        public static List<Drug> GetTestDrugs()
        {
            return new List<Drug>
            {
              new Drug
                {
                    Id = Guid.Parse(DrugOneId),
                    IsValid = true,
                    DrugDescription = DrugOneDescription
                },
              new Drug
              {
                  Id = Guid.Parse(DrugTwoId),
                  DrugDescription = DrugTwoDescription,
                  IsValid = true
              }
            };
        }

        public static Mock<DrugRepository> GetMockedDrugsRepository()
        {
            var drugItems = GetTestDrugs();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(drugItems);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Drugs).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<Drug, DrugRepository>(mockPuow, mockSet, drugItems);
            return mockedRepository;
        }
    }
}
