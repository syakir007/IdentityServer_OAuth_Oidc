using System.Security.Claims;
using IdentityModel;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Api.Entities;
using Serilog;

namespace IdentityServerAspNetIdentity;

public class SeedData
{
    //try to seed role --- failed
    /*public static async Task Seed(IServiceProvider services)
    {
        await SeedRoles(services);
        await SeedStaff(services);
    }
*/

    /*private static async Task SeedStaff(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();

        var staffUser = await context.Users.FirstOrDefaultAsync(User => User.UserName == "StaffUser");

        if(staffUser != null)
        {
            staffUser = new ApplicationUser { UserName = "StaffUser", Email = "staff@email.com" };
            await userManager.CreateAsync(staffUser, "staffPassword!1");
            await userManager.AddToRoleAsync(staffUser, Role.Staff);
        }
    }*/

    /*private static async Task SeedRoles(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await roleManager.CreateAsync(new IdentityRole(Role.Staff));
        await roleManager.CreateAsync(new IdentityRole(Role.User));
    }*/

    public static async void EnsureSeedData(WebApplication app)
    {
        /*using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context2 = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context2.Database.Migrate();

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            roleMgr.CreateAsync(new IdentityRole(Role.Staff));
            roleMgr.CreateAsync(new IdentityRole(Role.User));
        }*/

        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            //context.Database.EnsureDeleted();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var alice = userMgr.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true,
                    
                };
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                await userMgr.AddToRoleAsync(alice,Role.Staff);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }

    }
}
