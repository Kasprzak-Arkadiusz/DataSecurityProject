using System;

namespace Application.Entities
{
    public class LoginFailure
    {
        public int Id { get; set; }
        public byte NumberOfFailedAttempts { get; set; }
        public bool IsTemporaryLockout { get; set; }
        public DateTime LockoutTo { get; set; }
        public byte NumberOfLockoutsInARow { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }
    }
}