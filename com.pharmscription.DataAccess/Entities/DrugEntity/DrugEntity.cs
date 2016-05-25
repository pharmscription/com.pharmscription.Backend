using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.DrugEntity
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class Drug: Entity, ICloneable<Drug>, IEquatable<Drug>
    {
        public string DrugDescription { get; set; }
        public string PackageSize { get; set; }
        public string Unit { get; set; }
        public string Composition { get; set; }
        public string NarcoticCategory { get; set; }
        public bool IsValid { get; set; }
        public Drug Clone()
        {
            return new Drug
            {
                DrugDescription = DrugDescription,
                PackageSize = PackageSize,
                Unit = Unit,
                Composition = Composition,
                NarcoticCategory = NarcoticCategory,
                IsValid = IsValid
            };
        }

        public bool Equals(Drug other)
        {
            if (other == null)
            {
                return false;
            }
            return Id.Equals(other.Id) && DrugDescription == other.DrugDescription && PackageSize == other.PackageSize && Unit == other.Unit
                   && Composition == other.Composition && NarcoticCategory == other.NarcoticCategory
                   && IsValid == other.IsValid;
        }

        public override bool Equals(object obj)
        {
            bool isEqual = base.Equals(obj);
            if (obj is Drug)
            {
                isEqual = isEqual && Equals(obj as Drug);
            }
            return isEqual;
        }

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "There is no precedence")]
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + DrugDescription.GetHashCode();
                hash = hash * 23 + PackageSize.GetHashCode();
                hash = hash * 23 + Unit.GetHashCode();
                hash = hash * 23 + Composition.GetHashCode();
                hash = hash * 23 + NarcoticCategory.GetHashCode();
                hash = hash * 23 + IsValid.GetHashCode();
                return hash;
            }
        }
    }
}
