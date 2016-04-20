using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    public static class DispenseConversionExtensions
    {
        public static List<DispenseDto> ConvertToDtos(this List<Dispense> list)
        {
            var newList = new List<DispenseDto>(list.Count);
            newList.AddRange(list.Select(dispenseItem => dispenseItem.ConvertToDto()));
            return newList;
        }

        public static List<Dispense> ConvertToEntites(this List<DispenseDto> list)
        {
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
                Date = dispense.Date.ToString(CultureInfo.InvariantCulture),
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
            var dispenseGuid = string.IsNullOrWhiteSpace(dispenseDto.Id) ? new Guid() : new Guid(dispenseDto.Id);
            var dispense = new Dispense
            {
                Remark = dispenseDto.Remark,
                Id = dispenseGuid,
                DrugItems = dispenseDto.DrugItems.ConvertToEntites(),
                Date = DateTime.Parse(dispenseDto.Date)
            };
            return dispense;
        }

        public static bool DtoEqualsEntity(this DispenseDto dispenseDto, Dispense dispense)
        {
            return dispenseDto.Remark == dispense.Remark && dispense.Id.ToString() == dispenseDto.Id &&
                   dispenseDto.Date == dispense.Date.ToString(CultureInfo.InvariantCulture) &&
                   dispenseDto.DrugItems.DtoListEqualsEntityList(dispense.DrugItems);
        }
        public static bool EntityEqualsDto(this Dispense dispense, DispenseDto dispenseDto)
        {
            return DtoEqualsEntity(dispenseDto, dispense);
        }

        public static bool DtoListEqualsEntityList(this List<DispenseDto> dispenseDtos, List<Dispense> dispenses)
        {
            return !dispenseDtos.Where((t, i) => !dispenseDtos.ElementAt(i).DtoEqualsEntity(dispenses.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this List<Dispense> dispenses, List<DispenseDto> dispenseDtos)
        {
            return DtoListEqualsEntityList(dispenseDtos, dispenses);
        }
    }
}
