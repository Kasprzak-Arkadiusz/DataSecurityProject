using System.Threading.Tasks;
using ApiLibrary.Entities;

namespace ApiLibrary.UserPasswordReset
{
    public interface ITokenProvider
    {
        public Task<string> GenerateAsync(User user);

        public bool Validate(User user, string token);
    }
}