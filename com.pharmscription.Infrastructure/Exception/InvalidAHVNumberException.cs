namespace com.pharmscription.Infrastructure.Exception
{
    using System;

    [Serializable]
    public class InvalidAhvNumberException : ArgumentException
    {
        public InvalidAhvNumberException()
        {
        }

        public InvalidAhvNumberException(string message)
        : base(message)
        {
        }

        public InvalidAhvNumberException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
