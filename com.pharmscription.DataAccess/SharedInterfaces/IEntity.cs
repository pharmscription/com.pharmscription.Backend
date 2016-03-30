using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.Infrastructure.SharedInterfaces
{
    public interface IEntity
    {
        DateTime? CreatedDate { get; set; }
        Guid Id { get; set; }
        bool IsTransient();
        DateTime? ModifiedDate { get; set; }
    }
}
