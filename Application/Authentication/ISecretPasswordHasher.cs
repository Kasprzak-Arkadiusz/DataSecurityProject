namespace Application.Authentication
{
    public interface ISecretPasswordHasher
    {
        public string EncryptPassword(string password);
        public string DecryptPassword(string password);
    }
}