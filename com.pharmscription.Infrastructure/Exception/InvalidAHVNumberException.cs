namespace com.pharmscription.Infrastructure.Exception
{
    public class InvalidAhvNumberException : System.Exception
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
