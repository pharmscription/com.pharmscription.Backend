using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Insurance.Tests
{
    [TestClass]
    public class InsurancePatientTest
    {
        private IInsurance insurance;

        [TestInitialize]
        public void TestInitialize()
        {
            insurance = new Insurance(new PatientStoreTest());
        }

        [TestMethod]
        public void FindPatient()
        {
            var patient = insurance.FindPatient("123-1234-1234-12");
            Assert.IsNotNull(patient);
        }

        [TestMethod]
        public void FindNoPatient()
        {
            var patient = insurance.FindPatient("de oli isch de chef");
            Assert.IsNull(patient);
        }
    }
}
