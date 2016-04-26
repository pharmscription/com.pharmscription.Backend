
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Prescription;
using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Tests.Prescription
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PrescriptionManagerTest
    {
        private IPrescriptionManager _prescriptionManager;

        [TestInitialize]
        public void SetUp()
        {

            var counterProposalRepository = TestEnvironmentHelper.GetMockedCounterProposalRepository();
            var dispenseRepository = TestEnvironmentHelper.GetMockedDispenseRepository();
            var prescriptionRepository = TestEnvironmentHelper.GetMockedPrescriptionRepository();
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
            var patientRepository = TestEnvironmentHelper.GetMockedPatientRepository();
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

            _prescriptionManager = new PrescriptionManager(prescriptionRepository.Object, patientRepository.Object, counterProposalRepository.Object, dispenseRepository.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestManagerThrowsWhenPatientRepoWasNull()
        {
            var manager = new PrescriptionManager(null, null, null, null);
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
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetPrescriptionByPatientIdThrowsOnNotFound()
        {
            await _prescriptionManager.Get("jksdjksadfksd");
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdReturnsOnFound()
        {
            var prescription = await _prescriptionManager.Get("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38");
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
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnNotFound()
        {
            await _prescriptionManager.Get("jksdjksadfksd", "ajsksdk");
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsOnFound()
        {

            var prescription = await _prescriptionManager.Get("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
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
                        IsValid = true,
                        DrugDescription = "Aspirin"
                    }
                },
                new DrugItemDto
                {
                    Drug = new DrugDto
                    {
                        IsValid = true,
                        DrugDescription = "Mebucain"
                    }
                }
            };
            var prescriptionToInsert = new PrescriptionDto
            {
                EditDate = DateTime.Now.ToString("dd.MM.yyyy"),
                IssueDate = DateTime.Now.ToString("dd.MM.yyyy"),
                IsValid = true,
                Type = "Standing",
                ValidUntil = DateTime.Now.AddDays(2).ToString("dd.MM.yyyy"),
                Drugs = drugs

            };
            var prescription = await _prescriptionManager.Add("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescriptionToInsert);
            Assert.IsNotNull(prescription);
            var prescriptionInserted = await _prescriptionManager.Get("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescription.Id);
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.AreEqual("Aspirin", insertedDrugs.First().Drug.DrugDescription);
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
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddCounterProposalThrowsOnPatientNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionManager.AddCounterProposal("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", counterProposalDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddCounterProposalThrowsOnPrescriptionNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionManager.AddCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sndfsfsfjlks", counterProposalDto);
        }

        [TestMethod]
        public async Task TestAddCounterProposal()
        {
            const string message = "Dieses Rezept ist gemein gefährlich";
            var prescriptionToInsert = new CounterProposalDto
            {
                Date = DateTime.Now.ToString("dd.MM.yyyy"),
                Message = message


            };
            var counterProposal = await _prescriptionManager.AddCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", prescriptionToInsert);
            Assert.IsNotNull(counterProposal);
            var counterProposalInserted = await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", counterProposal.Id);
            Assert.IsNotNull(counterProposalInserted);
            Assert.AreEqual(message, counterProposalInserted.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalsThrowsOnNull()
        {
            await _prescriptionManager.GetCounterProposal(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalsThrowsOnEmpty()
        {
            await _prescriptionManager.GetCounterProposal("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalsThrowsOnPatientNotFound()
        {
            await _prescriptionManager.GetCounterProposal("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalsThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetCounterProposals()
        {
            var counterProposals = await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.IsNotNull(counterProposals);
            Assert.AreEqual(5, counterProposals.Count);
            Assert.AreEqual("This is not right", counterProposals.First().Message);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalThrowsOnNull()
        {
            await _prescriptionManager.GetCounterProposal(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetCounterProposalThrowsOnEmpty()
        {
            await _prescriptionManager.GetCounterProposal("", "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalThrowsOnPatientNotFound()
        {
            await _prescriptionManager.GetCounterProposal("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalThrowsOnCounterProposalNotFound()
        {
            await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetCounterProposal()
        {
            var prescription = await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e42");
            Assert.IsNotNull(prescription);
            Assert.AreEqual("This is not right", prescription.Message);
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
            await _prescriptionManager.EditCounterProposal("1baf86d5-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", new CounterProposalDto
            {
                Id = "1bcb86b0-1e14-4f4c-b05a-5c9dd00e8e38"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestEditCounterProposalThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.EditCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e69-4f4c-b05a-5c9dd00e8e38", new CounterProposalDto
            {
                Id = "1bcb86b0-1e14-4f4c-b05a-5c9dd00e8e38"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditCounterProposalThrowsOnCounterProposalNotFound()
        {
            await _prescriptionManager.EditCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", new CounterProposalDto());
        }

        [TestMethod]
        public async Task TestEditCounterProposal()
        {
            const string id = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e42";
            var counterPropsal = new CounterProposalDto
            {
                Id = id,
                Message = "Rezept geändert",
                Date = DateTime.Now.ToString("dd.MM.yyyy")
            };
            var counterProposalBeforeEdit =
                await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", id);
            await _prescriptionManager.EditCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", counterPropsal);
            var counterProposalAfterEdit =
                await _prescriptionManager.GetCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", id);
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
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddDispenseThrowsOnPatientNotFound()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionManager.AddDispense("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddDispenseThrowsOnPrescriptionNotFound()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionManager.AddDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sndfsfsfjlks", dispenseDto);
        }

        [TestMethod]
        public async Task TestAddDispense()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto
            {
                Date = DateTime.Now.ToString("dd.MM.yyyy"),
                Remark = remark
            };
            var dispense = await _prescriptionManager.AddDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseToInsert);
            Assert.IsNotNull(dispense);
            var dispenseInserted = await _prescriptionManager.GetDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispense.Id);
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
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispensesThrowsOnPatientNotFound()
        {
            await _prescriptionManager.GetDispense("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispensesThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetDispenses()
        {
            var dispenses = await _prescriptionManager.GetDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.IsNotNull(dispenses);
            Assert.AreEqual(1, dispenses.Count);
            Assert.AreEqual("Did a Dispense", dispenses.First().Remark);
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
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispenseThrowsOnPatientNotFound()
        {
            await _prescriptionManager.GetDispense("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispenseThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispenseThrowsOnDispenseNotFound()
        {
            await _prescriptionManager.GetDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetDispense()
        {
            var dispense = await _prescriptionManager.GetDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e43");
            Assert.IsNotNull(dispense);
            Assert.AreEqual("Did a Dispense", dispense.Remark);
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
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetGetPrescriptionDrugThrowsOnPatientNotFound()
        {
            await _prescriptionManager.GetPrescriptionDrugs("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetPrescriptionDrugThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetPrescriptionDrugs("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrug()
        {
            var drugs = await _prescriptionManager.GetPrescriptionDrugs("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.IsNotNull(drugs);
            Assert.AreEqual("Aspirin", drugs.FirstOrDefault().Drug.DrugDescription);
        }
    }
}

