using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.Infrastructure.Exception
{
    using System.Runtime.Serialization;

    using Exception = System.Exception;

    public class NotFoundException: System.Exception
    {
        public NotFoundException()
        {
            
        }

        public NotFoundException(string message)
            : base(message)
        {
            
        }

        public NotFoundException(string message, Exception innerException):
            base(message, innerException)
        {
            
        }

        public NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
