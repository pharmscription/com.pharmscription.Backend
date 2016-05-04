using System.Diagnostics.CodeAnalysis;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(com.pharmscription.Service.Startup))]
namespace com.pharmscription.Service
{
    using com.pharmscription.BusinessLogic.Identity;

    [ExcludeFromCodeCoverage]
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
