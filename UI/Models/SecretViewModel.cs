using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class SecretViewModel
    {
        public string ServiceName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MasterPassword { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}