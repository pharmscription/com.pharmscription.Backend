using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Service.Startup))]
namespace Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
