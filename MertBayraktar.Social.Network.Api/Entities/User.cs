using Microsoft.AspNetCore.Identity;

namespace MertBayraktar.Social.Network.Api.Entities
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }


    }
}
