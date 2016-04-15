namespace com.pharmscription.Infrastructure.Exception
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidArgumentException : ArgumentException
    {
        public InvalidArgumentException()
        {
            
        }

        public InvalidArgumentException(string message)
            : base(message)
        {
            
        }

        public InvalidArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public InvalidArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
