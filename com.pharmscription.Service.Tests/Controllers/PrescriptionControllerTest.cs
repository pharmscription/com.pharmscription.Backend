using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Prescription;
using com.pharmscription.DataAccess.Repositories.CounterProposal;
using com.pharmscription.DataAccess.Repositories.Dispense;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Repositories.Prescription;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.EntityHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Controllers;

namespace Service.Tests.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    [TestClass]
    [ExcludeFromCodeCoverage]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Rule makes no sense in tests.")]
    public class PrescriptionControllerTest
    {
        private PrescriptionController _prescriptionController;
        private IPharmscriptionUnitOfWork _puow;
        private IPrescriptionRepository _prescriptionRepository;
        private IPatientRepository _patientRepository;
        private ICounterProposalRepository _counterProposalRepository;
        private IDispenseRepository _dispenseRepository;
        private IPrescriptionManager _prescriptionManager;
        
        [TestInitialize]
        public void SetUp()
        {
            _puow = new PharmscriptionUnitOfWork();
            _prescriptionRepository = new PrescriptionRepository(_puow);
            _patientRepository = new PatientRepository(_puow);
            _counterProposalRepository = new CounterProposalRepository(_puow);
            _dispenseRepository = new DispenseRepository(_puow);
            _prescriptionManager= new PrescriptionManager(_prescriptionRepository, _patientRepository, _counterProposalRepository, _dispenseRepository);
            _prescriptionController = new PrescriptionController(_prescriptionManager);
            SetupTestData();
        }

        private void SetupTestData()
        {
            var patients = PatientTestEnvironment.GetTestPatients();
            foreach (var patient in patients)
            {
                _patientRepository.Add(patient);
            }
            _puow.Commit();
            var rafi = _patientRepository.GetWithAllNavs().FirstOrDefault(e => e.FirstName == "Rafael");
            var markus = _patientRepository.GetWithAllNavs().FirstOrDefault(e => e.FirstName == "Markus");

            var counterProposals = TestEnvironmentHelper.GetTestCounterProposals();
            foreach (var counterProposal in counterProposals)
            {
                _counterProposalRepository.Add(counterProposal);
                _puow.Commit();

            }
            var counterProposalsToConnect = _counterProposalRepository.GetAll();
            var prescriptions = PrescriptionTestEnvironment.GetTestPrescriptions();

            foreach (var prescription in prescriptions)
            {
                _prescriptionRepository.Add(prescription);

                _puow.Commit();
            }
            rafi.Prescriptions.Add(prescriptions.First());
            markus.Prescriptions.Add(prescriptions.Skip(1).FirstOrDefault());

            var dispenses = TestEnvironmentHelper.GetTestDispenses();
            foreach (var dispense in dispenses)
            {
                _dispenseRepository.Add(dispense);
                _puow.Commit();

            }

            var prescriptionsToConnect = _prescriptionRepository.GetWithAllNavs().OrderBy(e => e.Id);
            foreach (var counterProposal in counterProposalsToConnect.Skip(1))
            {
                prescriptionsToConnect.FirstOrDefault().CounterProposals.Add(counterProposal);
            }
            prescriptionsToConnect.OrderBy(e => e.Id).Skip(1).FirstOrDefault().CounterProposals.Add(counterProposalsToConnect.FirstOrDefault());
        }

        [TestCleanup]
        public void CleanUp()
        {
            _puow.ExecuteCommand("Delete From DrugItems");
            _puow.Commit();
            _puow.ExecuteCommand("Delete From CounterProposals");
            _puow.Commit();
            _puow.ExecuteCommand("Delete From Dispenses");
            _puow.Commit();
            _puow.ExecuteCommand("Delete From Prescriptions");
            _puow.Commit();
            _puow.ExecuteCommand("Delete From Drugs");
            _puow.Commit();
            _puow.ExecuteCommand("Delete From Patients");
            _puow.Commit();
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptions(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdThrowsOnEmpty()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptions(string.Empty);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdThrowsOnNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptions("jksdjksadfksd");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdReturnsNoContentOnNoPrescriptions()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptions(PatientTestEnvironment.PatientWithNoPrescriptionId);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdReturnsNoContentOnNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptions(IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdReturnsOnFound()
        {
            var prescription = await _prescriptionController.GetPrescriptions("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38");
            Assert.IsNotNull(prescription);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptionById(null, null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnEmpty()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptionById(string.Empty, string.Empty);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptionById("jksdjksadfksd", "ajsksdk");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsNoContentOnPatientNotFound()
        {
            var prescription = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptionById(IdentityGenerator.NewSequentialGuid().ToString(), "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.AreEqual((int)HttpStatusCode.NoContent, prescription.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsNoContentOnPrescriptionNotFound()
        {
            var prescription = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptionById("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, prescription.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsOnFound()
        {
            var prescription = await _prescriptionController.GetPrescriptionById("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.IsNotNull(prescription);
        }

        [TestMethod]
        public async Task TestAddPrescriptionThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.CreatePrescription(null, null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPrescriptionThrowsOnEmpty()
        {
            var prescription = new PrescriptionDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreatePrescription(string.Empty, prescription);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPrescriptionThrowsOnPatientNotFound()
        {
            var prescription = new PrescriptionDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreatePrescription("jksdjksadfksd", prescription);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPrescriptionReturnsNoContentOnPatientNotFound()
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
                EditDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IssueDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IsValid = true,
                Type = "Standing",
                ValidUntil = DateTime.Now.AddDays(2).ToString(PharmscriptionConstants.DateFormat),
                Drugs = drugs
            };
            var result = (HttpStatusCodeResult)await _prescriptionController.CreatePrescription(IdentityGenerator.NewSequentialGuid().ToString(), prescriptionToInsert);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPrescriptionThrowWithNoValidUntilDate()
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
                EditDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IssueDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IsValid = true,
                Type = "Standing",
                Drugs = drugs

            };
            var result =
                (HttpStatusCodeResult)
                 await
                 _prescriptionController.CreatePrescription(
                     "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38",
                     prescriptionToInsert);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
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
                EditDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IssueDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IsValid = true,
                Type = "Standing",
                ValidUntil = DateTime.Now.AddDays(2).ToString(PharmscriptionConstants.DateFormat),
                Drugs = drugs
            };
            var prescription = (PrescriptionDto)((JsonResult)await _prescriptionController.CreatePrescription("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescriptionToInsert)).Data;
            Assert.IsNotNull(prescription);
            var prescriptionInserted = (PrescriptionDto)((JsonResult)await _prescriptionController.GetPrescriptionById("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescription.Id)).Data;
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.AreEqual("Aspirin", insertedDrugs.First().Drug.DrugDescription);
        }

        [TestMethod]
        public async Task TestAddCounterProposalThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal(null, null, null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
       public async Task TestAddCounterProposalThrowsOnEmpty()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal(string.Empty, string.Empty, counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddCounterProposalThrowsOnPatientNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddCounterProposalThrowsOnPrescriptionNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sndfsfsfjlks", counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [TestMethod]
        public async Task TestAddCounterProposalReturnsNoContentOnPatientNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal(IdentityGenerator.NewSequentialGuid().ToString(), "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestAddCounterProposalReturnsNoContentOnPrescriptionNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", IdentityGenerator.NewSequentialGuid().ToString(), counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddCounterProposal()
        {
            const string Message = "Dieses Rezept ist gemein gefährlich";
            var prescriptionToInsert = new CounterProposalDto
            {
                Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Message = Message
            };
            var counterProposal = (CounterProposalDto)((JsonResult)await _prescriptionController.CreateCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", prescriptionToInsert)).Data;
            Assert.IsNotNull(counterProposal);
            var counterProposals = (List<CounterProposalDto>)((JsonResult)await _prescriptionController.GetCounterProposals("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data;
            Assert.IsNotNull(counterProposals);
            var counterProposalInserted = counterProposals.FirstOrDefault(e => e.Message == Message);
            Assert.IsNotNull(counterProposalInserted);
        }

        [TestMethod]
        public async Task TestGetCounterProposalsThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals(null, null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetCounterProposalsThrowsOnEmpty()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals(string.Empty, string.Empty);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetCounterProposalsThrowsOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetCounterProposalsThrowsOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetCounterProposalsReturnsNoContentOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals(IdentityGenerator.NewSequentialGuid().ToString(), "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetCounterProposalsReturnsNoContentOnPrescriptonNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetCounterProposalsReturnsNoContentOnEmptyCounterProposals()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals(
                PatientTestEnvironment.PatientWithEmptyPrescriptionId, PatientTestEnvironment.EmptyPrescriptionId);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetCounterProposals()
        {
            var counterProposals = (List<CounterProposalDto>)((JsonResult)await _prescriptionController.GetCounterProposals("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data;
            Assert.IsNotNull(counterProposals);
            Assert.AreEqual(4, counterProposals.Count);
            Assert.AreEqual("This isnt even a Prescription, it is a giraffe", counterProposals.First().Message);
        }

        [TestMethod]
        public async Task TestAddDispenseThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense(null, null, null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseThrowsOnEmpty()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense(string.Empty, string.Empty, dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseThrowsOnPatientNotFound()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseThrowsOnPrescriptionNotFound()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sndfsfsfjlks", dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseReturnNoContentOnPrescriptionNotFound()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", IdentityGenerator.NewSequentialGuid().ToString(), dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseReturnNoContentOnPatientNotFound()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense(IdentityGenerator.NewSequentialGuid().ToString(), "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
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
            var dispense = (DispenseDto)((JsonResult)await _prescriptionController.CreateDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
            var dispenseInserted = ((List<DispenseDto>)((JsonResult)await _prescriptionController.GetDispenses("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data).FirstOrDefault(e => e.Id == dispense.Id);
            Assert.IsNotNull(dispenseInserted);
            Assert.AreEqual(remark, dispenseInserted.Remark);
        }

        [TestMethod]
        public async Task TestGetDispensesThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses(null, null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetDispensesThrowsOnEmpty()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses(string.Empty, string.Empty);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetDispensesThrowsOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetDispensesThrowsOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetDispensesReturnNoContentOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetDispensesReturnNoContentOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses(IdentityGenerator.NewSequentialGuid().ToString(), "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetDispensesReturnNoContentOnEmptyDispenses()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses(PatientTestEnvironment.PatientWithEmptyPrescriptionId, PatientTestEnvironment.EmptyPrescriptionId);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetDispenses()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto
            {
                Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Remark = remark
            };
            var dispense = (DispenseDto)((JsonResult)await _prescriptionController.CreateDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
            var dispenses = (List<DispenseDto>)((JsonResult)await _prescriptionController.GetDispenses("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data;
            Assert.IsNotNull(dispenses);
            Assert.AreEqual(1, dispenses.Count);
            Assert.AreEqual(remark, dispenses.First().Remark);
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrugThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs(null, null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrugThrowsOnEmpty()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs(string.Empty, string.Empty);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetGetPrescriptionDrugThrowsOnPatientNotFound()
        {
            await _prescriptionController.GetDrugs("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrugThrowsOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPrescriptionDrugReturnNoContentOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPrescriptionDrugReturnNoContentOnPresciptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs(IdentityGenerator.NewSequentialGuid().ToString(), "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPrescriptionDrugReturnNoContentOnEmptyDrugs()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs(PatientTestEnvironment.PatientWithEmptyPrescriptionId, PatientTestEnvironment.EmptyPrescriptionId);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrug()
        {
            var drugs = (List<DrugItemDto>)((JsonResult)await _prescriptionController.GetDrugs(
                PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId)).Data;
            Assert.IsNotNull(drugs);
            Assert.AreEqual("Aspirin", drugs.FirstOrDefault().Drug.DrugDescription);
        }
    }
}

