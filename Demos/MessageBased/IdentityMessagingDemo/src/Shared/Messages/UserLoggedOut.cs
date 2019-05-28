using System;

namespace Shared.Messages
{
    public class UserLoggedOut
    {
        public string UserName { get; }
        public DateTimeOffset OccuredAt { get; }

        public UserLoggedOut(string userName, DateTimeOffset occuredAt)
        {
            UserName = userName;
            OccuredAt = occuredAt;
        }
    }
}