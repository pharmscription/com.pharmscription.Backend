using System;
using System.Collections.Generic;
using System.Linq;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;

namespace com.pharmscription.BusinessLogic.Converter
{
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

        public static List<Prescription> ConvertToEntites(this ICollection<PrescriptionDto> list)
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
                IssueDate = prescription.IssueDate.ToString(PharmscriptionConstants.DateFormat),
                Type = prescription.GetPrescriptionType(),
                EditDate = prescription.EditDate.ToString(PharmscriptionConstants.DateFormat),
                Dispenses = prescription.Dispenses.ConvertToDtos(),
                Drugs = prescription.DrugItems.ConvertToDtos(),
                CounterProposals = prescription.CounterProposals.ConvertToDtos(),
                PrescriptionHistory = prescription.PrescriptionHistory.ConvertToDtos()
            };
            if (prescription.SignDate != null)
            {
                prescriptionDto.SignDate = prescription.SignDate.Value.ToString(PharmscriptionConstants.DateFormat);
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
                prescription = new SinglePrescription
                {
                    IsValid = prescriptionDto.IsValid,
                    CounterProposals = prescriptionDto.CounterProposals.ConvertToEntites(),
                    DrugItems = prescriptionDto.Drugs.ConvertToEntites(),
                    Dispenses = prescriptionDto.Dispenses.ConvertToEntites(),
                    Patient = prescriptionDto.Patient.ConvertToEntity(),
                    PrescriptionHistory = prescriptionDto.PrescriptionHistory.ConvertToEntites(),
                };
                if (prescriptionDto.SignDate != null)
                {
                    prescription.SignDate = DateTime.Parse(prescriptionDto.SignDate);
                }
                if (prescriptionDto.EditDate != null)
                {
                    prescription.EditDate = DateTime.Parse(prescriptionDto.EditDate);
                }
                if (prescriptionDto.IssueDate != null)
                {
                    prescription.IssueDate = DateTime.Parse(prescriptionDto.IssueDate);
                }
            }
            else if(prescriptionDto.Type == "S")
            {
                prescription = new StandingPrescription
                {
                    IsValid = prescriptionDto.IsValid,
                    CounterProposals = prescriptionDto.CounterProposals.ConvertToEntites(),
                    DrugItems = prescriptionDto.Drugs.ConvertToEntites(),
                    Dispenses = prescriptionDto.Dispenses.ConvertToEntites(),
                    Patient = prescriptionDto.Patient.ConvertToEntity(),
                    PrescriptionHistory = prescriptionDto.PrescriptionHistory.ConvertToEntites(),
                    ValidUntill = DateTime.Parse(prescriptionDto.ValidUntil)
                };
                if (prescriptionDto.SignDate != null)
                {
                    prescription.SignDate = DateTime.Parse(prescriptionDto.SignDate);
                }
                if (prescriptionDto.EditDate != null)
                {
                    prescription.EditDate = DateTime.Parse(prescriptionDto.EditDate);
                }
                if (prescriptionDto.IssueDate != null)
                {
                    prescription.IssueDate = DateTime.Parse(prescriptionDto.IssueDate);
                }

            }

            else
            {
                throw new InvalidArgumentException("Invalid type: " + prescriptionDto.Type);
            }
            if (!string.IsNullOrWhiteSpace(prescriptionDto.Id))
            {
                prescription.Id = new Guid(prescriptionDto.Id);
            }
            return prescription;
        }

        public static bool DtoEqualsEntity(this PrescriptionDto prescriptionDto, Prescription prescription)
        {
            return prescriptionDto.IsValid == prescription.IsValid &&
                   prescriptionDto.CounterProposals.DtoListEqualsEntityList(prescription.CounterProposals.ToList()) &&
                   prescriptionDto.Dispenses.DtoListEqualsEntityList(prescription.Dispenses.ToList()) &&
                   prescriptionDto.Drugs.DtoListEqualsEntityList(prescription.DrugItems.ToList()) &&
                   prescriptionDto.EditDate == prescription.EditDate.ToString(PharmscriptionConstants.DateFormat) &&
                   prescriptionDto.IssueDate == prescription.IssueDate.ToString(PharmscriptionConstants.DateFormat) &&
                   prescriptionDto.Type == prescription.GetPrescriptionType() &&
                   prescriptionDto.PrescriptionHistory.DtoListEqualsEntityList(prescription.PrescriptionHistory.ToList());
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
