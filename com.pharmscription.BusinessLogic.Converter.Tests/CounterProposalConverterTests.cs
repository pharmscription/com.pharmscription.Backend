using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Tests.TestEnvironment;
    using Infrastructure.Constants;
    using Infrastructure.Dto;
    using Infrastructure.EntityHelper;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CounterProposalConverterTests
    {
        [TestMethod]
        public void TestCanConvertSingleEntityToDto()
        {
            var entity = new CounterProposal
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Date = DateTime.Now,
                Message = "Did a small adjustment"
            };
            var dtoFromEntity = entity.ConvertToDto();
            Assert.IsTrue(dtoFromEntity.DtoEqualsEntity(entity));
        }

        [TestMethod]
        public void TestCanConvertNormalDtoToEntity()
        {
            var dto = new CounterProposalDto
            {
                Id = new Guid().ToString(),
                Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Message = "Did a small change"
            };
            var entityFromDto = dto.ConvertToEntity();
            Assert.IsTrue(entityFromDto.EntityEqualsDto(dto));
        }

        [TestMethod]
        public void TestConvertsNullFromNullDto()
        {
            CounterProposalDto dto = null;
            Assert.IsNull(dto.ConvertToEntity());
        }

        [TestMethod]
        public void TestConvertsNullFromNullEntity()
        {
            CounterProposal counterProposal = null;
            Assert.IsNull(counterProposal.ConvertToDto());
        }

        [TestMethod]
        public void TestCanConvertListOfEntity()
        {

            var entityList = CounterProposalTestEnvironment.GetTestCounterProposals();
            var counterProposalDtos = entityList.ConvertToDtos();
            for (var i = 0; i < counterProposalDtos.Count; i++)
            {
                Assert.IsTrue(entityList.ElementAt(i).EntityEqualsDto(counterProposalDtos.ElementAt(i)));
            }
            Assert.IsTrue(entityList.EntityListEqualsDtoList(counterProposalDtos));
            Assert.IsTrue(counterProposalDtos.DtoListEqualsEntityList(entityList));
        }

        [TestMethod]
        public void TestCanConvertListOfDtos()
        {
            var entityList = CounterProposalTestEnvironment.GetTestCounterProposals();
            var counterProposalDtos = entityList.ConvertToDtos();
            var convertedEntities = counterProposalDtos.ConvertToEntities();
            for (var i = 0; i < convertedEntities.Count; i++)
            {
                Assert.IsTrue(convertedEntities.ElementAt(i).EntityEqualsDto(counterProposalDtos.ElementAt(i)));
            }
            Assert.IsTrue(counterProposalDtos.DtoListEqualsEntityList(convertedEntities.ToList()));
            Assert.IsTrue(convertedEntities.ToList().EntityListEqualsDtoList(counterProposalDtos));
        }
    }
}
