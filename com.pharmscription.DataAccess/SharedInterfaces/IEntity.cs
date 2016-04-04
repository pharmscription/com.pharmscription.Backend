using System;

namespace com.pharmscription.DataAccess.SharedInterfaces
{
    public interface IEntity
    {
        DateTime? CreatedDate { get; set; }
        Guid Id { get; set; }
        bool IsTransient();
        DateTime? ModifiedDate { get; set; }

    }

    public interface ICloneable<out T>
    {
        T Clone();
    }
}
