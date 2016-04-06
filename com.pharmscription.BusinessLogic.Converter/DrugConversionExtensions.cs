using System;
using System.Collections.Generic;
using System.Linq;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public static class DrugConversionExtensions
    {
        public static List<DrugDto> ConvertToDtos(this List<Drug> list)
        {
            var newList = new List<DrugDto>(list.Count);
            newList.AddRange(list.Select(drug => drug.ConvertToDto()));
            return newList;
        }

        public static List<Drug> ConvertToEntites(this List<DrugDto> list)
        {
            var newList = new List<Drug>(list.Count);
            newList.AddRange(list.Select(drug => drug.ConvertToEntity()));
            return newList;
        }

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
            var drugGuid = string.IsNullOrWhiteSpace(drugDto.Id) ? new Guid() : new Guid(drugDto.Id);
            var drug = new Drug
            {
                DrugDescription = drugDto.DrugDescription,
                Composition = drugDto.Composition,
                NarcoticCategory = drugDto.NarcoticCategory,
                PackageSize = drugDto.PackageSize,
                Unit = drugDto.Unit,
                Id = drugGuid,
                IsValid = drugDto.IsValid
            };
            return drug;
        }
    }
}
