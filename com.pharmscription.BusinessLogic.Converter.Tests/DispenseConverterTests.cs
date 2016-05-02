using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Tests.TestEnvironment;
    using Infrastructure.Constants;
    using Infrastructure.Dto;
    using Infrastructure.EntityHelper;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DispenseConverterTests
    {
        [TestMethod]
        public void TestCanConvertSingleEntityToDto()
        {
            var entity = new Dispense
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Date = DateTime.Now,
                Remark = "Did a dispense",
                DrugItems = new List<DrugItem>
                {
                    new DrugItem
                    {
                        Id = IdentityGenerator.NewSequentialGuid(),
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
                        ModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        Quantity = 200
                    }
                }
            };
            var dtoFromEntity = entity.ConvertToDto();
            Assert.IsTrue(dtoFromEntity.DtoEqualsEntity(entity));
        }

        [TestMethod]
        public void TestCanConvertNormalDtoToEntity()
        {
            var dto = new DispenseDto
            {
                Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Remark = "Did a dispense",
                Id = IdentityGenerator.NewSequentialGuid().ToString(),
                DrugItems = new List<DrugItemDto>
                {
                    new DrugItemDto
                    {
                        Id = IdentityGenerator.NewSequentialGuid().ToString(),
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
                    }
                }
            };
            var entityFromDto = dto.ConvertToEntity();
            Assert.IsTrue(entityFromDto.EntityEqualsDto(dto));
        }

        [TestMethod]
        public void TestConvertsNullFromNullDto()
        {
            DispenseDto dto = null;
            Assert.IsNull(dto.ConvertToEntity());
        }

        [TestMethod]
        public void TestConvertsNullFromNullEntity()
        {
            Dispense dispense = null;
            Assert.IsNull(dispense.ConvertToDto());
        }

        [TestMethod]
        public void TestCanConvertListOfEntity()
        {

            var entityList = DispenseTestEnvironment.GetTestDispenses();
            var dispenseDtos = entityList.ConvertToDtos();
            for (var i = 0; i < dispenseDtos.Count; i++)
            {
                Assert.IsTrue(entityList.ElementAt(i).EntityEqualsDto(dispenseDtos.ElementAt(i)));
            }
            Assert.IsTrue(entityList.EntityListEqualsDtoList(dispenseDtos));
            Assert.IsTrue(dispenseDtos.DtoListEqualsEntityList(entityList));
        }

        [TestMethod]
        public void TestCanConvertListOfDtos()
        {
            var entityList = DispenseTestEnvironment.GetTestDispenses();
            var dispenseDtos = entityList.ConvertToDtos();
            var convertedEntities = dispenseDtos.ConvertToEntites();
            for (var i = 0; i < convertedEntities.Count; i++)
            {
                Assert.IsTrue(convertedEntities.ElementAt(i).EntityEqualsDto(dispenseDtos.ElementAt(i)));
            }
            Assert.IsTrue(dispenseDtos.DtoListEqualsEntityList(convertedEntities.ToList()));
            Assert.IsTrue(convertedEntities.ToList().EntityListEqualsDtoList(dispenseDtos));
        }
    }
}
