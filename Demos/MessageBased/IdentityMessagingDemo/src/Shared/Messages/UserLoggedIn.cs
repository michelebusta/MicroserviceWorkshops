using System;

namespace Shared.Messages
{
    public class UserLoggedIn
    {
        public string UserName { get; }
        public DateTimeOffset OccuredAt { get; }

        public UserLoggedIn(string userName, DateTimeOffset occuredAt)
        {
            UserName = userName;
            OccuredAt = occuredAt;
        }
    }
}