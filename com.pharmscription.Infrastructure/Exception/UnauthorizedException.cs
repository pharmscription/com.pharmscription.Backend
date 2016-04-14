namespace com.pharmscription.Infrastructure.Exception
{
    public class UnauthorizedException : System.Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message)
        : base(message)
        {
        }

        public UnauthorizedException(string message, System.Exception inner)
        : base(message, inner)
        {
        }
    }
}
