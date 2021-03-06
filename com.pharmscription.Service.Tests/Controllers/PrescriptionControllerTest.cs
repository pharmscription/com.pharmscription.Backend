﻿using System;
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
using System.Net;
using System.Web.Mvc;

namespace com.pharmscription.Service.Tests.Controllers
{
    using System.Globalization;

    using com.pharmscription.BusinessLogic.Converter;

    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugItem;
    using Service.Controllers;
    using Controllers;
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
        private IDrugRepository _drugRepository;
        private IDrugItemRepository _drugItemRepository;
        
        [TestInitialize]
        public void SetUp()
        {
            _puow = new PharmscriptionUnitOfWork();
            _prescriptionRepository = new PrescriptionRepository(_puow);
            _patientRepository = new PatientRepository(_puow);
            _counterProposalRepository = new CounterProposalRepository(_puow);
            _dispenseRepository = new DispenseRepository(_puow);
            _drugRepository = new DrugRepository(_puow);
            _drugItemRepository = new DrugItemRepository(_puow);
            _prescriptionManager= new PrescriptionManager(_prescriptionRepository, _patientRepository, _counterProposalRepository, _dispenseRepository, _drugRepository, _drugItemRepository);
            _prescriptionController = new PrescriptionController(_prescriptionManager);
            SetupTestData();
        }

        private void SetupTestData()
        {
            CleanUp();
            var patients = PatientTestEnvironment.GetTestPatients();
            foreach (var patient in patients)
            {
                _patientRepository.Add(patient);
            }
            _puow.Commit();
            var rafiTask = _patientRepository.GetWithAllNavs(e => e.FirstName == "Rafael");
            var markusTask = _patientRepository.GetWithAllNavs(e => e.FirstName == "Markus");
            rafiTask.Wait();
            markusTask.Wait();
            var rafi = rafiTask.Result.FirstOrDefault();
            var markus = markusTask.Result.FirstOrDefault();

            var counterProposals = CounterProposalTestEnvironment.GetTestCounterProposals();
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
            rafi?.Prescriptions.Add(prescriptions.First());
            markus?.Prescriptions.Add(prescriptions.Skip(1).FirstOrDefault());

            var dispenses = DispenseTestEnvironment.GetTestDispenses();
            foreach (var dispense in dispenses)
            {
                _dispenseRepository.Add(dispense);
                _puow.Commit();

            }
            var prescriptionTask = _prescriptionRepository.GetWithAllNavs(prescription => true);
            prescriptionTask.Wait();
            var prescriptionsToConnect = prescriptionTask.Result;
            var proposalsToConnect = counterProposalsToConnect as IList<CounterProposal> ?? counterProposalsToConnect.ToList();
            foreach (var counterProposal in proposalsToConnect.Skip(1).ToList())
            {
                _puow.Attach(counterProposal);
                var prescriptionA = prescriptionsToConnect.FirstOrDefault();
                prescriptionA?.CounterProposals.Add(counterProposal);
                _puow.Attach(prescriptionsToConnect.FirstOrDefault());
                _puow.Commit();
            }
            var prescriptionB = prescriptionsToConnect.OrderBy(e => e.Id).Skip(1).FirstOrDefault();
            prescriptionB?.CounterProposals.Add(proposalsToConnect.FirstOrDefault());
            _puow.Commit();
        }

        [TestCleanup]
        public void CleanUp()
        {
            foreach (var deleteStatment in TestEnvironmentHelper.DeleteStatments)
            {
                var newContext = new PharmscriptionUnitOfWork();
                newContext.ExecuteCommand(deleteStatment);
                newContext.Commit();
            }
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
            var prescription = await _prescriptionController.GetPrescriptions(PatientTestEnvironment.PatientIdOne);
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
            var prescription = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptionById(IdentityGenerator.NewSequentialGuid().ToString(), PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.AreEqual((int)HttpStatusCode.NoContent, prescription.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsNoContentOnPrescriptionNotFound()
        {
            var prescription = (HttpStatusCodeResult)await _prescriptionController.GetPrescriptionById(PatientTestEnvironment.PatientIdOne, IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, prescription.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsOnFound()
        {
            var prescription = await _prescriptionController.GetPrescriptionById(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId);
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
                        DrugDescription = DrugTestEnvironment.DrugOneDescription
                    }
                },
                new DrugItemDto
                {
                    Drug = new DrugDto
                    {
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
                        DrugDescription = DrugTestEnvironment.DrugOneDescription
                    }
                },
                new DrugItemDto
                {
                    Drug = new DrugDto
                    {
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
                Drugs = drugs

            };
            var result =
                (HttpStatusCodeResult)
                 await
                 _prescriptionController.CreatePrescription(
                     PatientTestEnvironment.PatientIdOne,
                     prescriptionToInsert);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPrescription()
        {
            foreach (var testDrug in DrugTestEnvironment.GetTestDrugs())
            {
                if (_drugRepository.Get(testDrug.Id) == null)
                {
                    _drugRepository.Add(testDrug);
                }

            }
            await _puow.CommitAsync();
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
            var prescription = (PrescriptionDto)((JsonResult)await _prescriptionController.CreatePrescription(PatientTestEnvironment.PatientIdOne, prescriptionToInsert)).Data;
            Assert.IsNotNull(prescription);
            var prescriptions =
                (List<PrescriptionDto>)
                    ((JsonResult) await _prescriptionController.GetPrescriptions(PatientTestEnvironment.PatientIdOne))
                        .Data;
            var prescriptionInserted = prescriptions.FirstOrDefault(e => e.Id == prescription.Id);
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.AreEqual(DrugTestEnvironment.DrugOneDescription, insertedDrugs.First().Drug.DrugDescription);
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
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddCounterProposalThrowsOnPrescriptionNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal(PatientTestEnvironment.PatientIdOne, "sndfsfsfjlks", counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [TestMethod]
        public async Task TestAddCounterProposalReturnsNoContentOnPatientNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal(IdentityGenerator.NewSequentialGuid().ToString(), PrescriptionTestEnvironment.StandingPrescriptionOneId, counterProposalDto);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestAddCounterProposalReturnsNoContentOnPrescriptionNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateCounterProposal(PatientTestEnvironment.PatientIdOne, IdentityGenerator.NewSequentialGuid().ToString(), counterProposalDto);
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
            var counterProposal = (CounterProposalDto)((JsonResult)await _prescriptionController.CreateCounterProposal(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, prescriptionToInsert)).Data;
            Assert.IsNotNull(counterProposal);
            var counterProposals = (List<CounterProposalDto>)((JsonResult)await _prescriptionController.GetCounterProposals(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId)).Data;
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
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetCounterProposalsThrowsOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals(PatientTestEnvironment.PatientIdOne, "sdfklsdf");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetCounterProposalsReturnsNoContentOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals(IdentityGenerator.NewSequentialGuid().ToString(), PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetCounterProposalsReturnsNoContentOnPrescriptonNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetCounterProposals(PatientTestEnvironment.PatientIdOne, IdentityGenerator.NewSequentialGuid().ToString());
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
            var counterProposals = (List<CounterProposalDto>)((JsonResult)await _prescriptionController.GetCounterProposals(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId)).Data;
            Assert.IsNotNull(counterProposals);
            Assert.AreEqual(4, counterProposals.Count);
            Assert.AreEqual(CounterProposalTestEnvironment.CounterProposalFiveMessage, counterProposals.First().Message);
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
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseThrowsOnPrescriptionNotFound()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense(PatientTestEnvironment.PatientIdOne, "sndfsfsfjlks", dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseReturnNoContentOnPrescriptionNotFound()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense(PatientTestEnvironment.PatientIdOne, IdentityGenerator.NewSequentialGuid().ToString(), dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispenseReturnNoContentOnPatientNotFound()
        {
            var dispenseDto = new DispenseDto();
            var result = (HttpStatusCodeResult)await _prescriptionController.CreateDispense(IdentityGenerator.NewSequentialGuid().ToString(), PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseDto);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddDispense()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto
            {
                Remark = remark
            };
            var dispense = (DispenseDto)((JsonResult)await _prescriptionController.CreateDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
            var dispenseInserted = ((List<DispenseDto>)((JsonResult)await _prescriptionController.GetDispenses(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId)).Data).FirstOrDefault(e => e.Id == dispense.Id);
            Assert.IsNotNull(dispenseInserted);
            Assert.AreEqual(remark, dispenseInserted.Remark);
        }

        [TestMethod]
        public async Task TestEditDispense()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto
            {
                Remark = remark
            };
            var dispense = (DispenseDto)((JsonResult)await _prescriptionController.CreateDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseToInsert)).Data;
            dispenseToInsert.Id = dispense.Id;
            dispenseToInsert.Date = DateTime.Now.ToString(
                PharmscriptionConstants.DateFormat,
                CultureInfo.InvariantCulture);
            dispense =
                (DispenseDto)
                ((JsonResult)
                 await
                 _prescriptionController.EditDispense(
                     PatientTestEnvironment.PatientIdOne,
                     PrescriptionTestEnvironment.StandingPrescriptionOneId,
                     dispenseToInsert.Id,
                     dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
        }

        [TestMethod]
        public async Task TestEditDispensWithDrugItem()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto
            {
                Remark = remark
            };
            dispenseToInsert.DrugItems = new List<DrugItemDto>();
            DrugItemTestEnvironment.GetTestDrugItems().ForEach(item => dispenseToInsert.DrugItems.Add(item.ConvertToDto()));
            var dispense = (DispenseDto)((JsonResult)await _prescriptionController.CreateDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseToInsert)).Data;
            dispenseToInsert.Id = dispense.Id;
            dispenseToInsert.Date = DateTime.Now.ToString(
                PharmscriptionConstants.DateFormat,
                CultureInfo.InvariantCulture);
            dispense =
                (DispenseDto)
                ((JsonResult)
                 await
                 _prescriptionController.EditDispense(
                     PatientTestEnvironment.PatientIdOne,
                     PrescriptionTestEnvironment.StandingPrescriptionOneId,
                     dispenseToInsert.Id,
                     dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
        }

        [TestMethod]
        public async Task TestEditDispenseEmptyDispenseId()
        {
            var result = (HttpStatusCodeResult)
                await _prescriptionController.EditDispense(
                PatientTestEnvironment.PatientIdOne,
                PrescriptionTestEnvironment.StandingPrescriptionOneId,
                string.Empty,
                DispenseTestEnvironment.GetTestDispenses().First().ConvertToDto());
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestEditDispensePatientNotFound()
        {
            var result =
                (HttpStatusCodeResult)
                await
                _prescriptionController.EditDispense(
                    new Guid().ToString(), 
                    PrescriptionTestEnvironment.StandingPrescriptionOneId,
                    DispenseTestEnvironment.DispenseOneId,
                    DispenseTestEnvironment.GetTestDispenses().First().ConvertToDto());
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task TestEditDispensePrescriptionNotFound()
        {
            var result =
                (HttpStatusCodeResult)
                await
                _prescriptionController.EditDispense(
                    PatientTestEnvironment.PatientIdOne,
                    new Guid().ToString(), 
                    DispenseTestEnvironment.DispenseOneId,
                    DispenseTestEnvironment.GetTestDispenses().First().ConvertToDto());
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task TestEditDispenseBadPatientId()
        {
            var result =
                (HttpStatusCodeResult)
                await
                _prescriptionController.EditDispense(
                    string.Empty,
                    PrescriptionTestEnvironment.StandingPrescriptionOneId,
                    DispenseTestEnvironment.DispenseOneId,
                    DispenseTestEnvironment.GetTestDispenses().First().ConvertToDto());
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestEditDispenseBadPrescriptionId()
        {
            var result =
                (HttpStatusCodeResult)
                await
                _prescriptionController.EditDispense(
                    PatientTestEnvironment.PatientIdOne,
                    string.Empty,
                    DispenseTestEnvironment.DispenseOneId,
                    DispenseTestEnvironment.GetTestDispenses().First().ConvertToDto());
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
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
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetDispensesThrowsOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses(PatientTestEnvironment.PatientIdOne, "sdfklsdf");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetDispensesReturnNoContentOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses(PatientTestEnvironment.PatientIdOne, IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetDispensesReturnNoContentOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDispenses(IdentityGenerator.NewSequentialGuid().ToString(), PrescriptionTestEnvironment.StandingPrescriptionOneId);
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
            var dispense = (DispenseDto)((JsonResult)await _prescriptionController.CreateDispense(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId, dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
            var dispenses = (List<DispenseDto>)((JsonResult)await _prescriptionController.GetDispenses(PatientTestEnvironment.PatientIdOne, PrescriptionTestEnvironment.StandingPrescriptionOneId)).Data;
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
            await _prescriptionController.GetDrugs("jksdjksadfksd", PrescriptionTestEnvironment.StandingPrescriptionOneId);
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrugThrowsOnPrescriptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs(PatientTestEnvironment.PatientIdOne, "sdfklsdf");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPrescriptionDrugReturnNoContentOnPatientNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs(PatientTestEnvironment.PatientIdOne, IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }
        [TestMethod]
        public async Task TestGetPrescriptionDrugReturnNoContentOnPresciptionNotFound()
        {
            var result = (HttpStatusCodeResult)await _prescriptionController.GetDrugs(IdentityGenerator.NewSequentialGuid().ToString(), PrescriptionTestEnvironment.StandingPrescriptionOneId);
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
            Assert.AreEqual(PrescriptionTestEnvironment.DrugDescriptionOne, drugs.First().Drug.DrugDescription);
        }
    }
}

