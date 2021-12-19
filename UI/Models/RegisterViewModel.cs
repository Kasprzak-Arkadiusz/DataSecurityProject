using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("User name")]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$",
            ErrorMessage = "User name can only contain alphanumeric, '.' and '_' and also '.' and '_' cannot be next to each other" )]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //Min 8 characters, one upper case and one number
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$",
            ErrorMessage = "The master password must be at least 8 character long, contain one number and one upper case character")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Master password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$",
            ErrorMessage = "The master password must be at least 8 character long, contain one number and one upper case character")]
        public string MasterPassword { get; set; }

        [Required]
        [Display(Name = "Confirm master password")]
        [DataType(DataType.Password)]
        [Compare(nameof(MasterPassword), ErrorMessage = "The master password and master confirmation password do not match.")]
        public string ConfirmMasterPassword { get; set; }
    }
}