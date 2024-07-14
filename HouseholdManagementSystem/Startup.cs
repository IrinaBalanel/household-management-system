using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseholdManagementSystem.Startup))]
namespace HouseholdManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
