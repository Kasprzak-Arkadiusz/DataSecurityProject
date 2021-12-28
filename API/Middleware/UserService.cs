using ApiLibrary.Repositories.UserRepository;
using CommonLibrary.Dto;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            var userDto = new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName
            };

            return userDto;
        }
    }
}