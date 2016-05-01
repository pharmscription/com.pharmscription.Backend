namespace com.pharmscription.BusinessLogic.Communication
{
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    public class EMailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.FromResult(0);
        }
    }
}