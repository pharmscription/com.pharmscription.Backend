using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    using Infrastructure.Constants;

    public static class DispenseConversionExtensions
    {
        public static List<DispenseDto> ConvertToDtos(this ICollection<Dispense> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<DispenseDto>(list.Count);
            newList.AddRange(list.Select(dispenseItem => dispenseItem.ConvertToDto()));
            return newList;
        }

        public static ICollection<Dispense> ConvertToEntities(this IReadOnlyCollection<DispenseDto> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<Dispense>(list.Count);
            newList.AddRange(list.Select(dispense => dispense.ConvertToEntity()));
            return newList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispense"></param>
        /// <returns>null when it get null as parameter</returns>
        public static DispenseDto ConvertToDto(this Dispense dispense)
        {
            if (dispense == null) return null;
            var dispenseDto = new DispenseDto
            {
                Remark = dispense.Remark,
                Date = dispense.Date.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
                Id = dispense.Id.ToString(),
                DrugItems = dispense.DrugItems.ConvertToDtos()
            };
            return dispenseDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispenseDto"></param>
        /// <returns>null when it get null as parameter</returns>
        public static Dispense ConvertToEntity(this DispenseDto dispenseDto)
        {
            if (dispenseDto == null) return null;
            var dispense = new Dispense
            {
                Remark = dispenseDto.Remark,
                DrugItems = dispenseDto.DrugItems.ConvertToEntites()
            };
            if (dispenseDto.Date != null)
            {
                dispense.Date = DateTime.Parse(dispenseDto.Date, CultureInfo.CurrentCulture);
            }
            if (!string.IsNullOrWhiteSpace(dispenseDto.Id))
            {
                dispense.Id = new Guid(dispenseDto.Id);
            }
            return dispense;
        }

        public static bool DtoEqualsEntity(this DispenseDto dispenseDto, Dispense dispense)
        {
            if (dispenseDto == null || dispense == null)
            {
                return false;
            }
            var ownPropertiesAreEqual = dispenseDto.Remark == dispense.Remark &&
                                        dispense.Id.ToString() == dispenseDto.Id &&
                                        dispenseDto.Date == dispense.Date.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
            if (dispense.DrugItems != null)
            {
                return ownPropertiesAreEqual && dispenseDto.DrugItems.DtoListEqualsEntityList(dispense.DrugItems.ToList());
            }
            return ownPropertiesAreEqual;
        }
        public static bool EntityEqualsDto(this Dispense dispense, DispenseDto dispenseDto)
        {
            return DtoEqualsEntity(dispenseDto, dispense);
        }

        public static bool DtoListEqualsEntityList(this IReadOnlyCollection<DispenseDto> dispenseDtos, IReadOnlyCollection<Dispense> dispenses)
        {
            return !dispenseDtos.Where((t, i) => !dispenseDtos.ElementAt(i).DtoEqualsEntity(dispenses.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this IReadOnlyCollection<Dispense> dispenses, IReadOnlyCollection<DispenseDto> dispenseDtos)
        {
            return DtoListEqualsEntityList(dispenseDtos, dispenses);
        }
    }
}
