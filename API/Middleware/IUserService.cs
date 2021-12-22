using System.Threading.Tasks;
using Application.Common.Dto;

namespace API.Middleware
{
    public interface IUserService
    {
        public Task<UserDto> GetById(int id);
    }
}
