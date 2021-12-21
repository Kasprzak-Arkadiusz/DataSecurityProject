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
        private readonly ILoginFailureRepository _loginFailureRepository;

        public AuthenticationService(IPasswordHasher passwordHasher,
            IUserRepository userRepository,
            ILoginFailureRepository loginFailureRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _loginFailureRepository = loginFailureRepository;
        }

        public async Task<Result> RegisterAsync(RegisterDto userDto)
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

        public async Task<Result> LoginAsync(LoginDto loginDto)
        {
            var failureResponse = new[] {"Invalid attempt"};

            var providedUserName = loginDto.UserName;
            var providedPassword = loginDto.Password;

            var user = await _userRepository.GetUserByNameAsync(providedUserName);

            if (user is null)
                return Result.Failure(failureResponse);

            if (user.IsLockedOut())
                return Result.Failure(new[] {"This account is currently locked out."});

            if(!await TryLoginAsync(user, providedPassword))
                return Result.Failure(failureResponse);

            //TODO Generate JWT Token 
            
            return Result.Success();
        }

        private async Task<bool> TryLoginAsync(User user, string providedPassword)
        {
            var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(user.Password, providedPassword);
            var loginFailure = user.LoginFailure;

            if (!isPasswordCorrect)
            {
                loginFailure.FailedAttempt();
                await _loginFailureRepository.UpdateLoginFailureAsync(loginFailure);

                return false;
            }

            loginFailure.SuccessfulAttempt();

            return true;
        }

        public void Logout(string userName)
        {
            throw new NotImplementedException();
        }
    }
}