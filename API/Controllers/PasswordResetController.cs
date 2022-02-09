using ApiLibrary.Authentication;
using ApiLibrary.Common;
using ApiLibrary.Entities;
using ApiLibrary.Repositories.PasswordResetRepository;
using ApiLibrary.Repositories.UserRepository;
using ApiLibrary.UserPasswordReset;
using ApiLibrary.Validators;
using ApiLibrary.Validators.DetailedValidators;
using CommonLibrary.Dto;
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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticationService _authenticationService;

        public PasswordResetController(IUserRepository userRepository,
            IPasswordResetRepository passwordResetRepository,
            ITokenProvider tokenProvider,
            IPasswordHasher passwordHasher,
            IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _passwordResetRepository = passwordResetRepository;
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetResetPasswordToken(string emailAddress)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            if (!EmailValidator.Validate(emailAddress))
            {
                return BadRequest();
            }

            var user = await _userRepository.GetUserByEmailAsync(emailAddress);
            if (user is null)
            {
                return BadRequest();
            }

            var passwordReset = await _passwordResetRepository.GetByUserId(user.Id);
            await DeleteIfExpired(passwordReset);

            if (passwordReset is not null)
                return Ok(passwordReset.ResetToken);

            var token = await _tokenProvider.GenerateAsync(user);
            passwordReset = new PasswordReset
            {
                ResetToken = token,
                ValidTo = DateTime.Now + TimeSpan.FromMinutes(Constants.ResetPasswordExpirationTimeInMinutes),
                User = user
            };

            await _passwordResetRepository.CreatePasswordReset(passwordReset);

            return Ok(token);
        }

        private async Task DeleteIfExpired(PasswordReset passwordReset)
        {
            if (passwordReset is null)
                return;

            if (passwordReset.ValidTo < DateTime.Now)
            {
                await _passwordResetRepository.DeleteById(passwordReset.Id);
            }
        }

        [HttpGet("Validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateResetPasswordToken(string emailAddress, string token)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            var user = await _userRepository.GetUserByEmailAsync(emailAddress);
            if (user is null)
            {
                return BadRequest();
            }

            var result = _tokenProvider.Validate(user, token);
            if (result)
                return Ok(true);

            return BadRequest();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordDto dto)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            if (!UpdatePasswordValidator.Validate(dto))
            {
                return BadRequest();
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user is null)
            {
                return BadRequest();
            }

            // Unlock user account
            await _authenticationService.ResetFailedAttemptsAsync(user);

            var encodedNewPassword = _passwordHasher.HashPassword(dto.Password);
            user.Password = encodedNewPassword;
            await _userRepository.UpdateUserAsync(user);

            var passwordReset = await _passwordResetRepository.GetByUserId(user.Id);
            await _passwordResetRepository.DeleteById(passwordReset.Id);

            return Ok();
        }
    }
}