using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Insurance.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class InsurancePatientTest
    {
        private IInsurance _insurance;

        [TestInitialize]
        public void TestInitialize()
        {
            _insurance = new Insurance(new PatientStoreTest());
        }

        [TestMethod]
        public async Task FindPatient()
        {
            var patient = await _insurance.FindPatient("123-1234-1234-12");
            Assert.IsNotNull(patient);
        }

        [TestMethod]
        public async Task FindNoPatient()
        {
            var patient = await _insurance.FindPatient("de oli isch de chef");
            Assert.IsNull(patient);
        }
    }
}
