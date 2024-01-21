using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRMAdmin.Startup))]
namespace CRMAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
