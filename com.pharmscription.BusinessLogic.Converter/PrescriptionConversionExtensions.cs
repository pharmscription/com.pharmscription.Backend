﻿using System;
using System.Collections.Generic;
using System.Linq;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    using System.Globalization;
    using Infrastructure.Exception;

    public static class PrescriptionConversionExtensions
    {
        public static List<PrescriptionDto> ConvertToDtos(this ICollection<Prescription> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<PrescriptionDto>(list.Count);
            newList.AddRange(list.Select(prescription => prescription.ConvertToDto()));
            return newList;
        }

        public static ICollection<Prescription> ConvertToEntities(this IReadOnlyCollection<PrescriptionDto> list)
        {
            if (list == null)
            {
                return null;
            }
            var newList = new List<Prescription>(list.Count);
            newList.AddRange(list.Select(prescriptionDto => prescriptionDto.ConvertToEntity()));
            return newList;
        }

        /// <summary>
        /// bla
        /// </summary>
        /// <param name="prescription"></param>
        /// <returns>null when it get null as parameter</returns>
        public static PrescriptionDto ConvertToDto(this Prescription prescription)
        {
            if (prescription == null) return null;
            var prescriptionDto = new PrescriptionDto
            {
                IsValid = prescription.IsValid,
                Id = prescription.Id.ToString(),
                IssueDate = prescription.IssueDate.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
                Type = prescription.GetPrescriptionType(),
                EditDate = prescription.EditDate.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture),
                Dispenses = prescription.Dispenses.ConvertToDtos(),
                Drugs = prescription.DrugItems.ConvertToDtos(),
                CounterProposals = prescription.CounterProposals.ConvertToDtos(),
                PrescriptionHistory = prescription.PrescriptionHistory.ConvertToDtos()
            };
            if (prescription.SignDate != null)
            {
                prescriptionDto.SignDate = prescription.SignDate.Value.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
            }
            var standingPrescription = prescription as StandingPrescription;
            if (standingPrescription != null)
            {
                prescriptionDto.ValidUntil =
                    standingPrescription.ValidUntill.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture);
            }
            return prescriptionDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prescriptionDto"></param>
        /// <returns>null when it get null as parameter</returns>
        public static Prescription ConvertToEntity(this PrescriptionDto prescriptionDto)
        {
            if (prescriptionDto == null) return null;
            Prescription prescription;
            if (prescriptionDto.Type == "N")
            {
                prescription = new SinglePrescription();
            }
            else if(prescriptionDto.Type == "S")
            {
                prescription = new StandingPrescription
                {
                    ValidUntill = DateTime.Parse(prescriptionDto.ValidUntil, CultureInfo.CurrentCulture)
                };
            }
            else
            {
                throw new InvalidArgumentException("Invalid type: " + prescriptionDto.Type);
            }
            prescription.IsValid = prescriptionDto.IsValid;
            prescription.CounterProposals = prescriptionDto.CounterProposals.ConvertToEntities();
            prescription.DrugItems = prescriptionDto.Drugs.ConvertToEntities();
            prescription.Dispenses = prescriptionDto.Dispenses.ConvertToEntities();
            prescription.Patient = prescriptionDto.Patient.ConvertToEntity();
            prescription.PrescriptionHistory = prescriptionDto.PrescriptionHistory.ConvertToEntities();
            if (prescriptionDto.IssueDate != null)
            {
                prescription.IssueDate = DateTime.Parse(prescriptionDto.IssueDate, CultureInfo.CurrentCulture);
            }
            if (prescriptionDto.EditDate != null)
            {
                prescription.EditDate = DateTime.Parse(prescriptionDto.EditDate, CultureInfo.CurrentCulture);
            }
            if (prescriptionDto.SignDate != null)
            {
                prescription.SignDate = DateTime.Parse(prescriptionDto.SignDate, CultureInfo.CurrentCulture);
            }
            if (!string.IsNullOrWhiteSpace(prescriptionDto.Id))
            {
                prescription.Id = new Guid(prescriptionDto.Id);
            }
            return prescription;
        }

        public static bool DtoEqualsEntity(this PrescriptionDto prescriptionDto, Prescription prescription)
        {
            if (prescriptionDto == null || prescription == null)
            {
                return false;
            }
            var ownPropertiesAreEqual = prescriptionDto.IsValid == prescription.IsValid
                                        &&
                                        prescriptionDto.EditDate ==
                                        prescription.EditDate.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture) &&
                                        prescriptionDto.IssueDate ==
                                        prescription.IssueDate.ToString(PharmscriptionConstants.DateFormat, CultureInfo.CurrentCulture) &&
                                        prescriptionDto.Type == prescription.GetPrescriptionType();
            var counterProposalsAreEqual = true;
            if (prescription.CounterProposals != null)
            {
                counterProposalsAreEqual =
                    prescriptionDto.CounterProposals.DtoListEqualsEntityList(prescription.CounterProposals.ToList());
            }
            var dispensesAreEqual = true;
            if (prescription.Dispenses != null)
            {
                dispensesAreEqual = prescriptionDto.Dispenses.DtoListEqualsEntityList(prescription.Dispenses.ToList());
            }
            var drugsAreEqual = true;
            if (prescription.DrugItems != null)
            {
                drugsAreEqual = prescriptionDto.Drugs.DtoListEqualsEntityList(prescription.DrugItems.ToList());
            }
            var historiesAreEqual = true;
            if (prescription.PrescriptionHistory != null)
            {
                historiesAreEqual = prescriptionDto.PrescriptionHistory.DtoListEqualsEntityList(prescription.PrescriptionHistory.ToList());
            }
            return ownPropertiesAreEqual && counterProposalsAreEqual && dispensesAreEqual && drugsAreEqual && historiesAreEqual;
        }
        public static bool EntityEqualsDto(this Prescription prescription, PrescriptionDto prescriptionDto)
        {
            return DtoEqualsEntity(prescriptionDto, prescription);
        }

        public static bool DtoListEqualsEntityList(this IReadOnlyCollection<PrescriptionDto> prescriptionDtos, IReadOnlyCollection<Prescription> prescriptions)
        {
            return !prescriptionDtos.Where((t, i) => !prescriptionDtos.ElementAt(i).DtoEqualsEntity(prescriptions.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this IReadOnlyCollection<Prescription> prescriptions, IReadOnlyCollection<PrescriptionDto> prescriptionDtos)
        {
            return DtoListEqualsEntityList(prescriptionDtos, prescriptions);
        }
    }
}
