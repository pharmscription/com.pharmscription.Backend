using System;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;
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

            Assert.AreEqual(expectedPatient, patient);

        }

        [TestMethod]
        public void TestNull()
        {
            InsurancePatient insurancePatient = null;
            var patient = PatientConverter.Convert(insurancePatient);

            Assert.IsNull(patient);
        }

        [TestMethod]
        public void TestEmpty()
        {

        }
    }
}
