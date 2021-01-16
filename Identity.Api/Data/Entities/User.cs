using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public string Surname { get; set; }
    }
}