using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;
using WebInvoiceTokioMarine.Models;

[assembly: OwinStartupAttribute(typeof(WebInvoiceTokioMarine.Startup))]
namespace WebInvoiceTokioMarine
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CreateDefaultRolesAndUsers();
            ConfigureAuth(app);
        }

        public void CreateDefaultRolesAndUsers()
        {
            var _db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            IdentityRole supplierRole = new IdentityRole();
            Supplier supplier = new Supplier();
            if (!_db.Users.Any(a => a.UserName == "supplier"))
            {
                supplier.UserName = "supplier";
                supplier.LockoutEnabled = true;
                supplier.Email = "supplier@gmail.com";
                var Check = userManager.Create(supplier, "123456");
                if (!roleManager.RoleExists("Supplier"))
                {
                    supplierRole.Name = "Supplier";
                    roleManager.Create(supplierRole);

                    if (Check.Succeeded)
                    {
                        userManager.AddToRole(supplier.Id, "Supplier");
                    }
                }
            }
           
            IdentityRole customerRole = new IdentityRole();
            Customer customer = new Customer();
            if (!_db.Users.Any(a => a.UserName == "customer"))
            {
                customer.UserName = "customer";
                customer.LockoutEnabled = true;
                customer.Email = "customer@gmail.com";
                var Check = userManager.Create(customer, "123456");
                if (!roleManager.RoleExists("Customer"))
                {
                    customerRole.Name = "Customer";
                    roleManager.Create(customerRole);
                }
                if (Check.Succeeded)
                {
                    userManager.AddToRole(customer.Id, "Customer");
                }
            }

        }

    }
}
