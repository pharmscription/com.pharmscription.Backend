using System.Collections.Generic;
using com.pharmscription.DataAccess.Entities.DispenseEntity;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Entities.DispenseEntity
{
    [TestClass]
    public class DispenseEntityTest
    {
        private Dispense _dispense;
        [TestInitialize]
        public void SetUp()
        {
            _dispense = new Dispense
                            {
                                DrugItems = new List<DrugItem> { new DrugItem(), new DrugItem()}
                            };
        }
        [TestMethod]
        public void TestClone()
        {
            var clone = _dispense.Clone();
            CollectionAssert.AreNotEquivalent(_dispense.DrugItems, clone.DrugItems);
        }
    }
}
