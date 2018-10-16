using Identity.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models.Identity;

namespace GameStore.NetCore.Api.Controllers
{
    //remove this shit
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly UserManager<User> _userManager;
        private IConfiguration _config;

        public TokenController(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        //Endpoint is used to generate a bearer token
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = await Authenticate(login);

            if (user != null)
            {
                var tokenString = await BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private async Task<string> BuildToken(User user)
        {
            //To simplify the example, let's assume 1-1 user-role relation
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim("role", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> Authenticate(LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
                return null;
            return user;
        }
    }
}
