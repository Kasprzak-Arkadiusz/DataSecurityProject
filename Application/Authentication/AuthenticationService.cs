using Application.Entities;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Dto;

namespace Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<Result> RegisterAsync(UserDto userDto)
        {
            if (await _userRepository.GetUserByNameAsync(userDto.UserName) is not null)
                return Result.Failure(new List<string> {"A user with provided user name address already exists."});

            if (await _userRepository.GetUserByEmailAsync(userDto.Email) is not null)
                return Result.Failure(new List<string> {"A user with provided email address already exists."});
            
            var hashedPassword = _passwordHasher.HashPassword(userDto.Password);
            var hashedMasterPassword = _passwordHasher.HashPassword(userDto.MasterPassword);

            var user = new User(userDto.UserName, userDto.Email, hashedPassword, hashedMasterPassword);
            await _userRepository.CreateUserAsync(user);

            return Result.Success();
        }

        public bool Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout(string userName)
        {
            throw new NotImplementedException();
        }
    }
}