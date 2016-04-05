using System;
using System.ServiceModel;
using com.pharmscription.Infrastructure.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.IntegrationTests
{
    [TestClass]
    public class PatientBaseRestTest : BaseRestTest
    {
        [TestMethod]
        public void CreatePatient()
        {
            var patient = Client.CreatePatient(new PatientDto()
            {
                    FirstName = "Livia",
                    LastName = "Danuser",
                    Address = new AddressDto
                    {
                        Street = "Neue Jonastrasse",
                        StreetExtension = "3. Stockwerk",
                        Number = "112",
                        City = "Rapperswil-Jona",
                        CityCode = "8640"
                    },
                    AhvNumber = "11",
                    BirthDate = new DateTime(1991, 3, 21),
                    Insurance = "Generika",
                    InsuranceNumber = "2",
                    PhoneNumber = "112"
                ,
            });
            Assert.IsNotNull(patient);
            Assert.AreNotEqual(0, patient.Id);

            var storedPatient = Client.GetPatient(patient.Id);
            Assert.IsNotNull(storedPatient);
        }
    }

    
}
