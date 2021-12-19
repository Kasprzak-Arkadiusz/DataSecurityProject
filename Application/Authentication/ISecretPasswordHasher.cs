namespace Application.Authentication
{
    public interface ISecretPasswordHasher
    {
        public (byte[] encryptedPassword, byte[] iv) EncryptPassword(string password);
        public string DecryptPassword(byte[] encryptedPassword, byte[] iv);
    }
}