using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Service.Tests
{
    using System.Net;
    using System.ServiceModel.Web;

    using com.pharmscription.BusinessLogic.Prescription;

    using Infrastructure.Exception;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DrugServiceTest
    {
        private const string CorrectId = "1";

        private const string WrongId = "a";

        private static readonly List<DrugDto> DrugArray = new List<DrugDto>
                                          {
                                              new DrugDto
                                                  {
                                                      Id = "1",
                                                      DrugDescription =
                                                          "Remeron SolTab 30mg, Schmelztabletten",
                                                      Composition = "mirtazapinum 15mg",
                                                      NarcoticCategory = "B"
                                                  },
                                              new DrugDto
                                                  {
                                                      Id = "2",
                                                      DrugDescription =
                                                          "Remeron SolTab 30mg, Schmelztabletten",
                                                      Composition = "mirtazapinum 30mg",
                                                      NarcoticCategory = "B"
                                                  },
                                              new DrugDto
                                                  {
                                                      Id = "3",
                                                      DrugDescription =
                                                          "Remeron SolTab 45mg, Schmelztabletten",
                                                      Composition = "mirtazapium 45mg",
                                                      NarcoticCategory = "B"
                                                  }
                                          };

        private static Mock<IDrugManager> _mock;

        private static IRestService _service;

        [TestInitialize]
        public void SetUp()
        {
            _mock = new Mock<IDrugManager>();
            var mock2 =  new Mock<IPatientManager>();
            var mock3 = new Mock<IPrescriptionManager>();
            service = new RestService(mock2.Object, mock.Object, mock3.Object);
        }

        [TestMethod]
        public async Task TestGetDrugById()
        {
            var dto = new DrugDto
                          {
                              Id = CorrectId,
                              DrugDescription = "Diaphin",
                              Composition = "10g",
                              NarcoticCategory = "A",
                              PackageSize = "10g",
                              Unit = "g"
                          };
            _mock.Setup(m => m.GetById(CorrectId)).Returns(Task.Run(() => dto));
            DrugDto answerDto = await _service.GetDrug(CorrectId);
            Assert.AreEqual(dto, answerDto);
        }

        [TestMethod]
        public async Task TestGetInvalidDrugById()
        {
            _mock.Setup(m => m.GetById(WrongId)).Throws<NotFoundException>();
            try
            {
                await _service.GetDrug(WrongId);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestGetDrugWithNullParameter()
        {
            _mock.Setup(m => m.GetById(null)).Throws<ArgumentException>();
            try
            {
                await _service.GetDrug(null);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestGetDrugByIdServerError()
        {
            _mock.Setup(m => m.GetById(CorrectId)).Throws<Exception>();
            try
            {
                await _service.GetDrug(CorrectId);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchDrugPage()
        {
            _mock.Setup(m => m.SearchPaged("remeron", "1", "3")).ReturnsAsync(DrugArray);
            DrugDto[] answerDto = await _service.SearchDrugs("remeron", "1", "3");
            CollectionAssert.AreEqual(DrugArray.ToArray(), answerDto);
        }

        [TestMethod]
        public async Task TestSearchDrugpageInvalidParameter()
        {
            try
            {
                await _service.SearchDrugs("remeron", "a", "b");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchDrugPageNotFount()
        {
            _mock.Setup(m => m.SearchPaged("asbirin", "1", "3")).Throws<NotFoundException>();
            try
            {
                await _service.SearchDrugs("asbirin", "1", "3");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchDrugPageInvalidDrug()
        {
            _mock.Setup(m => m.SearchPaged(null, "1", "3")).Throws<InvalidArgumentException>();
            try
            {
                await _service.SearchDrugs(null, "1", "3");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchDrugPagedServerError()
        {
            _mock.Setup(m => m.SearchPaged("aspirin", "1", "3")).Throws<Exception>();
            try
            {
                await _service.SearchDrugs("aspirin", "1", "3");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchDrug()
        {
            _mock.Setup(m => m.Search("remeron")).ReturnsAsync(3);
            int answer = await _service.SearchDrugs("remeron");
            Assert.AreEqual(DrugArray.Count, answer);
        }

        [TestMethod]
        public async Task TestSearchDrugNotFound()
        {
            _mock.Setup(m => m.Search("sonda")).Throws<NotFoundException>();
            try
            {
                await _service.SearchDrugs("sonda");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }
        [TestMethod]
        public async Task TestSearchEmptyString()
        {
            _mock.Setup(m => m.Search(string.Empty)).Throws<InvalidArgumentException>();
            try
            {
                await _service.SearchDrugs(string.Empty);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchNullString()
        {
            _mock.Setup(m => m.Search(null)).Throws<ArgumentNullException>();
            try
            {
                await _service.SearchDrugs(null);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TaskSearchServerError()
        {
            _mock.Setup(m => m.Search("remeron")).Throws<Exception>();
            try
            {
                await _service.SearchDrugs("remeron");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, e.StatusCode);
            }
        }
    }
}
