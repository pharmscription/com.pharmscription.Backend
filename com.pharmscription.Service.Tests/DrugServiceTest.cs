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

        private static Mock<IDrugManager> mock;

        private static IRestService service;

        [TestInitialize]
        public void SetUp()
        {
            mock = new Mock<IDrugManager>();
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
            mock.Setup(m => m.GetById(CorrectId)).Returns(Task.Run(() => dto));
            DrugDto answerDto = await service.GetDrug(CorrectId);
            Assert.AreEqual(dto, answerDto);
        }

        [TestMethod]
        public async Task TestGetInvalidDrugById()
        {
            mock.Setup(m => m.GetById(WrongId)).Throws<NotFoundException>();
            try
            {
                await service.GetDrug(WrongId);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestGetDrugWithNullParameter()
        {
            mock.Setup(m => m.GetById(null)).Throws<ArgumentException>();
            try
            {
                await service.GetDrug(null);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchDrugPage()
        {
            mock.Setup(m => m.SearchPaged("remeron", 1, 3)).ReturnsAsync(DrugArray);
            DrugDto[] answerDto = await service.SearchDrugs("remeron", "1", "3");
            CollectionAssert.AreEqual(DrugArray.ToArray(), answerDto);
        }

        [TestMethod]
        public async Task TestSearchDrug()
        {
            mock.Setup(m => m.Search("remeron")).ReturnsAsync(DrugArray);
            int answer = await service.SearchDrugs("remeron");
            Assert.AreEqual(DrugArray.Count, answer);
        }
        [TestMethod]
        public async Task TestSearchEmptyString()
        {
            mock.Setup(m => m.Search(string.Empty)).Throws<InvalidArgumentException>();
            try
            {
                await service.SearchDrugs(string.Empty);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchNullString()
        {
            mock.Setup(m => m.Search(null)).Throws<ArgumentNullException>();
            try
            {
                await service.SearchDrugs(null);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }


    }
}
