using System;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Entities.AdressEntitiy.CityCodeEntity
{
    [TestClass]
    public class SwissCityCodeTest
    {
        [TestMethod]
        public void TestValidCityCode()
        {
            SwissCityCode.CreateInstance("7000");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTooLongCityCode()
        {
            SwissCityCode.CreateInstance("99090");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTooShortCityCode()
        {
            SwissCityCode.CreateInstance("123");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLetterInCityCode()
        {
            SwissCityCode.CreateInstance("12NW");
        }

    }
}
