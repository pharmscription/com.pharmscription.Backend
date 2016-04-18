namespace com.pharmscription.DataAccess.Tests.Entities
{
    using System.Collections.Generic;

    using com.pharmscription.DataAccess.Entities.DispenseEntity;
    using com.pharmscription.DataAccess.Entities.DrugItemEntity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    [TestClass]
    public class DispenseEntityTest
    {
        private Dispense _dispense;
        [TestInitialize]
        public void SetUp()
        {
            _dispense = new Dispense
                            {
                                DrugItem = new List<DrugItem> { new DrugItem(), new DrugItem()}
                            };
        }
        [TestMethod]
        public void TestClone()
        {
            var clone = _dispense.Clone();
            CollectionAssert.AreNotEquivalent(_dispense.DrugItem, clone.DrugItem);
        }
    }
}
