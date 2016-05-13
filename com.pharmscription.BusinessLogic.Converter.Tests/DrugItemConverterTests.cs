using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Tests.TestEnvironment;
    using Infrastructure.Dto;
    using Infrastructure.EntityHelper;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DrugItemConverterTests
    {
        [TestMethod]
        public void TestCanConvertSingleEntityToDto()
        {
            var entity = new DrugItem
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DosageDescription = "Viel",
                Drug = new Drug
                {
                    Id = IdentityGenerator.NewSequentialGuid(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    DrugDescription = "Sehr gute Droge",
                    IsValid = true,
                    Composition = "bla",
                    NarcoticCategory = "A",
                    PackageSize = "200",
                    Unit = "10"
                },
                Quantity = 200
            };
            var dtoFromEntity = entity.ConvertToDto();
            Assert.IsTrue(dtoFromEntity.DtoEqualsEntity(entity));
        }

        [TestMethod]
        public void TestCanConvertNormalDtoToEntity()
        {
            var dto = new DrugItemDto
            {
                Id = new Guid().ToString(),
                DosageDescription = "Viel",
                Drug = new DrugDto
                {
                    Id = IdentityGenerator.NewSequentialGuid().ToString(),
                    DrugDescription = "Sehr gute Droge",
                    IsValid = true,
                    Composition = "bla",
                    NarcoticCategory = "A",
                    PackageSize = "200",
                    Unit = "10"
                },
                Quantity = 200
            };
            var entityFromDto = dto.ConvertToEntity();
            Assert.IsTrue(entityFromDto.EntityEqualsDto(dto));
        }

        [TestMethod]
        public void TestConvertsNullFromNullDto()
        {
            DrugItemDto dto = null;
            Assert.IsNull(dto.ConvertToEntity());
        }

        [TestMethod]
        public void TestConvertsNullFromNullEntity()
        {
            DrugItem drugItem = null;
            Assert.IsNull(drugItem.ConvertToDto());
        }

        [TestMethod]
        public void TestCanConvertListOfEntity()
        {

            var entityList = DrugItemTestEnvironment.GetTestDrugItems();
            var drugItemDtos = entityList.ConvertToDtos();
            for (var i = 0; i < drugItemDtos.Count; i++)
            {
                Assert.IsTrue(entityList.ElementAt(i).EntityEqualsDto(drugItemDtos.ElementAt(i)));
            }
            Assert.IsTrue(entityList.EntityListEqualsDtoList(drugItemDtos));
            Assert.IsTrue(drugItemDtos.DtoListEqualsEntityList(entityList));
        }

        [TestMethod]
        public void TestCanConvertListOfDtos()
        {
            var entityList = DrugItemTestEnvironment.GetTestDrugItems();
            var prescriptionDtos = entityList.ConvertToDtos();
            var convertedEntities = prescriptionDtos.ConvertToEntities();
            for (var i = 0; i < convertedEntities.Count; i++)
            {
                Assert.IsTrue(convertedEntities.ElementAt(i).EntityEqualsDto(prescriptionDtos.ElementAt(i)));
            }
            Assert.IsTrue(prescriptionDtos.DtoListEqualsEntityList(convertedEntities.ToList()));
            Assert.IsTrue(convertedEntities.ToList().EntityListEqualsDtoList(prescriptionDtos));
        }
    }
}
