namespace com.pharmscription.DataAccess.Entities.BaseUser
{
    using com.pharmscription.DataAccess.Entities.BaseEntity;

    public class BaseUser : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
