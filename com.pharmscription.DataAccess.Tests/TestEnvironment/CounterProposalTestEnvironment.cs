

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Repositories.CounterProposal;
    using Moq;

    [ExcludeFromCodeCoverage]
    public class CounterProposalTestEnvironment
    {
        public const string CounterProposalOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e42";
        public const string CounterProposalOneMessage = "This is not right";
        public const string CounterProposalTwoId = "1baf86b0-1e14-4f4c-b05a-5c9dd01e8e42";
        public const string CounterProposalTwoMessage = "This is not for Malaria";
        public const string CounterProposalThreeId = "1baf86b0-1e14-4f4c-b05a-5d9dd00e8e42";
        public const string CounterProposalThreeMessage = "The Patient is already dead";
        public const string CounterProposalFourId = "1baf86b0-1e14-5f4c-b05a-5c9dd00e8e42";
        public const string CounterProposalFourMessage = "Aspirin is better than a plaster";
        public const string CounterProposalFiveId = "1baf86b1-1e14-4f4c-b05a-5c9dd00e8e42";
        public const string CounterProposalFiveMessage = "This isnt even a Prescription, it is a giraffe";
        public static List<CounterProposal> GetTestCounterProposals()
        {
            return new List<CounterProposal>
            {
                new CounterProposal
                {
                    Id = Guid.Parse(CounterProposalOneId),
                    Message = CounterProposalOneMessage,
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse(CounterProposalTwoId),
                    Message = CounterProposalTwoMessage,
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse(CounterProposalThreeId),
                    Message = CounterProposalThreeMessage,
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse(CounterProposalFourId),
                    Message = CounterProposalFourMessage,
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse(CounterProposalFiveId),
                    Message = CounterProposalFiveMessage,
                    Date = DateTime.Now
                }
            };
        }

        public static Mock<CounterProposalRepository> GetMockedCounterProposalRepository()
        {
            var counterProposals = GetTestCounterProposals();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(counterProposals);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.CounterProposals).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<CounterProposal, CounterProposalRepository>(mockPuow, mockSet, counterProposals);
            return mockedRepository;
        }
    }
}
