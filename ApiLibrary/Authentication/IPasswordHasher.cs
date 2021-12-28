namespace ApiLibrary.Authentication
{
    public interface IPasswordHasher
    {
        public byte[] HashPassword(string password);

        public bool VerifyHashedPassword(byte[] hashedPassword, string providedPassword);
    }
}