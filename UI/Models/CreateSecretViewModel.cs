using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UI.Models
{
    public class CreateSecretViewModel
    {
        [DisplayName("Service name")]
        public string ServiceName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
