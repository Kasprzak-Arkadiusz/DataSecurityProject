using API.Middleware;
using ApiLibrary.Common;
using ApiLibrary.Entities;
using ApiLibrary.Repositories.LastConnectionRepository;
using ApiLibrary.Repositories.UserRepository;
using CommonLibrary.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class LastConnectionController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILastConnectionRepository _lastConnectionRepository;

        public LastConnectionController(IUserRepository userRepository,
            ILastConnectionRepository lastConnectionRepository)
        {
            _userRepository = userRepository;
            _lastConnectionRepository = lastConnectionRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastConnections(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(userName);
                if (user is null)
                    return BadRequest();

                var lastConnections =
                    await _lastConnectionRepository.GetByUserIdAsync(user.Id, Constants.NumberOfLastConnections);

                var lastConnectionsDto = lastConnections.Select(i => new LastConnectionDto
                {
                    DeviceType = i.DeviceType,
                    BrowserName = i.BrowserName,
                    PlatformName = i.PlatformName,
                    City = i.City,
                    Region = i.Region,
                    Country = i.Country,
                    ConnectionTime = i.ConnectionTime,
                });

                return Ok(lastConnectionsDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLastConnection(LastConnectionDto dto)
        {
            var user = await _userRepository.GetUserByNameAsync(dto.UserName);
            if (user is null)
                return BadRequest();

            var lastConnection = new LastConnection
            {
                DeviceType = dto.DeviceType,
                BrowserName = dto.BrowserName,
                PlatformName = dto.PlatformName,
                City = dto.City,
                Region = dto.Region,
                Country = dto.Country,
                ConnectionTime = dto.ConnectionTime,
                User = user
            };

            await _lastConnectionRepository.CreateAsync(lastConnection);

            return Ok();
        }
    }
}