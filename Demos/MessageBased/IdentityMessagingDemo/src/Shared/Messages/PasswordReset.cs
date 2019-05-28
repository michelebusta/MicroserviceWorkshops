using System;

namespace Shared.Messages
{
    public class PasswordReset
    {
        public string UserName { get; }
        public DateTimeOffset OccuredAt { get; }

        public PasswordReset(string userName, DateTimeOffset occuredAt)
        {
            UserName = userName;
            OccuredAt = occuredAt;
        }
    }
}