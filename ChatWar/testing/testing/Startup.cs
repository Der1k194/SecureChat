using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(testing.Startup))]
namespace testing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
