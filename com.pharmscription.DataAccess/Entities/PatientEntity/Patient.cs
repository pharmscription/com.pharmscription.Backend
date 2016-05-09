using System;
using System.Collections.Generic;
using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.PatientEntity
{
    public class Patient : Entity, ICloneable<Patient>, IEquatable<Patient>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMailAddress { get; set; }
        public string AhvNumber { get; set; }
        public Address Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string Insurance { get; set; }

        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public Patient Clone()
        {
            return new Patient
            {
                FirstName = FirstName,
                LastName = LastName,
                AhvNumber = AhvNumber,
                Address = Address,
                BirthDate = BirthDate,
                PhoneNumber = PhoneNumber,
                InsuranceNumber = InsuranceNumber,
                Insurance = Insurance
            };
        }

        public bool Equals(Patient other)
        {
            if (other == null)
            {
                return false;
            }
            bool addressEquals = true;
            if (Address != null && other.Address != null)
            {
                addressEquals = Address.Equals(other.Address);
            }
            return addressEquals && FirstName == other.FirstName && LastName == other.LastName
                   && AhvNumber == other.AhvNumber && Insurance == other.Insurance
                   && InsuranceNumber == other.InsuranceNumber && BirthDate.Equals(other.BirthDate)
                   && PhoneNumber == other.PhoneNumber;
        }

        public void Update(Patient other)
        {
            EMailAddress = other.EMailAddress;
            FirstName = other.FirstName;
            Insurance = other.Insurance;
            InsuranceNumber = other.InsuranceNumber;
            LastName = other.LastName;
            PhoneNumber = other.PhoneNumber;
            Address = other.Address;
        }
    }
}
