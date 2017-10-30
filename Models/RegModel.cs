using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class RegModel : BaseEntity
    {
        [Required(ErrorMessage = "First Name is required")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="Non-Letter Characters are not permitted")]
        public string firstName {get; set;}
        [Required(ErrorMessage = "Last Name is required")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="Non-Letter Characters are not permitted")]
        public string lastName {get; set;}

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string email {get; set;}
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string password {get; set;}

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage="Password confirmation and password do not match.")]
        public string passwordConfirmation {get; set;}
    }
}