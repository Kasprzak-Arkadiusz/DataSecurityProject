using Application.Common.Dto;
using System.Collections.Generic;

namespace Application.Common
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public List<ClaimDto> Claims { get; set; }
        public Result Result { get; set; }
    }
}