using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Context;
using Identity.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastucture;
using WebApi.Models.Identity;

namespace WebApi.Controllers
{

    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        // GET: api/User
        [HttpGet]
        [Authorize]
        public IEnumerable<User> Get()
        {
            return userManager.Users.ToList();
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await userManager.Users.FirstOrDefaultAsync(p => p.Id == id));
        }

        // POST: api/User
        [HttpPost]
        [CustomValidation]
        public async Task<IActionResult> Post([FromBody]RegisterUser value)
        {
            if (await userManager.FindByEmailAsync(value.Email) != null)
            {
                throw new ArgumentException("User  with such email is already registered");
            }

            if (await userManager.FindByNameAsync(value.UserName) != null)
            {
                throw new ArgumentException("User  with such username is already registered");
            }

            User user = new User()
            {
                Surname = value.Surname,
                FirstName = value.FirstName,
                Email = value.Email,
                PhoneNumber = value.PhoneNumber,
                UserName = value.UserName,
            };


            IdentityResult result = await userManager.CreateAsync(user, value.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, "User was registered successfully");

        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
                return Ok();

            if (!result.Succeeded)
            {
                if (result.Errors != null)

                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);


                if (ModelState.IsValid)
                    return BadRequest();

                return BadRequest(ModelState);
            }
            return null;
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            var appUser = await userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                throw new ArgumentException("Invalid Used Id");
            }

            IdentityResult result = await userManager.DeleteAsync(appUser);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return NoContent();
        }


        [Route("{id}/roles")]
        [HttpPut]
        public async Task<IActionResult> AssignRolesToUser(string id, [FromBody] List<string> rolesToAssign)
        {

            var appUser = await userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await userManager.GetRolesAsync(appUser);

            var rolesNotExists = rolesToAssign.Except(roleManager.Roles.Select(x => x.Name)).ToList();

            if (rolesNotExists.Count() > 0)
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await userManager.RemoveFromRolesAsync(appUser, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await userManager.AddToRolesAsync(appUser, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();

        }
    }
}
