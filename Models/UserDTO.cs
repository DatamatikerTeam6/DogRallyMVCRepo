using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DogRallyMVC.Models
{
    public class UserDTO : IdentityUser
    {
        
        public string UserEmail { get; set; }

        public string Password { get; set; }
    }
}
