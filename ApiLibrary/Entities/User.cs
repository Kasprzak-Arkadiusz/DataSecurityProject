using System.Collections.Generic;

namespace ApiLibrary.Entities
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

        public User()
        { }

        public User(string userName, string email, byte[] password, byte[] masterPassword)
        {
            UserName = userName;
            Email = email;
            Password = password;
            MasterPassword = masterPassword;
            LoginFailure = new LoginFailure();
        }

        public bool IsLockedOut()
        {
            return LoginFailure is not null && LoginFailure.IsLockedOut();
        }
    }
}