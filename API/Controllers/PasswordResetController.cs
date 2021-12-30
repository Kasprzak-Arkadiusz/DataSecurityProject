using ApiLibrary.Common;
using ApiLibrary.Entities;
using ApiLibrary.Repositories.PasswordResetRepository;
using ApiLibrary.Repositories.UserRepository;
using ApiLibrary.UserPasswordReset;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PasswordResetController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _passwordResetRepository;
        private readonly ITokenProvider _tokenProvider;

        public PasswordResetController(IUserRepository userRepository,
            IPasswordResetRepository passwordResetRepository,
            ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _passwordResetRepository = passwordResetRepository;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetResetPasswordToken(string emailAddress)
        {
            var user = await _userRepository.GetUserByEmailAsync(emailAddress);
            if (user is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                return BadRequest();
            }

            var token = await _tokenProvider.GenerateAsync(user);

            var passwordReset = new PasswordReset
            {
                ResetToken = token,
                ValidTo = DateTime.Now + TimeSpan.FromMinutes(Constants.ResetPasswordExpirationTimeInMinutes),
                User = user
            };

            //await _passwordResetRepository.CreatePasswordReset(passwordReset);

            return Ok(token);
        }
    }
}