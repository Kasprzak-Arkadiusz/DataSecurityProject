using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class PasswordResetViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
    }
}
