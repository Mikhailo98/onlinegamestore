//using Identity.Context;
//using Identity.Entity;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Identity.Initialize
//{
//    public class DbInitializer : IDbInitializer
//    {
//        private readonly AppIdentityDbContext _context;
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<Role> _roleManager;

//        public DbInitializer(
//            AppIdentityDbContext context,
//            UserManager<User> userManager,
//            RoleManager<Role> roleManager)
//        {
//            _context = context;
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        public async void Initialize()
//        {

//                _context.Database.EnsureCreated();


//                var role1 = new Role() { Name = "Administrator" };
//                var role2 = new Role() { Name = "Manager" };
//                var role3 = new Role() { Name = "Publisher" };
//                var role4 = new Role() { Name = "Moderator" };
//                var role5 = new Role() { Name = "User" };
//                var role6 = new Role() { Name = "Guest" };

//                var admin = new User()
//                {
//                    UserName = "Adminische",
//                    Email = "admin@mail.com",
//                    SecurityStamp  = Guid.NewGuid().ToString()
//                };



//                await _roleManager.CreateAsync(role1);
//                await _roleManager.CreateAsync(role2);
//                await _roleManager.CreateAsync(role3);
//                await _roleManager.CreateAsync(role4);
//                await _roleManager.CreateAsync(role5);
//                await _roleManager.CreateAsync(role6);

//            await _userManager.AddToRoleAsync(admin, role1.Name);
//            await _userManager.CreateAsync(admin, "admin123");
            
//        }
//    }
//}
