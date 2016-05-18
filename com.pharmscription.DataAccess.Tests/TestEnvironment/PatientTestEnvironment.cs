namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.PatientEntity;
    using DataAccess.Entities.PrescriptionEntity;
    using DataAccess.Repositories.Patient;
    using Infrastructure.EntityHelper;
    using Moq;

    [ExcludeFromCodeCoverage]
    public static class PatientTestEnvironment
    {
        public const string PatientIdOne = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38";
        public const string AhvNumberPatientOne = "756.1234.5678.97";
        public const string PatientIdTwo = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e48";
        public const string AhvNumberPatientTwo = "756.1390.2077.81";
        public const string PatientWithNoPrescriptionId = "1baf86b0-1e14-4f4c-b23a-5c9dd00e8e48";
        public const string PatientWithEmptyPrescriptionId = "1baf86b0-1e14-4f64-b23a-5c9dd00e8e48";
        public const string EmptyPrescriptionId = "1baf86d7-1e14-4f64-b23a-5c9dd00e8e48";
        public const string AhvNumberPatientThree = "756.4475.6859.48";
        public const string AhvNumberPatientFour = "756.8999.4760.82";
        public const string AhvNumberNotInDatabase = "756.7362.0816.87";

        public static Patient PatientWithDetailsAndOpenDispenses = new Patient
        {
            FirstName = "Max",
            LastName = "Müller",
            Address = new Address
            {
                Street = "Bergstrasse",
                Number = "100",
                CityCode = SwissCityCode.CreateInstance("8080"),
                Location = "Zürich",
                StreetExtension = "Postfach 1234"
            },
            AhvNumber = "7561234567897",
            BirthDate = DateTime.Now,
            InsuranceNumber = "Zurich-12345",
            PhoneNumber = "056 217 21 21",
            Insurance = "Zurich",
            CreatedDate = DateTime.Now,
            Prescriptions = new List<Prescription>
            {
                new SinglePrescription
                {
                    Id = IdentityGenerator.NewSequentialGuid(),
                    CreatedDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    IssueDate = DateTime.Now,
                    IsValid = true,
                    CounterProposals = new List<CounterProposal>
                    {
                        new CounterProposal
                        {
                            Date = DateTime.Now,
                            Message = "Hallo"
                        }
                    },
                    Dispenses = new List<Dispense>
                    {
                        new Dispense
                        {
                            CreatedDate = DateTime.Now,
                            Date = DateTime.Now,
                            Remark = "War eine super Ausgabe",
                            DrugItems = new List<DrugItem>
                            {
                                new DrugItem
                                {
                                    Quantity = 2,
                                    Drug = new Drug
                                    {
                                        Id = new Guid("8ef38d52-4d11-c819-6e8b-08d3783dfd75")
                                    }
                                },
                                new DrugItem
                                {
                                    Quantity = 3,
                                    Drug = new Drug
                                    {
                                        Id = new Guid("6d32f5e6-3cda-c903-a925-08d3783dfd75")
                                    }
                                }
                            }
                        }
                    },
                    DrugItems = new List<DrugItem>
                    {
                        new DrugItem
                        {
                            Quantity = 2,
                            Drug = new Drug
                            {
                                Id = new Guid("8ef38d52-4d11-c819-6e8b-08d3783dfd75")
                            }
                        },
                        new DrugItem
                        {
                            Quantity = 3,
                            Drug = new Drug
                            {
                                Id = new Guid("6d32f5e6-3cda-c903-a925-08d3783dfd75")
                            }
                        }
                    }
                }
            }
        };

        public static List<Patient> GetTestPatients()
        {
            return new List<Patient>
            {
                new Patient
                {
                    Id = Guid.Parse(PatientIdOne),
                    FirstName = "Rafael",
                    LastName = "Krucker",
                    AhvNumber = AhvNumberPatientOne,
                    BirthDate = DateTime.Parse("17.03.1991")
                },
                new Patient
                {
                    Id = Guid.Parse(PatientIdTwo),
                    FirstName = "Markus",
                    LastName = "Schaden",
                    AhvNumber = AhvNumberPatientTwo,
                    BirthDate = DateTime.Parse("24.05.1991")
                },
                new Patient
                {
                    Id = Guid.Parse(PatientWithNoPrescriptionId),
                    FirstName = "I have no",
                    LastName = "Prescriptons",
                    BirthDate = DateTime.Parse("28.05.1991")
                },
                new Patient
                {
                    Id = Guid.Parse(PatientWithEmptyPrescriptionId),
                    FirstName = "I have",
                    LastName = "an empty Prescripton",
                    BirthDate = DateTime.Parse("28.05.1991"),
                    Prescriptions = new List<Prescription>
                    {
                        new SinglePrescription
                        {
                            Id = Guid.Parse(EmptyPrescriptionId),
                            IssueDate = DateTime.Now,
                            EditDate = DateTime.Now,
                            Dispenses = new List<Dispense>()
                        }
                    }
                }
            };
        }

        public static Mock<PatientRepository> GetMockedPatientRepository()
        {
            var patients = GetTestPatients();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(patients);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Patients).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<Patient, PatientRepository>(mockPuow,
                mockSet, patients);
            mockedRepository.Setup(m => m.GetWithPrescriptions(It.IsAny<Guid>()))
                .Returns<Guid>(e => Task.FromResult(patients.FirstOrDefault(a => a.Id == e)));
            mockedRepository.Setup(m => m.GetPrescriptions(It.IsAny<Guid>()))
                .Returns<Guid>(
                    e => Task.FromResult(patients.Where(a => a.Id == e).Select(a => a.Prescriptions).FirstOrDefault()));
            mockedRepository.Setup(m => m.GetAllWithUnreportedDispenses(It.IsAny<DateTime>()))
                .Returns<DateTime>(
                    lastRespectedDate =>
                        Task.FromResult(
                            patients.Any(
                                e =>
                                    e?.Prescriptions != null &&
                                    e.Prescriptions.SelectMany(a => a.Dispenses).Any(c => !c.Reported) &&
                                    e.CreatedDate > lastRespectedDate)
                                ? (ICollection<Patient>) patients.Where(
                                    e => e?.Prescriptions != null &&
                                        e.Prescriptions.SelectMany(a => a.Dispenses).Any(c => !c.Reported) &&
                                        e.CreatedDate > lastRespectedDate).ToList()
                                : new List<Patient>()));
            return mockedRepository;
        }
    }
}