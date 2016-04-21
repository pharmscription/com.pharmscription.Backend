using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(com.pharmscription.ServcieReal.Startup))]
namespace com.pharmscription.ServcieReal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
