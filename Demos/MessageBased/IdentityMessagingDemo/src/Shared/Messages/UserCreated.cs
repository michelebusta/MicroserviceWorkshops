using System;

namespace Shared.Messages
{
    public class UserCreated
    {
        public UserCreated(string email, string password, string userName, DateTimeOffset occuredAt)
        {
            Email = email;
            Password = password;
            UserName = userName;
            OccuredAt = occuredAt;
        }
        
        public string Email { get; }
        public string Password { get; }
        public string UserName { get; set; }
        public DateTimeOffset OccuredAt { get; }
    }
}
