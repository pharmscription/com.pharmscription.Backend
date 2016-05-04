using System;
using System.Diagnostics.CodeAnalysis;

using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

using DeepEqual.Syntax;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{


    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PatientConverterTest
    {
        
        [TestMethod]
        public void TestInsurancePatient()
        {
            DateTime birthDate = new DateTime(2000, 10, 10);
            var insurancePatient = new InsurancePatient
            {
                FirstName = "Max",
                LastName = "Müller",
                Street = "Bergstrasse",
                StreetNumber = "100",
                CityCode = "8000",
                City = " Zürich",
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var expectedPatient = new PatientDto
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new AddressDto
                { 
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = "8000",
                    Location = " Zürich"
                },
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate.ToString(PharmscriptionConstants.DateFormat),
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = insurancePatient.ConvertToDto();

            Assert.IsTrue(expectedPatient.IsDeepEqual(patient));

        }

        [TestMethod]
        public void TestInsurancePatientNull()
        {
            InsurancePatient insurancePatient = null;
            var patient = insurancePatient.ConvertToDto();

            Assert.IsNull(patient);
        }

        [TestMethod]
        public void TestEntityPatient()
        {
            DateTime birthDate = new DateTime(2000, 10, 10);
            var entityPatient = new Patient
            {
                Id = new Guid("bb3731be-7eac-4d95-af0e-8deae4efa630"),
                FirstName = "Max",
                LastName = "Müller",
                Address = new Address
                {
                    Id = new Guid("bb3731be-7eac-4d95-af0e-8deae4efa656"),
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = SwissCityCode.CreateInstance("8000"),
                    Location = " Zürich"
                },
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var expectedPatient = new PatientDto
            {
                Id = "bb3731be-7eac-4d95-af0e-8deae4efa630",
                FirstName = "Max",
                LastName = "Müller",
                Address = new AddressDto
                {
                    Id = "bb3731be-7eac-4d95-af0e-8deae4efa656",
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = "8000",
                    Location = " Zürich"
                },
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate.ToString(PharmscriptionConstants.DateFormat),
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = entityPatient.ConvertToDto();

            Assert.IsTrue(expectedPatient.IsDeepEqual(patient));
        }

        [TestMethod]
        public void TestEntityPatientNull()
        {
            Patient entityPatient = null;
            var patient = entityPatient.ConvertToDto();

            Assert.IsNull(patient);
        }

        [TestMethod]
        public void TestPatientDto()
        {
            DateTime birthDate = new DateTime(2000, 10, 10);
            var addressId = Guid.NewGuid();
            var identityId = Guid.NewGuid();
            var expectedPatient = new Patient
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new Address
                {
                    Id = addressId,
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = SwissCityCode.CreateInstance("8000"),
                    Location = " Zürich",
                    StreetExtension = "Postfach 1234"
                },
                User = new IdentityUser
                {
                    Id = identityId,
                    UserName  = "123-1234-1234-12"
                },
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patientDto = new PatientDto
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new AddressDto
                {
                    Id = addressId.ToString(),
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = "8000",
                    Location = " Zürich",
                    StreetExtension = "Postfach 1234"
                },
                Identity = new IdentityDto
                {
                    Id = identityId.ToString(),
                    UserName = "123-1234-1234-12"
                },
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate.ToString(PharmscriptionConstants.DateFormat),
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = patientDto.ConvertToEntity();

            Assert.IsTrue(expectedPatient.IsDeepEqual(patient));

        }

        [TestMethod]
        public void TestEntityPatientDtoNull()
        {
            PatientDto patientDto = null;
            var patient = patientDto.ConvertToEntity();

            Assert.IsNull(patient);
        }

    }
}
