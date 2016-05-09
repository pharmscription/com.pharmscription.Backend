using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.DrugEntity
{
    using System;
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
            return DrugDescription == other.DrugDescription && PackageSize == other.PackageSize && Unit == other.Unit
                   && Composition == other.Composition && NarcoticCategory == other.NarcoticCategory
                   && IsValid == other.IsValid;
        }
    }
}
