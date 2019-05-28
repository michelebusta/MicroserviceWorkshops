using System;

namespace Shared.Messages
{
    public class UserUnlocked
    {
        public string UserName { get; }
        public DateTimeOffset OccuredAt { get; }

        public UserUnlocked(string userName, DateTimeOffset occuredAt)
        {
            UserName = userName;
            OccuredAt = occuredAt;
        }
    }
}