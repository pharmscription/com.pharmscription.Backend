
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.DataAccess.Repositories.Prescription;
using Moq;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    public static class PrescriptionTestEnvironment
    {
        public const string StandingPrescriptionOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37";
        public const string StandingPrescriptionTwoId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e47";
        public const string DrugItemOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e39";
        public const string DrugOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e40";
        public const string DrugItemTwoId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e49";
        public const string DrugTwoId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e50";

        public static List<Prescription> GetTestPrescriptions()
        {
            return new List<Prescription>
            {
                new StandingPrescription
                {
                    Id = Guid.Parse(StandingPrescriptionOneId),
                    IsValid = true,
                    SignDate = DateTime.Now,
                    IssueDate = DateTime.Now,
                    ValidUntill = DateTime.Now.AddDays(2),
                    EditDate = DateTime.Now,
                    DrugItems = new List<DrugItem>
                    {
                        new DrugItem
                        {
                            Id = Guid.Parse(DrugItemOneId),
                            Drug = new DataAccess.Entities.DrugEntity.Drug
                            {
                                Id = Guid.Parse(DrugOneId),
                                IsValid = true,
                                DrugDescription = "Aspirin"
                            },
                            DosageDescription = "2/3/2"
                        }
                    }
                },
                new StandingPrescription
                {
                    Id = Guid.Parse(StandingPrescriptionTwoId),
                    IsValid = true,
                    SignDate = DateTime.Now,
                    IssueDate = DateTime.Now,
                    ValidUntill = DateTime.Now.AddDays(2),
                    EditDate = DateTime.Now,
                    DrugItems = new List<DrugItem>
                    {
                        new DrugItem
                        {
                            Id = Guid.Parse(DrugItemTwoId),
                            Drug = new DataAccess.Entities.DrugEntity.Drug
                            {
                                Id = Guid.Parse(DrugTwoId),
                                IsValid = true,
                                DrugDescription = "Mebucain"
                            },
                            DosageDescription = "2/3/2"
                        }
                    }
                }
            };
        }

        public static Mock<PrescriptionRepository> GetMockedPrescriptionRepository()
        {
            var prescriptions = GetTestPrescriptions();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(prescriptions);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Prescriptions).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<Prescription>()).Returns(mockSet.Object);
            var mockedRepository = new Mock<PrescriptionRepository>(mockPuow.Object);
            mockedRepository.Setup(m => m.GetByPatientId(It.IsAny<Guid>())).Returns<Guid>(e => Task.FromResult(prescriptions.Where(a => a.Patient.Id == e).ToList()));
            mockedRepository.Setup(m => m.GetAll()).Returns(prescriptions);
            mockedRepository.Setup(m => m.GetAsync(It.IsAny<Guid>()))
                .Returns<Guid>(e => Task.FromResult(prescriptions.FirstOrDefault(a => a.Id == e)));
            mockedRepository.Setup(m => m.UnitOfWork).Returns(mockPuow.Object);
            mockedRepository.Setup(m => m.Add(It.IsAny<Prescription>()))
                .Callback<Prescription>(e => mockSet.Object.Add(e));
            return mockedRepository;
        }
    }
}
