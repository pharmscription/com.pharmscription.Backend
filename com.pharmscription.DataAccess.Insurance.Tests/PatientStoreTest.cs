using System;
using System.Collections.Generic;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.DataAccess.Insurance.Tests
{
    public class PatientStoreTest : IPatientStore
    {
        public List<InsurancePatient> Patients => new List<InsurancePatient>
        {
            new InsurancePatient
            {
                FirstName = "Max",
                LastName = "Müller",
                Street = "Bergstrasse",
                StreetNumber = "100",
                CityCode = "8000",
                City = " Zürich",
                AhvNumber = "123-1234-1234-12",
                BirthDate = new DateTime(2000, 10, 10),
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            }
        };
    }
}
