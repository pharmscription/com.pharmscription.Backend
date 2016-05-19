using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Converter.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.PatientEntity;
    using DataAccess.Entities.PrescriptionEntity;
    using DataAccess.Tests.TestEnvironment;
    using Infrastructure.Constants;
    using Infrastructure.Dto;
    using Infrastructure.EntityHelper;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PrescriptionConverterTests
    {
        [TestMethod]
        public void TestCanConvertSingleEntityToDto()
        {
            var entity = new SinglePrescription
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                IsValid = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                EditDate = DateTime.Now,
                IssueDate = DateTime.Now,
                Patient = new Patient
                {
                    AhvNumber = "123",
                    BirthDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    EMailAddress = "Rafael_krucker@hotmail.com",
                    LastName = "Krucker",
                    FirstName = "Rafael",
                    Id = IdentityGenerator.NewSequentialGuid(),
                    InsuranceNumber = "1234",
                    Insurance = "Helsana",
                    ModifiedDate = DateTime.Now,
                    PhoneNumber = "123456789",
                    Address = new Address
                    {
                        Id = IdentityGenerator.NewSequentialGuid(),
                        ModifiedDate = DateTime.Now,
                        Location = "Wil",
                        CreatedDate = DateTime.Now,
                        Number = "123",
                        State = "Thurgau",
                        Street = "Steigstrasse",
                        StreetExtension = "2a",
                        CityCode = SwissCityCode.CreateInstance("1234")
                    }
                },
                CounterProposals = new List<CounterProposal>
                {
                    new CounterProposal
                    {
                        Message = "Hello",
                        Date = DateTime.Now,
                        Id = IdentityGenerator.NewSequentialGuid(),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }
                },
                Dispenses = new List<Dispense>
                {
                    new Dispense
                    {
                        Date = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        Remark = "Did a dispense",
                        Id = IdentityGenerator.NewSequentialGuid(),
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
                    }
                },
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
        public void TestCanConvertStandingEntityToDto()
        {
            var entity = new StandingPrescription
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                IsValid = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                EditDate = DateTime.Now,
                IssueDate = DateTime.Now,
                ValidUntil = DateTime.Now,
                Patient = new Patient
                {
                    AhvNumber = "123",
                    BirthDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    EMailAddress = "Rafael_krucker@hotmail.com",
                    LastName = "Krucker",
                    FirstName = "Rafael",
                    Id = IdentityGenerator.NewSequentialGuid(),
                    InsuranceNumber = "1234",
                    Insurance = "Helsana",
                    ModifiedDate = DateTime.Now,
                    PhoneNumber = "123456789",
                    Address = new Address
                    {
                        Id = IdentityGenerator.NewSequentialGuid(),
                        ModifiedDate = DateTime.Now,
                        Location = "Wil",
                        CreatedDate = DateTime.Now,
                        Number = "123",
                        State = "Thurgau",
                        Street = "Steigstrasse",
                        StreetExtension = "2a",
                        CityCode = SwissCityCode.CreateInstance("1234")
                    }
                },
                CounterProposals = new List<CounterProposal>
                {
                    new CounterProposal
                    {
                        Message = "Hello",
                        Date = DateTime.Now,
                        Id = IdentityGenerator.NewSequentialGuid(),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }
                },
                Dispenses = new List<Dispense>
                {
                    new Dispense
                    {
                        Date = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        Remark = "Did a dispense",
                        Id = IdentityGenerator.NewSequentialGuid(),
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
                    }
                },
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
            var dto = new PrescriptionDto
            {
                Id = new Guid().ToString(),
                IsValid = true,
                EditDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IssueDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Type = "N",
                Patient = new PatientDto
                {
                    AhvNumber = "123",
                    BirthDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                    EMailAddress = "Rafael_krucker@hotmail.com",
                    LastName = "Krucker",
                    FirstName = "Rafael",
                    Id = IdentityGenerator.NewSequentialGuid().ToString(),
                    InsuranceNumber = "1234",
                    Insurance = "Helsana",
                    PhoneNumber = "123456789",
                    Address = new AddressDto
                    {
                        Id = IdentityGenerator.NewSequentialGuid().ToString(),
                        Location = "Wil",
                        Number = "123",
                        State = "Thurgau",
                        Street = "Steigstrasse",
                        StreetExtension = "2a",
                        CityCode = SwissCityCode.CreateInstance("1234").ToString()
                    }
                },
                CounterProposals = new List<CounterProposalDto>
                {
                    new CounterProposalDto
                    {
                        Message = "Hello",
                        Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                        Id = IdentityGenerator.NewSequentialGuid().ToString()
                    }
                },
                Dispenses = new List<DispenseDto>
                {
                    new DispenseDto
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
                    }
                },
                Drugs = new List<DrugItemDto>
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
        public void TestCanConvertStandingDtoToEntity()
        {
            var dto = new PrescriptionDto
            {
                Id = new Guid().ToString(),
                IsValid = true,
                EditDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                IssueDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Type = "S",
                ValidUntil = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                Patient = new PatientDto
                {
                    AhvNumber = "123",
                    BirthDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                    EMailAddress = "Rafael_krucker@hotmail.com",
                    LastName = "Krucker",
                    FirstName = "Rafael",
                    Id = IdentityGenerator.NewSequentialGuid().ToString(),
                    InsuranceNumber = "1234",
                    Insurance = "Helsana",
                    PhoneNumber = "123456789",
                    Address = new AddressDto
                    {
                        Id = IdentityGenerator.NewSequentialGuid().ToString(),
                        Location = "Wil",
                        Number = "123",
                        State = "Thurgau",
                        Street = "Steigstrasse",
                        StreetExtension = "2a",
                        CityCode = SwissCityCode.CreateInstance("1234").ToString()
                    }
                },
                CounterProposals = new List<CounterProposalDto>
                {
                    new CounterProposalDto
                    {
                        Message = "Hello",
                        Date = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
                        Id = IdentityGenerator.NewSequentialGuid().ToString()
                    }
                },
                Dispenses = new List<DispenseDto>
                {
                    new DispenseDto
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
                    }
                },
                Drugs = new List<DrugItemDto>
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
            PrescriptionDto dto = null;
            Assert.IsNull(dto.ConvertToEntity());
        }

        [TestMethod]
        public void TestConvertsNullFromNullEntity()
        {
            Prescription prescription = null;
            Assert.IsNull(prescription.ConvertToDto());
        }

        [TestMethod]
        public void TestCanConvertListOfEntity()
        {

            var entityList = PrescriptionTestEnvironment.GetTestPrescriptions(); 
            var prescriptionDtos = entityList.ConvertToDtos();
            for (var i = 0; i < prescriptionDtos.Count; i++)
            {
                Assert.IsTrue(entityList.ElementAt(i).EntityEqualsDto(prescriptionDtos.ElementAt(i)));
            }
            Assert.IsTrue(entityList.EntityListEqualsDtoList(prescriptionDtos));
            Assert.IsTrue(prescriptionDtos.DtoListEqualsEntityList(entityList));
        }

        [TestMethod]
        public void TestCanConvertListOfDtos()
        {
            var entityList = PrescriptionTestEnvironment.GetTestPrescriptions();
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
