using Application.Entities;

namespace Application.Authentication
{
    public interface IPasswordHasher
    {
        public byte[] HashPassword(string password);
        public bool VerifyHashedPassword(byte[] hashedPassword, string providedPassword);
    }
}