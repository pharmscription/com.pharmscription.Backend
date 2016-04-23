/*
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Prescription;
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
            var prescriptionRepository = TestEnvironmentHelper.GetMockedPrescriptionRepository();
            var patientRepository = TestEnvironmentHelper.GetMockedPatientRepository();
            _prescriptionManager = new PrescriptionManager(prescriptionRepository.Object, patientRepository.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestManagerThrowsWhenPatientRepoWasNull()
        {
            var manager = new PrescriptionManager(null, null);
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
        [ExpectedException(typeof(NotFoundException))]
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
                EditDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                IssueDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                IsValid = true,
                Type = "Standing",
                ValidUntil = DateTime.Now.AddDays(2).ToString(CultureInfo.InvariantCulture),
                Drugs = drugs

            };
            var prescription = await _prescriptionManager.Add("IdIsInDatabase", prescriptionToInsert);
            Assert.IsNotNull(prescription);
            var prescriptionInserted = await _prescriptionManager.Get("IdIsInDatabase", prescription.Id);
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
            await _prescriptionManager.AddCounterProposal("jksdjksadfksd", "IdIsInDatabase",  counterProposalDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddCounterProposalThrowsOnPrescriptionNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionManager.AddCounterProposal("IdIsInDatabase", "sndfsfsfjlks", counterProposalDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddCounterProposal()
        {
            const string message = "Dieses Rezept ist gemein gefährlich";
            var prescriptionToInsert = new CounterProposalDto
            {
                Date = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Message = message
          

            };
            var counterProposal = await _prescriptionManager.AddCounterProposal("IdIsInDatabase", "IdIsINDatabase", prescriptionToInsert);
            Assert.IsNotNull(counterProposal);
            var counterProposalInserted = await _prescriptionManager.GetCounterProposal("IdIsInDatabase", "IdIsInDatabase", counterProposal.Id);
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
            await _prescriptionManager.GetCounterProposal("jksdjksadfksd", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalsThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetCounterProposal("IDIsInDatabse", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetCounterProposals()
        {
            var prescriptions = await _prescriptionManager.GetCounterProposal("IdIsInDatabase", "IdIsInDatabse");
            Assert.IsNotNull(prescriptions);
            Assert.AreEqual(5, prescriptions.Count);
            Assert.AreEqual("Dieses Rezept war für Malaria, der Patient hat aber Gelbfieber", prescriptions.First().Message);
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
            await _prescriptionManager.GetCounterProposal("jksdjksadfksd", "IDIsInDatabase", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetCounterProposal("IDIsInDatabse", "sdfklsdf", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetCounterProposalThrowsOnCounterProposalNotFound()
        {
            await _prescriptionManager.GetCounterProposal("IDIsInDatabse", "IDIsIDatabse", "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetCounterProposal()
        {
            var prescription = await _prescriptionManager.GetCounterProposal("IdIsInDatabase", "IdIsInDatabse", "IdIsInDatabse");
            Assert.IsNotNull(prescription);
            Assert.AreEqual("Dieses Rezept war für Malaria, der Patient hat aber Gelbfieber", prescription.Message);
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
            await _prescriptionManager.EditCounterProposal("jksdjksadfksd", "IDIsInDatabase", new CounterProposalDto());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestEditCounterProposalThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.EditCounterProposal("IDIsInDatabse", "sdfklsdf", new CounterProposalDto());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestEditCounterProposalThrowsOnCounterProposalNotFound()
        {
            await _prescriptionManager.EditCounterProposal("IDIsInDatabse", "IDIsIDatabse", new CounterProposalDto());
        }

        [TestMethod]
        public async Task TestEditCounterProposal()
        {
            const string id = "IdIsInDatabase";
            var counterPropsal = new CounterProposalDto
            {
                Id = id,
                Message = "Rezept geändert"
            };
            var counterProposalBeforeEdit =
                await _prescriptionManager.GetCounterProposal("IdIsInDatabase", "IdIsInDatabse", id);
            await _prescriptionManager.EditCounterProposal("IdIsInDatabase", "IdIsInDatabse", counterPropsal);
            var counterProposalAfterEdit =
                await _prescriptionManager.GetCounterProposal("IdIsInDatabase", "IdIsInDatabse", id);
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
            await _prescriptionManager.AddDispense("jksdjksadfksd", "IdIsInDatabase", dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddDispenseThrowsOnPrescriptionNotFound()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionManager.AddDispense("IdIsInDatabase", "sndfsfsfjlks", dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestAddDispense()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto()
            {
                Date = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Remark = "Diese Ausgabe gelang super"
            };
            var dispense = await _prescriptionManager.AddDispense("IdIsInDatabase", "IdIsINDatabase", dispenseToInsert);
            Assert.IsNotNull(dispense);
            var dispenseInserted = await _prescriptionManager.GetDispense("IdIsInDatabase", "IdIsInDatabase", dispense.Id);
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
            await _prescriptionManager.GetDispense("jksdjksadfksd", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispensesThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetDispense("IDIsInDatabse", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetDispenses()
        {
            var dispenses = await _prescriptionManager.GetDispense("IdIsInDatabase", "IdIsInDatabse");
            Assert.IsNotNull(dispenses);
            Assert.AreEqual(3, dispenses.Count);
            Assert.AreEqual("Dies ist meine Lieblingsausgabe", dispenses.First().Remark);
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
            await _prescriptionManager.GetDispense("jksdjksadfksd", "IDIsInDatabase", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispenseThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetDispense("IDIsInDatabse", "sdfklsdf", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetDispenseThrowsOnCounterProposalNotFound()
        {
            await _prescriptionManager.GetDispense("IDIsInDatabse", "IDIsIDatabse", "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetDispense()
        {
            var dispense = await _prescriptionManager.GetDispense("IdIsInDatabase", "IdIsInDatabse", "IdIsInDatabse");
            Assert.IsNotNull(dispense);
            Assert.AreEqual("Dies ist meine Lieblingsausgabe", dispense.Remark);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionDrugThrowsOnNull()
        {
            await _prescriptionManager.GetPrescriptionDrugs(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetPrescriptionDrugThrowsOnEmpty()
        {
            await _prescriptionManager.GetPrescriptionDrugs("", "", "");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetGetPrescriptionDrugThrowsOnPatientNotFound()
        {
            await _prescriptionManager.GetPrescriptionDrugs("jksdjksadfksd", "IDIsInDatabase", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetPrescriptionDrugThrowsOnPrescriptionNotFound()
        {
            await _prescriptionManager.GetPrescriptionDrugs("IDIsInDatabse", "sdfklsdf", "IDIsInDatabase");
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetPrescriptionDrugThrowsOnCounterProposalNotFound()
        {
            await _prescriptionManager.GetPrescriptionDrugs("IDIsInDatabse", "IDIsIDatabse", "sdfskjdfkjsfkj");
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrug()
        {
            var drug = await _prescriptionManager.GetPrescriptionDrugs("IdIsInDatabase", "IdIsInDatabse", "IdIsInDatabse");
            Assert.IsNotNull(drug);
            Assert.AreEqual("Aspirin", drug.Drug.DrugDescription);
        }
    }
}
*/
