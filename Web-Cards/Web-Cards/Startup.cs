using Web_Cards.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_Cards.Startup))]
namespace Web_Cards
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Get Membership Context
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                if (!roleManager.RoleExists("Player"))
                {
                    role = new IdentityRole();
                    role.Name = "Player";
                    roleManager.Create(role);
                }

                var user = new ApplicationUser();
                user.UserName = "admin@test.com";
                user.Email = "admin@test.com";

                string userPWD = "drowssap";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin 
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
