using System.Collections.Generic;
using CommonLibrary.Dto;

namespace CommonLibrary.Common
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public List<ClaimDto> Claims { get; set; }
        public Result Result { get; set; }
    }
}