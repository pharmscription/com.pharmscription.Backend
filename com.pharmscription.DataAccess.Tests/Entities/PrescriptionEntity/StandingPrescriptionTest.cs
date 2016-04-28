using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace com.pharmscription.DataAccess.Tests.Entities.PrescriptionEntity
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.PrescriptionEntity;


    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StandingPrescriptionTest
    {
        private Prescription _prescription;

        [TestInitialize]
        public void SetUp()
        {
            Dispense dispense = new Dispense
                                    {
                                        DrugItems = new List<DrugItem> { new DrugItem() }
                                    };
            _prescription = new StandingPrescription
                                {
                                    CounterProposals = new List<CounterProposal> { new CounterProposal(), new CounterProposal() },
                                    Dispenses = new List<Dispense> { dispense.Clone(), dispense.Clone() },
                                    DrugItems = new List<DrugItem> { new DrugItem(), new DrugItem() },
                                    PrescriptionHistory = new List<Prescription>()
                                };
        }
        [TestMethod]
        public void TestClone()
        {
            var clone = _prescription.Clone();
            CollectionAssert.AreNotEquivalent(_prescription.CounterProposals.ToList(), clone.CounterProposals.ToList(), "Counter proposal list is equivalent to original");
            CollectionAssert.AreNotEquivalent(_prescription.Dispenses.ToList(), clone.Dispenses.ToList(), "Dispnese list is equivalent to original");
            CollectionAssert.AreNotEquivalent(_prescription.DrugItems.ToList(), clone.DrugItems.ToList(), "DrugItem list is equivalent to original");
            //// Hier wird nicht `CollectionAssert` verwendet, da es sich um eine leere Liste handelt, und leere Konstruktoren nicht getestet werden
            Assert.AreNotEqual(_prescription.PrescriptionHistory, clone.PrescriptionHistory, "Prescription history is equivalent to original");
        }
    }
}
