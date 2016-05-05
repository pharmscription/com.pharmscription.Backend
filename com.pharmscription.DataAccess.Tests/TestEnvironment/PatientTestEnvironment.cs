using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.DataAccess.Repositories.Patient;
using Moq;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
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
                            EditDate = DateTime.Now
                        }
                    }
                }
            };
        }

        public static List<PatientDto> GetPatientDtos()
        {
            return new List<PatientDto>
            {
                new PatientDto
                {
                    Id = PatientIdOne,
                    FirstName = "Rafael",
                    LastName = "Krucker",
                    AhvNumber = AhvNumberPatientOne,
                    BirthDate = "17.03.1991"
                },
                new PatientDto
                {
                    Id = PatientIdTwo,
                    FirstName = "Markus",
                    LastName = "Schaden",
                    AhvNumber = AhvNumberPatientTwo,
                    BirthDate = "24.05.1991"
                },
                new PatientDto
                {
                    Id = PatientWithNoPrescriptionId,
                    FirstName = "I have no",
                    LastName = "Prescriptons",
                    BirthDate = "28.05.1991"
                },
                new PatientDto
                {
                    Id = PatientWithEmptyPrescriptionId,
                    FirstName = "I have",
                    LastName = "an empty Prescripton",
                    BirthDate = "28.05.1991"
                }
            };
        }

        public static Mock<PatientRepository> GetMockedPatientRepository()
        {
            var patients = GetTestPatients();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(patients);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Patients).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<Patient, PatientRepository>(mockPuow, mockSet, patients);
            mockedRepository.Setup(m => m.GetWithPrescriptions(It.IsAny<Guid>()))
            .Returns<Guid>(e => Task.FromResult(patients.FirstOrDefault(a => a.Id == e)));
            mockedRepository.Setup(m => m.GetPrescriptions(It.IsAny<Guid>()))
                .Returns<Guid>(
                    e => Task.FromResult(patients.Where(a => a.Id == e).Select(a => a.Prescriptions).FirstOrDefault()));
            return mockedRepository;
        }
    }
}
