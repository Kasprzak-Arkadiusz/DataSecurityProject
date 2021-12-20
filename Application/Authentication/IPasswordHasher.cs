using Application.Entities;

namespace Application.Authentication
{
    public interface IPasswordHasher
    {
        public byte[] HashPassword(string password);
        public bool VerifyHashedPassword(User user, byte[] hashedPassword, string providedPassword);
    }
}