using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;
using WebDemo.Models;
using System.Configuration;
using Stripe;
using Unity;


[assembly: OwinStartupAttribute(typeof(WebDemo.Startup))]
namespace WebDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // Truy cập giá trị khóa API từ cấu hình
            string stripeApiKey = ConfigurationManager.AppSettings["StripeApiKey"];

            // Đặt khóa API của Stripe
            StripeConfiguration.ApiKey = stripeApiKey;
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

                userManager.Create(newUser, "appadmin");
                userManager.AddToRole(newUser.Id, "Admin");
                
                db.SaveChanges();
            }
        }
    }
}
