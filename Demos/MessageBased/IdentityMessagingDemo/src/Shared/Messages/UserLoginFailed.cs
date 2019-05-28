using System;

namespace Shared.Messages
{
    public class UserLoginFailed
    {
        public string UserName { get; }
        public int RetryCount { get; }
        public DateTimeOffset OccuredAt { get; }

        public UserLoginFailed(string userName, int retryCount, DateTimeOffset occuredAt)
        {
            UserName = userName;
            RetryCount = retryCount;
            OccuredAt = occuredAt;
        }
    }
}