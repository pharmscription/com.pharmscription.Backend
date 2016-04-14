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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drug"></param>
        /// <returns>null when it get null as parameter</returns>
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
                Id = drug.Id.ToString()
            };
            return drugDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drugDto"></param>
        /// <returns>null when it get null as parameter</returns>
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

        public static bool DtoEqualsEntity(this DrugDto drugDto, Drug drug)
        {
            return drugDto.IsValid == drug.IsValid && drug.Composition == drugDto.Composition &&
                   drugDto.DrugDescription == drug.DrugDescription && drugDto.NarcoticCategory == drug.NarcoticCategory &&
                   drugDto.PackageSize == drug.PackageSize && drugDto.Unit == drug.Unit && drugDto.Id == drug.Id.ToString();
        }
        public static bool EntityEqualsDto(this Drug drug, DrugDto drugDto)
        {
            return drug.IsValid == drugDto.IsValid && drugDto.Composition == drug.Composition &&
                   drug.DrugDescription == drugDto.DrugDescription && drug.NarcoticCategory == drugDto.NarcoticCategory &&
                   drug.PackageSize == drugDto.PackageSize && drug.Unit == drugDto.Unit && drug.Id.ToString() == drugDto.Id;
        }
    }
}
