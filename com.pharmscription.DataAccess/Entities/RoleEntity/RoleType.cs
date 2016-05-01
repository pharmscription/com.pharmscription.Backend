namespace com.pharmscription.DataAccess.Entities.RoleEntity
{
    public static class RoleType
    {
        public static readonly string Doctor = "Doctor";

        public static readonly string Patient = "Patient";

        public static readonly string DrugStoreEmployee = "DrugStoreEmployee";

        public static readonly string Drugist = "Drugist";
        public static bool IsValid(string role)
        {
            return role.Equals(Doctor) || role.Equals(Patient) || role.Equals(DrugStoreEmployee) || role.Equals(Drugist);
        }
    }
}