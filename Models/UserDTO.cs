using Microsoft.AspNetCore.Identity;

namespace DogRallyMVC.Models
{
    public class UserDTO : IdentityUser
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
    }
}
