using Microsoft.AspNetCore.Identity;

namespace Revisao_ASP.NET_Web_API.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
