using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Context;
using Identity.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Seed")]
    public class SeedController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public SeedController(
           UserManager<User> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Initialize()
        {
            var role1 = new IdentityRole() { Name = "Admin" };
            var role2 = new IdentityRole() { Name = "Manager" };
            var role3 = new IdentityRole() { Name = "Publisher" };
            var role4 = new IdentityRole() { Name = "Moderator" };
            var role5 = new IdentityRole() { Name = "User" };
            var role6 = new IdentityRole() { Name = "Guest" };

            var admin = new User()
            {
                UserName = "Adminische",
                Email = "admin@mail.com",
                SecurityStamp = Guid.NewGuid().ToString(),

            };

            var r = await userManager.CreateAsync(admin, "Admin123$");

            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role2);
            await roleManager.CreateAsync(role3);
            await roleManager.CreateAsync(role4);
            await roleManager.CreateAsync(role5);
            await roleManager.CreateAsync(role6);

            var name = await roleManager.FindByNameAsync("Admin");

            var result = await userManager.AddToRoleAsync(admin, name.Name);
                       
            return Ok();

        }
    }

}
