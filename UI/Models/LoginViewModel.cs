using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class LoginViewModel
    {
        [Required]
        [RegularExpression(@"^(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$",
            ErrorMessage = "User name can only contain alphanumeric, '.' and '_' and also '.' and '_' cannot be next to each other")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}