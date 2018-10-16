using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Roles;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/roles")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }


        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await roleManager.Roles.ToListAsync());
        }

        // GET: api/Role/5
        [HttpGet("{id}", Name = "GetRoleById")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await roleManager.Roles.FirstOrDefaultAsync(p => p.Id == id));
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateRole value)
        {

            if (await roleManager.FindByNameAsync(value.Name) != null)
            {
                throw new ArgumentException("Suck Role already exists");
            }


            var role = new IdentityRole { Name = value.Name };


            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, "Role was created successfully");
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

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {

            var roleToDelete = await roleManager.FindByIdAsync(id);
            if (roleToDelete == null)
            {
                throw new ArgumentException("Invalid Used Id");
            }

            IdentityResult result = await roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return NoContent();

        }
    }
}
