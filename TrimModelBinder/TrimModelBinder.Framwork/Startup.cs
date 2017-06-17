using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrimModelBinder.Framwork.Startup))]
namespace TrimModelBinder.Framwork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
