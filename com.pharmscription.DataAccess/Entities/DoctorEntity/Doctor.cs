using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;

namespace com.pharmscription.DataAccess.Entities.DoctorEntity
{
    using System;

    using com.pharmscription.DataAccess.SharedInterfaces;
    public class Doctor: Entity, ICloneable<Doctor>, IEquatable<Doctor>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string ZsrNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }

        public Doctor Clone()
        {
            return new Doctor
            {
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                ZsrNumber = ZsrNumber,
                PhoneNumber = PhoneNumber,
                FaxNumber = FaxNumber
            };
        }

        public bool Equals(Doctor other)
        {
            if (other == null)
            {
                return false;
            }
            return FirstName == other.FirstName && LastName == other.LastName && Address.Equals(other.Address)
                   && ZsrNumber == other.ZsrNumber && PhoneNumber == other.PhoneNumber && FaxNumber == other.FaxNumber;
        }
    }
}
