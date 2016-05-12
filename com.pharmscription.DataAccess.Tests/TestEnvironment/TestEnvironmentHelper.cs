using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.EntityHelper;
using com.pharmscription.Infrastructure.Exception;
using Moq;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{

    [ExcludeFromCodeCoverage]
    public class TestEnvironmentHelper
    {

        public static ICollection<string> DeleteStatments => new List<string>
        {
            "Delete From DrugItems",
            "Delete From CounterProposals",
            "Delete From Dispenses",
            "Delete From Prescriptions",
            "Delete From Drugs",
            "Delete From Patients"
        };

        public static Mock<PharmscriptionUnitOfWork> GetMockedDataContext()
        {
            return new Mock<PharmscriptionUnitOfWork>();
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
                .Returns<TEntity>(
                    e =>
                        {
                            mockSet.Object.Add(e);
                            return e;
                        });
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

            mockSet.Setup(d => d.Add(It.IsAny<TEntity>())).Returns<TEntity>(e =>
            {
                e.Id = IdentityGenerator.NewSequentialGuid();
                sampleData.Add(e);
                return e;
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
