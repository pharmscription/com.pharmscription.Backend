
namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using com.pharmscription.Infrastructure.Constants;
    using com.pharmscription.Infrastructure.Dto;

    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Repositories.Dispense;
    using Moq;

    [ExcludeFromCodeCoverage]
    public class DispenseTestEnvironment
    {
        private static readonly DateTime NowDateTime = DateTime.Now;

        public const string DispenseOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e43";
        public const string DispenseOneRemark = "Did a Dispense";
        public static List<Dispense> GetTestDispenses()
        {
            return new List<Dispense>
            {
                new Dispense
                {
                    Id = Guid.Parse(DispenseOneId),
                    Date = NowDateTime,
                    Remark = DispenseOneRemark,
                }
            };
        }

        public static List<DispenseDto> GetTestDispenseDtos()
        {
            return new List<DispenseDto>
            {
                new DispenseDto
                    {
                        Id = DispenseOneId,
                        Date = NowDateTime.ToString(PharmscriptionConstants.DateFormat, CultureInfo.InvariantCulture),
                        Remark = DispenseOneRemark
                    }
            };
        }
        public static Mock<DispenseRepository> GetMockedDispenseRepository()
        {
            var dispenses = GetTestDispenses();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(dispenses);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Dispenses).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<Dispense, DispenseRepository>(mockPuow, mockSet, dispenses);
            return mockedRepository;
        }

    }
}
