using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.Infrastructure.Dto;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Service.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DrugServiceTest
    {
        private static readonly string CorrectId = "1";

        private static readonly string WrongId = "a";

        private static Mock<IDrugManager> mock;

        private static IRestService service;

        [TestInitialize]
        public void SetUp()
        {
            mock = new Mock<IDrugManager>();
            service= new RestService(mock.Object);
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
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestGetInvalidDrugById()
        {
            mock.Setup(m => m.GetById(WrongId)).Throws<ArgumentException>();
            await service.GetDrug(WrongId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetDrugWithNullParameter()
        {
            mock.Setup(m => m.GetById(null)).Throws<ArgumentNullException>();
            await service.GetDrug(null);
        }

        [TestMethod]
        public async Task TestSearchDrug()
        {
            List<DrugDto> drugArray = new List<DrugDto>
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
            mock.Setup(m => m.Search("remeron")).Returns(Task.Run(() => drugArray));
            DrugDto[] answerDto = await service.SearchDrugs("remeron");
            CollectionAssert.AreEqual(drugArray.ToArray(), answerDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestSearchEmptyString()
        {
            mock.Setup(m => m.Search(string.Empty)).Throws<ArgumentException>();
            await service.SearchDrugs(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestSearchNullString()
        {
            mock.Setup(m => m.Search(null)).Throws<ArgumentNullException>();
            await service.SearchDrugs(null);
        }


    }
}
