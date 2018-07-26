using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iotwebt2.Startup))]
namespace iotwebt2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
