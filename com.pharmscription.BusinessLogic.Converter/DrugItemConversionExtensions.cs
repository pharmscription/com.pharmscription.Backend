using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:StatementMustNotBeOnSingleLine", Justification = "New line does not increase readability in this class")]
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

        public static ICollection<DrugItem> ConvertToEntities(this IReadOnlyCollection<DrugItemDto> list)
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
        /// Convert entity to dto
        /// </summary>
        /// <param name="drug"><see cref="DrugItem"/> to convert</param>
        /// <returns><see cref="DrugItemDto"/>null when it get null as parameter</returns>
        public static DrugItemDto ConvertToDto(this DrugItem drug)
        {
            if (drug == null) { return null; }
            var drugItemDto = new DrugItemDto
            {
                Drug = drug.Drug.ConvertToDto(),
                DosageDescription = drug.DosageDescription,
                Quantity = drug.Quantity,
                Id = drug.Id.ToString()
                
            }; 
            return drugItemDto;
        }

        /// <summary>
        /// Convert dto to entity
        /// </summary>
        /// <param name="drugItemDto"><see cref="DrugItemDto"/> to convert</param>
        /// <returns><see cref="DrugItem"/> or null when it get null as parameter</returns>
        public static DrugItem ConvertToEntity(this DrugItemDto drugItemDto)
        {
            if (drugItemDto == null) { return null; }
            var drugItem = new DrugItem
            {
                Drug = drugItemDto.Drug.ConvertToEntity(),
                DosageDescription = drugItemDto.DosageDescription,
                Quantity = drugItemDto.Quantity
            };
            if (!string.IsNullOrWhiteSpace(drugItemDto.Id))
            {
                drugItem.Id = new Guid(drugItemDto.Id);
            }
            return drugItem;
        }

        public static bool DtoEqualsEntity(this DrugItemDto drugItemDto, DrugItem drugItem)
        {
            if (drugItemDto == null || drugItem == null)
            {
                return false;
            }
            return drugItemDto.DosageDescription == drugItem.DosageDescription &&
                   drugItemDto.Drug.DtoEqualsEntity(drugItem.Drug)
                   && drugItemDto.Id == drugItem.Id.ToString()
                   && drugItemDto.Quantity == drugItem.Quantity;
        }
        public static bool EntityEqualsDto(this DrugItem drugItem, DrugItemDto drugItemDto)
        {
            return DtoEqualsEntity(drugItemDto, drugItem);
        }

        public static bool DtoListEqualsEntityList(this IReadOnlyCollection<DrugItemDto> drugItemDtos, IReadOnlyCollection<DrugItem> drugItems)
        {
            return !drugItemDtos.Where((t, i) => !drugItemDtos.ElementAt(i).DtoEqualsEntity(drugItems.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this IReadOnlyCollection<DrugItem> drugItems, IReadOnlyCollection<DrugItemDto> drugItemDtos)
        {
            return DtoListEqualsEntityList(drugItemDtos, drugItems);
        }
    }
}
