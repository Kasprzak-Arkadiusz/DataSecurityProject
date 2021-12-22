using Application.Common;
using Application.Common.Dto;
using Application.Entities;
using Application.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
                return Result.Failure(new List<string> { "A user with provided user name address already exists." });

            if (await _userRepository.GetUserByEmailAsync(userDto.Email) is not null)
                return Result.Failure(new List<string> { "A user with provided email address already exists." });

            var hashedPassword = _passwordHasher.HashPassword(userDto.Password);
            var hashedMasterPassword = _passwordHasher.HashPassword(userDto.MasterPassword);

            var user = new User(userDto.UserName, userDto.Email, hashedPassword, hashedMasterPassword);
            await _userRepository.CreateUserAsync(user);

            return Result.Success();
        }

        public async Task<LoginResponse> LoginAsync(LoginDto loginDto, string jwtKey)
        {
            var failureResponse = new[] { "Invalid attempt" };

            var providedUserName = loginDto.UserName;
            var providedPassword = loginDto.Password;

            var user = await _userRepository.GetUserByNameAsync(providedUserName);

            if (user is null)
                return new LoginResponse { Result = Result.Failure(failureResponse) };

            if (user.IsLockedOut())
                return new LoginResponse { Result = Result.Failure(new[] { "This account is currently locked out. Try again later." }) };

            if (!await TryLoginAsync(user, providedPassword))
                return new LoginResponse { Result = Result.Failure(failureResponse) };

            var token = GenerateJwtToken(user, jwtKey);

            var response = new LoginResponse()
            {
                Token = token,
                Claims = GenerateClaims(user),
                Result = Result.Success()
            };

            return response;
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

        private static string GenerateJwtToken(User user, string jwtKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(Constants.JwtExpirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<ClaimDto> GenerateClaims(User user)
        {
            var claims = new List<ClaimDto>
            {
                new(nameof(user.Email), user.Email),
                new("Name", user.UserName)
            };

            return claims;
        }

        public void Logout(string userName)
        {
            throw new NotImplementedException();
        }
    }
}