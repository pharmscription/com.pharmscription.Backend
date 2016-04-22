

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
    using Infrastructure.Exception;

    public static class PrescriptionConversionExtensions
    {
        public static List<PrescriptionDto> ConvertToDtos(this List<Prescription> list)
        {
            var newList = new List<PrescriptionDto>(list.Count);
            newList.AddRange(list.Select(prescription => prescription.ConvertToDto()));
            return newList;
        }

        public static List<Prescription> ConvertToEntites(this List<PrescriptionDto> list)
        {
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
                IssueDate = prescription.IssueDate.ToString(CultureInfo.InvariantCulture),
                Patient = prescription.Patient.ConvertToDto(),
                SignDate = prescription.SignDate.ToString(CultureInfo.InvariantCulture),
                Type = "Bla",
                EditDate = prescription.EditDate.ToString(CultureInfo.InvariantCulture),
                Dispenses = prescription.Dispenses.ConvertToDtos(),
                Drugs = prescription.DrugItems.ConvertToDtos(),
                CounterProposals = prescription.CounterProposals.ConvertToDtos(),
                PrescriptionHistory = prescription.PrescriptionHistory.ConvertToDtos()
            };
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
            var prescriptionId = string.IsNullOrWhiteSpace(prescriptionDto.Id) ? new Guid() : new Guid(prescriptionDto.Id);
            Prescription prescription;
            if (prescriptionDto.Type == "Single")
            {
                prescription = new SinglePrescription
                {
                    Id = prescriptionId,
                    IsValid = prescriptionDto.IsValid,
                    CounterProposals = prescriptionDto.CounterProposals.ConvertToEntites(),
                    DrugItems = prescriptionDto.Drugs.ConvertToEntites(),
                    Dispenses = prescriptionDto.Dispenses.ConvertToEntites(),
                    Patient = prescriptionDto.Patient.ConvertToEntity(),
                    IssueDate = DateTime.Parse(prescriptionDto.IssueDate),
                    PrescriptionHistory = prescriptionDto.PrescriptionHistory.ConvertToEntites(),
                    SignDate = DateTime.Parse(prescriptionDto.SignDate),
                    EditDate = DateTime.Parse(prescriptionDto.EditDate)
                };
            }
            else if(prescriptionDto.Type == "Standing")
            {
                prescription = new StandingPrescription
                {
                    Id = prescriptionId,
                    IsValid = prescriptionDto.IsValid,
                    CounterProposals = prescriptionDto.CounterProposals.ConvertToEntites(),
                    DrugItems = prescriptionDto.Drugs.ConvertToEntites(),
                    Dispenses = prescriptionDto.Dispenses.ConvertToEntites(),
                    Patient = prescriptionDto.Patient.ConvertToEntity(),
                    IssueDate = DateTime.Parse(prescriptionDto.IssueDate),
                    PrescriptionHistory = prescriptionDto.PrescriptionHistory.ConvertToEntites(),
                    SignDate = DateTime.Parse(prescriptionDto.SignDate),
                    EditDate = DateTime.Parse(prescriptionDto.EditDate)
                };
            }
            else
            {
                throw new InvalidArgumentException("Invalid type: " + prescriptionDto.Type);
            }
            return prescription;
        }

        public static bool DtoEqualsEntity(this PrescriptionDto prescriptionDto, Prescription prescription)
        {
            return prescriptionDto.IsValid == prescription.IsValid &&
                   prescriptionDto.CounterProposals.DtoListEqualsEntityList(prescription.CounterProposals) &&
                   prescriptionDto.Dispenses.DtoListEqualsEntityList(prescription.Dispenses) &&
                   prescriptionDto.Drugs.DtoListEqualsEntityList(prescription.DrugItems) &&
                   prescriptionDto.EditDate == prescription.EditDate.ToString(CultureInfo.InvariantCulture) &&
                   prescriptionDto.IssueDate == prescription.IssueDate.ToString(CultureInfo.InvariantCulture) &&
                   prescriptionDto.Type == "Bla" &&
                   prescriptionDto.SignDate == prescription.SignDate.ToString(CultureInfo.InvariantCulture) &&
                   prescriptionDto.PrescriptionHistory.DtoListEqualsEntityList(prescription.PrescriptionHistory);
        }
        public static bool EntityEqualsDto(this Prescription prescription, PrescriptionDto prescriptionDto)
        {
            return DtoEqualsEntity(prescriptionDto, prescription);
        }

        public static bool DtoListEqualsEntityList(this List<PrescriptionDto> prescriptionDtos, List<Prescription> prescriptions)
        {
            return !prescriptionDtos.Where((t, i) => !prescriptionDtos.ElementAt(i).DtoEqualsEntity(prescriptions.ElementAt(i))).Any();
        }

        public static bool EntityListEqualsDtoList(this List<Prescription> prescriptions, List<PrescriptionDto> prescriptionDtos)
        {
            return DtoListEqualsEntityList(prescriptionDtos, prescriptions);
        }
    }
}
