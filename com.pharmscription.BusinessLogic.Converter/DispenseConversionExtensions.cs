using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    using System.Diagnostics.CodeAnalysis;

    using Infrastructure.Constants;

    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:StatementMustNotBeOnSingleLine", Justification = "Rule does not increase readability in this class.")]
    public static class DispenseConversionExtensions
    {
        public static ICollection<DispenseDto> ConvertToDtos(this ICollection<Dispense> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<DispenseDto>(list.Count);
            newList.AddRange(list.Select(dispenseItem => dispenseItem.ConvertToDto()));
            return newList;
        }

        public static ICollection<Dispense> ConvertToEntities(this ICollection<DispenseDto> list)
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
        /// Convert Entity to Dto
        /// </summary>
        /// <param name="dispense">Dispense to convert</param>
        /// <returns>Dispense entity or null when it get null as parameter</returns>
        public static DispenseDto ConvertToDto(this Dispense dispense)
        {
            if (dispense == null) { return null; }
            var dispenseDto = new DispenseDto
            {
                Remark = dispense.Remark,
                Date = dispense.Date?.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
                Id = dispense.Id.ToString(),
                DrugItems = dispense.DrugItems.ConvertToDtos()
            };
            return dispenseDto;
        }

        /// <summary>
        /// Convert Dto to Entity
        /// </summary>
        /// <param name="dispenseDto">Dto to be converted</param>
        /// <returns>Entity object or null when it get null as parameter</returns>
        public static Dispense ConvertToEntity(this DispenseDto dispenseDto)
        {
            if (dispenseDto == null) { return null; }
            var dispense = new Dispense
            {
                Remark = dispenseDto.Remark,
                DrugItems = dispenseDto.DrugItems.ConvertToEntities()
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
                                        dispenseDto.Date == dispense.Date?.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
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

        public static bool DtoListEqualsEntityList(this ICollection<DispenseDto> dispenseDtos, ICollection<Dispense> dispenses)
        {
            return !dispenseDtos.Where((t, i) => !dispenseDtos.ElementAt(i).DtoEqualsEntity(dispenses.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this ICollection<Dispense> dispenses, ICollection<DispenseDto> dispenseDtos)
        {
            return DtoListEqualsEntityList(dispenseDtos, dispenses);
        }
    }
}
