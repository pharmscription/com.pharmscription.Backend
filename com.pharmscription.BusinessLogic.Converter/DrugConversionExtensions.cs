using System;

using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public static class DrugConversionExtensions
    {
        public static DrugDto ConvertToDto(this Drug drug)
        {
            if (drug == null) return null;
            var drugDto = new DrugDto
            {
                DrugDescription = drug.DrugDescription,
                Composition = drug.Composition,
                NarcoticCategory = drug.NarcoticCategory,
                PackageSize = drug.PackageSize,
                Unit = drug.Unit,
                IsValid = drug.IsValid,
                Id = drug.Id.ToString("N")
            };
            return drugDto;
        }

        public static Drug ConvertToEntity(this DrugDto drugDto)
        {
            if (drugDto == null) return null;
            var drug = new Drug
            {
                DrugDescription = drugDto.DrugDescription,
                Composition = drugDto.Composition,
                NarcoticCategory = drugDto.NarcoticCategory,
                PackageSize = drugDto.PackageSize,
                Unit = drugDto.Unit,
                Id = new Guid(drugDto.Id),
                IsValid = drugDto.IsValid
            };
            return drug;
        }
    }
}
