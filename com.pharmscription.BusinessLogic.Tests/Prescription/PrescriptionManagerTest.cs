﻿
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Prescription;
using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.pharmscription.Infrastructure.EntityHelper;
using com.pharmscription.BusinessLogic.Converter;
using com.pharmscription.DataAccess.Repositories.Dispense;

using Moq;

namespace com.pharmscription.BusinessLogic.Tests.Prescription
{
    using System.Globalization;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PrescriptionManagerTest
    {
        private IPrescriptionManager _prescriptionManager;

        private Mock<DispenseRepository> _dispenseRepository;

        [TestInitialize]
        public void SetUp()
        {

            var counterProposalRepository = CounterProposalTestEnvironment.GetMockedCounterProposalRepository();
            _dispenseRepository = DispenseTestEnvironment.GetMockedDispenseRepository();
            var prescriptionRepository = PrescriptionTestEnvironment.GetMockedPrescriptionRepository();
            var drugRepository = DrugTestEnvironment.GetMockedDrugsRepository();
            var drugItemRepository = DrugItemTestEnvironment.GetMockedDrugItemsRepository();
            foreach (var prescription in prescriptionRepository.Object.GetAll())
            {
                if (prescription.Dispenses == null)
                {
                    prescription.Dispenses = new List<Dispense>();
                }
                if (prescription.CounterProposals == null)
                {
                    prescription.CounterProposals = new List<CounterProposal>();
                }
            }
            var patientRepository = PatientTestEnvironment.GetMockedPatientRepository();
            foreach (var patient in patientRepository.Object.GetAll())
            {
                if (patient.Prescriptions == null)
                {
                    patient.Prescriptions = new List<DataAccess.Entities.PrescriptionEntity.Prescription>();
                }
            }
            var patientA = patientRepository.Object.GetAll().FirstOrDefault();
            var prescriptionA = prescriptionRepository.Object.GetAll().FirstOrDefault();
            if (patientA != null)
            {
                patientA.Prescriptions.Add(prescriptionA);
                if (prescriptionA != null)
                {
                    prescriptionA.Patient = patientA;
                    foreach (var counterProposal in counterProposalRepository.Object.GetAll())
                    {
                        prescriptionA.CounterProposals.Add(counterProposal);

                    }
                    foreach (var dispense in _dispenseRepository.Object.GetAll())
                    {
                        prescriptionA.Dispenses.Add(dispense);
                    }
                }
            }
            var patientB = patientRepository.Object.GetAll().Skip(1).FirstOrDefault();
            var prescriptionB = prescriptionRepository.Object.GetAll().Skip(1).FirstOrDefault();
            if (patientB != null)
            {
                patientB.Prescriptions.Add(prescriptionB);
                if (prescriptionB != null) { prescriptionB.Patient = patientB; }
            }

            _prescriptionManager = new PrescriptionManager(prescriptionRepository.Object, patientRepository.Object, counterProposalRepository.Object, _dispenseRepository.Object, drugRepository.Object, drugItemRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestManagerThrowsWhenPatientRepoWasNull()
        {
            var manager = new PrescriptionManager(null, null, null, null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionByPatientIdThrowsOnNull()
        {
            await _prescriptionManager.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionByPatientIdThrowsOnEmpty()
        {
            await _prescriptionManager.Get("");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionByPatientIdThrowsOnGarbageID()
        {
            await _prescriptionManager.Get("jksdjksadfksd");
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdReturnsOnFound()
        {
            var prescription = await _prescriptionManager.Get(PatientTestEnvironment.PatientIdOne);
            Assert.IsNotNull(prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnNull()
        {
            await _prescriptionManager.Get(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnEmpty()
        {
            await _prescriptionManager.Get("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnGarbagePrescriptionId()
        {
            await _prescriptionManager.Get("jksdjksadfksd", "ajsksdk");
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsOnFound()
        {

            var prescription = await _prescriptionManager.Get(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.IsNotNull(prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddPrescriptionThrowsOnNull()
        {
            await _prescriptionManager.Add(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddPrescriptionThrowsOnEmpty()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionManager.Add("", prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddPrescriptionThrowsOnPatientNotFound()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionManager.Add("jksdjksadfksd", prescription);
        }

        [TestMethod]
        public async Task TestAddPrescription()
        {
            var prescriptionToInsert = GetTestPrescriptionDto("N");
            var prescription = await _prescriptionManager.Add(PatientTestEnvironment.PatientIdOne, prescriptionToInsert);
            Assert.IsNotNull(prescription);
            var prescriptions = await _prescriptionManager.Get(PatientTestEnvironment.PatientIdOne);

            var prescriptionInserted = prescriptions.FirstOrDefault(e => e.Id == prescription.Id);
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.AreEqual(DrugTestEnvironment.DrugOneDescription, insertedDrugs.First().Drug.DrugDescription);
        }

        [TestMethod]
        public async Task TestAddStandingPrescription()
        {
            var prescriptionToInsert = GetTestPrescriptionDto("S");
            var prescription = await _prescriptionManager.Add(PatientTestEnvironment.PatientIdOne, prescriptionToInsert);
            Assert.IsNotNull(prescription);
            var prescriptions = await _prescriptionManager.Get(PatientTestEnvironment.PatientIdOne);

            var prescriptionInserted = prescriptions.FirstOrDefault(e => e.Id == prescription.Id);
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.AreEqual(DrugTestEnvironment.DrugOneDescription, insertedDrugs.First().Drug.DrugDescription);
        }

        [TestMethod]
        public async Task TestAddPrescriptionAddsCounterProposals()
        {
            var counterProposals = new List<CounterProposalDto>
            {
                new CounterProposalDto
                {
                    Message = CounterProposalTestEnvironment.CounterProposalOneMessage
                }
            };
            var drugs = new List<DrugItemDto>
            {
                new DrugItemDto
                {
                    Drug = new DrugDto
                    {
                        Id = DrugTestEnvironment.DrugOneId,
                        IsValid = true,
                        DrugDescription = DrugTestEnvironment.DrugOneDescription
                    }
                },
                new DrugItemDto
                {
                    Drug = new DrugDto
                    {
                        Id = DrugTestEnvironment.DrugTwoId,
                        IsValid = true,
                        DrugDescription = DrugTestEnvironment.DrugTwoDescription
                    }
                }
            };
            var prescriptionToInsert = new PrescriptionDto
            {
                EditDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IssueDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IsValid = true,
                Type = "S",
                ValidUntil = DateTime.Now.AddDays(2).ToString(PharmscriptionConstants.DateFormat),
                Drugs = drugs,
                CounterProposals = counterProposals

            };
            var prescription = await _prescriptionManager.Add(PatientTestEnvironment.PatientIdOne, prescriptionToInsert);
            Assert.IsNotNull(prescription);
            var prescriptions = await _prescriptionManager.Get(PatientTestEnvironment.PatientIdOne);

            var prescriptionInserted = prescriptions.FirstOrDefault(e => e.Id == prescription.Id);
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            var insertedCounterProposals = prescriptionInserted.CounterProposals;
            Assert.IsNotNull(insertedDrugs);
            Assert.IsNotNull(insertedCounterProposals);
            Assert.AreEqual(DrugTestEnvironment.DrugOneDescription, insertedDrugs.First().Drug.DrugDescription);
            Assert.AreEqual(CounterProposalTestEnvironment.CounterProposalOneMessage, counterProposals.First().Message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestUpdateTaskPrescriptionThrowsOnNull()
        {
            await _prescriptionManager.Update(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestUpdatePrescriptionThrowsOnEmpty()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionManager.Update("", PrescriptionTestEnvironment.StandingPrescriptionOneId, prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestUpdatePrescriptionThrowsOnEmptyPrescriptionId()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionManager.Update(PatientTestEnvironment.PatientIdOne, "", prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestUpdatePrescriptionThrowsOnPatientNotFound()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionManager.Update("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestUpdatePrescriptionThrowsOnPrescriptionNotFound()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionManager.Update(PatientTestEnvironment.PatientIdOne, "dsjkfkjsdfjksdk", prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestUpdatePrescriptionThrowOnNoCounterProposal()
        {
            var prescriptionToInsert = GetTestPrescriptionDto("N");
            var prescription = await _prescriptionManager.Update(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, prescriptionToInsert);
            Assert.IsNotNull(prescription);
            var prescriptions = await _prescriptionManager.Get(PatientTestEnvironment.PatientIdOne);

            var prescriptionInserted = prescriptions.FirstOrDefault(e => e.Id == prescription.Id);
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.IsNotNull(prescriptionInserted.CounterProposals);
            Assert.AreEqual(DrugTestEnvironment.DrugOneDescription, insertedDrugs.First().Drug.DrugDescription);
        }

        [TestMethod]
        public async Task TestUpdatePrescription()
        {
            var prescriptionToInsert = GetTestPrescriptionDto("N");
            prescriptionToInsert.CounterProposals = new List<CounterProposalDto>
            {
                new CounterProposalDto
                {
                    Message = "Did an Update"
                }
            };
            prescriptionToInsert.PrescriptionHistory = new List<PrescriptionDto>();
            var prescription = await _prescriptionManager.Update(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, prescriptionToInsert);
            Assert.IsNotNull(prescription);
            var prescriptions = await _prescriptionManager.Get(PatientTestEnvironment.PatientIdOne);

            var prescriptionInserted = prescriptions.FirstOrDefault(e => e.Id == prescription.Id);
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.IsNotNull(prescriptionInserted.CounterProposals);
            Assert.AreEqual(DrugTestEnvironment.DrugOneDescription, insertedDrugs.First().Drug.DrugDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddCounterProposalThrowsOnNull()
        {
            await _prescriptionManager.AddCounterProposal(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddCounterProposalThrowsOnEmpty()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionManager.AddCounterProposal("", "", counterProposalDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddCounterProposalThrowsOnPatientIdInvalid()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionManager.AddCounterProposal("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, counterProposalDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddCounterProposalThrowsOnPrescriptionIdInvalid()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionManager.AddCounterProposal(PatientTestEnvironment.PatientIdOne, "sndfsfsfjlks", counterProposalDto);
        }

        [TestMethod]
        public async Task TestAddCounterProposal()
        {
            const string message = "Dieses Rezept ist gemein gefährlich";
            var prescriptionToInsert = new CounterProposalDto
            {
                Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Message = message


            };
            var counterProposal = await _prescriptionManager.AddCounterProposal(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, prescriptionToInsert);
            Assert.IsNotNull(counterProposal);
            var counterProposalInserted = await _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, counterProposal.Id);
            Assert.IsNotNull(counterProposalInserted);
            Assert.AreEqual(message, counterProposalInserted.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalsThrowsOnNull()
        {
            await _prescriptionManager.GetCounterProposals(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalsThrowsOnEmpty()
        {
            await _prescriptionManager.GetCounterProposals("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalsThrowsOnPatientIdInvalid()
        {
            await _prescriptionManager.GetCounterProposals("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalsThrowsOnPrescriptionIdInvalid()
        {
            await _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne, "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetCounterProposals()
        {
            var counterProposals = await _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.IsNotNull(counterProposals);
            Assert.AreEqual(5, counterProposals.Count);
            Assert.AreEqual(CounterProposalTestEnvironment.CounterProposalOneMessage, counterProposals.First().Message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalThrowsOnNull()
        {
            await _prescriptionManager.GetCounterProposals(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalThrowsOnEmpty()
        {
            await _prescriptionManager.GetCounterProposals("", "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalThrowsOnPatientIdInvalid()
        {
            await _prescriptionManager.GetCounterProposals("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, CounterProposalTestEnvironment.CounterProposalOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalThrowsOnPrescriptionIdInvalid()
        {
            await _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne, "sdfklsdf", CounterProposalTestEnvironment.CounterProposalOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalThrowsOnCounterProposalIdInvalid()
        {
            await _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetCounterProposal()
        {
            var prescription =
                await
                    _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne,
                        PrescriptionTestEnvironment.StandingPrescriptionOneId,
                        CounterProposalTestEnvironment.CounterProposalOneId);
            Assert.IsNotNull(prescription);
            Assert.AreEqual(CounterProposalTestEnvironment.CounterProposalOneMessage, prescription.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditCounterProposalThrowsOnNull()
        {
            await _prescriptionManager.EditCounterProposal(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditCounterProposalThrowsOnEmpty()
        {
            await _prescriptionManager.EditCounterProposal("", "", new CounterProposalDto());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestEditCounterProposalThrowsOnPatientNotFound()
        {
            await _prescriptionManager.EditCounterProposal(IdentityGenerator.NewSequentialGuid().ToString(), PrescriptionTestEnvironment.StandingPrescriptionOneId, new CounterProposalDto
            {
                Id = CounterProposalTestEnvironment.CounterProposalOneId
            });
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestEditCounterProposalThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.EditCounterProposal(PatientTestEnvironment.PatientIdOne, IdentityGenerator.NewSequentialGuid().ToString(), new CounterProposalDto
            {
                Id = CounterProposalTestEnvironment.CounterProposalOneId
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditCounterProposalThrowsOnCounterProposalNotFound()
        {
            await _prescriptionManager.EditCounterProposal(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, new CounterProposalDto());
        }

        [TestMethod]
        public async Task TestEditCounterProposal()
        {
            const string id = CounterProposalTestEnvironment.CounterProposalOneId;
            var counterPropsal = new CounterProposalDto
            {
                Id = id,
                Message = "Rezept geändert",
                Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat)
            };
            var counterProposalBeforeEdit =
                await _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, id);
            await _prescriptionManager.EditCounterProposal(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, counterPropsal);
            var counterProposalAfterEdit =
                await _prescriptionManager.GetCounterProposals(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, id);
            Assert.IsNotNull(counterProposalAfterEdit);
            Assert.AreNotEqual(counterProposalBeforeEdit.Message, counterProposalAfterEdit.Message);
            Assert.AreEqual(counterPropsal.Message, counterProposalAfterEdit.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddDispenseThrowsOnNull()
        {
            await _prescriptionManager.AddDispense(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddDispenseThrowsOnEmpty()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionManager.AddDispense("", "", dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddDispenseThrowsOnPatientIdInvalid()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionManager.AddDispense("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestAddDispenseThrowsOnPrescriptionIdInvalid()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionManager.AddDispense(PatientTestEnvironment.PatientIdOne, "sndfsfsfjlks", dispenseDto);
        }

        [TestMethod]
        public async Task TestAddDispense()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto
            {
                Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Remark = remark
            };
            var dispense = await _prescriptionManager.AddDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseToInsert);
            Assert.IsNotNull(dispense);
            var dispenseInserted = await _prescriptionManager.GetDispenses(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, dispense.Id);
            Assert.IsNotNull(dispenseInserted);
            Assert.AreEqual(remark, dispenseInserted.Remark);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnNull()
        {
            await _prescriptionManager.GetDispenses(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnEmpty()
        {
            await _prescriptionManager.GetDispenses("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnPatientIdInvalid()
        {
            await _prescriptionManager.GetDispenses("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnPrescriptionIdInvalid()
        {
            await _prescriptionManager.GetDispenses(PatientTestEnvironment.PatientIdOne, "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetDispenses()
        {
            var dispenses = await _prescriptionManager.GetDispenses(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.IsNotNull(dispenses);
            Assert.AreEqual(2, dispenses.Count);
            Assert.AreEqual(DispenseTestEnvironment.DispenseOneRemark, dispenses.First().Remark);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnNull()
        {
            await _prescriptionManager.GetDispenses(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnEmpty()
        {
            await _prescriptionManager.GetDispenses("", "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnPatientIdInvalid()
        {
            await _prescriptionManager.GetDispenses("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, DispenseTestEnvironment.DispenseOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnPrescriptionIdInvalid()
        {
            await _prescriptionManager.GetDispenses(PatientTestEnvironment.PatientIdOne, "sdfklsdf", DispenseTestEnvironment.DispenseOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnDispenseIdInvalid()
        {
            await _prescriptionManager.GetDispenses(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetDispense()
        {
            var dispense = await _prescriptionManager.GetDispenses(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, DispenseTestEnvironment.DispenseOneId);
            Assert.IsNotNull(dispense);
            Assert.AreEqual(DispenseTestEnvironment.DispenseOneRemark, dispense.Remark);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionDrugThrowsOnNull()
        {
            await _prescriptionManager.GetPrescriptionDrugs(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionDrugThrowsOnEmpty()
        {
            await _prescriptionManager.GetPrescriptionDrugs("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetGetPrescriptionDrugThrowsOnPatientIdInvalid()
        {
            await _prescriptionManager.GetPrescriptionDrugs("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionDrugThrowsOnPrescriptionIdInvalid()
        {
            await _prescriptionManager.GetPrescriptionDrugs(PatientTestEnvironment.PatientIdOne, "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrug()
        {
            var drugs = await _prescriptionManager.GetPrescriptionDrugs(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.IsNotNull(drugs);
            Assert.AreEqual(PrescriptionTestEnvironment.DrugDescriptionOne, drugs.First().Drug.DrugDescription);
        }

        [TestMethod]
        public async Task TestEditDispense()
        {
            var prescription = PrescriptionTestEnvironment.GetTestPrescriptions().FirstOrDefault();
            var patientId = PatientTestEnvironment.PatientIdOne;
            var dispense = DispenseTestEnvironment.GetTestDispenses().FirstOrDefault();
            var prescriptiondto = await _prescriptionManager.Add(patientId, prescription.ConvertToDto());
            var dispenseDto = await _prescriptionManager.AddDispense(patientId, prescriptiondto.Id, dispense.ConvertToDto());
            var dateString = DateTime.Now.ToString(PharmscriptionConstants.DateFormat);
            dispenseDto.Date = dateString;
            var newDispenseDto =
                await _prescriptionManager.ModifyDispense(patientId, prescriptiondto.Id, dispenseDto.Id, dispenseDto);
            Assert.IsNotNull(newDispenseDto);
            Assert.AreNotEqual(dispenseDto, newDispenseDto);
            _dispenseRepository.Verify(repository => repository.Modify(It.IsAny<Dispense>()), Times.Once);
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditDispenseWithNullObject()
        {
            var prescription = PrescriptionTestEnvironment.GetTestPrescriptions().FirstOrDefault();
            var patientId = PatientTestEnvironment.PatientIdOne;
            var dispense = DispenseTestEnvironment.GetTestDispenses().FirstOrDefault();
            var prescriptiondto = await _prescriptionManager.Add(patientId, prescription.ConvertToDto());
            var dispenseDto = await _prescriptionManager.AddDispense(patientId, prescriptiondto.Id, dispense.ConvertToDto());
            await _prescriptionManager.ModifyDispense(patientId, prescriptiondto.Id, dispenseDto.Id, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditDispenseWithWrongId()
        {
            var prescription = PrescriptionTestEnvironment.GetTestPrescriptions().FirstOrDefault();
            var patientId = PatientTestEnvironment.PatientIdOne;
            var dispense = DispenseTestEnvironment.GetTestDispenses().FirstOrDefault();
            var dispenseId = DispenseTestEnvironment.DispenseTwoId;
            var prescriptiondto = await _prescriptionManager.Add(patientId, prescription.ConvertToDto());
            var dispenseDto = await _prescriptionManager.AddDispense(patientId, prescriptiondto.Id, dispense.ConvertToDto());
            await _prescriptionManager.ModifyDispense(patientId, prescriptiondto.Id, dispenseId, dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditDispenseAlreadySigned()
        {
            var prescription = PrescriptionTestEnvironment.GetTestPrescriptions().FirstOrDefault();
            var patientId = PatientTestEnvironment.PatientIdOne;
            var dispense = DispenseTestEnvironment.GetTestDispenses().FirstOrDefault();
            dispense.Date = DateTime.Now.AddDays(-1);
            var dispenseId = DispenseTestEnvironment.DispenseTwoId;
            var prescriptiondto = await _prescriptionManager.Add(patientId, prescription.ConvertToDto());
            var dispenseDto = await _prescriptionManager.AddDispense(patientId, prescriptiondto.Id, dispense.ConvertToDto());
            await _prescriptionManager.ModifyDispense(patientId, prescriptiondto.Id, dispenseId, dispenseDto);
        }

        private static List<DrugItemDto> GetTestDrugItems()
        {
            var drugs = new List<DrugItemDto>
            {
                new DrugItemDto
                {
                    Drug = new DrugDto
                    {
                        Id = DrugTestEnvironment.DrugOneId,
                        IsValid = true,
                        DrugDescription = DrugTestEnvironment.DrugOneDescription
                    }
                },
                new DrugItemDto
                {
                    Drug = new DrugDto
                    {
                        Id = DrugTestEnvironment.DrugTwoId,
                        IsValid = true,
                        DrugDescription = DrugTestEnvironment.DrugTwoDescription
                    }
                }
            };
            return drugs;
        }

        private static PrescriptionDto GetTestPrescriptionDto(string prescriptionType)
        {
            var prescriptionToInsert = new PrescriptionDto
            {
                EditDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IssueDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IsValid = true,
                Type = prescriptionType,
                ValidUntil = DateTime.Now.AddDays(2).ToString(PharmscriptionConstants.DateFormat),
                Drugs = GetTestDrugItems()
            };
            return prescriptionToInsert;
        }
    }
}

