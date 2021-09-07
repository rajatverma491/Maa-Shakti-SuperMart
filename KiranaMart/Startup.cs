using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KiranaMart.Startup))]
namespace KiranaMart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
