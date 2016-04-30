using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.DataAccess.Repositories.CounterProposal;
using com.pharmscription.DataAccess.Repositories.Dispense;
using com.pharmscription.DataAccess.Repositories.DrugItem;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.EntityHelper;
using com.pharmscription.Infrastructure.Exception;
using Moq;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Repositories.Drug;

    [ExcludeFromCodeCoverage]
    public class TestEnvironmentHelper
    {
        public static Mock<PharmscriptionUnitOfWork> GetMockedDataContext()
        {
            return new Mock<PharmscriptionUnitOfWork>();
        }

        public static List<DrugItem> GetTestDrugItems()
        {
            return new List<DrugItem>
            {
                new DrugItem
                {
                    Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e39"),
                    Drug = new DataAccess.Entities.DrugEntity.Drug
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

        public static List<CounterProposal> GetTestCounterProposals()
        {
            return new List<CounterProposal>
            {
                new CounterProposal
                {
                    Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e42"),
                    Message = "This is not right",
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5c9dd01e8e42"),
                    Message = "This is not for Malaria",
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5d9dd00e8e42"),
                    Message = "The Patient is already dead",
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse("1baf86b0-1e14-5f4c-b05a-5c9dd00e8e42"),
                    Message = "Aspirin is better than a plaster",
                    Date = DateTime.Now
                },
                new CounterProposal
                {
                    Id = Guid.Parse("1baf86b1-1e14-4f4c-b05a-5c9dd00e8e42"),
                    Message = "This isnt even a Prescription, it is a giraffe",
                    Date = DateTime.Now
                }
            };
        }

        public static List<Dispense> GetTestDispenses()
        {
            return new List<Dispense>
            {
                new Dispense
                {
                    Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e43"),
                    Date = DateTime.Now,
                    Remark = "Did a Dispense",
                }
            };
        }

        public static Mock<CounterProposalRepository> GetMockedCounterProposalRepository()
        {
            var counterProposals = GetTestCounterProposals();
            var mockSet = GetMockedAsyncProviderDbSet(counterProposals);
            var mockPuow = GetMockedDataContext();
            mockPuow.Setup(m => m.CounterProposals).Returns(mockSet.Object);
            var mockedRepository = CreateMockedRepository<CounterProposal, CounterProposalRepository>(mockPuow, mockSet, counterProposals);
            return mockedRepository;
        }

        public static Mock<DispenseRepository> GetMockedDispenseRepository()
        {
            var dispenses = GetTestDispenses();
            var mockSet = GetMockedAsyncProviderDbSet(dispenses);
            var mockPuow = GetMockedDataContext();
            mockPuow.Setup(m => m.Dispenses).Returns(mockSet.Object);
            var mockedRepository = CreateMockedRepository<Dispense, DispenseRepository>(mockPuow, mockSet, dispenses);
            return mockedRepository;
        }

        public static Mock<DrugRepository> GetMockedDrugsRepository()
        {
            var drugItems = new List<Drug>();
            var mockSet = GetMockedAsyncProviderDbSet(drugItems);
            var mockPuow = GetMockedDataContext();
            mockPuow.Setup(m => m.Drugs).Returns(mockSet.Object);
            var mockedRepository = CreateMockedRepository<Drug, DrugRepository>(mockPuow, mockSet, drugItems);
            return mockedRepository;
        }

        public static Mock<DrugItemRepository> GetMockedDrugItemsRepository()
        {
            var drugItems = GetTestDrugItems();
            var mockSet = GetMockedAsyncProviderDbSet(drugItems);
            var mockPuow = GetMockedDataContext();
            mockPuow.Setup(m => m.DrugItems).Returns(mockSet.Object);
            var mockedRepository = CreateMockedRepository<DrugItem, DrugItemRepository>(mockPuow, mockSet, drugItems);
            return mockedRepository;
        }

        public static Mock<TRepository> CreateMockedRepository<TEntity, TRepository>(
    Mock<PharmscriptionUnitOfWork> mockPuow, Mock<DbSet<TEntity>> mockSet, List<TEntity> initialData)
    where TEntity : class, IEntity, ICloneable<TEntity> where TRepository : class, IRepository<TEntity>
        {
            mockPuow.Setup(m => m.CreateSet<TEntity>()).Returns(mockSet.Object);
            var mockedRepository = new Mock<TRepository>(mockPuow.Object);
            mockedRepository.Setup(m => m.GetAsync(It.IsAny<Guid>()))
                .Returns<Guid>(e => Task.FromResult(initialData.FirstOrDefault(a => a.Id == e)));
            mockedRepository.Setup(m => m.GetAll()).Returns(initialData);
            mockedRepository.Setup(m => m.GetAsync(It.IsAny<Guid>()))
                .Returns<Guid>(e => Task.FromResult(initialData.FirstOrDefault(a => a.Id == e)));
            mockedRepository.Setup(m => m.UnitOfWork).Returns(mockPuow.Object);
            mockedRepository.Setup(m => m.Add(It.IsAny<TEntity>()))
                .Callback<TEntity>(e => mockSet.Object.Add(e));
            mockedRepository.Setup(m => m.GetAsyncOrThrow(It.IsAny<Guid>())).Returns<Guid>(e =>
            {
                var entity = initialData.FirstOrDefault(a => a.Id == e);
                if (entity == null)
                {
                    throw new NotFoundException("No Such Entity");
                }
                return Task.FromResult(entity);
            });
            mockedRepository.Setup(m => m.CheckIfEntityExists(It.IsAny<Guid>())).Returns<Guid>(e =>
            {
                {
                    var entity = initialData.FirstOrDefault(a => a.Id == e);
                    if (entity == null)
                    {
                        throw new NotFoundException("No Such Entity");
                    }
                    return Task.FromResult(true);
                }
            });
            return mockedRepository;
        }

        public static Mock<DbSet<TEntity>> GetMockedAsyncProviderDbSet<TEntity>(List<TEntity> sampleData)
            where TEntity : class, IEntity, ICloneable<TEntity>
        {
            var mockSet = GetMockedDbSet(sampleData);
            mockSet.As<IDbAsyncEnumerable<TEntity>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(() => new DbAsyncEnumeratorMock<TEntity>(sampleData.GetEnumerator()));

            mockSet.As<IQueryable<TEntity>>()
                .Setup(m => m.Provider)
                .Returns(new DbAsyncQueryProviderMock<TEntity>(sampleData.AsQueryable().Provider));

            return mockSet;
        } 
        public static Mock<DbSet<TEntity>> GetMockedDbSet<TEntity>(List<TEntity> sampleData) where TEntity : class, IEntity, ICloneable<TEntity>
        {
            var mockSet = new Mock<DbSet<TEntity>>();
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(sampleData.AsQueryable().Provider);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(sampleData.AsQueryable().Expression);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(sampleData.AsQueryable().ElementType);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => sampleData.GetEnumerator());

            mockSet.Setup(d => d.Add(It.IsAny<TEntity>())).Callback<TEntity>(e =>
            {
                e.Id = IdentityGenerator.NewSequentialGuid();
                sampleData.Add(e);
            });
            mockSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<TEntity>>()))
                .Callback((IEnumerable<TEntity> list) => sampleData.AddRange(list));
            mockSet.Setup(d => d.Remove(It.IsAny<TEntity>()))
                .Callback((TEntity el) => sampleData.Remove(el));
            mockSet.Setup(d => d.RemoveRange(It.IsAny<IEnumerable<TEntity>>()))
                .Callback((IEnumerable<TEntity> l) => sampleData.RemoveAll(l.Contains));

            mockSet.Setup(d => d.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => sampleData.FirstOrDefault(d => d.Id == (Guid)ids[0]));
            mockSet.Setup(d => d.AsNoTracking()).Returns(
                () => {

                    var mockSetCopy = new Mock<DbSet<TEntity>>();
                    var patientsCopy = new List<TEntity>(sampleData.Count);
                    sampleData.ForEach(e => patientsCopy.Add(e.Clone()));

                    mockSetCopy.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(patientsCopy.AsQueryable().Provider);
                    mockSetCopy.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(patientsCopy.AsQueryable().Expression);
                    mockSetCopy.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(patientsCopy.AsQueryable().ElementType);
                    mockSetCopy.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(() => patientsCopy.GetEnumerator());

                    return mockSetCopy.Object;
                }
            );
            return mockSet;
        }
    }
}
