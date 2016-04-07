using System;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{
    [TestClass]
    public class DrugConverterTests
    {
        [TestMethod]
        public void TestCanConvertEntityToDto()
        {
            var entity = new Drug()
            {
                Id = new Guid(),
                DrugDescription = "Hallo Meine Droge",
                IsValid = true,
                NarcoticCategory = "A",
                PackageSize = "Big",
                Unit = "2",
                Composition = "HD",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var dtoFromEntity = entity.ConvertToDto();
            Assert.AreEqual(entity.Id, dtoFromEntity.Id);
            Assert.AreEqual(entity.DrugDescription, dtoFromEntity.DrugDescription);
            Assert.AreEqual(entity.IsValid, dtoFromEntity.IsValid);
            Assert.AreEqual(entity.NarcoticCategory, dtoFromEntity.NarcoticCategory);
            Assert.AreEqual(entity.PackageSize, dtoFromEntity.PackageSize);
            Assert.AreEqual(entity.Unit, dtoFromEntity.Unit);
            Assert.AreEqual(entity.Composition, dtoFromEntity.Composition);

        }
    }
}
