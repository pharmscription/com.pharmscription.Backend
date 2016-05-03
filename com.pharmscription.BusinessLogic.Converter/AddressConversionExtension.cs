
namespace com.pharmscription.BusinessLogic.Converter
{
    using System;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using Infrastructure.Dto;

    public static class AddressConversionExtension
    {
        public static Address ConvertToEntity(this AddressDto addressDto)
        {
            if (addressDto == null)
            {
                return null;
            }
            var address = new Address
            {
                CityCode = SwissCityCode.CreateInstance(addressDto.CityCode),
                Location = addressDto.Location,
                Number = addressDto.Number,
                State = addressDto.State,
                Street = addressDto.Street,
                StreetExtension = addressDto.StreetExtension
            };
            if (!string.IsNullOrWhiteSpace(addressDto.Id))
            {
                address.Id = Guid.Parse(addressDto.Id);
            }
            return address;
        }

        public static AddressDto ConvertToDto(this Address address)
        {
            if (address == null)
            {
                return null;
            }
            var addressDto = new AddressDto
            {
                CityCode = address.CityCode.CityCode,
                Location = address.Location,
                Number = address.Number,
                State = address.State,
                Street = address.Street,
                StreetExtension = address.StreetExtension,
                Id = address.Id.ToString()
            };
            return addressDto;
        }

        public static bool IsEqual(this Address addressLeft, Address addressRight)
        {
            if (addressLeft == null || addressRight == null)
            {
                return false;
            }
            return addressLeft.CityCode.CityCode == addressRight.CityCode.CityCode && addressLeft.Location == addressRight.Location &&
                   addressLeft.Number == addressRight.Number && addressLeft.State == addressRight.State &&
                   addressLeft.Street == addressRight.Street &&
                   addressLeft.StreetExtension == addressRight.StreetExtension;
        }
    }
}
