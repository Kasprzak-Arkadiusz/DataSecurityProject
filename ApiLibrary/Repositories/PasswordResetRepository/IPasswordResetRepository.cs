using System.Threading.Tasks;
using ApiLibrary.Entities;

namespace ApiLibrary.Repositories.PasswordResetRepository
{
    public interface IPasswordResetRepository
    {
        public Task<PasswordReset> GetPasswordResetById(int id);
        public Task CreatePasswordReset(PasswordReset passwordReset);
        public Task DeleteByUserId(int userId);
    }
}