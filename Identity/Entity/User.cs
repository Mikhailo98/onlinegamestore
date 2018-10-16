using Microsoft.AspNetCore.Identity;

namespace Identity.Entity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
