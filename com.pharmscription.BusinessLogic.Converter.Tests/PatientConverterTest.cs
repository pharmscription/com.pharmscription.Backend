﻿using System;
using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{
    [TestClass]
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
                Address = new AddressDto { 
                Street = "Bergstrasse",
                Number = "100",
                CityCode = "8000",
                City = " Zürich"},
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = PatientConverter.Convert(insurancePatient);
            
            Assert.IsTrue(expectedPatient.IsDeepEqual(patient));

        }

        [TestMethod]
        public void TestInsurancePatientNull()
        {
            InsurancePatient insurancePatient = null;
            var patient = PatientConverter.Convert(insurancePatient);

            Assert.IsNull(patient);
        }

        [TestMethod]
        public void TestEntityPatient()
        {
            DateTime birthDate = new DateTime(2000, 10, 10);
            var entityPatient = new Patient
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new Address
                {
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
                FirstName = "Max",
                LastName = "Müller",
                Address = new AddressDto
                {
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = "8000",
                    City = " Zürich"
                },
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = PatientConverter.Convert(entityPatient);

            Assert.IsTrue(expectedPatient.IsDeepEqual(patient));

        }

        [TestMethod]
        public void TestEntityPatientNull()
        {
            Patient entityPatient = null;
            var patient = PatientConverter.Convert(entityPatient);

            Assert.IsNull(patient);
        }



        [TestMethod]
        public void TestPatientDto()
        {
            DateTime birthDate = new DateTime(2000, 10, 10);
            var expectedPatient = new Patient
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new Address
                {
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = SwissCityCode.CreateInstance("8000"),
                    Location = " Zürich",
                    StreetExtension = "Postfach 1234"
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
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = "8000",
                    City = " Zürich",
                    StreetExtension = "Postfach 1234"
                },
                AhvNumber = "123-1234-1234-12",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = PatientConverter.Convert(patientDto);

            Assert.IsTrue(expectedPatient.IsDeepEqual(patient));

        }

        [TestMethod]
        public void TestEntityPatientDtoNull()
        {
            PatientDto patientDto = null;
            var patient = PatientConverter.Convert(patientDto);

            Assert.IsNull(patient);
        }

    }
}
