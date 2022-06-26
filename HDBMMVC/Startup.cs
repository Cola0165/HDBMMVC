using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HDBMMVC.Startup))]
namespace HDBMMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
