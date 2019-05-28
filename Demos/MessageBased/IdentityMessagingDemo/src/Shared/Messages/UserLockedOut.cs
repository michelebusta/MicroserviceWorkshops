using System;

namespace Shared.Messages
{
    public class UserLockedOut
    {
        public string UserName { get; }
        public DateTimeOffset OccuredAt { get; }

        public UserLockedOut(string userName, DateTimeOffset occuredAt)
        {
            UserName = userName;
            OccuredAt = occuredAt;
        }
    }
}