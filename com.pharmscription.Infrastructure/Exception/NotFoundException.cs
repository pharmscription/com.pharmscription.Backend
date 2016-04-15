namespace com.pharmscription.Infrastructure.Exception
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotFoundException : Exception
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
