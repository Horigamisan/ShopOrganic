using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Linq;
using WebDemo.Models;

[assembly: OwinStartupAttribute(typeof(WebDemo.Startup))]
namespace WebDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            PopulateUserAndRoles();
        }

        public void PopulateUserAndRoles()
        {
            var db = new ApplicationDbContext();

            if (!db.Roles.Any(x => x.Name == "Admin")){
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Admin"));
                db.SaveChanges();
            }

            if (!db.Roles.Any(x => x.Name == "User")){
                db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("User"));
                db.SaveChanges();
            }

            if (!db.Users.Any(x => x.UserName == "admin@gmail.com")){

                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new ApplicationUserManager(userStore);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var newUser = new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };

                userManager.Create(newUser, ConfigurationManager.AppSettings["AdminPassword"]);
                userManager.AddToRole(newUser.Id, "Admin");
                
                db.SaveChanges();
            }
        }
    }
}
