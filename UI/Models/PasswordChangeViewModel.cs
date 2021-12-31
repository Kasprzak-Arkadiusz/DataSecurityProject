using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class PasswordChangeViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$",
            ErrorMessage = "The master password must be at least 8 character long, contain one number and one upper case character")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}