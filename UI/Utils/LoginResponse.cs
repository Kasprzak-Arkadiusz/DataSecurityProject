﻿using System.Collections.Generic;

namespace UI.Utils
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public List<ClaimDto> Claims { get; set; }
        public Result Result { get; set; }
    }
}