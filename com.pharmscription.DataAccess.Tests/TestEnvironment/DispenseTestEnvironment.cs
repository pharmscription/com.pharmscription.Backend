using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Repositories.Dispense;
using Moq;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    [ExcludeFromCodeCoverage]
    public class DispenseTestEnvironment
    {
        public const string DispenseOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e43";
        public const string DispenseTwoId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e66";
        public const string DispenseOneRemark = "Did a Dispense";
        public const string DispenseTwoRemark = "Another Dispense";

        public static List<Dispense> GetTestDispenses()
        {
            return new List<Dispense>
            {
                new Dispense
                {
                    Id = Guid.Parse(DispenseOneId),
                    Remark = DispenseOneRemark,
                },
                new Dispense
                    {
                        Id = Guid.Parse(DispenseTwoId),
                        Remark = DispenseTwoRemark
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
            mockedRepository.Setup(m => m.Modify(It.IsAny<Dispense>())).Callback(
                (Dispense dispense) =>
                {
                    mockPuow.Object.Dispenses.Remove(dispense);
                    mockPuow.Object.Dispenses.Add(dispense);
                }).Verifiable();
            return mockedRepository;
        }
    }
}
