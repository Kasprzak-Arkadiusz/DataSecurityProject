using ApiLibrary.Authentication;
using ApiLibrary.Common;
using ApiLibrary.Entities;
using ApiLibrary.Repositories.PasswordResetRepository;
using ApiLibrary.Repositories.UserRepository;
using ApiLibrary.UserPasswordReset;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CommonLibrary.Dto;

namespace API.Controllers
{
    public class PasswordResetController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _passwordResetRepository;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPasswordHasher _passwordHasher;

        public PasswordResetController(IUserRepository userRepository,
            IPasswordResetRepository passwordResetRepository,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordResetRepository = passwordResetRepository;
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
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

        [HttpGet("Validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateResetPasswordToken(string emailAddress, string token)
        {
            var user = await _userRepository.GetUserByEmailAsync(emailAddress);
            if (user is null)
            {
                return BadRequest();
            }

            var result = _tokenProvider.Validate(user, token);
            if (!result)
            {
                return BadRequest();
            }

            return Ok(true);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user is null)
            {
                return BadRequest();
            }

            var encodedNewPassword = _passwordHasher.HashPassword(dto.Password);
            user.Password = encodedNewPassword;
            await _userRepository.UpdateUserAsync(user);
            await _passwordResetRepository.DeleteByUserId(user.Id);

            return Ok();
        }
    }
}