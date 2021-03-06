﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{

    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:StatementMustNotBeOnSingleLine", Justification = "New line does not increase readability in this class")]
    public static class DrugConversionExtensions
    {
        public static ICollection<DrugDto> ConvertToDtos(this ICollection<Drug> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<DrugDto>(list.Count);
            newList.AddRange(list.Select(drug => drug.ConvertToDto()));
            return newList;
        }

        public static ICollection<Drug> ConvertToEntities(this ICollection<DrugDto> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<Drug>(list.Count);
            newList.AddRange(list.Select(drug => drug.ConvertToEntity()));
            return newList;
        }

        /// <summary>
        /// Convert entity do dto
        /// </summary>
        /// <param name="drug"><see cref="Drug"/> to convert</param>
        /// <returns><see cref="DrugDto"/> or null when it get null as parameter</returns>
        public static DrugDto ConvertToDto(this Drug drug)
        {
            if (drug == null) { return null; }
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
        /// Convert dto to entity
        /// </summary>
        /// <param name="drugDto"><see cref="DrugDto"/> to convert</param>
        /// <returns><see cref="Drug"/> or null when it get null as parameter</returns>
        public static Drug ConvertToEntity(this DrugDto drugDto)
        {
            if (drugDto == null) { return null; }
            var drug = new Drug
            {
                DrugDescription = drugDto.DrugDescription,
                Composition = drugDto.Composition,
                NarcoticCategory = drugDto.NarcoticCategory,
                PackageSize = drugDto.PackageSize,
                Unit = drugDto.Unit,
                IsValid = drugDto.IsValid
            };
            if (!string.IsNullOrWhiteSpace(drugDto.Id))
            {
                drug.Id = new Guid(drugDto.Id);
            }
            return drug;
        }

        public static bool DtoEqualsEntity(this DrugDto drugDto, Drug drug)
        {
            if (drugDto == null || drug == null)
            {
                return false;
            }
            return drugDto.IsValid == drug.IsValid && drug.Composition == drugDto.Composition &&
                   drugDto.DrugDescription == drug.DrugDescription && drugDto.NarcoticCategory == drug.NarcoticCategory &&
                   drugDto.PackageSize == drug.PackageSize && drugDto.Unit == drug.Unit && drugDto.Id == drug.Id.ToString();
        }
        public static bool EntityEqualsDto(this Drug drug, DrugDto drugDto)
        {
            return DtoEqualsEntity(drugDto, drug);
        }
    }
}
