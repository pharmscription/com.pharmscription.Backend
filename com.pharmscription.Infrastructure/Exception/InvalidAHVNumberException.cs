namespace com.pharmscription.Infrastructure.Exception
{
    using System;
    using Exception = System.Exception;

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

        public InvalidAhvNumberException(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}
