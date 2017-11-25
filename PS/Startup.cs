using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PS.Startup))]
namespace PS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
