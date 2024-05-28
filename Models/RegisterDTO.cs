using System.ComponentModel.DataAnnotations;

namespace DogRallyMVC.Models
{
    
        public class RegisterDTO
        {
        [Required(ErrorMessage = "En gyldig mailadresse er påkrævet")]
        [EmailAddress(ErrorMessage = "Ugyldig Email adresse")]
        [Display(Name = "Email")]
            public string Email { get; set; }

         [Required(ErrorMessage = "En gyldig adgangskode er påkrævet")]
        [StringLength(100, ErrorMessage = "{0} skal indeholde mindst {2} tegn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
            public string Password { get; set; }

       
    }
   
}