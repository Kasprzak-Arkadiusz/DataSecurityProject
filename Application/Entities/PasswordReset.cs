using System;

namespace Application.Entities
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public string ResetToken { get; set; }
        public DateTime ValidTo { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}