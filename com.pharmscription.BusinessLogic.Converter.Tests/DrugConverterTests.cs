using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.Infrastructure.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DrugConverterTests
    {
        [TestMethod]
        public void TestCanConvertEntityToDto()
        {
            var entity = new Drug
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
            Assert.IsTrue(dtoFromEntity.DtoEqualsEntity(entity));
        }

        [TestMethod]
        public void TestCanConvertDtoToEntity()
        {
            var dto = new DrugDto
            {
                Id = new Guid().ToString(),
                DrugDescription = "Hallo Meine Droge",
                IsValid = true,
                NarcoticCategory = "A",
                PackageSize = "Big",
                Unit = "2",
                Composition = "HD"
            };
            var entityFromDto = dto.ConvertToEntity();
            Assert.IsTrue(entityFromDto.EntityEqualsDto(dto));
        }

        [TestMethod]
        public void TestConvertsNullFromNullDto()
        {
            DrugDto dto = null;
            Assert.IsNull(dto.ConvertToEntity());
        }

        [TestMethod]
        public void TestConvertsNullFromNullEntity()
        {
            Drug drug = null;
            Assert.IsNull(drug.ConvertToDto());
        }

        [TestMethod]
        public void TestCanConvertListOfEntity()
        {
            var entityList = new List<Drug>
            {
                new Drug { 
                Id = new Guid(),
                DrugDescription = "Hallo Meine Droge",
                IsValid = true,
                NarcoticCategory = "A",
                PackageSize = "Big",
                Unit = "2",
                Composition = "HD"
                },
                new Drug {
                Id = new Guid(),
                DrugDescription = "pksdkosde",
                IsValid = false,
                NarcoticCategory = "C",
                PackageSize = "Small",
                Unit = "1",
                Composition = "HD"
                },
                new Drug {
                Id = new Guid(),
                DrugDescription = "Aspi",
                IsValid = false,
                NarcoticCategory = "D",
                PackageSize = "Ol",
                Unit = "5",
                Composition = "UQ"
                }
            };
            var drugDtoList = entityList.ConvertToDtos();
            for (var i = 0; i < drugDtoList.Count; i++)
            {
                Assert.IsTrue(entityList.ElementAt(i).EntityEqualsDto(drugDtoList.ElementAt(i)));
            }
        }

        [TestMethod]
        public void TestCanConvertListOfDtos()
        {
            var dtoList = new List<DrugDto>
            {
                new DrugDto
                {
                    Id = new Guid().ToString(),
                    DrugDescription = "Hallo Meine Droge",
                    IsValid = true,
                    NarcoticCategory = "A",
                    PackageSize = "Big",
                    Unit = "2",
                    Composition = "HD"
                },
                new DrugDto
                {
                    Id = new Guid().ToString(),
                    DrugDescription = "pksdkosde",
                    IsValid = false,
                    NarcoticCategory = "C",
                    PackageSize = "Small",
                    Unit = "1",
                    Composition = "HD"
                },
                new DrugDto
                {
                    Id = new Guid().ToString(),
                    DrugDescription = "Aspi",
                    IsValid = false,
                    NarcoticCategory = "D",
                    PackageSize = "Ol",
                    Unit = "5",
                    Composition = "UQ"
                }
            };
            var entitylist = dtoList.ConvertToEntities();
            for (var i = 0; i < entitylist.Count; i++)
            {
                Assert.IsTrue(entitylist.ElementAt(i).EntityEqualsDto(dtoList.ElementAt(i)));
            }
        }

    }
}
