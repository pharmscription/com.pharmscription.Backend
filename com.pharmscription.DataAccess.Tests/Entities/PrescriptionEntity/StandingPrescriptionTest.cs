namespace com.pharmscription.DataAccess.Tests.Entities.PrescriptionEntity
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using com.pharmscription.DataAccess.Entities.CounterProposalEntity;
    using com.pharmscription.DataAccess.Entities.DispenseEntity;
    using com.pharmscription.DataAccess.Entities.DrugItemEntity;
    using com.pharmscription.DataAccess.Entities.PrescriptionEntity;

    using Moq;

    [TestClass]
    public class StandingPrescriptionTest
    {
        private Prescription _prescription;

        [TestInitialize]
        public void SetUp()
        {
            Dispense dispense = new Dispense
                                    {
                                        DrugItem = new List<DrugItem> { new DrugItem() }
                                    };
            _prescription = new StandingPrescription
                                {
                                    CounterProposal = new List<CounterProposal> { new CounterProposal(), new CounterProposal() },
                                    Dispense = new List<Dispense> { dispense.Clone(), dispense.Clone() },
                                    Drug = new List<DrugItem> { new DrugItem(), new DrugItem() },
                                    PrescriptionHistory = new List<Prescription>()
                                };
        }
        [TestMethod]
        public void TestClone()
        {
            var clone = _prescription.Clone();
            CollectionAssert.AreNotEquivalent(_prescription.CounterProposal, clone.CounterProposal, "Counter proposal list is equivalent to original");
            CollectionAssert.AreNotEquivalent(_prescription.Dispense, clone.Dispense, "Dispnese list is equivalent to original");
            CollectionAssert.AreNotEquivalent(_prescription.Drug, clone.Drug, "DrugItem list is equivalent to original");
            //// Hier wird nicht `CollectionAssert` verwendet, da es sich um eine leere Liste handelt, und leere Konstruktoren nicht getestet werden
            Assert.AreNotEqual(_prescription.PrescriptionHistory, clone.PrescriptionHistory, "Prescription history is equivalent to original");
        }
    }
}
