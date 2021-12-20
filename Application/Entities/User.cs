using System.Collections.Generic;

namespace Application.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] MasterPassword { get; set; }

        public PasswordReset PasswordReset { get; set; }
        public LoginFailure LoginFailure { get; set; }
        public ICollection<LastConnection> LastConnections { get; set; }
        public ICollection<Secret> Secrets { get; set; }
    }
}