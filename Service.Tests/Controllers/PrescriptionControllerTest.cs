using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using com.pharmscription.BusinessLogic.Prescription;
using com.pharmscription.DataAccess.Repositories.CounterProposal;
using com.pharmscription.DataAccess.Repositories.Dispense;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.DrugItem;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Repositories.Prescription;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Controllers;

namespace Service.Tests.Controllers
{
    [TestClass]
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
            _prescriptionManager= new PrescriptionManager(_prescriptionRepository, _patientRepository, _counterProposalRepository, _dispenseRepository);
            _prescriptionController = new PrescriptionController(_prescriptionManager);
            SetupTestData();

        }

        private void SetupTestData()
        {
            var patientsInData = _patientRepository.GetAll().ToList();
            var patients = TestEnvironmentHelper.GetTestPatients();
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
            var prescriptions = TestEnvironmentHelper.GetTestPrescriptions();

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
            foreach (var prescription in _prescriptionRepository.GetWithAllNavs().ToList())
            {
                _prescriptionRepository.Remove(prescription);
            }
            _puow.Commit();

            foreach (var counterProposal in _counterProposalRepository.GetAll().ToList())
            {
                _counterProposalRepository.Remove(counterProposal);
            }
            _puow.Commit();

            foreach (var drugItem in _drugItemRepository.GetAll().ToList())
            {
                _drugItemRepository.Remove(drugItem);
            }
            _puow.Commit();
            
            foreach (var drug in _drugRepository.GetAll().ToList())
            {
                _drugRepository.Remove(drug);
            }
            _puow.Commit();

            foreach (var dispense in _dispenseRepository.GetAll().ToList())
            {
                _dispenseRepository.Remove(dispense);
            }
            _puow.Commit();

            foreach (var patient in _patientRepository.GetAll().ToList())
            {
                _patientRepository.Remove(patient);
            }
            _puow.Commit();
            var patients = _patientRepository.GetAll().ToList();
            var cx = 2;
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionByPatientIdThrowsOnNull()
        {
            await _prescriptionController.GetPrescriptions(null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionByPatientIdThrowsOnEmpty()
        {
            await _prescriptionController.GetPrescriptions("");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionByPatientIdThrowsOnNotFound()
        {
            await _prescriptionController.GetPrescriptions("jksdjksadfksd");
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPatientIdReturnsOnFound()
        {
            var prescription = await _prescriptionController.GetPrescriptions("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38");
            Assert.IsNotNull(prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnNull()
        {
            await _prescriptionController.GetPrescriptionById(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnEmpty()
        {
            await _prescriptionController.GetPrescriptionById("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionByPrescriptionIdThrowsOnNotFound()
        {
            await _prescriptionController.GetPrescriptionById("jksdjksadfksd", "ajsksdk");
        }

        [TestMethod]
        public async Task TestGetPrescriptionByPrescriptionIdReturnsOnFound()
        {
            var prescription = await _prescriptionController.GetPrescriptionById("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.IsNotNull(prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddPrescriptionThrowsOnNull()
        {
            await _prescriptionController.CreatePrescription(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddPrescriptionThrowsOnEmpty()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionController.CreatePrescription("", prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddPrescriptionThrowsOnPatientNotFound()
        {
            var prescription = new PrescriptionDto();
            await _prescriptionController.CreatePrescription("jksdjksadfksd", prescription);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
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
                EditDate = DateTime.Now.ToString("dd.MM.yyyy"),
                IssueDate = DateTime.Now.ToString("dd.MM.yyyy"),
                IsValid = true,
                Type = "Standing",
                Drugs = drugs

            };
            var prescription = (PrescriptionDto)(await _prescriptionController.CreatePrescription("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescriptionToInsert)).Data;
            Assert.IsNotNull(prescription);
            var prescriptionInserted = (PrescriptionDto)(await _prescriptionController.GetPrescriptionById("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescription.Id)).Data;
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.AreEqual("Aspirin", insertedDrugs.First().Drug.DrugDescription);
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
            var prescription = (PrescriptionDto)(await _prescriptionController.CreatePrescription("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescriptionToInsert)).Data;
            Assert.IsNotNull(prescription);
            var prescriptionInserted = (PrescriptionDto)(await _prescriptionController.GetPrescriptionById("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", prescription.Id)).Data;
            Assert.IsNotNull(prescriptionInserted);
            var insertedDrugs = prescriptionInserted.Drugs;
            Assert.IsNotNull(insertedDrugs);
            Assert.AreEqual("Aspirin", insertedDrugs.First().Drug.DrugDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddCounterProposalThrowsOnNull()
        {
            await _prescriptionController.CreateCounterProposal(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddCounterProposalThrowsOnEmpty()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionController.CreateCounterProposal("", "", counterProposalDto);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddCounterProposalThrowsOnPatientNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionController.CreateCounterProposal("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", counterProposalDto);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddCounterProposalThrowsOnPrescriptionNotFound()
        {
            var counterProposalDto = new CounterProposalDto();
            await _prescriptionController.CreateCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sndfsfsfjlks", counterProposalDto);
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
            var counterProposal = (CounterProposalDto)(await _prescriptionController.CreateCounterProposal("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", prescriptionToInsert)).Data;
            Assert.IsNotNull(counterProposal);
            var counterProposals = (List<CounterProposalDto>)(await _prescriptionController.GetCounterProposals("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data;
            Assert.IsNotNull(counterProposals);
            var counterProposalInserted = counterProposals.FirstOrDefault(e => e.Message == message);
            Assert.IsNotNull(counterProposalInserted);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetCounterProposalsThrowsOnNull()
        {
            await _prescriptionController.GetCounterProposals(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetCounterProposalsThrowsOnEmpty()
        {
            await _prescriptionController.GetCounterProposals("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetCounterProposalsThrowsOnPatientNotFound()
        {
            await _prescriptionController.GetCounterProposals("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetCounterProposalsThrowsOnPrescriptionNotFound()
        {
            await _prescriptionController.GetCounterProposals("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetCounterProposals()
        {
            var counterProposals = (List<CounterProposalDto>)(await _prescriptionController.GetCounterProposals("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data;
            Assert.IsNotNull(counterProposals);
            Assert.AreEqual(4, counterProposals.Count);
            Assert.AreEqual("This isnt even a Prescription, it is a giraffe", counterProposals.First().Message);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddDispenseThrowsOnNull()
        {
            await _prescriptionController.CreateDispense(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddDispenseThrowsOnEmpty()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionController.CreateDispense("", "", dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddDispenseThrowsOnPatientNotFound()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionController.CreateDispense("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseDto);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddDispenseThrowsOnPrescriptionNotFound()
        {
            var dispenseDto = new DispenseDto();
            await _prescriptionController.CreateDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sndfsfsfjlks", dispenseDto);
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
            var dispense = (DispenseDto)(await _prescriptionController.CreateDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
            var dispenseInserted = ((List<DispenseDto>)(await _prescriptionController.GetDispenses("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data).FirstOrDefault(e => e.Id == dispense.Id);
            Assert.IsNotNull(dispenseInserted);
            Assert.AreEqual(remark, dispenseInserted.Remark);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetDispensesThrowsOnNull()
        {
            await _prescriptionController.GetDispenses(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetDispensesThrowsOnEmpty()
        {
            await _prescriptionController.GetDispenses("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetDispensesThrowsOnPatientNotFound()
        {
            await _prescriptionController.GetDispenses("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetDispensesThrowsOnPrescriptionNotFound()
        {
            await _prescriptionController.GetDispenses("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetDispenses()
        {
            const string remark = "Dieses Rezept ist gemein gefährlich";
            var dispenseToInsert = new DispenseDto
            {
                Date = DateTime.Now.ToString("dd.MM.yyyy"),
                Remark = remark
            };
            var dispense = (DispenseDto)(await _prescriptionController.CreateDispense("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37", dispenseToInsert)).Data;
            Assert.IsNotNull(dispense);
            var dispenses = (List<DispenseDto>)(await _prescriptionController.GetDispenses("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data;
            Assert.IsNotNull(dispenses);
            Assert.AreEqual(1, dispenses.Count);
            Assert.AreEqual(remark, dispenses.First().Remark);
        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionDrugThrowsOnNull()
        {
            await _prescriptionController.GetDrugs(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionDrugThrowsOnEmpty()
        {
            await _prescriptionController.GetDrugs("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetGetPrescriptionDrugThrowsOnPatientNotFound()
        {
            await _prescriptionController.GetDrugs("jksdjksadfksd", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetPrescriptionDrugThrowsOnPrescriptionNotFound()
        {
            await _prescriptionController.GetDrugs("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "sdfklsdf");
        }

        [TestMethod]
        public async Task TestGetPrescriptionDrug()
        {
            var drugs = (List<DrugItemDto>)(await _prescriptionController.GetDrugs("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38", "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37")).Data;
            Assert.IsNotNull(drugs);
            Assert.AreEqual("Aspirin", drugs.FirstOrDefault().Drug.DrugDescription);
        }
    }
}
