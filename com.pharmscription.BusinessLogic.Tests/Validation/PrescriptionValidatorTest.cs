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
            var patientDto = new PrescriptionDto
            {
                ValidUntil = "01.01.0001"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(patientDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ValidatorThrowsOnNull()
        {
            var patientDto = new PrescriptionDto();
            var validator = new PrescriptionValidator();
            validator.Validate(patientDto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ValidatorThrowsOnUnparsableDate()
        {
            var patientDto = new PrescriptionDto
            {
                ValidUntil = "sdjksadlksadf"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(patientDto);
        }

        [TestMethod]
        public void ValidatorLetsCorrectDatePass()
        {
            var patientDto = new PrescriptionDto
            {
                ValidUntil = "17.03.1991"
            };
            var validator = new PrescriptionValidator();
            validator.Validate(patientDto);
        }
    }
}
