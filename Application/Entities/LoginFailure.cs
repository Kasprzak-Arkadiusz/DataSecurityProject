using Application.Common;
using System;

namespace Application.Entities
{
    public class LoginFailure
    {
        public int Id { get; set; }
        public byte NumberOfFailedAttempts { get; set; }

        // If true, account is blocked until user unblock it by sending an email
        public bool IsTemporaryLockout { get; set; }
        public DateTime LockoutTo { get; set; }
        public byte NumberOfLockoutsInARow { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        internal bool IsLockedOut()
        {
            return LockoutTo > DateTime.Now;
        }

        public void SuccessfulAttempt()
        {
            NumberOfFailedAttempts = 0;
            NumberOfLockoutsInARow = 0;
        }

        public void FailedAttempt()
        {
            NumberOfFailedAttempts++;

            if (NumberOfFailedAttempts != Constants.MaxNumberOfFailedAttempts)
                return;

            ApplyLockout();
        }

        private void ApplyLockout()
        {
            NumberOfFailedAttempts = 0;
            NumberOfLockoutsInARow++;

            if (NumberOfLockoutsInARow == Constants.MaxNumberOfLockouts)
                IsTemporaryLockout = true;

            LockoutTo = DateTime.Now + new TimeSpan(0, Constants.LockoutSpansInMinutes[NumberOfLockoutsInARow - 1], 0);
        }
    }
}