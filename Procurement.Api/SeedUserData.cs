using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Procurement.Api.Data;
using Procurement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Api
{
    public class SeedUserData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<AppDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var signinMgr = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
                    if (!userMgr.Users.Any())
                    {
                        var roles = new Role[]
                        {
                            new Role(){ Name ="User", NormalizedName="USER"},
                            new Role(){ Name ="Admin", NormalizedName="ADMIN"},
                            new Role(){ Name ="Vendor", NormalizedName="VENDOR"},
                            new Role(){ Name ="Approver", NormalizedName="APPROVER"},
                            new Role(){ Name ="Authoriser", NormalizedName="AUTHORISER"},
                        };

                        context.Roles.AddRange(roles);
                        var result = context.SaveChangesAsync().Result;
                        var user = new User
                        {
                            UserName = "user@procurement.com",
                            Email = "user@procurement.com"
                        };
                         var addUserResult = userMgr.CreateAsync(user, "Pass123$").Result;
                        var addRoleResult = userMgr.AddToRoleAsync(user, "User").Result;

                        var admin = new User
                        {
                            UserName = "admin@procurement.com",
                            Email= "admin@procurement.com"
                        };
                        addUserResult = userMgr.CreateAsync(admin, "Pass123$").Result;
                        addRoleResult = userMgr.AddToRoleAsync(admin, "Admin").Result;

                        var vendor = new User
                        {
                            UserName = "vendor@procurement.com",
                            Email = "vendor@procurement.com",
                            CompanyName = "Vendor 1"
                        };
                        addUserResult = userMgr.CreateAsync(vendor, "Pass123$").Result;
                        addRoleResult = userMgr.AddToRoleAsync(vendor, "Vendor").Result;

                        var approver = new User
                        {
                            UserName = "approver@procurement.com",
                            Email = "approver@procurement.com"
                        };
                        addUserResult = userMgr.CreateAsync(approver, "Pass123$").Result;
                        addRoleResult = userMgr.AddToRoleAsync(approver, "Approver").Result;

                        var authoriser = new User
                        {
                            UserName = "authoriser@procurement.com",
                            Email = "authoriser@procurement.com"
                        };
                        addUserResult = userMgr.CreateAsync(authoriser, "Pass123$").Result;
                        addRoleResult = userMgr.AddToRoleAsync(authoriser, "Authoriser").Result;
                    }
                }
            }
        }
    }
}
