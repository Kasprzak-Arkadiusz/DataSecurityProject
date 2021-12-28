using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class SecretViewModel
    {
        [DisplayName("Service name")]
        public string ServiceName { get; set; }

        [Required]
        [DisplayName("Master password")]
        [DataType(DataType.Password)]
        public string MasterPassword { get; set; }

        public string Password { get; set; }

        public int Id { get; set; }

        public SecretViewModel(string serviceName, int id)
        {
            ServiceName = serviceName;
            Password = "FakePassword";
            Id = id;
        }
    }
}