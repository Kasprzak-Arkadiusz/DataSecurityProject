using System;
using API.Middleware;
using ApiLibrary.Authentication;
using ApiLibrary.Repositories.SecretRepository;
using ApiLibrary.Repositories.UserRepository;
using CommonLibrary.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApiLibrary.Validators.DetailedValidators;
using Secret = ApiLibrary.Entities.Secret;

namespace API.Controllers
{
    [Authorize]
    public class SecretsController : BaseApiController
    {
        private readonly ISecretRepository _secretRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISecretPasswordHasher _secretHasher;

        public SecretsController(ISecretRepository secretRepository,
            IPasswordHasher passwordHasher,
            ISecretPasswordHasher secretHasher,
            IUserRepository userRepository)
        {
            _secretRepository = secretRepository;
            _passwordHasher = passwordHasher;
            _secretHasher = secretHasher;
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetServiceNames(string userName)
        {
            var serviceNames = await _secretRepository.GetUserServiceNamesAsync(userName);
            return Ok(serviceNames);
        }

        [HttpGet("Password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPassword(string masterPassword, string serviceName, string userName)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);

            if (user is null)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                return BadRequest();
            }

            var result = _passwordHasher.VerifyHashedPassword(user.MasterPassword, masterPassword);

            if (!result)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                return BadRequest();
            }

            var secret = await _secretRepository.GetSecretAsync(serviceName, userName);
            var password = _secretHasher.DecryptPassword(secret.Password, secret.Iv);

            return Ok(password);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateSecret([FromBody] SecretDto secretDto)
        {
            if (!SecretValidator.Validate(secretDto))
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                return BadRequest("Invalid values");
            }

            var user = await _userRepository.GetUserByNameAsync(secretDto.UserName);
            var (encryptedPassword, iv) = _secretHasher.EncryptPassword(secretDto.Password);

            var secret = new Secret
            {
                Password = encryptedPassword,
                Iv = iv,
                ServiceName = secretDto.ServiceName,
                User = user
            };

            await _secretRepository.CreateSecretAsync(secret);

            return Ok();
        }
    }
}