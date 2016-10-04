using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Novo_ASP_MVC.Startup))]
namespace Novo_ASP_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
