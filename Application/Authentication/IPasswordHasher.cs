using Application.Entities;

namespace Application.Authentication
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    }
}