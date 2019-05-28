using System;

namespace Shared.Messages
{
    public class EmailConfirmed
    {
        public EmailConfirmed(string userName, string token, DateTimeOffset occuredAt)
        {
            UserName = userName;
            Token = token;
            OccuredAt = occuredAt;
        }
        
        public string UserName { get; }
        public string Token { get; }
        public DateTimeOffset OccuredAt { get; }
    }
}