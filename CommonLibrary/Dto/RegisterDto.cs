﻿namespace CommonLibrary.Dto
{
    public record RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MasterPassword { get; set; }
    }
}