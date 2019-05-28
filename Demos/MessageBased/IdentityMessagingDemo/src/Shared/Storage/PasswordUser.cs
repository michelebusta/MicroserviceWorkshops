using System;
using Newtonsoft.Json;

namespace Shared.Storage
{
    public class PasswordUser
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool IsLockedOut { get; set; }
        public int UnsuccessfulLoginAttempts { get; set; }

        public string ConfirmationToken { get; set; }
    }
}