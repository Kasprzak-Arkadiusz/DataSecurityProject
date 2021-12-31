using System.Threading.Tasks;
using ApiLibrary.Entities;

namespace ApiLibrary.Repositories.PasswordResetRepository
{
    public interface IPasswordResetRepository
    {
        public Task<PasswordReset> GetByUserId(int userId);
        public Task CreatePasswordReset(PasswordReset passwordReset);
        public Task DeleteById(int id);
    }
}