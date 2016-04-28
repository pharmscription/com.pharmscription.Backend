using System;
using System.Collections.Generic;
using System.Linq;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public static class DrugItemConversionExtensions
    {
        public static List<DrugItemDto> ConvertToDtos(this ICollection<DrugItem> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<DrugItemDto>(list.Count);
            newList.AddRange(list.Select(drugItem => drugItem.ConvertToDto()));
            return newList;
        }

        public static List<DrugItem> ConvertToEntites(this ICollection<DrugItemDto> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<DrugItem>(list.Count);
            newList.AddRange(list.Select(drug => drug.ConvertToEntity()));
            return newList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drug"></param>
        /// <returns>null when it get null as parameter</returns>
        public static DrugItemDto ConvertToDto(this DrugItem drug)
        {
            if (drug == null) return null;
            var drugItemDto = new DrugItemDto
            {
                Drug = drug.Drug.ConvertToDto(),
                DosageDescription = drug.DosageDescription,
                Dispense = drug.Dispense.ConvertToDto()
            };
            return drugItemDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drugItemDto"></param>
        /// <returns>null when it get null as parameter</returns>
        public static DrugItem ConvertToEntity(this DrugItemDto drugItemDto)
        {
            if (drugItemDto == null) return null;
            var drugItem = new DrugItem
            {
                Dispense = drugItemDto.Dispense.ConvertToEntity(),
                Drug = drugItemDto.Drug.ConvertToEntity(),
                DosageDescription = drugItemDto.DosageDescription
            };
            if (!string.IsNullOrWhiteSpace(drugItemDto.Id))
            {
                drugItem.Id = new Guid(drugItemDto.Id);
            }
            return drugItem;
        }

        public static bool DtoEqualsEntity(this DrugItemDto drugItemDto, DrugItem drugItem)
        {
            return drugItemDto.DosageDescription == drugItem.DosageDescription &&
                    drugItem.Dispense.EntityEqualsDto(drugItemDto.Dispense) &&
                    drugItemDto.Prescription.DtoEqualsEntity(drugItem.Prescription) &&
                   drugItemDto.Drug.DtoEqualsEntity(drugItem.Drug)
                   && drugItemDto.Id == drugItem.Id.ToString();
        }
        public static bool EntityEqualsDto(this DrugItem drugItem, DrugItemDto drugItemDto)
        {
            return DtoEqualsEntity(drugItemDto, drugItem);
        }

        public static bool DtoListEqualsEntityList(this List<DrugItemDto> drugItemDtos, List<DrugItem> drugItems)
        {
            return !drugItemDtos.Where((t, i) => !drugItemDtos.ElementAt(i).DtoEqualsEntity(drugItems.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this List<DrugItem> drugItems, List<DrugItemDto> drugItemDtos)
        {
            return DtoListEqualsEntityList(drugItemDtos, drugItems);
        }
    }
}
