
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

namespace com.pharmscription.BusinessLogic.Tests.Prescription
{
    using Infrastructure.EntityHelper;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PrescriptionManagerTest
    {
        private IPrescriptionManager _prescriptionManager;

        [TestInitialize]
        public void SetUp()
        {

            var counterProposalRepository = CounterProposalTestEnvironment.GetMockedCounterProposalRepository();
            var dispenseRepository = DispenseTestEnvironment.GetMockedDispenseRepository();
            var prescriptionRepository = PrescriptionTestEnvironment.GetMockedPrescriptionRepository();
            var drugRepository = DrugTestEnvironment.GetMockedDrugsRepository();
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
            patientA.Prescriptions.Add(prescriptionA);
            prescriptionA.Patient = patientA;
            foreach (var counterProposal in counterProposalRepository.Object.GetAll())
            {
                prescriptionA.CounterProposals.Add(counterProposal);

            }
            foreach (var dispense in dispenseRepository.Object.GetAll())
            {
                prescriptionA.Dispenses.Add(dispense);
            }
            var patientB = patientRepository.Object.GetAll().Skip(1).FirstOrDefault();
            var prescriptionB = prescriptionRepository.Object.GetAll().Skip(1).FirstOrDefault();
            patientB.Prescriptions.Add(prescriptionB);
            prescriptionB.Patient = patientB;

            _prescriptionManager = new PrescriptionManager(prescriptionRepository.Object, patientRepository.Object, counterProposalRepository.Object, dispenseRepository.Object, drugRepository.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestManagerThrowsWhenPatientRepoWasNull()
        {
            var manager = new PrescriptionManager(null, null, null, null, null);
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
                Drugs = drugs

            };
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
            var dispenseInserted = await _prescriptionManager.GetDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, dispense.Id);
            Assert.IsNotNull(dispenseInserted);
            Assert.AreEqual(remark, dispenseInserted.Remark);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnNull()
        {
            await _prescriptionManager.GetDispense(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnEmpty()
        {
            await _prescriptionManager.GetDispense("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnPatientIdInvalid()
        {
            await _prescriptionManager.GetDispense("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispensesThrowsOnPrescriptionIdInvalid()
        {
            await _prescriptionManager.GetDispense(PatientTestEnvironment.PatientIdOne, "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetDispenses()
        {
            var dispenses = await _prescriptionManager.GetDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.IsNotNull(dispenses);
            Assert.AreEqual(1, dispenses.Count);
            Assert.AreEqual(DispenseTestEnvironment.DispenseOneRemark, dispenses.First().Remark);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnNull()
        {
            await _prescriptionManager.GetDispense(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnEmpty()
        {
            await _prescriptionManager.GetDispense("", "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnPatientIdInvalid()
        {
            await _prescriptionManager.GetDispense("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, DispenseTestEnvironment.DispenseOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnPrescriptionIdInvalid()
        {
            await _prescriptionManager.GetDispense(PatientTestEnvironment.PatientIdOne, "sdfklsdf", DispenseTestEnvironment.DispenseOneId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetDispenseThrowsOnDispenseIdInvalid()
        {
            await _prescriptionManager.GetDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetDispense()
        {
            var dispense = await _prescriptionManager.GetDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, DispenseTestEnvironment.DispenseOneId);
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
    }
}

