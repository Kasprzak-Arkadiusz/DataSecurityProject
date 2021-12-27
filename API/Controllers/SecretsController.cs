using API.Middleware;
using Application.Authentication;
using Application.Common.Dto;
using Application.Repositories.SecretRepository;
using Application.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Entities;

namespace API.Controllers
{
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetServiceNames([FromBody] string userName)
        {
            var serviceNames = await _secretRepository.GetUserServiceNamesAsync(userName);
            return Ok(serviceNames);
        }

        [HttpGet("Password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPassword([FromBody] string masterPassword, [FromBody] string serviceName,
            [FromBody] string userName)
        {
            var user = await _userRepository.GetUserByNameAsync(userName);
            var result = _passwordHasher.VerifyHashedPassword(user.MasterPassword, masterPassword);

            if (!result)
            {
                return BadRequest();
            }

            var secret = await _secretRepository.GetSecretAsync(serviceName, userName);
            var password = _secretHasher.DecryptPassword(secret.Password, secret.Iv);

            return Ok(password);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateSecret([FromBody] SecretDto secretDto)
        {
            if (secretDto == null)
                return BadRequest();

            var user = await _userRepository.GetUserByNameAsync(secretDto.UserName);

            var secret = new Secret
            {
                Password = secretDto.Password,
                ServiceName = secretDto.ServiceName,
                User = user
            };

            await _secretRepository.CreateSecretAsync(secret);

            return Ok();
        }
    }
}