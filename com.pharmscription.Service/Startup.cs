using System.Diagnostics.CodeAnalysis;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Service.Startup))]
namespace Service
{
    [ExcludeFromCodeCoverage]
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
