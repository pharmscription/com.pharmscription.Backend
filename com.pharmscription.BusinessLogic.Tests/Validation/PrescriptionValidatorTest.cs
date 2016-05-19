using System.Diagnostics.CodeAnalysis;
using com.pharmscription.BusinessLogic.Validation;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Tests.Validation
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PrescriptionValidatorTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ValidatorThrowsOnDefaultDateTime()
        {
            var prescriptionDto = new PrescriptionDto
            {
                Type = "N",
                ValidUntil = "01.01.0001"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(prescriptionDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ValidatorThrowsOnNull()
        {
            var prescriptionDto = new PrescriptionDto();
            var validator = new PrescriptionValidator();
            validator.Validate(prescriptionDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ValidatorThrowsOnUnparsableDate()
        {
            var prescriptionDto = new PrescriptionDto
            {
                Type = "S",
                ValidUntil = "sdjksadlksadf"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(prescriptionDto);
        }

        [TestMethod]
        public void ValidatorLetsCorrectDatePass()
        {
            var prescriptionDto = new PrescriptionDto
            {
                Type = "N",
                ValidUntil = "17.03.1991"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(prescriptionDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ValidatorThrowOnNotSupportedType()
        {
            var prescriptionDto = new PrescriptionDto
            {
                Type = "O",
                ValidUntil = "17.03.1991"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(prescriptionDto);
        }

        [TestMethod]
        public void ValidatorLetsCorrectTypePass()
        {
            var prescriptionDto = new PrescriptionDto
            {
                Type = "S",
                ValidUntil = "17.03.1991"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(prescriptionDto);
        }
    }
}
