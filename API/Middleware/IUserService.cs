using CommonLibrary.Dto;
using System.Threading.Tasks;

namespace API.Middleware
{
    public interface IUserService
    {
        public Task<UserDto> GetById(int id);
    }
}