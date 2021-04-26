using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainerClientADOnet.Startup))]
namespace TrainerClientADOnet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(IdentityModels.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            //app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            ConfigureAuth(app);

        }
    }
}
